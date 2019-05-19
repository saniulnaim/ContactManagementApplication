using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        private DbSet<T> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        public ContactManagementApplicationContext Context { get; set; }

        //public GenericRepository(IUnitOfWork<ContactManagementApplicationContext> unitOfWork)
        //        : this(unitOfWork.context)
        //{

        //}

        public GenericRepository(ContactManagementApplicationContext context)
        {
            _isDisposed = false;
            Context = context;
        }

        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Entities.Add(entity);
                Context.SaveChanges(); // need to delete when will add UOF
                if (Context == null || _isDisposed)
                    Context = new ContactManagementApplicationContext();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                //Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                if (Context == null || _isDisposed)
                    Context = new ContactManagementApplicationContext();
                SetEntryModified(entity);
            }
            catch (Exception ex)
            {

            }
        }
        private void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ContactManagementApplicationContext();
                Entities.Remove(entity);
            }
            catch
            {

            }
        }
    }
}

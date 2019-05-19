using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void BulkInsert(IEnumerable<T> entities);
        void Update(T obj);
        void Delete(T obj);
    }
}

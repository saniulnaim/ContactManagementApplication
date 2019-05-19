using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Abstract
{
    public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
    {
        TContext context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}

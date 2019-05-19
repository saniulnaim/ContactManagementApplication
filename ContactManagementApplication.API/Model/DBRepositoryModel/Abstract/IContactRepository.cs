using ContactManagementApplication.API.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Abstract
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
    }
}

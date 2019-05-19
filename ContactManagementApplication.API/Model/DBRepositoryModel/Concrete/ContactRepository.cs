using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Concrete
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(ContactManagementApplicationContext context) : base(context)
        {
        }



    }
}

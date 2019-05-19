using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DBRepositoryModel.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ContactManagementApplicationContext context) : base(context)
        {

        }
    }
}

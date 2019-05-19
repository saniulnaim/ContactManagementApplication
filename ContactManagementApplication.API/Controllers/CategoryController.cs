using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagementApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IUnitOfWork<ContactManagementApplicationContext> unitOfWork;
        private ICategoryRepository categoryRepository;

        public CategoryController(IUnitOfWork<ContactManagementApplicationContext> unitOfWorkParam, 
                                      ICategoryRepository categoryRepositoryParam)
        {
            unitOfWork = unitOfWorkParam;
            categoryRepository = categoryRepositoryParam;
        }

        [HttpPost]
        [Route("AddCategory")]
        public void AddCategory(string Title, string Description)
        {
            Category categoryObj = new Category();
            categoryObj.Title = Title;
            categoryObj.Description = Description;
            categoryObj.CreatedDate = DateTime.Now;
            categoryRepository.Insert(categoryObj);
            //unitOfWork.Save();
        }
      
    }
}
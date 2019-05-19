using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using ContactManagementApplication.API.Model.DBRepositoryModel.Concrete;
using ContactManagementApplication.API.Model.DomainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactManagementApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IUnitOfWork<ContactManagementApplicationContext> unitOfWork; 
        private IRegistrationRepository registrationRepository;
        private UserManager<AppUser> userManager;

        public RegistrationController(IUnitOfWork<ContactManagementApplicationContext> unitOfWorkParam, UserManager<AppUser> userManagerParam,
                                      IRegistrationRepository registrationRepositoryParam)
        {
            unitOfWork = unitOfWorkParam;
            registrationRepository = registrationRepositoryParam;
            userManager = userManagerParam;
        }


        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "saniul" && user.Password == "123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:44316",
                    audience: "http://localhost:44316",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<Registration> list = registrationRepository.GetAll().ToList();
            return new JsonResult(list);
        }

        [HttpPost]
        [Route("AddRegistration")]
        public async Task AddRegistrationAsync(string Name,string Email, string Mobile, string Address, string Password)
        {
            Registration registrationObj = new Registration();
            registrationObj.Name = Name;
            registrationObj.Email = Email;
            registrationObj.Mobile = Mobile;
            registrationObj.Address = Address;
            //registrationObj.Password = Password;
            registrationObj.CreatedDate = DateTime.Now;
            registrationRepository.Insert(registrationObj);
            //unitOfWork.Save();


            //AppUser isUserExist = await userManager.FindByEmailAsync(Email);
            //if (isUserExist == null)
            //{
            //    AppUser user = new AppUser
            //    {
            //        UserName = Email,
            //        Email = Email
            //    };
            //    Microsoft.AspNetCore.Identity.IdentityResult result = await userManager.CreateAsync(user, Password);
            //    if (result.Succeeded)
            //    {
            //    }
            //    else
            //    {
            //        foreach (IdentityError error in result.Errors)
            //        {
            //            ModelState.AddModelError("", error.Description);
            //        }
            //    }
            //}
        }

        [HttpPut, Authorize, Route("UpdateContact")]
        public void Update(decimal id,string Name, string Email, string Mobile, string Address, string Password)
        {
            Registration registrationObj = new Registration();
            registrationObj.Id = id;
            registrationObj.Name = Name;
            registrationObj.Email = Email;
            registrationObj.Mobile = Mobile;
            registrationObj.Address = Address;
            registrationRepository.Update(registrationObj);
        }
    }
}
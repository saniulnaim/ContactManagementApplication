using ContactManagementApplication.API;
using ContactManagementApplication.API.Controllers;
using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using ContactManagementApplication.API.Model.DBRepositoryModel.Concrete;
using ContactManagementApplication.API.Model.DomainModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace ContactManagementApplication.Test
{
    public class RegistrationControllerTest
    {
 
        [Fact]
        public void GetAll_ReturnJsonResult()
        {
            //Arrange
            RegistrationController registrationController =
                new RegistrationController(null,null, new RegistrationRepository(new ContactManagementApplicationContext()));

            //Act
            var result = registrationController.GetAll();

            //Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void GetAll_ReturnAllItems()
        {
            //Arrange
            RegistrationController registrationController =
                 new RegistrationController(null, null, new RegistrationRepository(new ContactManagementApplicationContext()));

            //Act
            JsonResult result = registrationController.GetAll() as JsonResult;

            //Assert
            JObject jObj = (JObject)JsonConvert.DeserializeObject(result.ToString());
            Assert.Equal(5, jObj.Count);
        }

        [Fact]
        public void Login_BadRequest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            var url = "api/Registration/login";
            var expected = HttpStatusCode.Unauthorized;

            // Act
            var response = client.GetAsync(url);

            // Assert
            Assert.Equal(expected, response.Result.StatusCode);
        }

        [Fact]
        public void Login_Unauthorized()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            var url = "api/Registration/login?UserName=test&Password=123";
            var expected = HttpStatusCode.Unauthorized;

            // Act
            var response = client.GetAsync(url);

            // Assert
            Assert.Equal(expected, response.Result.StatusCode);
        }
    }
}

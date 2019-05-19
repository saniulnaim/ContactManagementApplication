using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactManagementApplication.API.EntityModel;
using ContactManagementApplication.API.Model.DBRepositoryModel.Abstract;
using ContactManagementApplication.API.Model.DomainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ContactManagementApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IUnitOfWork<ContactManagementApplicationContext> unitOfWork;
        private IContactRepository contactRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

            public ContactController(IUnitOfWork<ContactManagementApplicationContext> unitOfWorkParam, IHostingEnvironment hostingEnvironment,
                                      IContactRepository contactRepositoryParam)
        {
            unitOfWork = unitOfWorkParam;
            contactRepository = contactRepositoryParam;
                _hostingEnvironment = hostingEnvironment;
            }

        [HttpPost, Authorize, Route("AddContact")]
        public IActionResult AddContact([FromBody]ContactDomainModel contactParam, IFormFile filesData)
        {
            if (contactParam == null)
            {
                return BadRequest("Invalid client request");
            }

            Contact contactObj = new Contact();
            contactObj.Name = contactParam.Name;
            contactObj.Email = contactParam.Email;
            contactObj.Mobile = contactParam.Mobile;
            contactObj.Address = contactParam.Address;
            contactObj.CategoryId = contactParam.CategoryId;
            contactObj.CreatedDate = DateTime.Now;
            #region ImageProcessing
            if (filesData != null && filesData.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    filesData.CopyToAsync(stream);
                    contactObj.ImageData = stream.ToArray();
                }
            }
            #endregion ImageProcessing
            contactRepository.Insert(contactObj);
            return new JsonResult("Contact has been saved succesfull");
        }

        [HttpDelete, Authorize, Route("DeleteContact")]
        public void Delete(decimal id)
        {
            Contact obj = contactRepository.GetById(id);
            contactRepository.Delete(obj);
        }

        [HttpPut, Authorize, Route("UpdateContact")]
        public void Update([FromBody]ContactDomainModel contactParam)
        {
            Contact contactObj = new Contact();
            contactObj.Id = contactParam.Id;
            contactObj.Name = contactParam.Name;
            contactObj.Email = contactParam.Email;
            contactObj.Mobile = contactParam.Mobile;
            contactObj.Address = contactParam.Address;
            contactObj.CategoryId = contactParam.CategoryId;
            contactRepository.Update(contactObj);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll(int pageSize, int pageIndex)
        {
            List<Contact> list = contactRepository.GetAll().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new JsonResult(list);
            
        }


        [HttpGet]
        [Route("ExportAsJson")]
        public JsonResult ExportAsJson()
        {
            List<Contact> list = contactRepository.GetAll().ToList();
            return new JsonResult(list);

        }

        [HttpGet]
        [Route("ExportAsExcel")]
        public IActionResult ExportAsExcel()
        {
            string export = "export";
            byte[] fileContents;
            List<Contact> list = contactRepository.GetAll().ToList();

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(export);
                //First add the headers
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Mobile";
                worksheet.Cells[1, 4].Value = "CategoryId";
                worksheet.Cells[1, 5].Value = "Address";

                int cellRowNumber = 2;
                foreach(Contact c in list)
                {
                    //Add values
                    worksheet.Cells["A" + cellRowNumber].Value = c.Name;
                    worksheet.Cells["B" + cellRowNumber].Value = c.Email;
                    worksheet.Cells["C" + cellRowNumber].Value = c.Mobile;
                    worksheet.Cells["D" + cellRowNumber].Value = c.CategoryId;
                    worksheet.Cells["E" + cellRowNumber].Value = c.Address;
                    cellRowNumber++;
                }
                package.Save();
                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: export + ".xlsx"
            );
        }


        [HttpGet, Authorize]
        [Route("ImportAsExcel")]
        public IActionResult ImportAsExcel(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return BadRequest();
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            var list = new List<Contact>();

            using (var stream = new MemoryStream())
            {
                formFile.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new Contact
                        {
                            Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Email = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Mobile = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            Address = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            CategoryId = decimal.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                            CreatedDate = DateTime.Now
                        });
                    }
                }
            }

            contactRepository.BulkInsert(list);
            return new JsonResult(list);
        }
    }
}
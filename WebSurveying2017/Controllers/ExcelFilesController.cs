using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Helper;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/ExcelFiles")]
    public class ExcelFilesController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private IExcelFilesService excelFilesService;

        public ExcelFilesController(IUnitOfWork unitOfWork, IExcelFilesService excelFilesService)
        {
            this.unitOfWork = unitOfWork;
            this.excelFilesService = excelFilesService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("{surveyId}")]
        public IHttpActionResult GetExcelFilesForSurvey(int surveyId)
        {

            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            try
            {
                var files = excelFilesService.GetFiles(surveyId).ToList();
                if (files.Count() > 0 && files.FirstOrDefault().Survey.UserId != userId)
                    return Unauthorized();
                return Ok(files);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        
    }
}

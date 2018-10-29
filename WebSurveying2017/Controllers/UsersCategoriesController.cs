using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/userscategories")]
    public class UsersCategoriesController : ApiController
    {
        private IUserCategoryService userCategoryService;
        private IUnitOfWork unitOfWork;

        public UsersCategoriesController(IUnitOfWork unitOfWork, IUserCategoryService userCategoryService)
        {
            this.userCategoryService = userCategoryService;
            this.unitOfWork = unitOfWork;
        }

       
    }
}
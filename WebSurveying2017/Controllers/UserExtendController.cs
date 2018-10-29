using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Infrastructure;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserExtend")]
    public class UserExtendController : ApiController
    {
        public IUnitOfWork unitOfWork;
        public IUserExtendService userExtendService;

        public UserExtendController(IUnitOfWork unitOfWork, IUserExtendService userExtendService)
        {
            this.unitOfWork = unitOfWork;
            this.userExtendService = userExtendService;
        }

        
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetUserExtend(int id)
        {

            UserExtendViewModel user = null;
            try
            {
                var userInfo = userExtendService.Get(id);
                var notificationCount = userExtendService.NotificationCount(id);
                var roleName = userExtendService.GetRoleName(id);

                user = new UserExtendViewModel()
                {
                    Id = userInfo.ApplicationUser.Id,
                    FirstName = userInfo.ApplicationUser.FirstName,
                    LastName = userInfo.ApplicationUser.LastName,
                    Email = userInfo.ApplicationUser.Email,
                    NotificationCount = notificationCount,
                    RoleName = roleName
                };
            } catch(Exception e)
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}
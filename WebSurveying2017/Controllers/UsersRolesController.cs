using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Service;

namespace WebSurveying2017.Controllers
{

    [Authorize]
    [RoutePrefix("api/usersroles")]
    public class UsersRolesController : ApiController
    {

        public IUserRoleService userRoleService;
        public IUnitOfWork unitOfWork;


        public UsersRolesController(IUserRoleService userRoleService, IUnitOfWork unitOfWork)
        {
            this.userRoleService = userRoleService;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{userId}/{roleName}")]
        public IHttpActionResult UpdateUserRole(int userId, string roleName)
        {
            if (roleName == null)
                return BadRequest();
            try
            {
                userRoleService.Update(userId, roleName);
                unitOfWork.Commit();
                
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return Ok();
        }
    }
}

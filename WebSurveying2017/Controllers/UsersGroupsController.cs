using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    public class UsersGroupsController : ApiController
    {

        private IUserGroupService userGroupService;
        private IUnitOfWork unitOfWork;

        public UsersGroupsController(IUnitOfWork unitOfWork, IUserGroupService userGroupService)
        {

            this.unitOfWork = unitOfWork;
            this.userGroupService = userGroupService;
        }


        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        [Route("api/usersgroups/addgroup")]
        public IHttpActionResult PostUsersGroups(GroupBindingModel groupBM)
        {

            if (groupBM.UserGroupList == null)
                return BadRequest();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);


            groupBM.UserGroupList.Remove(groupBM.UserGroupList.Where(ug => ug.UserId == userId).FirstOrDefault());
            groupBM.UserId = userId;

            var userGroupList = AutoMapper.Mapper.Map<List<UserGroupBindingModel>, List<UserGroup>>(groupBM.UserGroupList);


            try
            {

                userGroupService.AddUserGroup(userGroupList,groupBM.Name,userId);
                unitOfWork.Commit();
               
            }
            catch (Exception e)
            {
                if (ModelState.Count() == 0)
                    ModelState.AddModelError("", "Postoji grupa sa unetim imenom");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPut]
        [Route("api/usersgroups/{groupId}")]
        public IHttpActionResult UpdateUsersGroups(List<UserGroupBindingModel> userGroup, int groupId)
        {

            if (userGroup == null || userGroup.Count() == 0)
            {
                ModelState.AddModelError("","Grupa mora imati članove");
                return BadRequest(ModelState);
            }

            

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            userGroup.Remove(userGroup.Where(ug => ug.UserId == userId).FirstOrDefault());
            var old = userGroupService.GetForUserAndGroup(userId, groupId);

            

            if (old.First().Group.UserId != userId)
                return Unauthorized();

            var members = AutoMapper.Mapper.Map<List<UserGroupBindingModel>, List<UserGroup>>(userGroup);

            try
            {

                userGroupService.UpdateMembers(members, old.ToList());
                
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }


    }
}
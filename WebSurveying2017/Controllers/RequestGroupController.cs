using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/requests")]
    public class RequestGroupController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private IRequestGroupService requestGroupService;

        public RequestGroupController(IUnitOfWork unitOfWork, IRequestGroupService requestGroupService)
        {
            this.unitOfWork = unitOfWork;
            this.requestGroupService = requestGroupService;

        }

      
        

        [Authorize(Roles = "Admin, User, Moderator")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostRequest(RequestGroup request)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.Claims;
                request.UserId = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                requestGroupService.Create(request);
                unitOfWork.Commit();
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("group/{id}")]
        public IHttpActionResult GetRequests(int id)
        {

            
            try
            {
                var requests = requestGroupService.GetMany(r => r.GroupId == id);

                if (requests.Count() == 0)
                    return Ok(requests);

                var requstsVM = new RequestsViewModel()
                {
                    Group = AutoMapper.Mapper.Map<Group, GroupViewModel>(requestGroupService.GetGroup(requests.First().GroupId)),
                    Users = AutoMapper.Mapper.Map<List<ApplicationUser>, List<UserViewModel>>(requestGroupService.GetUsers(requests.Select(r => r.UserId).ToList()).ToList())
                    
                };
                return Ok(requstsVM);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("{groupId}/{userId}/{respond}")]
        public IHttpActionResult RespondOnRequest(int groupId, int userId, bool respond)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var _userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            var group = requestGroupService.GetGroup(groupId);
            if (group.UserId != _userId)
                return Unauthorized();
            var request = new RequestGroup()
            {
                GroupId = groupId,
                UserId = userId
            };
            try
            {

                requestGroupService.RespondOnRequest(request, group, respond);
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
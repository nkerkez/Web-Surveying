using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/groups")]
    public class GroupsController : ApiController
    {
        public IGroupService groupService;
        public IUnitOfWork unitOfWork;


        public GroupsController(IGroupService groupService, IUnitOfWork unitOfWork)
        {
            this.groupService = groupService;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles ="Admin,Moderator,User")]
        [HttpGet]
        [Route("{page}/{size}")]
        public IHttpActionResult GetGroups(int page, int size, [FromUri] GroupQueryParam obj)
        {
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            List<GroupViewModel> retVal = new List<GroupViewModel>();
            try
            {
                if (obj == null)
                    return BadRequest();
                var tuple = groupService.GetAllForPage( page, size, obj.Name, obj.AuthorFirstName, obj.AuthorLastName);
                var groups = tuple.Item1.ToList();
                var count = tuple.Item2;
                var users = groupService.GetAuthors(groups.Select(g => g.UserId).ToList()).ToList();
                retVal = AutoMapper.Mapper.Map<List<Group>, List<GroupViewModel>>( groups );
                this.SetVMValues(retVal, groups, users, userId);

                var pageModel = new PageModel<GroupViewModel>()
                {
                    CurrentPage = page,
                    Size = size,
                    Models = retVal,
                    Count = count

                };
                return Ok(pageModel);
            }
            catch (Exception)
            {

                return BadRequest();
            }
    
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("authorAndMember")]
        public IHttpActionResult GetForAuthorAndMember()
        {
            try
            {
                //get user
                var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

                return Ok(groupService.GetForAuthorAndMember(userId));
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("user/{page}/{size}")]
        public IHttpActionResult GetForAuthor(int page, int size, [FromUri] GroupQueryParam obj)
        {
            //get user
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            try
            {
                if (obj == null)
                    return BadRequest();
                var tuple = groupService.GetForAuthor(userId, page, size, obj.Name, obj.AuthorFirstName, obj.AuthorLastName);
                var groups = tuple.Item1.ToList();
                var count = tuple.Item2; 
                //var groups = groupService.GetForAuthor(userId).ToList();
                var users = groupService.GetAuthors(groups.Select(g => g.UserId).ToList()).ToList();
                var retVal = AutoMapper.Mapper.Map<List<Group>, List<GroupViewModel>>(groups);
                this.SetVMValues(retVal, groups, users, userId);

                var pageModel = new PageModel<GroupViewModel>()
                {
                    CurrentPage = page,
                    Size = size,
                    Models = retVal,
                    Count = count

                };
                return Ok(pageModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }



        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("member")]
        public IHttpActionResult GetForMember()
        {
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            try
            {
                return Ok(groupService.GetForMember(userId));
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("member/{page}/{size}")]
        public IHttpActionResult GetForMember(int page, int size, [FromUri] GroupQueryParam obj)
        {
            //get user
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            try
            {
                if (obj == null)
                    return BadRequest();
                var tuple = groupService.GetForMember(userId, page, size, obj.Name, obj.AuthorFirstName, obj.AuthorLastName);
                var groups = tuple.Item1.ToList();
                var count = tuple.Item2;
                var users = groupService.GetAuthors(groups.Select(g => g.UserId).ToList()).ToList();
                var retVal = AutoMapper.Mapper.Map<List<Group>, List<GroupViewModel>>(groups);
                this.SetVMValues(retVal, groups, users, userId);
                var pageModel = new PageModel<GroupViewModel>()
                {
                    CurrentPage = page,
                    Size = size,
                    Models = retVal,
                    Count = count

                };
                return Ok(pageModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, GroupBindingModel groupBM)
        {
            //get user
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var old = groupService.Get(id);
            if (old.UserId != userId)
                return Unauthorized();
            groupBM.UserId = userId;
            var group = AutoMapper.Mapper.Map<GroupBindingModel, Group>(groupBM);
            try
            {
                groupService.Update(old, group);
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }



        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteGroup(int id)
        {
            //get user
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var old = groupService.Get(id);
            if (old.UserId != userId)
                return Unauthorized();
            
            try
            {
                groupService.Delete(id);
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("","Grupa nije obrisana. Napomena : Grupa ne može biti obrisana ako postoje ankete");
                return BadRequest(ModelState);
            }



        }

        private void SetVMValues(List<GroupViewModel> retVal, List<Group> groups, List<ApplicationUser> users, int userId)
        {
            retVal.ForEach(obj =>
            {
                var group = groups.First(g => g.Id == obj.Id);
                obj.NumbOfMembers = group.GroupMembers.Count();
                obj.NumbOfSurveys = group.Surveys.Count();
                var user = users.First(u => u.Id == group.UserId);
                obj.IsMember = group.GroupMembers.Select(gm => gm.UserId).Contains(userId);
                obj.User = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(user);
                if(group.Requests != null)
                {
                    obj.NumbOfRequests = group.Requests.Count();
                }
            });
        }
    }
}

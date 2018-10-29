using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebSurveying2017.Models;
using WebSurveying2017.Providers;
using WebSurveying2017.Results;
using WebSurveying2017.ViewModels;
using System.Linq;
using Facebook;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Service;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {

        private IUserService userService;
        private IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork,
            IUserService userService)
        {

            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

       
        [AllowAnonymous]
        public IHttpActionResult GetUser(int id)
        {

            var user = userService.GetManyAsNoTracking(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return BadRequest();
            }
            var userVM = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(user);
            userVM.RoleName = userService.GetRoleName(id);
            userVM.ExternalLogin = user.PasswordHash == null;
            

            return Ok(userVM);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("group/{groupId}")]
        public IHttpActionResult GetUsersForGroup(int groupId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var group = userService.GetGroup(groupId);
            if (userId != group.UserId)
                return Unauthorized();
            var users = userService.GetForGroup(groupId);

            var allModerators = userService.GetForRoleName("Moderator");

            List<UserViewModel> retVal = new List<UserViewModel>();
            foreach (var user in users)
            {
                UserViewModel userVM = new UserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender,
                    City = user.City
                 
                    //  IsModerator = allModerators.Select( allMod => allMod.Id).Contains(user.Id)
                };
                retVal.Add(userVM);
            }

            return Ok(retVal);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("{page}/{size}")]
        public IHttpActionResult GetUsers(int page, int size)
        {
            var users = userService.GetForPage(page, size).ToList();
            var retVal = AutoMapper.Mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);

            PageModel<UserViewModel> pageModel = new PageModel<UserViewModel>() { Models = retVal };
            pageModel.CurrentPage = page;
            pageModel.Count = userService.GetCount();
            pageModel.Size = size;

            return Ok(pageModel);
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpGet]
        [Route("search/{page}/{size}")]
        public IHttpActionResult SearchUsers([FromUri] UserQueryParams obj, int page, int size)
        {
            try
            {
                if (obj == null)
                    return BadRequest();

                var usersTuple = userService.SearchUsers(obj.FirstName, obj.LastName, obj.City,
                    obj.BirthdayFrom, obj.BirthdayTo, obj.Gender, obj.RoleName, page, size);
                var users = usersTuple.Item1.ToList();

                var retVal = AutoMapper.Mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);


                retVal.ForEach(
                    retValObj =>
                    {
                        retValObj.RoleName = userService.GetRoleName(retValObj.Id);
                    });
                PageModel<UserViewModel> pageModel = new PageModel<UserViewModel>() { Models = retVal };
                pageModel.CurrentPage = page;
                pageModel.Count = usersTuple.Item2;
                pageModel.Size = size;

                return Ok(pageModel);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }
        
       

        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                userService.Delete(userService.Get(id));
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        public IHttpActionResult PutUser(int id, UpdateUserBindingModel u)
        {

            var user = AutoMapper.Mapper.Map<UpdateUserBindingModel, ApplicationUser>(u);
            var oldUser = userService.Get(id);
        //    user.Birthday = oldUser.Birthday;
            user.Gender = oldUser.Gender;
            user.UserName = oldUser.Email;
            user.Id = oldUser.Id;
            user.PasswordHash = oldUser.PasswordHash;
            user.EmailConfirmed = oldUser.EmailConfirmed;
            user.SecurityStamp = oldUser.SecurityStamp;
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            if (oldUser == null || userId != user.Id)
                return BadRequest();
            try
            {
                userService.Update(oldUser, user);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/LikeOrDislikeComment")]
    public class LikeOrDislikeCommentController : ApiController
    {
        public ILikeOrDislikeCommentService likeOrDislikeCommentService;
        public IUnitOfWork unitOfWork;

        public LikeOrDislikeCommentController(IUnitOfWork unitOfWork, ILikeOrDislikeCommentService likeOrDislikeCommentService)
        {
            this.unitOfWork = unitOfWork;
            this.likeOrDislikeCommentService = likeOrDislikeCommentService;
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("like/{id}")]
        public IHttpActionResult GetLikes(int id)
        {
            try
            {

                var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var ids = likeOrDislikeCommentService.GetLikesForComment(id).Select(lod => lod.UserId);

                var comment = likeOrDislikeCommentService.GetComment(id);
                if (comment == null)
                    return BadRequest();
                if (comment.UserId != userId)
                    return Unauthorized();

                var users = likeOrDislikeCommentService.GetUsers(ids.ToList()).ToList();

                var retVal = AutoMapper.Mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);

                foreach (var user in retVal)
                {

                    user.RoleName = likeOrDislikeCommentService.GetRoleName(user.Id);


                }
                return Ok(retVal);
            }
            catch(Exception)
            {
                return BadRequest();
            }
           
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("dislike/{id}")]
        public IHttpActionResult GetDislikes(int id)
        {
            try
            {

                var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var ids = likeOrDislikeCommentService.GetDislikesForComment(id).Select(lod => lod.UserId);

                var comment = likeOrDislikeCommentService.GetComment(id);
                if (comment == null)
                    return BadRequest();
                if (comment.UserId != userId)
                    return Unauthorized();

                var users = likeOrDislikeCommentService.GetUsers(ids.ToList()).ToList();

                var retVal = AutoMapper.Mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);

                foreach (var user in retVal)
                {

                    user.RoleName = likeOrDislikeCommentService.GetRoleName(user.Id);


                }
                return Ok(retVal);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        public IHttpActionResult PostLod(LikeOrDislikeComment obj)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            obj.UserId = userId;
            try
            {
                likeOrDislikeCommentService.CreateLikeOrDislike(obj);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();




        }

        [AllowAnonymous]
        [HttpPut]
        [Route("{commentId}")]
        public IHttpActionResult PutLod(LikeOrDislikeComment obj, int commentId)
        {
            
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            obj.UserId = userId;
            
            try
            {
                likeOrDislikeCommentService.Update(obj);
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
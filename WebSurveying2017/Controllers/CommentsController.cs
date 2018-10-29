using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    public class CommentsController : ApiController
    {

        public ICommentService commentService;
        public IUnitOfWork unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork, ICommentService commentService)
        {
            this.unitOfWork = unitOfWork;
            this.commentService = commentService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/comments/{id}")]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = commentService.Get(id);
            List<CommentViewModel> retList = new List<CommentViewModel>();
            CommentViewModel retVal = new CommentViewModel();
            if (comment == null)
            {
                return BadRequest();
            }
            var userId = 0;
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.IsAuthenticated)
            {
                userId = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }


            this.CreateVMList(comment, ref retVal, userId);

            return Ok(retVal);
        }

        [Authorize(Roles = "Admin, User, Moderator")]
        [HttpGet]
        [Route("api/comments/user/{page}/{size}")]
        public IHttpActionResult GetCommentsForUser(int page, int size)
        {

            int id = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            //claims
            var tuple = commentService.GetForUser(id, page, size);
            var comments = tuple.Item1.ToList();
            var commentsVM = AutoMapper.Mapper.Map<List<Comment>, List<CommentViewModel>>(comments);

            commentsVM.ToList().ForEach(
                obj =>
                {
                    var comment = comments.Where(c => c.Id == obj.Id).FirstOrDefault();
                    obj.LikeCount = comment.LikesOrDislikes.Where(lod => lod.IsLike).Count();
                    obj.DislikeCount = comment.LikesOrDislikes.Where(lod => !lod.IsLike).Count();
                }
                );
            var pageModel = new PageModel<CommentViewModel>()
            {
                CurrentPage = page,
                Size = size,
                Models = commentsVM,
                Count = tuple.Item2
            };
            return Ok(pageModel);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/comments/survey/{surveyId}/{page}/{size}")]
        public IHttpActionResult GetComments(int surveyId, int page, int size)
        {
            var userId = 0;
            var identity = (ClaimsIdentity)User.Identity;
            if(identity.IsAuthenticated)
            {
                userId = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            

            var roots = commentService.GetCommentsDateAsc(surveyId, page, size).ToList();
            List<CommentViewModel> retList = new List<CommentViewModel>();

            
            this.CreateVMList(roots, retList, userId);

            PageModel<CommentViewModel> pageModel = new PageModel<CommentViewModel> { Models = retList };
            pageModel.CurrentPage = page;
            pageModel.Count = commentService.GetRootCount(surveyId);
            return Ok(pageModel);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        public IHttpActionResult PostComment(CommentBindingModel commentBM)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);


            var comment = AutoMapper.Mapper.Map<CommentBindingModel, Comment>(commentBM);
           
            try
            {
                
                commentService.Create(comment, userId);
               

                unitOfWork.Commit();

            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPut]
        [Route("api/comments/{id}")]
        public IHttpActionResult PutComment(int id, Comment c)
        {
            if (c.Id != id)
                return BadRequest();

            try
            {
                commentService.Update(c);
                unitOfWork.Commit();

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpDelete]
        [Route("api/comments/{id}")]
        public IHttpActionResult DeleteComment(int id)
        {
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var roleName = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            try
            {
                var old = commentService.Get(id);
                
                if (roleName.Equals("User") && old.UserId != userId)
                    return Unauthorized();

                commentService.DeleteComment(old);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {

                return BadRequest();
            }

            return Ok();
        }



        [AllowAnonymous]
        private void CreateVMList(List<Comment> commentList, List<CommentViewModel> retList, int userId)
        {
            foreach (var comment in commentList)
            {

                
                var obj = AutoMapper.Mapper.Map<Comment, CommentViewModel>(comment);
                obj.SubComments = new List<CommentViewModel>();
                obj.LoggedUserLikeOrDislike = comment.LikesOrDislikes.Where(lod => lod.UserId == userId).FirstOrDefault();
                obj.LikeCount = comment.LikesOrDislikes.Where(lod => lod.IsLike).Count();
                obj.DislikeCount = comment.LikesOrDislikes.Where(lod => !lod.IsLike).Count();
                obj.User = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(commentService.GetAuthor(comment.UserId));
                
                retList.Add(obj);
                this.CreateVMList(comment.SubComments.ToList(), obj.SubComments, userId);
                obj.SubCommentsCount = obj.SubComments.Count();
            }
        }


        [AllowAnonymous]
        private void CreateVMList(Comment comment, ref CommentViewModel retVal, int userId)
        {
            
            retVal = AutoMapper.Mapper.Map<Comment, CommentViewModel>(comment);
            retVal.SubComments = new List<CommentViewModel>();
            retVal.LoggedUserLikeOrDislike  = comment.LikesOrDislikes.Where(lod => lod.UserId == userId).FirstOrDefault();
            retVal.LikeCount = comment.LikesOrDislikes.Where(lod => lod.IsLike).Count();
            retVal.DislikeCount = comment.LikesOrDislikes.Where(lod => !lod.IsLike).Count();
            retVal.User = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(commentService.GetAuthor(comment.UserId));



            this.CreateVMList(comment.SubComments.ToList(), retVal.SubComments, userId);
            retVal.SubCommentsCount = retVal.SubComments.Count();
        }
    }
}

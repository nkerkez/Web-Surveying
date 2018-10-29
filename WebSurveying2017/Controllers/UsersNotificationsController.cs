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
    [RoutePrefix("api/usersnotifications")]
    public class UsersNotificationsController :ApiController
    {
        public IUnitOfWork unitOfWork;
        public IUserNotificationService userNotificationService;
        public UsersNotificationsController(IUnitOfWork unitOfWork, IUserNotificationService userNotificationService)
        {
            this.unitOfWork = unitOfWork;
            this.userNotificationService = userNotificationService;
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAllForUser()
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            IEnumerable<Notification> list = userNotificationService.GetAllNotificationsForUser(userId, NotificationType.ALL);

            List<NotificationViewModel> retList = new List<NotificationViewModel>();

            foreach (var obj in list)
            {
                retList.Add(
                     new NotificationViewModel()
                     {
                         DateTime = obj.DateTime,
                         Message = obj.Message,
                         CommentId = obj.CommentId,
                         SurveyId = obj.SurveyId,
                         NotificationType = obj.NotificaionType,
                         Id = obj.Id,
                         Operation = obj.Operation,
                         UserId = obj.UserId,
                         GroupId = obj.GroupId
                     }
                    );
            }

            return Ok(retList);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("personal")]
        public IHttpActionResult GetPersonalNotificationsForUser()
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            IEnumerable<Notification> list = userNotificationService.GetAllNotificationsForUser(userId, NotificationType.PERSONAL);

            List<NotificationViewModel> retList = new List<NotificationViewModel>();

            foreach (var obj in list)
            {
                retList.Add(
                     new NotificationViewModel()
                     {
                         DateTime = obj.DateTime,
                         Message = obj.Message,
                         CommentId = obj.CommentId,
                         SurveyId = obj.SurveyId,
                         NotificationType = obj.NotificaionType,
                         Id = obj.Id,
                         Operation = obj.Operation,
                         UserId = obj.UserId,
                         GroupId = obj.GroupId
                     }
                    );
            }

            return Ok(retList);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("moderator")]
        public IHttpActionResult GetModeratorNotificationsForUser()
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            var list = userNotificationService.GetAllNotificationsForUser(userId, NotificationType.MODERATOR);

            List<NotificationViewModel> retList = new List<NotificationViewModel>();

            foreach (var obj in list)
            {
                retList.Add(
                     new NotificationViewModel()
                     {
                         DateTime = obj.DateTime,
                         Message = obj.Message,
                         CommentId = obj.CommentId,
                         SurveyId = obj.SurveyId,
                         NotificationType = obj.NotificaionType,
                         Id = obj.Id,
                         Operation = obj.Operation,
                         UserId = obj.UserId
                     }
                    );
            }

            return Ok(retList);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPut]
        [Route("{notificationId}")]
        public IHttpActionResult Read(UserNotification un, int notificationId)
        {

            if (!un.IsRead)
                return BadRequest();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            un.UserId = userId;

            try
            {
                userNotificationService.Update(un);
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
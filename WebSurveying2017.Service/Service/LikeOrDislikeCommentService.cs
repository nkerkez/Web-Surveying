using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Data.Repositories;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Infrastructure;

namespace WebSurveying2017.Service.Service
{
    public interface ILikeOrDislikeCommentService : IService<LikeOrDislikeComment>
    {
        IEnumerable<LikeOrDislikeComment> GetLikesForComment(int commentId);
        IEnumerable<LikeOrDislikeComment> GetDislikesForComment(int commentId);
        IEnumerable<ApplicationUser> GetUsers(List<int> ids);
        void CreateLikeOrDislike(LikeOrDislikeComment likeOrDislikeComment);
        Comment GetComment(int id);
        string GetRoleName(int userId);

    }
    public class LikeOrDislikeCommentService : ServiceBase<LikeOrDislikeComment>, ILikeOrDislikeCommentService
    {
        private readonly ILikeOrDislikeCommentRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IRoleRepository roleRepository;

        public LikeOrDislikeCommentService(ILikeOrDislikeCommentRepository repository,
            IUnitOfWork unitOfWork, IUserRepository userRepository,
            IUserNotificationRepository userNotificationRepository, ICommentRepository commentRepository,
            IRoleRepository roleRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.userNotificationRepository = userNotificationRepository;
            this.commentRepository = commentRepository;
            this.roleRepository = roleRepository;
        }

       

        public override void Update(LikeOrDislikeComment likeOrDislikeComment)
        {
            var user = userRepository.GetById(likeOrDislikeComment.UserId);
            
            var  old = repository.GetMany(lod => lod.UserId == likeOrDislikeComment.UserId && lod.CommentId == likeOrDislikeComment.CommentId).ToList().FirstOrDefault();

            base.Delete(lod => lod.UserId == likeOrDislikeComment.UserId && lod.CommentId == likeOrDislikeComment.CommentId);
            string s = "";
            if (likeOrDislikeComment.IsLike)
            {

                s = " se svidja vas komentar.";
            }
            else
            {
                s = " se ne svidja vas komentar.";

            }
            Comment comment = commentRepository.GetById(likeOrDislikeComment.CommentId);
            if (comment.UserId != likeOrDislikeComment.UserId)
            {
                Notification notification = new Notification("Korisniku " + user.FirstName + " " + user.LastName +
                    s, DateTime.Now, Operation.LIKEORDISLIKE, NotificationType.PERSONAL);
                notification.UserId = likeOrDislikeComment.UserId;

                notification.CommentId = comment.Id;
                notification.SurveyId = comment.SurveyId;

                UserNotification u = new UserNotification(comment.UserId, notification, false);
                userNotificationRepository.Add(u);
            }


            base.Create(likeOrDislikeComment);
            
        }
        void ILikeOrDislikeCommentService.CreateLikeOrDislike(LikeOrDislikeComment likeOrDislikeComment)
        {

            
            var user = userRepository.GetById(likeOrDislikeComment.UserId);

            string s = "";
            if (likeOrDislikeComment.IsLike)
            {

                s = " se sviđa Vaš komentar.";
            }
            else
            {
                s = " se ne sviđa Vaš komentar.";

            }
            Comment comment = commentRepository.GetById(likeOrDislikeComment.CommentId);
            if (comment.UserId != likeOrDislikeComment.UserId)
            {
                Notification notification = new Notification("Korisniku " + user.FirstName + " " + user.LastName +
                    s, DateTime.Now, Operation.LIKEORDISLIKE, NotificationType.PERSONAL);
                notification.UserId = likeOrDislikeComment.UserId;

                notification.CommentId = comment.Id;
                notification.SurveyId = comment.SurveyId;

                UserNotification u = new UserNotification(comment.UserId, notification, false);
                userNotificationRepository.Add(u);
            }


            base.Create(likeOrDislikeComment);

        }

        IEnumerable<LikeOrDislikeComment> ILikeOrDislikeCommentService.GetDislikesForComment(int commentId)
        {
            return repository.GetMany(lod => !lod.IsLike && lod.CommentId == commentId);
        }

        IEnumerable<LikeOrDislikeComment> ILikeOrDislikeCommentService.GetLikesForComment(int commentId)
        {
            return repository.GetMany(lod => lod.IsLike && lod.CommentId == commentId);
        }

        IEnumerable<ApplicationUser> ILikeOrDislikeCommentService.GetUsers(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id));
        }

        public string GetRoleName(int userId)
        {
            var s = roleRepository.GetManyWithInclude(r => r.Users.Select(ur => ur.UserId).Contains(userId), r => r.Users).FirstOrDefault().Name;
            return s;
        }

        public Comment GetComment(int id)
        {
            return commentRepository.GetComment(id);
        }
    }
}

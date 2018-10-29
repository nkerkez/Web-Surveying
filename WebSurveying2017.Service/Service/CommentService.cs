using System;
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
    public interface ICommentService : IService<Comment>
    {
        IEnumerable<Comment> GetCommentsDateAsc(int surveyId, int page, int size);
        IEnumerable<LikeOrDislikeComment> GetLikesAndDislikesForComment(int id);
        int GetRootCount(int surveyId);
        void Create(Comment comment, int userId);
        int SubCommentsCount(int parentId);
        void DeleteComment(Comment comment);
        ApplicationUser GetAuthor(int userId);
        Tuple<IEnumerable<Comment>, int> GetForUser(int userId, int page, int size);
        
    }
    public class CommentService : ServiceBase<Comment>, ICommentService
    {
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;
        private readonly ILikeOrDislikeCommentRepository likeOrDislikeCommentRepository;

        public CommentService(ICommentRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, ISurveyRepository surveyRepository,
            IUserNotificationRepository userNotificationRepository, IUserCategoryRepository userCategoryRepository,
            INotificationRepository notificationRepository, ILikeOrDislikeCommentRepository likeOrDislikeCommentRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userNotificationRepository = userNotificationRepository;
            this.surveyRepository = surveyRepository;
            this.userRepository = userRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.notificationRepository = notificationRepository;
            this.likeOrDislikeCommentRepository = likeOrDislikeCommentRepository;
        }

        public IEnumerable<LikeOrDislikeComment> GetLikesAndDislikesForComment(int id)
        {
            return likeOrDislikeCommentRepository.GetManyAsNoTracking(lod => lod.CommentId == id);
        }

        void ICommentService.Create(Comment comment, int userId)
        {
            
            var user = userRepository.GetById(userId);
            comment.CreationDate = DateTime.Now;
            comment.LastEdited = DateTime.Now;

            
            comment.UserId = userId;

            //VALIDACIJA !!!
            
            List<UserNotification> list = new List<UserNotification>();
            //Za autora ankete i autora komentara
            Notification notification = new Notification(user.FirstName + " " + user.LastName + " je prokomentarisao/la Vašu anketu.", DateTime.Now, Operation.ADD, NotificationType.PERSONAL);
            notification.UserId = comment.UserId;
            notification.SurveyId = comment.SurveyId;
            notification.CommentId = comment.Id;

            Survey survey = surveyRepository.GetByIdAsNoTracking(comment.SurveyId);
            if (survey.UserId != userId)
            {
                UserNotification forSurveyAuthor = new UserNotification(survey.UserId, notification, false);
                list.Add(forSurveyAuthor);
            }
            if(comment.ParentId != null)
            {
                
                Comment parent = repository.GetById((int)comment.ParentId);
                if (parent.UserId == survey.UserId)
                {
                    
                }
                else
                {
                    Notification notificationForCommentAuthor = new Notification(user.FirstName + " " + user.LastName + " je odgovorio/la na Vaš komentar.",
                        DateTime.Now, Operation.ADD, NotificationType.PERSONAL);
                    notification.UserId = comment.UserId;
                    notification.SurveyId = comment.SurveyId;
                    notification.CommentId = comment.Id;
                    UserNotification forCommentAuthor = new UserNotification(parent.UserId, notification, false);
                    list.Add(forCommentAuthor);
                }
            }

            base.Create(comment);
            userNotificationRepository.AddUserNotifications(list);
        }

        IEnumerable<Comment> ICommentService.GetCommentsDateAsc(int surveyId, int page, int size)
        {
            return repository.GetCommentsDateDesc(surveyId, page, size);
        }

        public override void Update(Comment entity)
        {

            // entity.CommentState = State.WAITING_PERMISION_FOR_UPDATE;
            //notification for moderators
            Comment old = repository.GetComment(entity.Id);
            var sub = repository.GetManyAsNoTracking(c => c.ParentId == entity.Id).ToList();
            if (old.LikesOrDislikes.Count() > 0 || sub.Count() > 0)
                throw new Exception("Komentar se ne može izmeniti. Postoje glasovi ili podkomentari.");
            Survey s = surveyRepository.GetByIdAsNoTracking(entity.SurveyId);
            var user = userRepository.GetById(entity.UserId);

            var ids = userCategoryRepository.GetMany(uc => uc.CategoryId == s.CategoryId).Select(uc => uc.UserId).ToList();
            List<UserNotification> list = new List<UserNotification>();
            Notification n = new Notification(
                       user.FirstName + " " + user.LastName + "je izmenio/la komentar.",
                       DateTime.Now, Operation.UPDATE, NotificationType.MODERATOR);

            n.UserId = user.Id;
            n.SurveyId = s.Id;
            n.CommentId = entity.Id;

            if (ids.Count() > 0)
            {
                ids.ForEach(modId => {
                    UserNotification un = new UserNotification(modId, n, false);
                    list.Add(un);
                });
                
                
            }
            else
            {
                var admins = userRepository.GetForRoleName("Admin").ToList();
                admins.ForEach(admin => {
                    UserNotification un = new UserNotification(admin.Id, n, false);
                    list.Add(un);
                });
                
            }

            
            base.Update(old, entity);
            userNotificationRepository.AddUserNotifications(list);
            

        }
        
        int ICommentService.SubCommentsCount(int parentId)
        {
            return repository.GetMany(c => c.ParentId == parentId).Count();
        }

        public ApplicationUser GetAuthor(int userId)
        {
            return userRepository.GetById(userId);
        }

        public Tuple<IEnumerable<Comment>, int> GetForUser(int userId, int page, int size)
        {
            return repository.GetCommentsForUser(userId, page, size);
        }

        public int GetRootCount(int surveyId)
        {
            return repository.GetRootCount(surveyId);
        }

        public void DeleteComment(Comment comment)
        {
            repository.DeleteComment(comment);
        }
    }
}

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
    public interface IRequestGroupService : IService<RequestGroup>
    {
        Group GetGroup(int id);
        IEnumerable<ApplicationUser> GetUsers(List<int> ids);
        void RespondOnRequest(RequestGroup request, Group group, bool accepted);
    }
    public class RequestGroupService : ServiceBase<RequestGroup>, IRequestGroupService
    {
        private readonly IRequestGroupRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly IUserGroupRepository userGroupRepository;

        public RequestGroupService(IRequestGroupRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, IGroupRepository groupRepository,
            INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository,
            IUserGroupRepository userGroupRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
            this.notificationRepository = notificationRepository;
            this.userNotificationRepository = userNotificationRepository;
            this.userGroupRepository = userGroupRepository;
        }

        public void RespondOnRequest(RequestGroup request, Group group, bool accepted)
        {

            var user = userRepository.GetById(request.UserId);
            //delete request
            repository.Delete(r => r.GroupId == request.GroupId && r.UserId == request.UserId);
            //delete notification
            notificationRepository.Delete(n => n.GroupId == group.Id || n.UserId == request.UserId);
            if (accepted)
            {
                var author = userRepository.GetById(group.UserId);
                //create new notification
                Notification notification = new Notification()
                {
                    DateTime = DateTime.Now,
                    Message = author.FirstName + " " + author.LastName +
                    " je prihvatio Vaš zahtev. Postali ste član grupe  " + group.Name,
                    GroupId = group.Id,
                    UserId = group.UserId,
                    Operation = Operation.ACCEPTED,
                    NotificaionType = NotificationType.PERSONAL

                };

                Notification notificationForAuthor = new Notification()
                {
                    DateTime = DateTime.Now,
                    Message = user.FirstName + " " + user.LastName +
                     " je postao član Vaše grupe " + group.Name,
                    GroupId = group.Id,
                    UserId = user.Id,
                    Operation = Operation.ACCEPTED,
                    NotificaionType = NotificationType.PERSONAL

                };
                
                userNotificationRepository.AddUserNotifications(new List<UserNotification>(
                    new UserNotification[] {
                        new UserNotification(request.UserId, notification, false),
                        new UserNotification(group.UserId, notificationForAuthor, true)

                    })
                 
                );
                
                userGroupRepository.Add(new UserGroup() { GroupId = group.Id, UserId = request.UserId });
            }
            
            
            
            
        }
        public override void Create(RequestGroup entity)
        {
            var group = groupRepository.GetById(entity.GroupId);
            var user = userRepository.GetById(entity.UserId);
            Notification n = new Notification() {
                DateTime = DateTime.Now,
                GroupId = group.Id,
                NotificaionType = NotificationType.PERSONAL,
                UserId = entity.UserId,
                Operation = Operation.REQUEST,
                Message = user.FirstName + " " + user.LastName + " je poslao zahtev za pristup grupi " + group.Name,
                

            };
            var userNotification = new UserNotification(group.UserId, n, false);
            userNotificationRepository.Add(userNotification);

            base.Create(entity);
        }

        public Group GetGroup(int id)
        {
            return groupRepository.GetById(id);
        }

        public IEnumerable<ApplicationUser> GetUsers(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id));
        }


    }
}

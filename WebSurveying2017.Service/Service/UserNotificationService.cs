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
    public interface IUserNotificationService : IService<UserNotification>
    {
        void AddUserNotifications(List<UserNotification> notifications);

        List<Notification> GetAllNotificationsForUser(int userId, NotificationType type);
    }
    public class UserNotificationService : ServiceBase<UserNotification>, IUserNotificationService
    {
        private readonly IUserNotificationRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public UserNotificationService(IUserNotificationRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        void IUserNotificationService.AddUserNotifications(List<UserNotification> notifications)
        {
            repository.AddUserNotifications(notifications);
        }

        List<Notification> IUserNotificationService.GetAllNotificationsForUser(int userId, NotificationType type)
        {
            return repository.GetAllNotificationsForUser(userId, type).ToList();
        }
    }
}

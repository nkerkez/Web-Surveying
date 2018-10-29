using System.Collections.Generic;
using System.Linq;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        void AddUserNotifications(List<UserNotification> notifications);
        int GetUnreadedCountForUser(int userId);
        IEnumerable<Notification> GetAllNotificationsForUser(int userId, NotificationType type);
    }

    public class UserNotificationRepository : RepositoryBase<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void AddUserNotifications(List<UserNotification> notifications)
        {
            
            base.dbSet.AddRange(notifications);
        }

        public IEnumerable<Notification> GetAllNotificationsForUser(int userId, NotificationType type)
        {
            if(type == NotificationType.ALL)
                return base.dbSet.Where(un => un.UserId == userId && !un.IsRead).Select(n => n.Notification).OrderByDescending(n => n.DateTime);

            return base.dbSet.Where(un => un.UserId == userId && !un.IsRead).Select(n => n.Notification).Where(n => n.NotificaionType == type).OrderByDescending(n => n.DateTime);
        }


        public int GetUnreadedCountForUser(int userId)
        {
            return dbSet.Where(un => un.UserId == userId && !un.IsRead).Count();
        }
    }
}

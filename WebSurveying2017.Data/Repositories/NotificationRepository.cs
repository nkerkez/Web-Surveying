using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
    }

    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

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
    public interface INotificationService : IService<Notification>
    {

    }
    public class NotificationService : ServiceBase<Notification>, INotificationService
    {
        private readonly INotificationRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public NotificationService(INotificationRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
    }
}

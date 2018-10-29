using System;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/notifications")]
    public class NotificationsController : ApiController
    {
        public INotificationService notificationService;
        public IUnitOfWork unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            this.unitOfWork = unitOfWork;
            this.notificationService = notificationService;
        }


    }
}
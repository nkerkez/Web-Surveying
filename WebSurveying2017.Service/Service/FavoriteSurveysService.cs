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
    public interface IFavoriteSurveysService : IService<FavoriteSurveys>
    {

        void Delete(int surveyId, int userId);
    }

    public class FavoriteSurveysService : ServiceBase<FavoriteSurveys>, IFavoriteSurveysService
    {
        private readonly IFavoriteSurveysRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly ISurveyRepository surveyRepository;

        public FavoriteSurveysService(IFavoriteSurveysRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, IUserNotificationRepository userNotificationRepository,
            ISurveyRepository surveyRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.surveyRepository = surveyRepository;
            this.userNotificationRepository = userNotificationRepository;

        }

        public override void Create(FavoriteSurveys entity)
        {
            var user = userRepository.GetById(entity.UserId);
            Notification n = new Notification(user.FirstName + " " + user.LastName + 
                " se sviđa vasa anketa.", DateTime.Now, Operation.LIKEORDISLIKE, NotificationType.PERSONAL);
            n.SurveyId = entity.SurveyId;
            n.UserId = entity.UserId;

            var survey = surveyRepository.GetByIdAsNoTracking(entity.SurveyId);
            userNotificationRepository.Add(new UserNotification(survey.UserId, n, false));
            base.Create(entity);
        }

        public void Delete(int surveyId, int userId)
        {
            repository.Delete(fs => fs.SurveyId == surveyId && fs.UserId == userId);
        }
        
    }
}

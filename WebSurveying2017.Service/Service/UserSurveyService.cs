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
    public interface IUserSurveyService : IService<UserSurvey>
    {
        IEnumerable<UserSurvey> GetForSurvey(int surveyId);
        IEnumerable<ApplicationUser> GetUsers(List<int> ids);
        Survey GetSurvey(int surveyId);
    }


    public class UserSurveyService : ServiceBase<UserSurvey>, IUserSurveyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserSurveyRepository repository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserRepository userRepository;

        public UserSurveyService(IUserSurveyRepository repository, IUnitOfWork unitOfWork,
                ISurveyRepository surveyRepository, IUserRepository userRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.surveyRepository = surveyRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<UserSurvey> GetForSurvey(int surveyId)
        {
            return repository.GetSurveyUsers(surveyId).ToList();
        }

        public Survey GetSurvey(int surveyId)
        {
            return surveyRepository.GetByIdAsNoTracking(surveyId);
        }

        public IEnumerable<ApplicationUser> GetUsers(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id)).ToList();
        }
    }
}

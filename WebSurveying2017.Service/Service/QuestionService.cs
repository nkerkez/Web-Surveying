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
    public interface IQuestionService : IService<Question>
    {
        Question GetQuestion(int id);
        IEnumerable<UserSurvey> GetForSurvey(int surveyId);
        IEnumerable<ApplicationUser> GetUsers(List<int> ids);
        Survey GetSurvey(int surveyId);
    }
    public class QuestionService : ServiceBase<Question>, IQuestionService
    {
        private readonly IQuestionRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserSurveyRepository userSurveyRepository;
        private readonly IUserRepository userRepository;

        public QuestionService(IQuestionRepository repository, IUnitOfWork unitOfWork,
            ISurveyRepository surveyRepository, IUserRepository userRepository,
            IUserSurveyRepository userSurveyRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userSurveyRepository = userSurveyRepository;
            this.userRepository = userRepository;
            this.surveyRepository = surveyRepository;
        }

        public Question GetQuestion(int id)
        {
            return repository.GetManyWithInclude(q => q.Id == id, q => q.Survey, q => q.QuestionAnswers, q => q.UserAnswers.Select(ua => ua.Answer)).FirstOrDefault();
        }

        public IEnumerable<UserSurvey> GetForSurvey(int surveyId)
        {
            return userSurveyRepository.GetSurveyUsers(surveyId).ToList();
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

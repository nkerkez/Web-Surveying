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
    public interface IExcelFilesService : IService<ExcelFiles>
    {
        Survey GetWithResult(int id);
        List<ApplicationUser> UsersWhoFilledSurvey(List<int> ids);
        IEnumerable<ExcelFiles> GetFiles(int surveyId);
        
    }
    public class ExcelFilesService : ServiceBase<ExcelFiles>, IExcelFilesService
    {
        private readonly IExcelFilesRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserRepository userRepository;

        public ExcelFilesService(IExcelFilesRepository repository, IUnitOfWork unitOfWork,
            ISurveyRepository surveyRepository, IUserRepository userRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.surveyRepository = surveyRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<ExcelFiles> GetFiles(int surveyId)
        {
            return repository.GetManyWithInclude(ef => ef.SurveyId == surveyId, ef => ef.Survey).OrderByDescending(ef => ef.CreationDate);
        }

        public Survey GetWithResult(int id)
        {
            return surveyRepository.GetWithResult(id);
        }
        public List<ApplicationUser> UsersWhoFilledSurvey(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id)).ToList();
        }
        
    }

}
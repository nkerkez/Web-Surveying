using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Data.Repositories;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Infrastructure;

namespace WebSurveyingS2017.Service.Service
{
    public interface ICategoryService : IService<Category>
    {
        IEnumerable<Category> GetRoots();
        List<int> GetAllSub(int id);
        void DeleteCategory(int id);
        
    }
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        private readonly ICategoryRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurveyRepository surveyRepository;

        public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork, ISurveyRepository surveyRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.surveyRepository = surveyRepository;
        }


        IEnumerable<Category> ICategoryService.GetRoots()
        {
            return repository.GetRoots();
        }
        

        public List<int> GetAllSub(int id)
        {
            return repository.GetAllSub(id, new List<int>()).ToList();
        }

        public void DeleteCategory(int id)
        {
            var subcategories = repository.GetMany(c => c.CategoryId == id).ToList();
            var surveys = surveyRepository.GetMany(s => s.CategoryId == id).ToList();
            subcategories.ForEach(
                child =>
                {
                    child.CategoryId = null;
                    repository.Update(child);
                }
                );
            surveys.ForEach(
                obj =>
                {
                    var _obj = surveyRepository.GetById(obj.Id);
                    obj.CategoryId = null;
                    surveyRepository.Update(_obj, obj);
                }
                );
            repository.Delete(c => c.Id == id);
        }
    }
}
 
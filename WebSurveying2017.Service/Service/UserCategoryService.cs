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
    public interface IUserCategoryService : IService<UserCategory>
    {
        void AddCategories(IEnumerable<UserCategory> categories);
        List<UserCategory> GetForModerator(int userId);
        List<UserCategory> GetForCategory(int categoryId);
    }
    public class UserCategoryService : ServiceBase<UserCategory>, IUserCategoryService
    {
        private readonly IUserCategoryRepository repository;
        private readonly IUnitOfWork unitOfWork;
        public UserCategoryService(IUserCategoryRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        void IUserCategoryService.AddCategories(IEnumerable<UserCategory> categories)
        {
            repository.AddCategories(categories);
        }

        List<UserCategory> IUserCategoryService.GetForCategory(int categoryId)
        {
            return repository.GetMany(uc => uc.CategoryId == categoryId).ToList();
        }

        List<UserCategory> IUserCategoryService.GetForModerator(int userId)
        {
            return repository.GetMany(uc => uc.UserId == userId).ToList();
        }
    }
}

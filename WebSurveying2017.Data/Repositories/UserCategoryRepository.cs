using System.Collections.Generic;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserCategoryRepository : IRepository<UserCategory>
    {
        void AddCategories(IEnumerable<UserCategory> userCategories);
    }

    public class UserCategoryRepository : RepositoryBase<UserCategory>, IUserCategoryRepository
    {
        public UserCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        public void AddCategories(IEnumerable<UserCategory> userCategories)
        {

            base.dbSet.AddRange(userCategories);
        }
    }
}

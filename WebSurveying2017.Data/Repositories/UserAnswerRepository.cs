using System.Collections.Generic;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserAnswerRepository : IRepository<UserAnswer>
    {
        void AddAnswers(IEnumerable<UserAnswer> userAnswers);
       
    }

    public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void AddAnswers(IEnumerable<UserAnswer> userAnswers)
        {
            base.dbSet.AddRange(userAnswers);
        }

      
    }
}

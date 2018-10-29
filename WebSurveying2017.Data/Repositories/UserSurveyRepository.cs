using System;
using System.Linq;
using System.Collections.Generic;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using System.Data.Entity;

namespace WebSurveying2017.Data.Repositories
{

    public interface IUserSurveyRepository : IRepository<UserSurvey>
    {
        IEnumerable<int> GetUsers(int id);
        IEnumerable<UserSurvey> GetSurveyUsers(int id);
    }

    public class UserSurveyRepository : RepositoryBase<UserSurvey>, IUserSurveyRepository
    {
        public UserSurveyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<UserSurvey> GetSurveyUsers(int id)
        {
            return base.dbSet.Where(us => us.SurveyId == id);
        }

        public IEnumerable<int> GetUsers(int id)
        {
            return base.dbSet.Where(us => us.SurveyId == id).Select(us => us.UserId);
        }
    }

    
}

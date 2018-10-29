using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IFavoriteSurveysRepository : IRepository<FavoriteSurveys>
    {
    }

    public class FavoriteSurveysRepository : RepositoryBase<FavoriteSurveys>, IFavoriteSurveysRepository
    {
        public FavoriteSurveysRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

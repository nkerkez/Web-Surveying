using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IExcelFilesRepository : IRepository<ExcelFiles>
    {
    }

    public class ExcelFilesRepository : RepositoryBase<ExcelFiles>, IExcelFilesRepository
    {
        public ExcelFilesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

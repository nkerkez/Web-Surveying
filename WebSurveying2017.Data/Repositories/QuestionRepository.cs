using System.Data.Entity;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using System.Linq;

namespace WebSurveying2017.Data.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public override Question GetById(int id)
        {

            DbSet<Question> qDbSet = base.dbSet;
            return qDbSet.AsNoTracking().Include(q => q.QuestionAnswers.Select( qa => qa.Users)).SingleOrDefault(quest => quest.Id == id);

        }
    }
}

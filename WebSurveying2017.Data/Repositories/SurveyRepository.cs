using RefactorThis.GraphDiff;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using Z.EntityFramework.Plus;
using System;

namespace WebSurveying2017.Data.Repositories
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        Survey GetWithResult(int id);
        Survey GetByIdAsNoTracking(int id);
        Survey GetWithUserAnswers(int id, int userId);

        int GetSurveysCount(int page, int size, bool? isPublic, int? userId, int? authorId, int? groupId, int? categoryId, string name, string description, string authorFirstName, string authorLastName, string questionText);
        Tuple<IEnumerable<Survey>, int> GetActiveSurveys(int page, int size, bool? isPublic, int? filledUserId, int? authorId, int? favoriteUserId, int? groupUserId, int? groupId, string name, string description, string authorFirstName, string authorLastName, string questionText, List<int> categories, int state);

    }

    public class SurveyRepository : RepositoryBase<Survey>, ISurveyRepository
    {
        public SurveyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override IEnumerable<Survey> GetAll()
        {

            return base.dbSet.Include(s => s.Author).Include(s => s.Category);
        }

        public override Survey GetById(int id)
        {

            return base.dbSet.Include(s => s.Author)
                .Include(s => s.Category)
                .Include(s => s.Questions.Select(q => q.QuestionAnswers))
                .SingleOrDefault(s => s.Id == id);
        }

        public Survey GetWithUserAnswers(int id, int userId)
        {
            return base.dbSet.IncludeFilter(s => s.Users).IncludeFilter(s => s.Category).IncludeFilter(s => s.Author).IncludeFilter(s => s.Questions).IncludeFilter(s => s.Questions.Select(q => q.QuestionAnswers)).IncludeFilter(s => s.Questions.Select(q => q.UserAnswers.Where(ua => ua.UserId == userId))).IncludeFilter(s => s.Questions.Select(q => q.UserAnswers.Select(ua => ua.Answer))).SingleOrDefault(s => s.Id == id);
        }

        public Survey GetByIdAsNoTracking(int id)
        {
            return base.dbSet.AsNoTracking().Include(s => s.FavoriteSurveys).Include(s => s.Groups).Include(s => s.Users).Include(s => s.Comments).Include(s => s.Author).Include(s => s.Category).Include(s => s.Questions.Select(q => q.QuestionAnswers)).SingleOrDefault(s => s.Id == id);

        }

        public Survey GetWithResult(int id)
        {
            return base.dbSet.IncludeFilter(s => s.Users)
               .IncludeFilter(s => s.Questions.OrderBy(q => q.OrdinalNumber))
               .IncludeFilter(s => s.Questions.Select(q => q.QuestionAnswers))
               .IncludeFilter(s => s.Questions.Select(q => q.QuestionAnswers.Select(qa => qa.Users.Where(u => s.Users.Any(us => us.UserId == u.UserId)))))
               .SingleOrDefault(s => s.Id == id);
        }

        public override void Update(Survey entity)
        {

            if (entity.Questions != null)
            {
                entity.Questions.ToList().ForEach(x => { x.SurveyId = x.Id == 0 ? 0 : entity.Id; });
            }
            base._context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Questions, with =>
          with.OwnedCollection(q => q.QuestionAnswers)).OwnedCollection(p => p.Groups));

        }

        public Tuple<IEnumerable<Survey>, int> GetActiveSurveys(int page, int size, bool? isPublic, int? filledUserId, int? authorId, int? favoriteUserId, int? groupUserId, int? groupId,
            string name, string description, string authorFirstName, string authorLastName, string questionText, List<int> categories, int state)
        {
            List<int> users = new List<int>();
            List<int> groups = new List<int>();
            bool hasCategories = categories.Count() > 0 ? true : false;

            bool withoutCats = false;
            if (categories.Count() == 1 && categories.First() == 0)
                withoutCats = true;

            if (authorFirstName != null || authorLastName != null)
                users = base._context.Users
                        .Where(u => authorFirstName == null || u.FirstName.Contains(authorFirstName))
                        .Where(u => authorLastName == null || u.LastName.Contains(authorLastName)).Select(u => u.Id).ToList();

            if(groupUserId != null)
                groups = base._context.UsersGroups.Where(ug => ug.UserId == groupUserId).Select(ug => ug.GroupId).ToList();



            var surveys = dbSet.AsNoTracking().Include(s => s.Author).Include(s => s.Category)
               .Include(s => s.Comments).Include(s => s.FavoriteSurveys)
               .Include(s => s.Questions).Include(s => s.Groups)
               .Include(s => s.Users).Include(s => s.FavoriteSurveys)
               .Where(s => ( state !=1 && state != 2) || (s.State == State.OPENED && state == 1) || ((s.State == State.CLOSED && state == 2)))
               .Where(s => favoriteUserId == null || s.FavoriteSurveys.Any(fs => fs.UserId == favoriteUserId))
               .Where(s => groupId == null || s.Groups.Any(g => g.GroupId == groupId))
               .Where(s => filledUserId == null || s.Users.Any(g => g.UserId == filledUserId))
               .Where(s => authorId == null || s.UserId == authorId)
               .Where(s => isPublic == null || s.Public)
               .Where(s => s.Name.Contains(name) || name == null)
               .Where(s => s.Description.Contains(description) || description == null)
               .Where(s => s.Questions.Any(q => q.QuestionText.Contains(questionText)) || questionText == null)
               .Where(s => (authorFirstName == null && authorLastName == null) || users.Contains(s.UserId))
               .Where(s => isPublic == null || (groupUserId == null || s.Groups.Any(g => groups.Contains(g.GroupId)) || s.Public == isPublic))
               .Where(s => (!hasCategories || categories.Any(c => c == s.CategoryId)) || (withoutCats && s.CategoryId == null));
            int count = surveys.Count();


            surveys = surveys.OrderByDescending(s => s.CreationDate).Skip((page - 1) * size).Take(size);


            var retVal = new Tuple<IEnumerable<Survey>, int>(surveys, count);


            return retVal;
        }

        public int GetSurveysCount(int page, int size, bool? isPublic, int? userId, int? authorId, int? groupId, int? categoryId, string name, string description, string authorFirstName, string authorLastName, string questionText)
        {
            var surveys = dbSet.Include(s => s.Author).Include(s => s.Category)
               .Include(s => s.Comments).Include(s => s.FavoriteSurveys)
               .Include(s => s.Questions).Include(s => s.Groups)
               .Include(s => s.Users)
               .Where(s => s.CategoryId == categoryId || categoryId == null)
               .Where(s => s.Public || isPublic == null)
               .Where(s => groupId == null || s.Groups.Any(g => g.GroupId == groupId))
               .Where(s => userId == null || s.Users.Any(g => g.UserId == userId))
               .Where(s => authorId == null || s.UserId == authorId)
               .Where(s => s.Name.Contains(name) || name == null)
               .Where(s => s.Description.Contains(description) || description == null)
               .Where(s => s.Questions.Any(q => q.QuestionText.Contains(questionText)) || questionText == null);

            if (authorLastName != null || authorFirstName != null)
            {
                var users = base._context.Users
                    .Where(u => authorFirstName == null || u.FirstName.Contains(authorFirstName))
                    .Where(u => authorLastName == null || u.LastName.Contains(authorLastName));

                surveys = surveys.Where(s => users.Any(u => u.Id == s.UserId));

            }


            return surveys.Count();
        }

        
    }
}

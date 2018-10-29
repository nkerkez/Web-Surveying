using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using Z.EntityFramework.Plus;

namespace WebSurveying2017.Data.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetCommentsDateDesc(int surveyId, int page, int size);
        Comment GetComment(int id);
        Tuple<IEnumerable<Comment>, int> GetCommentsForUser(int userId, int page, int size);
        int GetRootCount(int surveyId);
        void DeleteComment(Comment comment);
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        IEnumerable<Comment> ICommentRepository.GetCommentsDateDesc(int surveyId, int page, int size)
        {

            IEnumerable<Comment> roots = dbSet
                .Include(c => c.LikesOrDislikes)
                .Where(c => c.ParentComment == null && c.SurveyId == surveyId)
                .OrderByDescending(c => c.CreationDate).Skip((page - 1) * size).Take(size);

            foreach (var comment in roots)
            {
                this.PopulateChildren(comment);

            }

            return roots;
        }
        public Tuple<IEnumerable<Comment>, int> GetCommentsForUser(int userId, int page, int size)
        {
            var count = base.dbSet.AsNoTracking()
                .Where(c => c.UserId == userId).Count();
            var items = base.dbSet.AsNoTracking().Include(c => c.LikesOrDislikes).Include(c => c.Survey)
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.CreationDate)
                .Skip((page - 1) * size)
                .Take(size);

            return new Tuple<IEnumerable<Comment>, int>(items, count);
        }
        private void PopulateChildren(Comment parent)
        {
            parent.SubComments = dbSet.AsNoTracking().Include(c => c.LikesOrDislikes)
                .Where(c => c.ParentId == parent.Id).OrderByDescending(c => c.CreationDate).ToList();
            foreach (var sub in parent.SubComments)
                this.PopulateChildren(sub);
        }


        public override Comment GetById(int id)
        {
            Comment comment = base.dbSet.AsNoTracking().Include(c => c.LikesOrDislikes)

                .Where(c => c.Id == id).SingleOrDefault();





            this.PopulateChildren(comment);
            var rootComment = this.PopulateParent(comment);

            return rootComment;


        }

        private Comment PopulateParent(Comment c)
        {

            if (c.ParentId != null)
            {
                Comment parent = base.dbSet.AsNoTracking().Include(p => p.LikesOrDislikes)
                .Where(p => p.Id == c.ParentId).FirstOrDefault();
                parent.SubComments = new List<Comment>();
                parent.SubComments.Add(c);
                Comment retVal = this.PopulateParent(parent);
                if (retVal != null)
                    return retVal;
            }
            else
            {

                return c;
            }

            return null;

        }


        Comment ICommentRepository.GetComment(int id)
        {
            return base.dbSet.Include(c => c.LikesOrDislikes).SingleOrDefault(c => c.Id == id);
        }

        public int GetRootCount(int surveyId)
        {
            return dbSet.Where(c => c.SurveyId == surveyId && c.ParentId == null).Count();
        }

        public void DeleteComment(Comment comment)
        {
            comment.SubComments.ToList().ForEach(
                c =>
                {
                    this.DeleteComment(c);
                });
            base.Attach(comment);
            base.Delete(comment);
        }
    }
}

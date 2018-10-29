using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
namespace WebSurveying2017.Data.Repositories
{
    public interface ILikeOrDislikeCommentRepository : IRepository<LikeOrDislikeComment>
    {
        
    }

    public class LikeOrDislikeCommentRepository : RepositoryBase<LikeOrDislikeComment>, ILikeOrDislikeCommentRepository
    {
        public LikeOrDislikeCommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override void Update(LikeOrDislikeComment entity)
        {
            var a = dbSet.Local;
            LikeOrDislikeComment local = dbSet.Local.SingleOrDefault(x => x.UserId == entity.UserId && x.CommentId == entity.CommentId);
            if (local == null)
            {
                dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Unchanged;
            }
            else
            {
                _context.Entry(local).State = EntityState.Unchanged;
            }
            _context.Entry(entity).State = EntityState.Modified;




        }

        
    }

    
}

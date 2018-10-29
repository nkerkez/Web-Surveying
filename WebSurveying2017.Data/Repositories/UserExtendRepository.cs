using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using System.Data.Entity;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using Z.EntityFramework.Plus;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserExtendRepository : IRepository<UserExtend>
    {
        IEnumerable<UserExtend> GetUE(int groupId);
    }

    public class UserExtendRepository : RepositoryBase<UserExtend>, IUserExtendRepository
    {
        public UserExtendRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override UserExtend GetById(int id)
        {
            return base.dbSet.AsNoTracking()
                .Include(u => u.ApplicationUser)
                .Where(u => u.UserId == id)
                .SingleOrDefault();

        }

        public IEnumerable<UserExtend> GetUE(int groupId)
        {
            return base.dbSet.IncludeFilter(ue => ue.Groups.Where(g => g.GroupId == groupId));
        }
    }
}

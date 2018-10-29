using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        void AddGroups(IEnumerable<UserGroup> Groups);
    }

    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void AddGroups(IEnumerable<UserGroup> Groups)
        {
            base.dbSet.AddRange(Groups);
        }
        
    }
}

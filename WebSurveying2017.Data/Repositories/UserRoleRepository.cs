using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data.Repositories
{
    public interface IUserRoleRepository : IRepository<MyUserRole>
    {
    }

    public class UserRoleRepository : RepositoryBase<MyUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


    }
}

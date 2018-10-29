using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using System;

namespace WebSurveying2017.Data.Repositories
{

    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetForRoleName(string roleName);
        IEnumerable<ApplicationUser> GetForRoleName(List<string> roleName);
        Tuple<IEnumerable<ApplicationUser>, int> SearchUsers(string firstName, string lastName, string city, DateTime? birthdayFrom, DateTime? birthdayTo, List<Gender> gender, List<string> roleName, int page, int size);
    }

    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }


        public override ApplicationUser GetById(int id)
        {

            return base.dbSet.Include(user => user.Roles).Where(user => user.Id == id).FirstOrDefault();

        }
        public override IEnumerable<ApplicationUser> GetAll()
        {
            return base.GetAll();
        }

        public IEnumerable<ApplicationUser> GetForRoleName(string roleName)
        {
            var _role = base._context.Roles.Where(role => role.Name.Equals(roleName)).FirstOrDefault();

            return dbSet.Include(user => user.Roles).Where(user => user.Roles.Any(r => r.RoleId == _role.Id));
        }

        public IEnumerable<ApplicationUser> GetForRoleName(List<string> roleName)
        {
            var _role = base._context.Roles.Where(role => roleName.Contains(role.Name));

            return dbSet.Include(user => user.Roles).Where(user => user.Roles.Any(r => _role.Select(_r => _r.Id).Contains(r.RoleId)));
        }

        public Tuple<IEnumerable<ApplicationUser>, int> SearchUsers(string firstName, string lastName, string city, DateTime? birthdayFrom, DateTime? birthdayTo, List<Gender> gender, List<string> roleName, int page, int size)
        {
            

            var result = dbSet.Include(u => u.Roles)
                .Where(u => firstName == null || u.FirstName.Contains(firstName))
                .Where(u => lastName == null || u.LastName.Contains(lastName))
                .Where(u => city == null || u.City.Contains(city))
                .Where(u => birthdayTo == null || u.Birthday <= birthdayTo)
                .Where(u => birthdayFrom == null || u.Birthday >= birthdayFrom);
            if ( gender != null)
                result = result.Where(u => gender.Any(g => g == u.Gender));
            if( roleName != null)
            {
                var _role = base._context.Roles.Where(role => roleName.Contains(role.Name));
                result = result.Where(u => u.Roles.Any(r => _role.Select(_r => _r.Id).Contains(r.RoleId)));

            }
            int count = result.Count();
            result = result.OrderBy(u => u.FirstName).Skip((page - 1) * size).Take(size);
            return new Tuple<IEnumerable<ApplicationUser>, int>(result, count );

        }

        public override IEnumerable<ApplicationUser> GetForPage(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                size = 1;
                page = 10;
            }

            return dbSet.OrderBy(x => x.FirstName).Skip((page - 1) * size).Take(size);
        }
    }
}

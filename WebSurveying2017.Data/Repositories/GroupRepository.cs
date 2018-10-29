using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using System.Data.Entity;
using Z.EntityFramework.Plus;

namespace WebSurveying2017.Data.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<Group> GetForAuthor(int userId);
        IEnumerable<Group> GetForMember(int userId);
        IEnumerable<Group> GetForAuthorAndMember(int userId);
        Tuple<IEnumerable<Group>, int> GetForAuthor(int userId,int page, int size, string name, string firstName, string lastName);
        Tuple<IEnumerable<Group>, int> GetAllForPage(int page, int size, string name, string firstName, string lastName);
        Tuple<IEnumerable<Group>, int> GetForMember(int userId, int page, int size, string name, string firstName, string lastName);
        
    }

    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        
        public GroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

     
        public IEnumerable<Group> GetForAuthor(int userId)
        {
            return base.dbSet.Where(g => g.UserId == userId);
        }

        public override IEnumerable<Group> GetAll()
        {

            return dbSet.Include(g => g.User).Include(g => g.Surveys).Include(g => g.GroupMembers);
        }

        public Tuple<IEnumerable<Group>, int> GetForAuthor(int userId, int page, int size, string name, string firstName, string lastName)
        {
            
            var groups = base.dbSet
                .Include(g => g.Surveys)
                .Include(g => g.GroupMembers)
                .Include(g => g.Requests)
                .Where(g => g.UserId == userId)
                .Where(g => name == null || g.Name.Contains(name))
                .OrderBy(g => g.Name)
                .Skip((page-1)*size)
                .Take(size);
            var count = base.dbSet
                .Where(g => g.UserId == userId)
                .Where(g => name == null || g.Name.Contains(name)).Count();
            return new Tuple<IEnumerable<Group>, int>(groups, count);


        }

        public Tuple<IEnumerable<Group>, int> GetAllForPage(int page, int size, string name, string firstName, string lastName)
        {
            var users = _context.Users
                .Where(u => firstName == null || u.FirstName.Contains(firstName))
                .Where(u => lastName == null || u.LastName.Contains(lastName)).Select(u => u.Id);
            var count = base.dbSet
                .Where(g => name == null || g.Name.Contains(name))
                .Where(g => users.Contains(g.UserId))
                .Count();
            var groups = base.dbSet.AsNoTracking()
                .Include(g => g.Surveys)
                .Include(g => g.GroupMembers)
                .Include(g => g.Requests)
                .Where(g => name == null || g.Name.Contains(name))
                .Where(g =>  users.Contains(g.UserId))
                .OrderBy(g => g.Name)
                .Skip((page - 1) * size)
                .Take(size);
            return new Tuple<IEnumerable<Group>, int>(groups, count);
        }

        public Tuple<IEnumerable<Group>, int> GetForMember(int userId, int page, int size, string name, string firstName, string lastName)
        {
            var users = _context.Users
                .Where(u => firstName == null || u.FirstName.Contains(firstName))
                .Where(u => lastName == null || u.LastName.Contains(lastName)).Select(u => u.Id);
            var count = base.dbSet
                .Where(g => g.GroupMembers.Any(gm => gm.UserId.Equals(userId)))
                .Where(g => name == null || g.Name.Contains(name))
                .Where(g => users.Contains(g.UserId))
                .Count();
            var groups = base.dbSet
                .Include(g => g.Surveys)
                .Include(g => g.GroupMembers)
                .Include(g => g.Requests)
                .Where(g => g.GroupMembers.Any(gm => gm.UserId.Equals(userId)))
                .Where(g => name == null || g.Name.Contains(name))
                .Where(g =>  users.Contains(g.UserId))
                .OrderBy(g => g.Name)
                .Skip((page - 1) * size)
                .Take(size);
            return new Tuple<IEnumerable<Group>, int>(groups, count);
        }

        public IEnumerable<Group> GetForMember(int userId)
        {
            return base.dbSet.Include(g => g.GroupMembers).Where(g => g.GroupMembers.Any(gm => gm.UserId.Equals(userId)));
        }

        public IEnumerable<Group> GetForAuthorAndMember(int userId)
        {
            return base.dbSet.Include(g => g.GroupMembers)
                .Where(g => g.UserId == userId || g.GroupMembers.Select(gm => gm.UserId).Contains(userId)).Distinct();
        }
    }
}

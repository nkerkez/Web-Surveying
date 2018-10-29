using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Data.Repositories;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Infrastructure;

namespace WebSurveying2017.Service.Service
{
    public interface IGroupService : IService<Group>
    {
        
        Tuple<IEnumerable<Group>, int> GetForAuthor(int userId, int page, int size, string name, string firstName, string lastName);
        Tuple<IEnumerable<Group>, int> GetForMember(int userId, int page, int size, string name, string firstName, string lastName);
        Tuple<IEnumerable<Group>, int> GetAllForPage(int page, int size, string name, string firstName, string lastName);
        IEnumerable<ApplicationUser> GetAuthors(List<int> ids);
        IEnumerable<Group> GetForAuthor(int userId);
        IEnumerable<Group> GetForMember(int userId);
        IEnumerable<Group> GetForAuthorAndMember(int userId);
    }
    public class GroupService : ServiceBase<Group>, IGroupService
    {
        private readonly IGroupRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public GroupService(IGroupRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public Tuple<IEnumerable<Group>, int> GetAllForPage(int page, int size, string name, string firstName, string lastName)
        {
            return repository.GetAllForPage(page, size, name, firstName, lastName);
        }

        public IEnumerable<ApplicationUser> GetAuthors(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id));
        }

        public IEnumerable<Group> GetForAuthor(int userId)
        {
            return repository.GetForAuthor(userId);
        }

        public IEnumerable<Group> GetForAuthorAndMember(int userId )
        {
            return repository.GetForAuthorAndMember(userId);
        }

        public IEnumerable<Group> GetForMember(int userId)
        {
            return repository.GetForMember(userId);
        }

        Tuple<IEnumerable<Group>, int> IGroupService.GetForAuthor(int userId, int page, int size, string name, string firstName, string lastName)
        {
            return repository.GetForAuthor(userId, page, size, name, firstName, lastName);
        }

        Tuple<IEnumerable<Group>, int> IGroupService.GetForMember(int userId, int page, int size, string name, string firstName, string lastName)
        {

            return repository.GetForMember(userId, page, size, name, firstName, lastName);
        }
    }
}

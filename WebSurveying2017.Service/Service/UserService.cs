using System;
using System.Collections;
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
    public interface IUserService : IService<ApplicationUser>
    {
        Group GetGroup(int id);
        string GetRoleName(int userId);
        IEnumerable<ApplicationUser> GetUsers(IEnumerable<int> ids);
        IEnumerable<ApplicationUser> GetForRoleName(string roleName);
        IEnumerable<UserCategory> GetModeratorsForCategory(int catId);
        IEnumerable<ApplicationUser> GetForGroup(int groupId);
        Tuple<IEnumerable<ApplicationUser>, int> SearchUsers(string firstName, string lastName, string city,
           DateTime? birthdayFrom, DateTime? birthdayTo, List<Gender> gender, List<string> roleName , int page, int size);
    }
    public class UserService : ServiceBase<ApplicationUser>, IUserService
    {
        private readonly IUserRepository repository;
        private readonly IUserExtendRepository userExtendRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUserCategoryRepository userCategoryRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGroupRepository groupRepository;

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork,
            IUserExtendRepository userExtendRepository, IUserCategoryRepository userCategoryRepository,
            IRoleRepository roleRepository, IUserGroupRepository userGroupRepository,
            IGroupRepository groupRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userCategoryRepository = userCategoryRepository;
            this.userExtendRepository =  userExtendRepository;
            this.userGroupRepository = userGroupRepository;
            this.roleRepository = roleRepository;
            this.groupRepository = groupRepository;
        }

        IEnumerable<UserCategory> IUserService.GetModeratorsForCategory(int catId)
        {
            return userCategoryRepository.GetMany(uc => uc.CategoryId == catId);
        }

        IEnumerable<ApplicationUser> IUserService.GetForRoleName(string roleName)
        {
            return repository.GetForRoleName(roleName);
        }

        IEnumerable<ApplicationUser> IUserService.GetUsers(IEnumerable<int> ids)
        {
            return repository.GetMany(user => ids.Contains(user.Id));
        }

        Tuple<IEnumerable<ApplicationUser>, int> IUserService.SearchUsers(string firstName, string lastName, string city, DateTime? birthdayFrom, DateTime ? birthdayTo, List<Gender> gender, List<string> roleName, int page, int size)
        {
            
            return repository.SearchUsers(firstName, lastName, city, birthdayFrom, birthdayTo ,gender, roleName, page, size);
        }

        public IEnumerable<ApplicationUser> GetForGroup(int groupId)
        {
            var users = userGroupRepository.GetMany(ug => ug.GroupId == groupId).Select(ug => ug.UserId);

            return repository.GetMany(u => users.Contains(u.Id));
        }

        public string GetRoleName(int userId)
        {
            var s = roleRepository.GetManyWithInclude(r => r.Users.Select(ur => ur.UserId).Contains(userId), r => r.Users).FirstOrDefault().Name;
            return s;
        }

        public Group GetGroup(int id)
        {
            return groupRepository.GetById(id);
        }
    }
}

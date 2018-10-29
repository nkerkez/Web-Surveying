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
    public interface IUserRoleService : IService<MyUserRole>
    {
        void Update(int userId, string roleName);
    }
    public class UserRoleService : ServiceBase<MyUserRole>, IUserRoleService
    {
        private readonly IUserRoleRepository repository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserCategoryRepository userCategoryRepository;

        public UserRoleService(IUserRoleRepository repository, IUnitOfWork unitOfWork,
            IRoleRepository roleRepository, IUserCategoryRepository userCategoryRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.roleRepository = roleRepository;
            this.userCategoryRepository = userCategoryRepository;


        }

        void IUserRoleService.Update(int userId, string roleName)
        {

            var role = roleRepository.GetMany(r => r.Name.Equals(roleName)).FirstOrDefault();
            if (role == null)
                return;
            MyUserRole obj = new MyUserRole() {
                UserId = userId,
                RoleId = role.Id
            };



            repository.Delete(ur => ur.UserId == userId);
            if(!roleName.Equals("Moderator"))
            {
                userCategoryRepository.Delete(uc => uc.UserId == userId);
            }
            repository.Add(obj);
            
            
        }
    }
}

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
    public interface IUserExtendService : IService<UserExtend>
    {
        

        int NotificationCount(int userId);
        string GetRoleName(int userId);
    }


    public class UserExtendService : ServiceBase<UserExtend>, IUserExtendService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserExtendRepository repository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly IRoleRepository roleRepository;



        public UserExtendService(IUserExtendRepository repository, IUnitOfWork unitOfWork,
                IUserNotificationRepository userNotificationRepository, IRoleRepository roleRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userNotificationRepository = userNotificationRepository;
            this.roleRepository = roleRepository;
        }

        public string GetRoleName(int userId)
        {
            return roleRepository.GetManyWithInclude(r => r.Users.Select(ur => ur.UserId).Contains(userId), r => r.Users).FirstOrDefault().Name;
            
        }

        

        int IUserExtendService.NotificationCount(int userId)
        {
            return userNotificationRepository.GetUnreadedCountForUser(userId);
        }
    }
}
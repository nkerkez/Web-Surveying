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
    public interface IUserGroupService : IService<UserGroup>
    {
        string AddUserGroup(List<UserGroup> list, string name, int userId);
        void UpdateMembers(List<UserGroup> members, List<UserGroup> old);
        IEnumerable<UserGroup> GetForUser(int userId);
        IEnumerable<UserGroup> GetForUserAndGroup(int userId, int groupId);
    }
    public class UserGroupService : ServiceBase<UserGroup>, IUserGroupService
    {
        private readonly IUserGroupRepository repository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly IUserRepository userRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserSurveyRepository userSurveyRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserGroupService(IUserGroupRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, IUserNotificationRepository userNotificationRepository,
            ISurveyRepository surveyRepository, IUserSurveyRepository userSurveyRepository,
            IGroupRepository groupRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.groupRepository = groupRepository;
            this.userNotificationRepository = userNotificationRepository;
            this.userRepository = userRepository;
            this.surveyRepository = surveyRepository;
            this.userSurveyRepository = userSurveyRepository;
        }

        public IEnumerable<UserGroup> GetForUserAndGroup(int userId, int groupId)
        {
            return repository.GetManyWithInclude(ug => ug.Group.UserId == userId && ug.GroupId == groupId, ug => ug.Group);
        }

        string IUserGroupService.AddUserGroup(List<UserGroup> list, string name, int userId)
        {

            

            Group group = new Group()
            {
                Name = name,
                UserId = userId
            };

            list.ForEach(
                obj => obj.Group = group    
            );

            if (list.Count() > 0)
                repository.AddGroups(list);
            else
                groupRepository.Add(group);

            return null;




        }

        IEnumerable<UserGroup> IUserGroupService.GetForUser(int userId)
        {
            return repository.GetMany(ug => ug.UserId == userId);
        }

        void IUserGroupService.UpdateMembers(List<UserGroup> members, List<UserGroup> old)
        {
            var forAdd = members.Where(ug => !old.Select(o => o.UserId).Contains(ug.UserId));
            var forDelete = old.Where(o => !members.Select(ug => ug.UserId).Contains(o.UserId));

            forDelete.ToList().ForEach(
                    obj => repository.Delete(ug => ug.GroupId == obj.GroupId && ug.UserId == obj.UserId)
                );


            repository.AddGroups(forAdd);
        }
    }
}

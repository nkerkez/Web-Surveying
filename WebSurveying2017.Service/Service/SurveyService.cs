using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Data.Repositories;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Infrastructure;

namespace WebSurveying2017.Service.Service
{
    public interface ISurveyService : IService<Survey>
    {
        List<int> GetAllSubCategories(int catId);
        Survey GetSurveyIncludeUsersAndQuestions(int id);
        void AddExcel(ExcelFiles obj);
        ExcelFiles GetFile(int id);
        void DeleteSurvey(Survey survey);
        List<int> GetUserGroups(int userId);
        List<ApplicationUser> UsersWhoFilledSurvey(List<int> ids);
        int GetSurveysCount(int page, int size, bool? isPublic, int? userId, int? authorId, int? favoriteUserId, int? groupId, int? categoryId, string name, string description, string authorFirstName, string authorLastName, string questionText);
        Tuple<IEnumerable<Survey>, int> GetActiveSurveys(int page, int size, bool? isPublic, int? fillUserId, int? authorId, int? favoriteUserId, int? groupUserId, int? groupId, string name, string description, string authorFirstName, string authorLastName, string questionText, List<int> categories, int state);

        Survey GetAsNoTracking(int id);
        Survey GetWithUserAnswers(int id, int userId);
        Survey GetWithResult(int id);
        int NumbOfUsers(int questionId);
        IEnumerable<UserAnswer> GetUserAnswerForQuestion(int questionId);
        UserAnswer GetUserAnswerWithAnswerObj(int questionId, int userId);
        void DeleteUserAnswer(UserAnswer ua);
        void DeleteUserAnswer(Expression<Func<UserAnswer, bool>> where);
        void Create(Survey survey, int userId);
        ApplicationUser GetAuthor(int userId);
        IEnumerable<UserAnswer> GetAnswersForUser(int userId);
        int GetGroupAuthor(int groupId);



    }

    public class SurveyService : ServiceBase<Survey>, ISurveyService
    {
        private readonly ISurveyRepository repository;
        private readonly IUserAnswerRepository userAnswerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUserSurveyRepository userSurveyRepository;
        private readonly IExcelFilesRepository excelFilesRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IAnswerRepository answerRepository;
        


        public SurveyService(ISurveyRepository repository, IUnitOfWork unitOfWork,
            IUserAnswerRepository userAnswerRepository, IUserNotificationRepository userNotificationRepository,
            IUserRepository userRepository, INotificationRepository notificationRepository,
            ICategoryRepository categoryRepository, IUserGroupRepository userGroupRepository,
            IUserSurveyRepository userSurveyRepository, IExcelFilesRepository excelFilesRepository,
            IGroupRepository groupRepository, ICommentRepository commentRepository,
            IAnswerRepository answerRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userAnswerRepository = userAnswerRepository;
            this.userNotificationRepository = userNotificationRepository;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
            this.notificationRepository = notificationRepository;
            this.userGroupRepository = userGroupRepository;
            this.userSurveyRepository = userSurveyRepository;
            this.excelFilesRepository = excelFilesRepository;
            this.groupRepository = groupRepository;
            this.commentRepository = commentRepository;
            this.answerRepository = answerRepository;
        }

       

        Survey ISurveyService.GetAsNoTracking(int id)
        {
            return repository.GetByIdAsNoTracking(id);
        }

        Survey ISurveyService.GetSurveyIncludeUsersAndQuestions(int id)
        {
            return repository.GetManyWithInclude(survey => survey.Id == id, survey => survey.Users, survey => survey.Questions).FirstOrDefault();

        }

        UserAnswer ISurveyService.GetUserAnswerWithAnswerObj(int questionId, int userId)
        {
            return userAnswerRepository.GetManyWithInclude(ua => ua.UserId == userId && questionId == ua.QuestionId, ua => ua.Answer).FirstOrDefault();

        }

       

        Survey ISurveyService.GetWithResult(int id)
        {
            return repository.GetWithResult(id);
        }

        Survey ISurveyService.GetWithUserAnswers(int id, int userId)
        {
           
            return repository.GetWithUserAnswers(id, userId);
        }

        int ISurveyService.NumbOfUsers(int questionId)
        {
            return userAnswerRepository
                   .GetMany(ua => ua.QuestionId == questionId).Select(ua => ua.UserId).Distinct().Count();
        }

        IEnumerable<UserAnswer> ISurveyService.GetUserAnswerForQuestion(int questionId)
        {
            return userAnswerRepository
                   .GetManyWithInclude(ua => ua.QuestionId == questionId, ua => ua.Answer);
        }

        void ISurveyService.DeleteUserAnswer(UserAnswer ua)
        {
            userAnswerRepository.Delete(ua);
        }



        void ISurveyService.DeleteUserAnswer(Expression<Func<UserAnswer, bool>> where)
        {
            userAnswerRepository.Delete(where);
        }

        void ISurveyService.Create(Survey survey, int userId)
        {
            survey.UserId = userId;

            
           // survey.State = State.WAITING_PERMISION_FOR_ADD;
            survey.CreationDate = DateTime.Now;

            var user = userRepository.GetById(userId);

            // create Notification for administrator

            Notification notification = new Notification(user.FirstName + " " + user.LastName + " je napravio/la anketu.", DateTime.Now, Operation.ADD, NotificationType.MODERATOR);
            notification.Survey = survey;
            notification.UserId = userId;
            List<UserNotification> notifications = this.CreateNotificationsForAdminsAndModerators(survey.CategoryId, notification, userId);
           



            base.Create(survey);
            userNotificationRepository.AddUserNotifications(notifications);

        }
        

        public ApplicationUser GetAuthor(int userId)
        {
            return userRepository.GetById(userId);
        }

        public int GetSurveysCount(int page, int size, bool? isPublic, int? userId, int? authorId, int? favoriteUserId, int? groupId, int? categoryId, string name, string description, string authorFirstName, string authorLastName, string questionText)
        {
            return repository.GetSurveysCount(page, size, isPublic, userId, authorId, groupId, categoryId, name, description, authorFirstName,
                authorLastName, questionText);
        }

        public Tuple<IEnumerable<Survey>, int> GetActiveSurveys(int page, int size, bool? isPublic, int? filledUserId, int? authorId,int? favoriteUserId, int? groupUserId, int? groupId, string name, string description, string authorFirstName, string authorLastName, string questionText, List<int> categories, int state)
        {
            return repository.GetActiveSurveys(page, size, isPublic, filledUserId, authorId, favoriteUserId, groupUserId, groupId, name, description, authorFirstName,
                authorLastName, questionText, categories, state);
        }

        

        public List<UserNotification> CreateNotificationsForAdminsAndModerators(int? categoryId, Notification n, int userId)
        {


            List<UserNotification> retVal = new List<UserNotification>();
         

            var moderators = userRepository.GetForRoleName("Moderator").ToList();
            moderators.ForEach(a =>
            {
                if (a.Id != userId)
                {
                    UserNotification u = new UserNotification(a.Id, n, false);
                    retVal.Add(u);
                }
            }
            );

            var admins = userRepository.GetForRoleName("Admin").ToList();
            admins.ForEach(a =>
            {
                if (a.Id != userId)
                {
                    UserNotification u = new UserNotification(a.Id, n, false);
                    retVal.Add(u);
                }
            }
            );

            return retVal;
        }

        public List<ApplicationUser> UsersWhoFilledSurvey(List<int> ids)
        {
            return userRepository.GetMany(u => ids.Contains(u.Id)).ToList();
        }

        public void AddExcel(ExcelFiles obj)
        {
            this.excelFilesRepository.Add(obj);
        }

        public ExcelFiles GetFile(int id)
        {
            return this.excelFilesRepository.GetManyWithInclude(e => e.Id == id, e => e.Survey).FirstOrDefault();
        }

        public List<int> GetUserGroups(int userId)
        {
            return userGroupRepository.GetMany(ug => ug.UserId == userId).Select(ug => ug.GroupId).ToList();
        }

        public List<int> GetAllSubCategories(int catId)
        {
            List<int> retVal = new List<int>();
            return categoryRepository.GetAllSub(catId, retVal).ToList();
        }

        public IEnumerable<UserAnswer> GetAnswersForUser(int userId)
        {
            return userAnswerRepository.GetManyWithInclude(ua => ua.UserId == userId, ua => ua.Answer);
        }

        public int GetGroupAuthor(int groupId)
        {
            var group = groupRepository.GetById(groupId);
            if (group != null)
                return group.UserId;
            return 0;
        }

        public void DeleteSurvey(Survey survey)
        {

            

            var notTextQuest = survey.Questions.Where(q => q.AnswerType != AnswerType.Text).Select(q => q.Id).ToList();
            

            var textQuest = survey.Questions.Where(q => q.AnswerType == AnswerType.Text).Select(q => q.Id).ToList();
            var answers = userAnswerRepository.GetMany(ua => textQuest.Contains(ua.QuestionId)).Select(ua => ua.AnswerId).ToList();


            
            userAnswerRepository.Delete(ua => textQuest.Contains(ua.QuestionId) );
            userAnswerRepository.Delete(ua => notTextQuest.Contains(ua.QuestionId));
            answerRepository.Delete(a => answers.Contains(a.Id));
            answerRepository.Delete(a => notTextQuest.Contains((int)a.QuestionId));

            commentRepository.Delete(c => c.SurveyId == survey.Id);
            base.Delete(survey.Id);


        }

    }
}

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
    public interface IUserAnswerService : IService<UserAnswer>
    {
        string AddAnswers(List<UserAnswer> userAnswers, int surveyId, string encriptedUserId);
        string UpdateAnswers(List<UserAnswer> userAnswers, int surveyId);
        string ResetSurvey(int userId, int surveyId);
    }
    public class UserAnswerService : ServiceBase<UserAnswer>, IUserAnswerService
    {
        #region repositories
        private readonly IUserAnswerRepository repository;
        private readonly IUserNotificationRepository userNotificationRepository;
        private readonly IUserRepository userRepository;
        private readonly ISurveyRepository surveyRepository;
        private readonly IUserSurveyRepository userSurveyRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        
        public UserAnswerService(IUserAnswerRepository repository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, IUserNotificationRepository userNotificationRepository,
            ISurveyRepository surveyRepository, IUserSurveyRepository userSurveyRepository,
            IUserGroupRepository userGroupRepository) : base(repository, unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.userNotificationRepository = userNotificationRepository;
            this.userRepository = userRepository;
            this.surveyRepository = surveyRepository;
            this.userSurveyRepository = userSurveyRepository;
            this.userGroupRepository = userGroupRepository;
        }

        public string ResetSurvey(int userId, int surveyId)
        {
            var survey = surveyRepository.GetByIdAsNoTracking(surveyId);
            //if user is unauthorized
            if (survey.UserId != userId)
                return "401";

            var users = survey.Users;
            var author = userRepository.GetById(userId);
            //notification
            Notification notification = new Notification(author.FirstName + " " + author.LastName
                + " je resetovao anketu. Molimo Vas da je ponovo popunite.", DateTime.Now, Operation.RESET, NotificationType.PERSONAL);
            notification.SurveyId = survey.Id;
            notification.UserId = author.Id;
            List<UserNotification> notifications = new List<UserNotification>();
            foreach(var u in users)
            {
                notifications.Add(new UserNotification(u.UserId, notification, false));
            }
            // delete all answers
            userSurveyRepository.Delete(us => us.SurveyId == surveyId);
            userNotificationRepository.AddUserNotifications(notifications);
            var questionIdList = survey.Questions.Select(q => q.Id);
            repository.Delete(ua => questionIdList.Contains(ua.QuestionId));
            return "";
        }

        string IUserAnswerService.AddAnswers(List<UserAnswer> userAnswers, int surveyId, string encriptedUserId)
        {

            var s = surveyRepository.GetManyWithInclude(survey => survey.Id == surveyId, survey => survey.Users, survey => survey.Questions, survey => survey.Groups).FirstOrDefault();
           
            //validation
            var validation = this.Validation(s.Questions.ToList(), userAnswers, s, true);
            if (validation != null)
                return validation;
            var userId = userAnswers.First().UserId;
            var user = userRepository.GetById(userAnswers.First().UserId);

            repository.AddAnswers(userAnswers);

            //add user and survey in table UserSurvey
            UserSurvey us = new UserSurvey();
            us.UserId = userId;
            us.SurveyId = surveyId;
            us.EncrptUserId = encriptedUserId;

            userSurveyRepository.Add(us);
            var userNameAndLastName = "Korisnik ";
            if(!s.Anonymous)
            {
                userNameAndLastName+=  user.FirstName + " " + user.LastName;
            }
             
            //notification for author
            Notification notification = new Notification(userNameAndLastName + " je popunio/la Vašu  anketu", DateTime.Now, Operation.FILLED, NotificationType.PERSONAL);
            notification.SurveyId = us.SurveyId;
            notification.UserId = us.UserId;
            UserNotification un = new UserNotification(s.UserId, notification, false);
            userNotificationRepository.Add(un);

            return null;
                  
        }

        string IUserAnswerService.UpdateAnswers(List<UserAnswer> userAnswers, int surveyId)
        {
            var questions = userAnswers.Select(q => q.QuestionId).Distinct();

            var userId = userAnswers.Select(ua => ua.UserId).FirstOrDefault();
            
            var s = surveyRepository.GetByIdAsNoTracking(surveyId);
            
            var user = userRepository.GetById(userId);



            var validation = this.Validation(s.Questions.Where(q => questions.Contains(q.Id)).ToList(), userAnswers, s, false);
            if (validation != null)
                return validation;

            foreach (var question in questions)
            {
                var oldAnswers = repository.GetMany(ua => ua.QuestionId == question && ua.UserId == userId);
                
                //IF OLD ANSWER ISN'T IN NEW LIST OF ANSWERS REMOVE OLD ANSWER
                foreach (var oldAnswer in oldAnswers)
                {
                    if(!userAnswers.Contains(oldAnswer))
                    {
                          
                            repository.Delete(oldAnswer);

                        
                    }
                }
                // ADDING NEW ANSWERS
                foreach (var answer in userAnswers.Where(a => a.QuestionId == question))
                {
                    if(!oldAnswers.Contains(answer))
                    {
                        
                        
                            repository.Add(answer);
                        
                    }
                }
            }

            var userNameAndLastName = "Korisnik ";
            if (!s.Anonymous)
            {
                userNameAndLastName += user.FirstName + " " + user.LastName;
            }
            Notification notification = new Notification(userNameAndLastName + " je ponovo popunio/la anketu", DateTime.Now, Operation.FILLED, NotificationType.PERSONAL);
            notification.SurveyId = surveyId;
            notification.UserId = userId;
            UserNotification un = new UserNotification(surveyRepository.GetByIdAsNoTracking(surveyId).UserId, notification, false);

            userNotificationRepository.Add(un);
            return null;
        }

        private string Validation(List<Question> questions, List<UserAnswer> userAnswers, Survey s, bool isAdd)
        {
            var userId = userAnswers.First().UserId;
            if (s == null) // dodati ako je user vec glasao
            {
                return "Anketa ne postoji.";
            }
            if (s.Users.Select(us => us.UserId).Contains(userId) && isAdd)
            {
                return "Vec ste popunili anketu.";
            }
            else if (!s.Users.Select(us => us.UserId).Contains(userId) && !isAdd)
            {
                return "Ne možete ponovo popuniti anketu, jer je niste popunili.";
            }
            if(s.State == State.CLOSED)
            {
                return "Ne možete popuniti anketu jer je zatvorena.";
            }
            if (!s.Public && s.UserId != userId)
            {
                

                var groups = s.Groups.Select(g => g.GroupId).ToList();
                var usersInGroups = userGroupRepository.GetMany(ug => groups.Contains(ug.GroupId)).Select(ug => ug.UserId);
               
                
                if (!usersInGroups.Contains(userId))
                    return "Unauthorized";
                   
            }

            

            foreach (var q in questions)
            {
                if (q.Required)
                {

                    if (q.AnswerType == AnswerType.Text)
                    {
                        UserAnswer ua = userAnswers.Where(_ua => _ua.QuestionId == q.Id).FirstOrDefault();
                        if (ua != null && ua.Answer != null && ua.Answer.AnswerText.Equals(""))
                        {
                            return q.QuestionText + " : " + "Popunite sva obavezna pitanja";

                        }
                    }
                    if (!userAnswers.Select(ua => ua.QuestionId).Contains(q.Id))
                    {
                        return q.QuestionText + " : " + "Popunite sva obavezna pitanja";
                    }
                   

                }
                if ((userAnswers.Where(ua => ua.QuestionId == q.Id).Count() > 0 && userAnswers.Where(ua => ua.QuestionId == q.Id).Count() < q.MinNumbOfAnswers)
                    || userAnswers.Where(ua => ua.QuestionId == q.Id).Count() > q.MaxNumbOfAnswers)
                    return q.QuestionText + " : " + "Broj odgovora nije u granicama definisanog";
            }
            return null;
        }
    }
}

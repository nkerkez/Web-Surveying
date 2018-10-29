using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/userssurvey")]
    public class UsersSurveysController : ApiController
    {
        private IUnitOfWork unitOfWork;
        private IUserSurveyService userSurveyService;

        public UsersSurveysController(IUnitOfWork unitOfWork, IUserSurveyService userSurveyService)
        {
            this.unitOfWork = unitOfWork;
            this.userSurveyService = userSurveyService;
        }



      //  [Authorize(Roles = "Admin, Moderator, User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("{surveyId}")]
        public IHttpActionResult GetForSurvey(int surveyId)
        {
            try
            {
                var retList =  userSurveyService.GetForSurvey(surveyId).ToList();
                var users = retList.Select(u => u.UserId).ToList();
                var profiles = userSurveyService.GetUsers(users).ToList();
                
                var survey = userSurveyService.GetSurvey(surveyId);
                var retVal = new UserSurveyViewModel()
                {
                    SurveyId = survey.Id,
                    SurveyName = survey.Name,
                    Users = new List<UserWithEncrptIdViewModel>()

                };

                profiles.ForEach(p =>
                {
                    if (!survey.Anonymous)
                    {
                        var objVM = new UserWithEncrptIdViewModel()
                        {
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            EncrptId = retList.FirstOrDefault(us => us.UserId == p.Id).EncrptUserId,
                            UserId = p.Id
                        };
                        retVal.Users.Add(objVM);
                    }
                    else
                    {
                        var objVM = new UserWithEncrptIdViewModel()
                        {
                            FirstName = profiles.IndexOf(p).ToString(),
                            LastName = "************",
                            EncrptId = retList.FirstOrDefault(us => us.UserId == p.Id).EncrptUserId,
                            UserId = 0
                        };
                        retVal.Users.Add(objVM);
                    }
                    
                       
                    
                });

                return Ok(retVal);
                
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
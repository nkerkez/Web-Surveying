using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/questions")]
    public class QuestionsController : ApiController
    {
        public IUnitOfWork unitOfWork;
        public IQuestionService questionService;

        public QuestionsController(IUnitOfWork unitOfWork, IQuestionService questionService)
        {
            this.unitOfWork = unitOfWork;
            this.questionService = questionService;
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetQuestion(int id)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var question = questionService.GetQuestion(id);

                if (question == null)
                    return BadRequest();

                var retList = questionService.GetForSurvey(question.SurveyId).ToList();
                var users = retList.Select(u => u.UserId).ToList();
                var profiles = questionService.GetUsers(users).ToList();
                var questionResultVM = new QuestionResultViewModel()
                {
                    Question = question,
                    Users = new List<UserWithEncrptIdViewModel>()
                    
                };
                
                if (question.Survey.UserId != userId)
                    return Unauthorized();

                profiles.ForEach(p =>
                {
                    var answers = new Dictionary<int, bool>();
                    if (!question.Survey.Anonymous)
                    {
                        var objVM = new UserWithEncrptIdViewModel()
                        {
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            EncrptId = retList.FirstOrDefault(us => us.UserId == p.Id).EncrptUserId,
                            UserId = p.Id
                        };
                        if (question.AnswerType != Model.Model.AnswerType.Text)
                        {
                            foreach (var questionAnswer in question.QuestionAnswers)
                            {
                                answers.Add(questionAnswer.Id, question.UserAnswers.Where(ua => ua.AnswerId == questionAnswer.Id).Select(ua => ua.UserId).Contains(p.Id));
                            }
                            objVM.Answers = answers;
                        }
                        else
                        {
                            var userAnswer = question.UserAnswers.Where(ua => ua.UserId == p.Id).FirstOrDefault();
                            if (userAnswer == null)
                            {
                                objVM.AnswerText = "Nije odgovorio";
                            }
                            else
                            {
                                objVM.AnswerText = userAnswer.Answer.AnswerText;
                            }
                        }
                        questionResultVM.Users.Add(objVM);
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
                        if (question.AnswerType != Model.Model.AnswerType.Text)
                        {
                            foreach (var questionAnswer in question.QuestionAnswers)
                            {
                                answers.Add(questionAnswer.Id, question.UserAnswers.Where(ua => ua.AnswerId == questionAnswer.Id).Select(ua => ua.UserId).Contains(p.Id));
                            }
                            objVM.Answers = answers;
                        }
                        else
                        {
                            var userAnswer = question.UserAnswers.Where(ua => ua.UserId == p.Id).FirstOrDefault();
                            if (userAnswer == null)
                            {
                                objVM.AnswerText = "Nije odgovorio";
                            }
                            else
                            {
                                objVM.AnswerText = userAnswer.Answer.AnswerText;
                            }
                        }
                        questionResultVM.Users.Add(objVM);
                    }



                });

                return Ok(questionResultVM);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using WebSurveying2017.BindingModels;
using WebSurveying2017.ViewModels;
using AutoMapper;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Service.Service;
using System.IO;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using OfficeOpenXml;
using System.Drawing;
using WebSurveying2017.Criptying;
using WebSurveying2017.Helper;
using Combinatorics.Collections;

namespace WebSurveying2017.Controllers
{

    [Authorize]
    [RoutePrefix("api/surveys")]
    public class SurveysController : ApiController
    {


        private ISurveyService surveyService;
        private IUnitOfWork unitOfWork;

        public SurveysController(IUnitOfWork unitOfWork, ISurveyService surveyService)
        {
            this.unitOfWork = unitOfWork;
            this.surveyService = surveyService;


        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Survey))]
        public IHttpActionResult PostSurvey(SurveyBindingModel surveyBM)
        {

            try
            {
                var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                this.ValidateOrdinalNumber(surveyBM.Questions.Select(q => q.OrdinalNumber).ToList(), surveyBM.Questions.Count());
                Survey survey = AutoMapper.Mapper.Map<SurveyBindingModel, Survey>(surveyBM);
                survey.State = State.OPENED;
                surveyService.Create(survey, userId);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                if (ModelState.Count() == 0)
                    ModelState.AddModelError("", "Postoji anketa sa unetim imenom");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{userEnum}/{isPublic}/{groupId}/{categoryId}/{subCategories}/{allSurveys}/{state}/{page}/{size}")]
        public IHttpActionResult GetSurveys([FromUri]SurveyQueryParams obj, int page, int size,
            int? groupId, UserSearchEnum userEnum, int categoryId, bool subCategories, bool? isPublic,
            bool allSurveys, int state)
        {
            groupId = groupId == 0 ? null : groupId;
            
            int? authorId = null;
            int? filledUserId = null;
            int? favoriteUserId = null;
            int? groupUserId = null;

            if (!(bool)isPublic)
                isPublic = null;
            var identity = (ClaimsIdentity)User.Identity;

            if (!identity.IsAuthenticated)
            {
                if (groupId != null)
                    return Unauthorized();
                isPublic = true;
            }
            else
            {
                var userId = int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

                if (groupId != null)
                {
                    var userGroups = surveyService.GetUserGroups(userId);
                    if (!userGroups.Contains((int)groupId) && userId != surveyService.GetGroupAuthor((int)groupId))
                    {
                        return Unauthorized();
                    }
                }
                //all surveys for user - public and all groups for user
                if (role.Equals("User") && allSurveys)
                {
                    isPublic = true;
                    groupUserId = userId;
                }
                if (!role.Equals("User") && allSurveys)
                {
                    isPublic = null;
                }
                if (allSurveys)
                    switch (userEnum)
                    {
                        case UserSearchEnum.AUTHOR:
                            authorId = userId;
                            break;
                        case UserSearchEnum.FILLSURVEY:
                            filledUserId = userId;
                            break;
                        case UserSearchEnum.FAVORITESURVEY:
                            favoriteUserId = userId;
                            break;
                        case UserSearchEnum.GROUP:
                            groupUserId = userId;
                            break;
                        default:
                            break;
                    }
            }
            if (categoryId >= 0)
            {
                if (categoryId == 0 && subCategories == false)
                {

                }
                else
                {
                    if (subCategories == true && categoryId > 0 )
                        obj.ListOfCategories = surveyService.GetAllSubCategories((int)categoryId);
                    if (obj.ListOfCategories == null)
                        obj.ListOfCategories = new List<int>();
                    obj.ListOfCategories.Add((int)categoryId);
                }
            }
            

            Tuple<IEnumerable<Survey>, int> returnValue = null;
            try
            {

                if (obj == null)
                {
                    return BadRequest();
                }
                else
                {
                    obj.ListOfCategories = obj.ListOfCategories == null ? new List<int>() : obj.ListOfCategories;
                    returnValue = surveyService.GetActiveSurveys(page, size, isPublic, filledUserId, authorId, favoriteUserId, groupUserId, groupId,
                        obj.Name, obj.Description, obj.AuthorFirstName, obj.AuthorLastName, obj.QuestionText, obj.ListOfCategories, state);
                }


                List<SurveyViewModel> retVal = AutoMapper.Mapper.Map<List<Survey>, List<SurveyViewModel>>(returnValue.Item1.ToList());

                this.SetSurveyVMValues(retVal, returnValue.Item1.ToList());

                PageModel<SurveyViewModel> pageModel = new PageModel<SurveyViewModel>()
                {
                    Models = retVal,
                    Size = size,
                    CurrentPage = page,
                    Count = returnValue.Item2
                };

                return Ok(pageModel);

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpPut]
        [Route("changeState/{id}")]
        public IHttpActionResult ChangeState(int id)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var survey = surveyService.GetAsNoTracking(id);
                if (survey == null)
                    return BadRequest();
                if (survey.UserId != userId)
                    return Unauthorized();
                if (survey.State == State.CLOSED)
                    survey.State = State.OPENED;
                else
                    survey.State = State.CLOSED;

                surveyService.Update(survey);
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpPut]
        [Route("changeCategory")]
        public IHttpActionResult ChangeCategory(SurveyBindingModel surveyBindingModel)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var survey = surveyService.GetAsNoTracking(surveyBindingModel.Id);
                var roleName = identity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                if (survey == null)
                    return BadRequest();
                if (survey.UserId != userId && roleName.Equals("User"))
                    return Unauthorized();
                survey.CategoryId = surveyBindingModel.CategoryId;

                surveyService.Update(survey);
                unitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSurvey(int id)
        {
            int userId = 0;
            string roleName = "";
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.IsAuthenticated)
            {
                var claims = identity.Claims;
                userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                roleName = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            }

            try
            {
                Survey survey = surveyService.GetAsNoTracking(id);


                if (survey == null)
                    return BadRequest();

                if (!survey.Public)
                {
                    if (userId == 0)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        var groups = surveyService.GetUserGroups(userId);
                        if (roleName.Equals("User") && !groups.Any(g => survey.Groups.Select(sg => sg.GroupId).Contains(g)))
                        {
                            return Unauthorized();
                        }

                    }
                }
                // Map to VM
                SurveyViewModel surveyVM = AutoMapper.Mapper.Map<Survey, SurveyViewModel>(survey);

                this.SetSurveyVMValues(surveyVM, survey);

                return Ok(surveyVM);


            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
        // GET: api/Surveys/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("useranswers/{id}")]
        public IHttpActionResult GetSurveyWithUserAnswers(int id)
        {
            //get user
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            //Survey survey = unitOfWork.surveyRepository.GetByIdAsNoTracking(id);
            //survey with user answers
            Survey survey = surveyService.GetWithUserAnswers(id, userId);


            if (survey == null)
            {
                return NotFound();
            }

            if (!survey.Users.Select(u => u.UserId).Contains(userId))
            {
                return Unauthorized();
            }

            SurveyViewModel surveyVM = AutoMapper.Mapper.Map<Survey, SurveyViewModel>(survey);
            surveyVM.UserVM = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(surveyService.GetAuthor(survey.UserId));
            var questions = surveyVM.Questions;

            // isprobati nova resenja
            foreach (var question in questions)
            {
                // fill answer text for all answers
                foreach (var answerVM in question.UserAnswers)
                {
                    var answerId = answerVM.AnswerId;
                    var questionObj = survey.Questions.Where(q => q.Id == question.Id).FirstOrDefault();
                    var answerText = questionObj.UserAnswers.Where(ua => ua.AnswerId == answerId).FirstOrDefault().Answer.AnswerText;
                    answerVM.AnswerText = answerText;
                    answerVM.Id = answerId;
                }
            }





            return Ok(surveyVM);
        }
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        [Route("results/{id}/{uuu}")]
        [ResponseType(typeof(Survey))]
        public IHttpActionResult GetSurveyWithResult(int id, string uuu)
        {
            int userId = 0;
            if (!int.TryParse(uuu, out userId))
            {
                uuu = uuu.Replace("=", "+");
                uuu = uuu.Replace("*", "/");
                int.TryParse(StringCipher.Decrypt(uuu, "WebSurveying2017"), out userId);
            }
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var loggedUserId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            Survey survey = surveyService.GetWithResult(id);
            int resultForUser = 0;
            //if want result for specific user, logged user must be author of survey
            if (userId != 0)
            {
                if (loggedUserId != survey.UserId || !survey.Users.Select(u => u.UserId).Contains((int)userId))
                    return Unauthorized();
                resultForUser = (int)userId;
            }
            else
            {
                if (loggedUserId != survey.UserId && !survey.Users.Select(u => u.UserId).Contains(loggedUserId)
                 || (survey.ResultAuthor && survey.UserId != loggedUserId))
                {
                    return Unauthorized();
                }
                resultForUser = loggedUserId;
            }


            //author - all
            //user - 1. Filled 2. !ResultAuthor 3. userId == loggedUserId

            if (survey == null)
            {
                return NotFound();
            }

            var users = surveyService.UsersWhoFilledSurvey(survey.Users.Select(u => u.UserId).ToList());
            MapSurveyResultToSurveyResultViewModel myMapper = new MapSurveyResultToSurveyResultViewModel(surveyService);
            SurveyResultViewModel surveyVM = AutoMapper.Mapper.Map<Survey, SurveyResultViewModel>(survey);
            myMapper.SetSurveyResultVMValues(surveyVM, survey, users, resultForUser, false);
            //  this.SetSurveyResultVMValues(surveyVM, survey, users, resultForUser, false);
            return Ok(surveyVM);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/save")]
        public IHttpActionResult SaveFile(int id)
        {

            Survey survey = surveyService.GetWithResult(id);
            if (survey == null)
            {
                return NotFound();
            }

            survey.Questions.ToList().ForEach(q =>
               {
                   q.QuestionAnswers = q.QuestionAnswers.OrderBy(qa => qa.OrdinalNumber).ToList();
               });


            var users = surveyService.UsersWhoFilledSurvey(survey.Users.Select(u => u.UserId).ToList());

            SurveyResultViewModel surveyVM = AutoMapper.Mapper.Map<Survey, SurveyResultViewModel>(survey);
            this.SetSurveyResultVMValues(surveyVM, survey, users, 0, true);

            using (var xlPackage = new ExcelPackage())
            {


                var ws = xlPackage.Workbook.Worksheets.Add("Anketa: " + survey.Name);

                CreateExcelFile createEF = new CreateExcelFile(surveyService);
                if (!survey.Anonymous)
                    createEF.CreateSurveyWorksheet(ws, surveyVM);
                createEF.CreateQuestionsOnSurveyWorksheet(ws, surveyVM);
                surveyVM.Questions.ToList().ForEach(q =>
                {

                    createEF.CreateQuestionWorksheet(xlPackage, q, surveyVM, survey);
                });
                users.ForEach(u =>
                {
                    var answers = surveyService.GetAnswersForUser(u.Id).ToList();
                    createEF.CreateUserWorksheet(xlPackage, u, survey, answers, users.IndexOf(u));
                });
                ExcelFiles obj = new ExcelFiles() { Bytes = xlPackage.GetAsByteArray(), SurveyId = survey.Id, CreationDate = DateTime.Now, Name = survey.Name + DateTime.Now.ToString() + ".xlsx" };

                surveyService.AddExcel(obj);
                unitOfWork.Commit();

            }




            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("download/{id}")]
        public HttpResponseMessage DownloadExcel(int id)
        {
            try
            {
                var obj = surveyService.GetFile(id);
                if (obj == null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var content = (byte[])obj.Bytes;


                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(content)
                };
                result.Content.Headers.ContentDisposition =
             new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
             {
                 FileName = obj.Survey.Name + DateTime.Now.ToString() + ".xlsx"
             };

                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        // PUT: api/Surveys/5
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin,Moderator,User")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurvey(int id, SurveyBindingModel surveyBM)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            int userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            string roleName = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            if (roleName.Equals("User") && userId != surveyBM.UserId)
                return BadRequest();


            if (id != surveyBM.Id)
            {
                return BadRequest();
            }





            try
            {
                this.ValidateOrdinalNumber(surveyBM.Questions.Select(q => q.OrdinalNumber).ToList(), surveyBM.Questions.Count());

                var survey = AutoMapper.Mapper.Map<SurveyBindingModel, Survey>(surveyBM);

                surveyService.Update(survey);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                if (ModelState.Count() == 0)
                    ModelState.AddModelError("", "Postoji anketa sa unetim imenom");
                return BadRequest(ModelState);
            }

            return Ok();
        }






        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSurvey(int id)
        {
            var userId = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var roleName = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
            
            try
            {
                var old = surveyService.Get(id);
                if (roleName.Equals("User") && old.UserId != userId)
                    return Unauthorized();

                surveyService.DeleteSurvey(old);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {

                return BadRequest();
            }

            return Ok();
        }


        private void SetSurveyVMValues(IEnumerable<SurveyViewModel> retVal, IEnumerable<Survey> surveys)
        {
            retVal.ToList().ForEach(
                    survey =>
                    {

                        this.SetSurveyVMValues(survey, surveys.Where(s => s.Id == survey.Id).First());

                    }
                    );
        }



        private void SetSurveyVMValues(SurveyViewModel retVal, Survey survey)
        {

            retVal.UserVM = AutoMapper.Mapper.Map<ApplicationUser, UserViewModel>(surveyService.GetAuthor(survey.UserId));
            retVal.CommentCount = survey.Comments.Count();
            retVal.NumbOfUsers = survey.Users.Count();


            retVal.FavoriteSurveyFor = survey.FavoriteSurveys.Select(fs => fs.UserId).ToList();
            retVal.FilledSurvey = survey.Users.Select(u => u.UserId).ToList();

        }

        private void SetSurveyResultVMValues(SurveyResultViewModel surveyVM, Survey survey,
            List<ApplicationUser> users, int? userId, bool forExcel)
        {
            surveyVM.NumbOfUsers = survey.Users.Count();
            if (!survey.Anonymous)
            {
                surveyVM.MaleCount = users.Where(u => u.Gender == Gender.MALE).Count();
                surveyVM.FemaleCount = users.Where(u => u.Gender == Gender.FEMALE).Count();



                var und18 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(18).CompareTo(DateTime.Now) > 0);
                var u18_25 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(26).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(18).CompareTo(DateTime.Now) <= 0);
                var u26_40 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(41).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(26).CompareTo(DateTime.Now) <= 0);
                var u41_60 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(61).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(41).CompareTo(DateTime.Now) <= 0);
                var u61 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(61).CompareTo(DateTime.Now) <= 0);

                surveyVM.Unknown = users.Where(u => u.Birthday == null).Count();
                surveyVM.UsersUnder18Count = und18.Count();
                surveyVM.Users18_25Count = u18_25.Count();
                surveyVM.Users26_40Count = u26_40.Count();
                surveyVM.Users41_60Count = u41_60.Count();
                surveyVM.Users61Count = u61.Count();

                if (forExcel)
                {
                    surveyVM.MaleUnder18 = und18.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.FemaleUnder18 = und18.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male18_25 = u18_25.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female18_25 = u18_25.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male26_40 = u26_40.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female26_40 = u26_40.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male41_60 = u41_60.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female41_60 = u41_60.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male61 = u61.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female61 = u61.Where(u => u.Gender == Gender.FEMALE).Count();
                }
            }
            foreach (var questionVM in surveyVM.Questions)
            {


                questionVM.NumbOfUsers = surveyService.NumbOfUsers(questionVM.Id);

                if (questionVM.AnswerType == AnswerType.Text)
                {
                    // if answer type text get answer from db
                    UserAnswer userAnswer = surveyService.GetUserAnswerWithAnswerObj((int)questionVM.Id, (int)userId);
                    if (userAnswer != null)
                    {
                        // must fill answer text
                        UsersAnswersVM _ua = new UsersAnswersVM
                        {
                            AnswerText = userAnswer.Answer.AnswerText
                        };

                        questionVM.QuestionAnswers.Add(_ua);
                    }

                }
                else
                {
                    var question = survey.Questions.Where(q => q.Id == questionVM.Id).FirstOrDefault();

                    foreach (var answerVM in questionVM.QuestionAnswers)
                    {
                        var answer = question.QuestionAnswers.Where(a => a.Id == answerVM.Id).FirstOrDefault();
                        answerVM.Count = answer.Users.ToList().Count();
                        answerVM.IsAnswerOfUser = false;


                        foreach (var ua in answer.Users)
                        {
                            if (ua.UserId == userId)
                            {
                                answerVM.IsAnswerOfUser = true;
                                break;
                            }
                        }


                        questionVM.NumbOfUA += answerVM.Count;

                    }
                }
            }

        }

        private void CreateQuestionWorksheet(ExcelPackage e, QuestionResultVM questionVM, SurveyResultViewModel surveyVM, Survey survey)
        {
            var ws = e.Workbook.Worksheets.Add(questionVM.QuestionText);

            ws.SetValue(1, 3, questionVM.QuestionText);
            var col = 3;

            var question = survey.Questions.Where(q => q.Id == questionVM.Id).FirstOrDefault();
            questionVM.QuestionAnswers.ForEach(qa =>
            {
                var row = 2;
                ws.SetValue(row, col, qa.AnswerText);
                var answer = question.QuestionAnswers.Where(an => an.Id == qa.Id).FirstOrDefault();
                var users = answer.Users.Select(u => u.UserId);
                row++;
                surveyVM.Questions.Where(q => q.Id != questionVM.Id).ToList().ForEach(q =>
                    {
                        var _question = survey.Questions.Where(_q => _q.Id == q.Id).FirstOrDefault();
                        ws.SetValue(row, 1, q.QuestionText);
                        ws.Cells[row, 1, row, questionVM.QuestionAnswers.Count() + 2].Merge = true;
                        row++;
                        q.QuestionAnswers.ForEach(_qa =>
                        {
                            var _answer = _question.QuestionAnswers.Where(an => an.Id == _qa.Id).FirstOrDefault();
                            var _users = _answer.Users.Select(u => u.UserId);
                            if (col == 3)
                                ws.SetValue(row, 2, _qa.AnswerText);
                            var value = users.Where(u => _users.Contains(u)).Count();
                            ws.SetValue(row, col, value);
                            row++;
                        });

                    });
                col++;
            });

            ws.Cells[1, 3, 1, questionVM.QuestionAnswers.Count() + 2].Merge = true;

        }

        private void ValidateOrdinalNumber(List<int> numbers, int questionsCount)
        {
            if (numbers.Distinct().Count() != questionsCount)
            {
                ModelState.AddModelError("", "Postoje duplikati rednih brojeva");
                throw new Exception();
            }

            numbers.ForEach(x =>
            {
                if (x > questionsCount)
                {
                    ModelState.AddModelError("", "Redni brojevi pitanja nisu dobro definisani");
                    throw new Exception();
                }
            });
        }
    }
}
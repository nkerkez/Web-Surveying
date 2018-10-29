using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebSurveying2017.BindModels;
using WebSurveying2017.Criptying;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    public class UsersAnswersController : ApiController
    {

        private IUserAnswerService userAnswerService;
        private IUnitOfWork unitOfWork;

        public UsersAnswersController(IUnitOfWork unitOfWork, IUserAnswerService userAnswerService)
        {

            this.unitOfWork = unitOfWork;
            this.userAnswerService = userAnswerService;
        }



        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        [Route("api/usersanswers/{id}")]
        public IHttpActionResult PostUserAnswers(int id, List<UserAnswerBindingModel> userAnswersBM)
        {

            if (userAnswersBM.Count() < 1)
            {
                ModelState.AddModelError("", "Morate odgovoriti na pitanja.");
                return BadRequest(ModelState);
            }

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            foreach (var ua in userAnswersBM)
            {
                ua.UserId = userId;
            }
            var encriptedUserId = StringCipher.Encrypt(userId.ToString(), "WebSurveying2017");
            var userAnswers = AutoMapper.Mapper.Map<List<UserAnswerBindingModel>, List<UserAnswer>>(userAnswersBM);


            try
            {
                string retVal = userAnswerService.AddAnswers(userAnswers, id, encriptedUserId);
                if (retVal == null)
                    unitOfWork.Commit();
                else
                {
                    if (retVal.Equals("Unauthorized"))
                        return Unauthorized();
                    ModelState.AddModelError("", retVal);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPut]
        [Route("api/usersanswers/{id}")]
        public IHttpActionResult PutAnswers(int id, List<UserAnswerBindingModel> answersBM)
        {

            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            if (answersBM.Count == 0)
            {
                ModelState.AddModelError("", "Niste izmenili nijedan odgovor.");
                return BadRequest(ModelState);
            }
            foreach (var ua in answersBM)
            {
                ua.UserId = userId;
            }

            var answers = AutoMapper.Mapper.Map<List<UserAnswerBindingModel>, List<UserAnswer>>(answersBM);

            try
            {


                string retVal = userAnswerService.UpdateAnswers(answers, id);
                if (retVal == null)
                    unitOfWork.Commit();
                else
                {
                    if (retVal.Equals("Unauthorized"))
                        return Unauthorized();
                    ModelState.AddModelError("", retVal);
                    return BadRequest(ModelState);
                }
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();

        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpDelete]
        [Route("api/usersanswers/survey/{surveyId}")]
        public IHttpActionResult ResetSurvey(int surveyId)
        {
            if (surveyId == 0)
                return BadRequest();
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);


            try
            {
                var retVal = userAnswerService.ResetSurvey(userId, surveyId);
                if (retVal.Equals("405"))
                    return Unauthorized();

                unitOfWork.Commit();

            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
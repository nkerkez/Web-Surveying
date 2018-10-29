using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Controllers
{
    [Authorize]
    [RoutePrefix("api/favoriteSurveys")]
    public class FavoriteSurveysController : ApiController
    {
        
        public IFavoriteSurveysService favoriteSurveysService;
        public IUnitOfWork unitOfWork;

        public FavoriteSurveysController(IUnitOfWork unitOfWork, IFavoriteSurveysService favoriteSurveysService)
        {
            this.unitOfWork = unitOfWork;
            this.favoriteSurveysService = favoriteSurveysService;
            
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpPost]
        public IHttpActionResult PostFavoriteSurvey(SurveyBindingModel surveyBM)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            

            try
            {
                FavoriteSurveys obj = new FavoriteSurveys()
                {
                    UserId = userId,
                    SurveyId = surveyBM.Id
                };

                favoriteSurveysService.Create(obj);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpDelete]
        [Route("{surveyId}")]
        public IHttpActionResult RemoveFromFavoriteSurveys(int surveyId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var userId = int.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            

            try
            {
                favoriteSurveysService.Delete(surveyId, userId);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        
    }
}
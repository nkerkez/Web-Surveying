using System.Data.Entity;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data
{
    public interface IWebSurveyingContext
    {
         DbSet<Answer> Answers { get; set; }
         DbSet<Category> Categories { get; set; }
         DbSet<Comment> Comments { get; set; }
         DbSet<Question> Questions { get; set; }
         DbSet<ApplicationUser> Users { get; set; }
         DbSet<Survey> Surveys { get; set; }
         DbSet<UserSurvey> AvaliableSurveysForUsers { get; set; }
         DbSet<UserAnswer> UsersAnswers { get; set; }

         void Commit();
    }
}

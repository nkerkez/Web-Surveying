using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserExtend : IModelBase
    {
        [NotMapped]
        public int Id { get; set; }
        [Key, ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        

    //    public ICollection<UserGroup> CreatedGroups { get; set; }
        public ICollection<Survey> CreatedSurveys { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
        public ICollection<UserSurvey> FilledSurvey { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserNotification> UsersNotifications { get; set; }
        public ICollection<UserCategory> UserCategories { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
        public ICollection<Group> AuthorGroups { get; set; }


      
        public UserExtend()
        {
            CreatedSurveys = new List<Survey>();
            UserAnswers = new List<UserAnswer>();
            FilledSurvey = new List<UserSurvey>();
            Comments = new List<Comment>();
            UsersNotifications = new List<UserNotification>();
            UserCategories = new List<UserCategory>();

        }

        
    }
}

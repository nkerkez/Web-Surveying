using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{   // Survey model class
    public class Survey : IModelBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        [Required]    
        public string Name { get; set; }

        [StringLength(350)]
        [Required]
        public string Description { get; set; }
        public bool Anonymous { get; set; }
        public bool ResultAuthor { get; set; }
        public bool Public { get; set; }
        public State State { get; set; }
        public DateTime CreationDate { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int UserId { get; set; }
        public UserExtend Author { get; set; }

        public ICollection<FavoriteSurveys> FavoriteSurveys { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<UserSurvey> Users { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SurveyGroup> Groups { get; set; }
        public ICollection<ExcelFiles> ExcelFiles { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
    
    public enum State
    {
        OPENED = 1,
        CLOSED

    }
}

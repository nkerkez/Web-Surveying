using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebSurveying2017.Model.Model
{
    public class Comment : IModelBase
    {
        public int Id { get; set; }
        
        public string Text { get; set; }

        public DateTime CreationDate { get; set; }
        
        public DateTime? LastEdited { get; set; }

        public int? ParentId { get; set; }
        public Comment ParentComment { get; set; }

       

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public int UserId { get; set; }
        public UserExtend User { get; set; }

        [InverseProperty("ParentComment")]
        public ICollection<Comment> SubComments;


        public ICollection<LikeOrDislikeComment> LikesOrDislikes { get; set; }

        public ICollection<Notification>  Notifications { get; set; }

        

        
    }
}

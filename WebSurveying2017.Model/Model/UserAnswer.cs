using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserAnswer : IModelBase
    {
        
        [NotMapped]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public UserExtend User { get; set; }

        [Key, Column(Order = 2)]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        
    }
}

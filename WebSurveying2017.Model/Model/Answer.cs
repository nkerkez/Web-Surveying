using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class Answer : IModelBase
    {
        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; }

        public int OrdinalNumber { get; set; }

        public int? QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<UserAnswer> Users { get; set; }


        public Answer(int id, string answerText)
        {
            this.Id = id;
            this.AnswerText = answerText;
            Users = new List<UserAnswer>();
        }

        public Answer()
        {
            Users = new List<UserAnswer>();
        }
    }
}

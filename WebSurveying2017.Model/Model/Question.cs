using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    //Question model class
    public class Question : IModelBase
    {
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        public string QuestionText { get; set; }
        public int MinNumbOfAnswers { get; set; }
        public int MaxNumbOfAnswers { get; set; }
        public int OrdinalNumber { get; set; }
        public AnswerType AnswerType { get; set; }
        public bool Required { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }

        public ICollection<Answer> QuestionAnswers { get; set; }
        
    }
    
    public enum AnswerType
    {
        Text = 1,
        Select = 2,
        MultiSelect = 3,
        Radio = 4,
        CheckBox = 5,
        MultiCheckBox = 6
    }

    
    
}

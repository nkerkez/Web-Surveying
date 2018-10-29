using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.BindModels
{
    public class UserAnswerBindingModel
    {
        
        public int UserId { get; set; }

        
        public int AnswerId { get; set; }
        public AnswerBindingModel Answer { get; set; }

        public int QuestionId { get; set; }
    }
}
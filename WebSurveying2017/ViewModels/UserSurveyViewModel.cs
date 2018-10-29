using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class UserSurveyViewModel
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public List<UserWithEncrptIdViewModel> Users { get; set; }
        
    }

    public class UserWithEncrptIdViewModel
    {
        public string EncrptId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Dictionary<int,bool> Answers { get; set; }
        public string AnswerText { get; set; }
    }
}
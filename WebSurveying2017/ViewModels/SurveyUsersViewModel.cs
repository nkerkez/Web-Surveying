using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class SurveyUsersViewModel
    {
        public SurveyViewModel Survey { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
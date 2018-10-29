using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class QuestionResultViewModel
    {
        public Question Question { get; set; }

        public List<UserWithEncrptIdViewModel> Users { get; set; }

        
    }
}
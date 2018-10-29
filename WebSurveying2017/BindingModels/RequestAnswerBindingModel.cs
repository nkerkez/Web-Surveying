using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.BindingModels
{
    public class RequestAnswerBindingModel
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool Accepted { get; set;  }
    }
}
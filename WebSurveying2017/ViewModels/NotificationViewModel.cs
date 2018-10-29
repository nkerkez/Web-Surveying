using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public int? SurveyId { get; set; }

        public int? CommentId { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        public NotificationType  NotificationType {get;set;} 

        public Operation Operation { get; set; }


    }
}
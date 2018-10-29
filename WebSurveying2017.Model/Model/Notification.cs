using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class Notification : IModelBase
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public Operation Operation { get; set; }

        public NotificationType NotificaionType { get; set; }

        public int? UserId { get; set; }
        public UserExtend User { get; set; }

        public int? SurveyId { get; set; }
        public Survey Survey { get; set; }

        public int? CommentId { get; set; }
        public Comment Comment { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }


        public ICollection<UserNotification> UsersNotifications { get; set; }

        public Notification()
        {
            UsersNotifications = new List<UserNotification>();
        }

        public Notification(string _message, DateTime _dateTime, Operation _operation
            , NotificationType type)
        {
            this.Message = _message;
            this.DateTime = _dateTime;
            this.Operation = _operation;
            this.NotificaionType = type;
            UsersNotifications = new List<UserNotification>();
        }
    }

    

    public enum Operation
    {
        ADD = 1,
        UPDATE,
        ACCEPTED,
        REJECTED,
        FILLED,
        LIKEORDISLIKE,
        RESET,
        REQUEST
    }

    public enum NotificationType
    {
        MODERATOR = 1,
        PERSONAL,
        ALL

    }


}

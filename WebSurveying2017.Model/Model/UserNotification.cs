using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserNotification : IModelBase
    {
        [NotMapped]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        [Key, Column(Order = 2)]
        public int UserId { get; set; }
        public UserExtend User { get; set; }

        public bool IsRead { get; set; }



        public UserNotification()
        {

        }

        public UserNotification(int userId, Notification  n, bool isRead)
        {
            this.UserId = userId;
            this.Notification = n;
            this.IsRead = isRead;
        }

    }

  
}

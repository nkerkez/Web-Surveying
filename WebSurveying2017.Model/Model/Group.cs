using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class Group : IModelBase
    {
        public int Id { get; set; }

        [StringLength(60)]
        [Index(IsUnique = true)]
        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }
        public UserExtend User { get; set; }

        public ICollection<UserGroup> GroupMembers { get; set; }
        public ICollection<SurveyGroup> Surveys { get; set; }
        public ICollection<RequestGroup> Requests { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}

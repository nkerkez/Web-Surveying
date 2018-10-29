using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserGroup : IModelBase
    {
        [NotMapped]
        int IModelBase.Id { get; set; }

        [Key, Column(Order = 1)]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        [Key, Column(Order = 2)]
        public int UserId { get; set; }
        public UserExtend User { get; set; }
    }
}

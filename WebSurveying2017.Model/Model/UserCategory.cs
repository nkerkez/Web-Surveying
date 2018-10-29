using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserCategory : IModelBase
    {
        [NotMapped]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public UserExtend User { get; set; }
        [Key, Column(Order = 2)]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

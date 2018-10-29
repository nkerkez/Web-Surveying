using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class UserSurvey : IModelBase
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public UserExtend User { get; set; }

        [Key, Column(Order = 2)]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public string EncrptUserId { get; set; }

        [NotMapped]
        public int Id { get; set; }

      
    }
}

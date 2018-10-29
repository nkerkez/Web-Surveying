using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class ExcelFiles : IModelBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        public DateTime CreationDate { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
    }
}

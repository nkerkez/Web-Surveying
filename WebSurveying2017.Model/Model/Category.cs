using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSurveying2017.Model.Model
{
    public class Category : IModelBase
    {
        public int Id { get; set; }

        [StringLength(60)]
        [Index(IsUnique = true)]
        [Required]
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        public Category ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Survey> Surveys { get; set; }
        public ICollection<UserCategory> CategoryUsers { get; set; }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            SubCategories = new List<Category>();
            Surveys = new List<Survey>();
            CategoryUsers = new List<UserCategory>();
        }

        public Category()
        {
            SubCategories = new List<Category>();
            Surveys = new List<Survey>();
            CategoryUsers = new List<UserCategory>();
        }
    }
}

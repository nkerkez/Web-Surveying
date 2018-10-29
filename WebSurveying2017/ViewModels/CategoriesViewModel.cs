using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class CategoriesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public List<CategoriesViewModel> SubCategories { get; set; }
    }
}
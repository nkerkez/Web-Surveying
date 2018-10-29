using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class UserCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsModerator { get; set; }

        public List<UserCategoryViewModel> SubCategories { get; set; }
    }
}
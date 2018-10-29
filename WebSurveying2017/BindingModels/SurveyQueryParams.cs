using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.BindingModels
{
    public class SurveyQueryParams
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> ListOfCategories { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string QuestionText { get; set; }
    }

    public enum UserSearchEnum
    {
        AUTHOR = 1,
        FILLSURVEY,
        FAVORITESURVEY,
        GROUP,
        NOTHING
    }
}
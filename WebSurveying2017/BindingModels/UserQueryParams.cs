using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.BindingModels
{
    public class UserQueryParams
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public DateTime? BirthdayFrom { get; set; }

        public DateTime? BirthdayTo { get; set; }

        public List<string> RoleName { get; set; }

        public List<Gender> Gender { get; set; }
    }
}
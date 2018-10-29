using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string City { get; set; }

        public string RoleName { get; set; }

        public bool IsModerator { get; set; }

        public bool ExternalLogin { get; set; }
        
    }
}
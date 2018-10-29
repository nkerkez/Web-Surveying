using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public UserViewModel User { get; set; }

        public int NumbOfMembers { get; set; }

        public int NumbOfSurveys { get; set; }

        public int NumbOfRequests { get; set; }

        public bool IsMember { get; set; }
    }
}
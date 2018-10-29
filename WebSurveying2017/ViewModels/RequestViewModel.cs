using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class RequestsViewModel
    {
        public GroupViewModel Group { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
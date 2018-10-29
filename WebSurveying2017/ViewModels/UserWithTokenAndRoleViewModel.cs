using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class UserWithTokenAndRoleViewModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
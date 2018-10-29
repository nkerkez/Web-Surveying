using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebSurveying2017.Validation;

namespace WebSurveying2017.BindingModels
{
    [Validator(typeof(GroupValidation))]
    public class GroupBindingModel
    {
        public int Id;
        public int UserId;
        public string Name;
        public List<UserGroupBindingModel> UserGroupList;
    }

    public class UserGroupBindingModel
    {
        public int UserId;
        public int GroupId;
    }
}
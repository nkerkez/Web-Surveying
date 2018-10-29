using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Validation;

namespace WebSurveying2017.BindingModels
{
    [Validator(typeof(CommentValidation))]
    public class CommentBindingModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string UpdatedText { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastEdited { get; set; }

     //   public State CommentState { get; set; }

        public int? ParentId { get; set; }

        public int SurveyId { get; set; }

        public int UserId { get; set; }
       
    }
}
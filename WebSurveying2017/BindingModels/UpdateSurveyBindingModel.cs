using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Validation;

namespace WebSurveying2017.BindingModels
{
    [Validator(typeof(UpdateSurveyValidation))]
    public class UpdateSurveyBindingModel
    {
        public SurveyBindingModel Survey { get; set; }

        public List<int> UpdatedQuestions { get; set; }
    }
}
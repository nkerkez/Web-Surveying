using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class UpdateSurveyValidation : AbstractValidator<UpdateSurveyBindingModel>
    {
        public UpdateSurveyValidation()
        {
            RuleFor(x => x.Survey.Name)
                .NotEmpty()
                .WithMessage("The Survey name can't be blank")
                .Length(0, 100)
                .WithMessage("The Survey name can't be more then 100 characters");

            RuleFor(x => x.Survey.Description)
                .NotEmpty()
                .WithMessage("The Survey description can't be blank")
                .Length(0, 350)
                .WithMessage("The Survey name can't be more then 350 characters");

            RuleFor(x => x.Survey.Questions)
                .NotEmpty()
                .WithMessage("The Survey must has questions");
        }
    }
}
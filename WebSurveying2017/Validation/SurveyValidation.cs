using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class SurveyValidation : AbstractValidator<SurveyBindingModel>
    {
        public SurveyValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Naziv ankete ne sme biti prazan.")
                .Length(0, 100)
                .WithMessage("Naziv ankete ne sme biti duzi od 100 karaktera.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Opis ankete ne sme biti prazan.")
                .Length(0, 350)
                .WithMessage("Opis ankete ne sme biti duzi od 350 karaktera.");

            RuleFor(x => x.Questions)
                .NotEmpty()
                .WithMessage("Anketa mora imati pitanja.");
            RuleFor(x => x.Groups)
                .NotEmpty()
                .When( x => !x.Public)
                .WithMessage("Niste uneli grupe.");
            RuleFor(x => x.Groups)
                .Empty()
                .When(x => x.Public)
                .WithMessage("Niste uneli grupe.");

        }
    }
}
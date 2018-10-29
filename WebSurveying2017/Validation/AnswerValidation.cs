using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class AnswerValidation : AbstractValidator<AnswerBindingModel>
    {
        public AnswerValidation()
        {
            RuleFor(x => x.AnswerText)
                .NotEmpty()
                .WithMessage("Tekst odgovora ne sme biti prazan")
                .Length(0, 150)
                .WithMessage("Tekst odgovora ne sme biti duži od 150 karaktera");
        }
    }
}
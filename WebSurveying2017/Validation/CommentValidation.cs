using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class CommentValidation : AbstractValidator<CommentBindingModel>
    {

        public CommentValidation()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Tekst komentara ne sme biti prazan")
                .Length(0, 400)
                .WithMessage("Tekst komentara ne sme biti duži od 400 karaktera");

            RuleFor(x => x.SurveyId)
                .GreaterThan(0)
                .WithMessage("Survey id is undefined");
            
        }
    }
}
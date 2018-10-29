using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class CategoryValidation : AbstractValidator<CategoryBindingModel>
    {
        public CategoryValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ime kategorije ne sme biti prazno")
                .Length(0, 60)
                .WithMessage("Ime kategorije ne sme biti duže od 60 karaktera");

           
        }
    }
}
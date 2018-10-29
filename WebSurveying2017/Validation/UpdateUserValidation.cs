using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Models;

namespace WebSurveying2017.Validation
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserBindingModel> 
    {

        public UpdateUserValidation()
        {

            RuleFor(x => x.FirstName)
              .NotEmpty()
              .WithMessage("Ime ne sme biti prazno")
              .Length(0, 30)
              .WithMessage("Ime ne sme biti duže od 30 karaktera");

            RuleFor(x => x.LastName)
               .NotEmpty()
               .WithMessage("Prezime ne sme biti prazno")
               .Length(0, 30)
               .WithMessage("Prezime ne sme biti duže od 30 karaktera");

            RuleFor(x => x.City)
               .NotEmpty()
               .WithMessage("Grad ne sme biti prazan");
            RuleFor(x => x.Birthday)
               .NotEmpty()
               .WithMessage("Datum rodjenja ne sme biti prazan");


        }

    }
}
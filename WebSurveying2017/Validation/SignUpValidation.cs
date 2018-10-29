using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Models;

namespace WebSurveying2017.Validation
{
    public class SignUpValidation : AbstractValidator<RegisterBindingModel>
    {

        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
        public SignUpValidation()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage("Email ne sme biti prazan")
               .Matches(MatchEmailPattern)
               .WithMessage("Format Email-a nije ispravan"); 
               

            RuleFor(x => x.FirstName)
               .NotEmpty()
               .WithMessage("Ime ne sme biti prazno")
               .Length(0, 30)
               .WithMessage("Ime ne sme biti duže od 30 karaktera");

            RuleFor(x => x.City)
               .NotEmpty()
               .WithMessage("Grad ne sme biti prazan");

            RuleFor(x => x.LastName)
               .NotEmpty()
               .WithMessage("Prezime ne sme biti prazno")
               .Length(0, 30)
               .WithMessage("Prezime ne sme biti duže od 30 karaktera");

            RuleFor(x => x.RoleName)
              .NotEmpty()
              .WithMessage("Uloga ne sme biti prazna");

            RuleFor(x => x.Birthday)
               .NotEmpty()
               .WithMessage("Datum rodjenja ne sme biti prazan");

            RuleFor(x => x.Password)
              .NotEmpty()
              .WithMessage("Šifra ne sme biti prazna")
              .Length(5, 100)
              .WithMessage("Šifra ne sme biti kraća od 5 karaktera i duža od 100 karaktera");

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword)
                .When(x => !String.IsNullOrWhiteSpace(x.Password))
                .WithMessage("Šifre se ne poklapaju");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Pol ne sme biti prazan");




        }
    }
}
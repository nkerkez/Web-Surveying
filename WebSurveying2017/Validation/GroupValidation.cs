using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;

namespace WebSurveying2017.Validation
{
    public class GroupValidation : AbstractValidator<GroupBindingModel>
    {

        public GroupValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Naziv grupe ne sme biti prazan")
                .Length(3,60)
                .WithMessage("Naziv grupe mora biti manji od 60 karaktera. Minimalan broj karaktera je 3");

            RuleFor(x => x.UserGroupList)
                .NotEmpty()
                .WithMessage("Grupa mora imati članove");

        }

    }
}
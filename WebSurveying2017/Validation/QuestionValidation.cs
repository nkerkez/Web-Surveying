using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Validation
{
    public class QuestionValidation : AbstractValidator<QuestionBindingModel>
    {
        public QuestionValidation()
        {
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage("Tekst pitanja ne sme biti prazan")
                .Length(0, 150)
                .WithMessage("Tekst pitanja ne sme biti duži od 150 karaktera");

            RuleFor(x => x.AnswerType)
                .NotEmpty().
                WithMessage("Tip pitanja ne sme biti prazan");

            RuleFor(x => x.QuestionAnswers)
                .NotEmpty()
                .When(x => x.AnswerType != AnswerType.Text)
                .WithMessage(x => x.QuestionText + " : " +  "Pitanje mora imati ponudjene odgovore");

            RuleFor(x => x.OrdinalNumber)
               .NotNull()
               .WithMessage(x => x.QuestionText + " : " +  "Redni broj pitanja mora biti definisan")
               .Must(on => on > 0)
               .WithMessage(x => x.QuestionText + " : " +  "Redni broj pitanja mora viti veći od 0");

            
            RuleFor(x => x.MinNumbOfAnswers)
                .Must(ma => ma > 0)
                .WithMessage(x => x.QuestionText + " : " + "Minimalan broj izabranih odgovora mora biti veći od 0")
                .LessThanOrEqualTo(x => x.QuestionAnswers.Count())
                .When(x => x.AnswerType == AnswerType.MultiCheckBox || x.AnswerType == AnswerType.MultiSelect)
                .WithMessage(x => x.QuestionText + " : " + "Minimalan broj izabranih odgovora ne sme biti veći od broja ponuđenih odgovora");

            RuleFor(x => x.MaxNumbOfAnswers)
                 .Must(ma => ma > 1)
                 .When(x => x.AnswerType == AnswerType.MultiCheckBox || x.AnswerType == AnswerType.MultiSelect)
                 .WithMessage(x => x.QuestionText + " : " + "Maksimalan broj broj ponuđenih odgovora mora biti veći od 1");
            RuleFor(x => x.MaxNumbOfAnswers)
                 .GreaterThanOrEqualTo(x => x.MinNumbOfAnswers)
                 .WithMessage(x => x.QuestionText + " : " + "Maksimalan broj broj izabranih odgovora ne sme biti manji od minimalnog broja izabranih odgovora")
                 .LessThanOrEqualTo(x => x.QuestionAnswers.Count())
                 .When(x => x.AnswerType == AnswerType.MultiCheckBox || x.AnswerType == AnswerType.MultiSelect)
                 .WithMessage(x => x.QuestionText + " : " + "Maksimalan broj izabranih odgovora ne sme biti veći od broja ponuđenih odgovora");


            RuleFor(x => x.MaxNumbOfAnswers)
                .Must(ma => ma == 1)
                .When(x => x.AnswerType != AnswerType.MultiCheckBox && x.AnswerType != AnswerType.MultiSelect)
                .WithMessage(x => x.QuestionText + " : " + "Maksimalan broj izabranih odgovora mora biti 1");
            RuleFor(x => x.MinNumbOfAnswers)
                .Must(ma => ma == 1)
                .When(x => x.AnswerType != AnswerType.MultiCheckBox && x.AnswerType != AnswerType.MultiSelect)
                .WithMessage(x => x.QuestionText + " : " + "Minimalan broj izabranih odgovora mora biti 1");
        }


    }
}
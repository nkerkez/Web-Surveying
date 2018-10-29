using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindModels;

namespace WebSurveying2017.Validation
{
    public class UserAnswerValidation : AbstractValidator<UserAnswerBindingModel>
    {

        public UserAnswerValidation()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("Question id is undefined");

            
        }
    }
}
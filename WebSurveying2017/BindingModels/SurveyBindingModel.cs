using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Validation;

namespace WebSurveying2017.BindingModels
{
    [Validator(typeof(SurveyValidation))]
    public class SurveyBindingModel
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        
        public string Description { get; set; }
        

        public State State { get; set; }

        public bool Public { get; set; }

        public bool ResultAuthor { get; set; }

        public bool Anonymous { get; set; }

        public DateTime CreationDate { get; set; }

        public int? CategoryId { get; set; }

        public int UserId { get; set; }
        public UserExtend Author { get; set; }

        public List<QuestionBindingModel> Questions { get; set; }

        public List<SurveyGroupBindingMModel> Groups { get; set; }

        
    }
    public class SurveyGroupBindingMModel
    {

        public int GroupId { get; set;  }
    }
    [Validator(typeof(QuestionValidation))]
    public class QuestionBindingModel
    {
        public int Id { get; set; }

        
        public string QuestionText { get; set; }

        public int MinNumbOfAnswers { get; set; }

        public int MaxNumbOfAnswers { get; set; }

        
        public AnswerType AnswerType { get; set; }

        public bool ExplainAnswer { get; set; }

        public bool Required { get; set; }

        public int OrdinalNumber { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public ICollection<AnswerBindingModel> QuestionAnswers { get; set; }

        
    }

    [Validator(typeof(AnswerValidation))]
    public class AnswerBindingModel 
    {
        public int Id { get; set; }

        public int OrdinalNumber { get; set; }

        public string AnswerText { get; set; }


        public int? QuestionId { get; set; }
        public Question Question { get; set; }

       
    }
}
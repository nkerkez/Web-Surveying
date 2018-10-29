using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class SurveyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public State State { get; set; }

        public int UserId { get; set; }

        public UserViewModel UserVM { get;set; }

        public DateTime CreationDate { get;set; }

        public int? CategoryId { get; set; }

        public CategoryViewModel Category { get; set; }

        public List<int> FilledSurvey { get; set; }
        
        public List<int> FavoriteSurveyFor { get; set; }
        
        public bool Public { get; set; }

        public bool ResultAuthor { get; set; }

        public bool Anonymous { get; set; }

        public int CommentCount { get; set; }
        public int NumbOfUsers { get; set; }
        public List<QuestionVM> Questions { get; set; }
        public List<SurveyGroupViewModel> Groups { get; set; }
    }

    public class SurveyGroupViewModel
    {
        public int GroupId { get; set; }
    }

    public class QuestionVM
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public int SurveyId { get; set; }

        public int MaxNumbOfAnswers { get; set; }

        public int MinNumbOfAnswers { get; set; }

        public AnswerType AnswerType { get; set; }

        public int OrdinalNumber { get; set; }

        public bool Required { get; set; }

        public List<AnswerVM> QuestionAnswers { get; set; }

        public List<AnswerVM> UserAnswers { get; set; }

    }

    public class AnswerVM
    {
        public int Id { get; set; }

        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public int? QuestionId { get; set; }
    }
}
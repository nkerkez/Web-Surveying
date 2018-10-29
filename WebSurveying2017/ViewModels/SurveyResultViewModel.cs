
using System.Collections.Generic;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class SurveyResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool Anonymous { get; set; }
        public int NumbOfUsers { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
        public int UsersUnder18Count { get; set; }
        public int Users18_25Count { get; set; }
        public int Users26_40Count { get; set; }
        public int Users41_60Count { get; set; }
        public int Users61Count { get; set; }
        public int MaleUnder18 { get; set; }
        public int Male18_25 { get; set; }
        public int Male26_40 { get; set; }
        public int Male41_60 { get; set; }
        public int Male61 { get; set; }
        public int FemaleUnder18 { get; set; }
        public int Female18_25 { get; set; }
        public int Female26_40 { get; set; }
        public int Female41_60 { get; set; }
        public int Female61 { get; set; }
        public int Unknown { get; set; }
        public List<QuestionResultVM> Questions{get;set;}

        public SurveyResultViewModel()
        {
            Questions = new List<QuestionResultVM>();
        }

    }


    public class QuestionResultVM
    {
        public int Id { get; set; }
        public int NumbOfUsers { get; set; }
        public int NumbOfUA { get; set; }
        public int MinNumbOfAnswers { get; set; }
        public int MaxNumbOfAnswers { get; set; }
        public int OrdinalNumber { get; set; }
        public string QuestionText { get; set; }

        public AnswerType AnswerType { get; set; }
        public List<UsersAnswersVM> QuestionAnswers {get;set;}
        public QuestionResultVM()
        {
            QuestionAnswers = new List<UsersAnswersVM>();
        }
    }

    public class UsersAnswersVM
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public int Count { get; set; }
        public bool IsAnswerOfUser  { get; set; }
    }
}
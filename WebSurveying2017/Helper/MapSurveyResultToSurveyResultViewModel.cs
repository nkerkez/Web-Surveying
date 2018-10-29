using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Helper
{
    public class MapSurveyResultToSurveyResultViewModel
    {
        private ISurveyService surveyService;

        public MapSurveyResultToSurveyResultViewModel(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        public void SetSurveyResultVMValues(SurveyResultViewModel surveyVM, Survey survey,
          List<ApplicationUser> users, int? userId, bool forExcel)
        {
            surveyVM.NumbOfUsers = survey.Users.Count();
            if (!survey.Anonymous)
            {
                surveyVM.MaleCount = users.Where(u => u.Gender == Gender.MALE).Count();
                surveyVM.FemaleCount = users.Where(u => u.Gender == Gender.FEMALE).Count();



                var und18 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(18).CompareTo(DateTime.Now) > 0);
                var u18_25 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(26).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(18).CompareTo(DateTime.Now) <= 0);
                var u26_40 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(41).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(26).CompareTo(DateTime.Now) <= 0);
                var u41_60 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(61).CompareTo(DateTime.Now) > 0
                && ((DateTime)u.Birthday).AddYears(41).CompareTo(DateTime.Now) <= 0);
                var u61 = users.Where(u => u.Birthday != null && ((DateTime)u.Birthday).AddYears(61).CompareTo(DateTime.Now) <= 0);

                surveyVM.Unknown = users.Where(u => u.Birthday == null).Count();
                surveyVM.UsersUnder18Count = und18.Count();
                surveyVM.Users18_25Count = u18_25.Count();
                surveyVM.Users26_40Count = u26_40.Count();
                surveyVM.Users41_60Count = u41_60.Count();
                surveyVM.Users61Count = u61.Count();

                if (forExcel)
                {
                    surveyVM.MaleUnder18 = und18.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.FemaleUnder18 = und18.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male18_25 = u18_25.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female18_25 = u18_25.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male26_40 = u26_40.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female26_40 = u26_40.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male41_60 = u41_60.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female41_60 = u41_60.Where(u => u.Gender == Gender.FEMALE).Count();

                    surveyVM.Male61 = u61.Where(u => u.Gender == Gender.MALE).Count();
                    surveyVM.Female61 = u61.Where(u => u.Gender == Gender.FEMALE).Count();
                }
            }
            foreach (var questionVM in surveyVM.Questions)
            {


                questionVM.NumbOfUsers = surveyService.NumbOfUsers(questionVM.Id);

                if (questionVM.AnswerType == AnswerType.Text)
                {
                    // if answer type text get answer from db
                    UserAnswer userAnswer = surveyService.GetUserAnswerWithAnswerObj((int)questionVM.Id, (int)userId);
                    if (userAnswer != null)
                    {
                        // must fill answer text
                        UsersAnswersVM _ua = new UsersAnswersVM
                        {
                            AnswerText = userAnswer.Answer.AnswerText
                        };

                        questionVM.QuestionAnswers.Add(_ua);
                    }

                }
                else
                {
                    var question = survey.Questions.Where(q => q.Id == questionVM.Id).FirstOrDefault();

                    foreach (var answerVM in questionVM.QuestionAnswers)
                    {
                        var answer = question.QuestionAnswers.Where(a => a.Id == answerVM.Id).FirstOrDefault();
                        answerVM.Count = answer.Users.ToList().Count();
                        answerVM.IsAnswerOfUser = false;


                        foreach (var ua in answer.Users)
                        {
                            if (ua.UserId == userId)
                            {
                                answerVM.IsAnswerOfUser = true;
                                break;
                            }
                        }


                        questionVM.NumbOfUA += answerVM.Count;

                    }
                }
            }

        }
    }
}

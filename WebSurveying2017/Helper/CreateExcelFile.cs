using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Service.Service;
using WebSurveying2017.ViewModels;
using Combinatorics.Collections;

namespace WebSurveying2017.Helper
{
    public class CreateExcelFile
    {
        ISurveyService surveyService;

        public CreateExcelFile(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }
        public void CreateSurveyWorksheet(ExcelWorksheet ws, SurveyResultViewModel surveyVM)
        {
            string[] secondRowValues = new string[] { "Manje od 18 godina", "%", "18 - 25", "%", "26 - 40", "%", "41 - 60", "%", "Vise od 60 godina", "%", "Svi godisnji uzrasti", "%" };
            double[] thirdRowValues = new double[]
            {
                surveyVM.MaleUnder18, (double)surveyVM.MaleUnder18 * 100 / surveyVM.NumbOfUsers, surveyVM.Male18_25, (double)surveyVM.Male18_25 * 100 / surveyVM.NumbOfUsers,
                surveyVM.Male26_40, (double)surveyVM.Male26_40 * 100 / surveyVM.NumbOfUsers, surveyVM.Male41_60, (double)surveyVM.Male41_60 * 100 / surveyVM.NumbOfUsers,
                surveyVM.Male61, (double)surveyVM.Male61 * 100 / surveyVM.NumbOfUsers,surveyVM.MaleCount, (double)surveyVM.MaleCount * 100 / surveyVM.NumbOfUsers
            };

            double[] fourthRowValues = new double[]
            {
                surveyVM.FemaleUnder18, (double)surveyVM.FemaleUnder18 * 100 / surveyVM.NumbOfUsers, surveyVM.Female18_25, (double)surveyVM.Female18_25 * 100 / surveyVM.NumbOfUsers,
                surveyVM.Female26_40, (double)surveyVM.Female26_40 * 100 / surveyVM.NumbOfUsers, surveyVM.Female41_60, (double)surveyVM.Female41_60 * 100 / surveyVM.NumbOfUsers,
                surveyVM.Female61, (double)surveyVM.Female61 * 100 / surveyVM.NumbOfUsers,surveyVM.FemaleCount, (double)surveyVM.FemaleCount * 100 / surveyVM.NumbOfUsers
            };

            double[] fifthRowValues = new double[]
            {
                surveyVM.UsersUnder18Count, (double)surveyVM.UsersUnder18Count * 100 / surveyVM.NumbOfUsers, surveyVM.Users18_25Count, (double)surveyVM.Users18_25Count * 100 / surveyVM.NumbOfUsers,
                surveyVM.Users26_40Count, (double)surveyVM.Users26_40Count * 100 / surveyVM.NumbOfUsers, surveyVM.Users41_60Count, (double)surveyVM.Users41_60Count * 100 / surveyVM.NumbOfUsers,
                surveyVM.Users61Count, (double)surveyVM.Users61Count * 100 / surveyVM.NumbOfUsers,surveyVM.NumbOfUsers, (double)surveyVM.NumbOfUsers * 100 / surveyVM.NumbOfUsers
            };
            ws.SetValue(2, 2, surveyVM.Name);
            ws.SetValue(4, 2, "Muskarci");
            ws.SetValue(5, 2, "Zene");
            ws.SetValue(6, 2, "Muskarci i zene");
            for (int i = 3; i < secondRowValues.Count() + 3; i++)
            {
                ws.SetValue(3, i, secondRowValues[i-3]);
                ws.SetValue(4, i, thirdRowValues[i-3]);
                ws.SetValue(5, i, fourthRowValues[i-3]);
                ws.SetValue(6, i, fifthRowValues[i-3]);
            }
            this.SetSurveyStyle(ws);
        }

        public void CreateForTextType(ExcelWorksheet ws , Survey survey, Question question)
        {
            var users = surveyService.UsersWhoFilledSurvey(survey.Users.Select(us => us.UserId).ToList()).ToList();
            var answers = surveyService.GetUserAnswerForQuestion(question.Id);
            var row = 2;
            var col = 2;
            ws.Cells[row, col].Value = question.QuestionText;
            row++;
            ws.Cells[row, 2].Value = "Ime";
            ws.Cells[row, 3].Value = "Prezime";
            ws.Cells[row, 4].Value = "Odgovor";
            row++;
            users.ForEach(u =>
            {
                ws.Cells[row, 2].Value = survey.Anonymous? "******" : u.FirstName;
                ws.Cells[row, 3].Value = survey.Anonymous ? "******" : u.LastName;
                ws.Cells[row, 4].Value = answers.Where(ua => ua.UserId == u.Id).Count() == 0 ? "Nije odgovorio" : answers.Where(ua => ua.UserId == u.Id).First().Answer.AnswerText;
                row++;
            });
            ws.Cells[2, 2, 2, 4].Merge = true;
            ws.Cells[2, 2, 2, 4].Style.Font.Size = 16;
            ws.Cells[2, 2, 2, 4].Style.Font.Bold = true;
            ws.Cells[2, 2, 2, 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            ws.Cells[3, 2, 3, 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            ws.Cells[2, 2, users.Count + 3, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            ws.Cells[3, 2, users.Count + 3, 4].Style.Font.Bold = true ;
            ws.Cells[3, 2, users.Count + 3, 4].Style.Font.Italic = true;
            ws.Cells[3, 2, users.Count + 3, 3].Style.Font.Color.SetColor(Color.Green);
            ws.Cells[3, 2, users.Count + 3, 4].Style.WrapText = true;
        }
        public void CreateQuestionWorksheet(ExcelPackage e, QuestionResultVM questionVM, SurveyResultViewModel surveyVM, Survey survey)
        {
            var ws = e.Workbook.Worksheets.Add((surveyVM.Questions.IndexOf(questionVM) + 1) +". " + questionVM.QuestionText);
            //first row - question text

            var question = survey.Questions.Where(q => q.Id == questionVM.Id).FirstOrDefault();
            if (questionVM.AnswerType == AnswerType.Text)
            {
                this.CreateForTextType(ws, survey, question);
                return;
            }
            ws.SetValue(2, 2, questionVM.QuestionText);
            var col = 4;

            //fill columns
           
                questionVM.QuestionAnswers.ForEach(qa =>
                {
                    var row = 3;
                // [2,3] ponudjen odgovor
                ws.SetValue(row, col, qa.AnswerText);
                    var answer = question.QuestionAnswers.Where(an => an.Id == qa.Id).FirstOrDefault();
                    var users = answer.Users.Select(u => u.UserId);
                    row++;
                    //other questions
                    if (survey.Questions.Where(q => q.AnswerType != AnswerType.Text).Count() > 1)
                    {
                        surveyVM.Questions.Where(q => q.Id != questionVM.Id && q.AnswerType != AnswerType.Text).ToList().ForEach(q =>
                        {

                            var _question = survey.Questions.Where(_q => _q.Id == q.Id).FirstOrDefault();
                        //question text
                        ws.SetValue(row, 2, q.QuestionText);

                            row++;
                            q.QuestionAnswers.ForEach(_qa =>
                            {
                                var _answer = _question.QuestionAnswers.Where(an => an.Id == _qa.Id).FirstOrDefault();
                                var _users = _answer.Users.Select(u => u.UserId);
                            //answer text
                            if (col == 4)
                                    ws.SetValue(row, 3, _qa.AnswerText);
                                var value = users.Where(u => _users.Contains(u)).Count();
                                ws.SetValue(row, col, value);
                                row++;
                            });

                        });
                        col++;
                    }
                    else
                    {
                        col++;
                    }
                });
            
            {
                var users = surveyService.UsersWhoFilledSurvey(survey.Users.Select(us => us.UserId).ToList()).ToList();
                col++;
                var row = 3;

                var oldCol = col;
                users.ToList().ForEach(user =>
                {
                    col = oldCol;
                    ws.Cells[row, col].Value = survey.Anonymous ? "******" :  user.FirstName;
                    col++;
                    ws.Cells[row, col].Value = survey.Anonymous ? "******" : user.LastName;
                    col++;
                    ws.Cells[row, col].Value = survey.Anonymous ? "******" : (user.City == null ? "" : user.City);
                    col++;
                    ws.Cells[row, col].Value = survey.Anonymous ? "******" : (user.Birthday == null ? "" : user.Birthday.ToString());
                    col++;
                    var questionWithUserAnswer = survey.Questions.Where(quest => quest.Id == questionVM.Id).FirstOrDefault();
                    questionWithUserAnswer.QuestionAnswers.ToList().ForEach(
                      a =>
                      {
                          var color = a.Users.Select(ua => ua.UserId).Contains(user.Id) ? Color.Green : Color.Red;
                          ws.Cells[row, col].Style.Font.Color.SetColor(color);
                          ws.Cells[row, col].Value = a.AnswerText;


                          col++;
                      });
                    row++;

                });

                ws.Cells[3, oldCol, row, col - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                ws.Cells[3, oldCol, row, col - 1].Style.Font.Italic = true;
                ws.Cells[3, oldCol, row, col - 1].Style.Font.Bold = true;
                ws.Cells[3, oldCol, row, col - 1].Style.WrapText = true;
            }
            if(question.AnswerType == AnswerType.MultiCheckBox || question.AnswerType == AnswerType.MultiSelect)
            {
                int min = question.MinNumbOfAnswers;
                int max = question.MaxNumbOfAnswers;
                var answers = question.QuestionAnswers.Select(a => a.Id).ToList();
                var oldCol = col;
                col++;
                oldCol = col;
                var row = 3;
                for(var i = min; i <= max; i++)
                {
                    
                    //create combinations 
                    var combinations = new Combinations<int>(answers, i);
                    foreach(var combination in combinations)
                    {
                        col = oldCol;
                        //set answer text
                        combination.ToList().ForEach(answerId =>
                        {
                            ws.Cells[row, col].Value = question.QuestionAnswers.Where(a => a.Id == answerId).First().AnswerText;
                            col++;
                        });
                        var userAnswersByUser = question.UserAnswers.GroupBy(u => u.UserId);
                        var numbOfUsers = 0;

                        ws.Cells[row, oldCol, row, col - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                        foreach (var userAnswers in userAnswersByUser)
                        {
                            
                            if (userAnswers.All(ua => combination.Contains(ua.AnswerId)) 
                                && combination.All(c => userAnswers.Select(ua => ua.AnswerId).Contains(c)))
                                numbOfUsers++;
                        }
                        ws.Cells[row, col].Value = numbOfUsers;
                        ws.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, Color.MediumAquamarine);
                        
                        row++;
                    }
                    
                }
            }
            this.SetQuestionStyle(ws, questionVM, surveyVM);

        }

        public void CreateQuestionsOnSurveyWorksheet(ExcelWorksheet ws, SurveyResultViewModel surveyVM)
        {
            var i = 9;
            ws.SetValue(i, 2, "Redni broj pitanja");
            ws.SetValue(i, 3, "Tekst pitanja");
            ws.SetValue(i, 4, "Ponudjeni odgovori");
            ws.SetValue(i, 5, "Broj korisnika koji su izabrali odgovor");
            ws.SetValue(i, 6, "% u odnosu na broj korisnika koji su popunili anketu");
            ws.SetValue(i, 7, "% u odnosu na broj korisnika koji su odgovorili na pitanje");
            ws.SetValue(i, 8, "% u odnosu na broj odgovara na pitanje");

            i++;
            surveyVM.Questions.Where(q => q.AnswerType!=AnswerType.Text).ToList().ForEach(q =>
            {
                
                ws.SetValue(i, 2, q.OrdinalNumber);
                ws.SetValue(i, 3, q.QuestionText);

                ws.SetValue(i, 6, surveyVM.NumbOfUsers);
                ws.SetValue(i, 7, q.NumbOfUsers);
                ws.SetValue(i, 8, q.NumbOfUA);

                i++;
                q.QuestionAnswers.ForEach(qa =>
                {
                    ws.SetValue(i, 4, qa.AnswerText);
                    ws.SetValue(i, 5, qa.Count);
                    ws.SetValue(i, 6, qa.Count * 100 / surveyVM.NumbOfUsers);
                    ws.SetValue(i, 7, qa.Count * 100 / q.NumbOfUsers);
                    ws.SetValue(i, 8, qa.Count * 100 / q.NumbOfUA);

                    i++;
                });

            });
            this.SetQuestionInSurveyWorksheetStyle(ws, surveyVM);
        }
        private void SetQuestionStyle(ExcelWorksheet ws, QuestionResultVM questionVM, SurveyResultViewModel surveyVM)
        {
            
                ws.Cells[2, 2, 2, questionVM.QuestionAnswers.Count + 3].Merge = true;
                ws.Cells[2, 2, 2, questionVM.QuestionAnswers.Count + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[2, 2].Style.Font.Size = 16;
                ws.Cells[2, 2].Style.Font.Bold = true;
                ws.Row(2).Height = 24;
                ws.Cells[2, 2, 2, questionVM.QuestionAnswers.Count + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[3, 2, 3, questionVM.QuestionAnswers.Count + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                ws.Cells[3, 3, 3, questionVM.QuestionAnswers.Count + 3].Style.Font.Size = 12;
                ws.Cells[3, 3, 3, questionVM.QuestionAnswers.Count + 3].Style.Font.Bold = true;
            
            var numbOfRows = 3;
            if (surveyVM.Questions.Where(q => q.AnswerType != AnswerType.Text).Count() > 1)
            {
                surveyVM.Questions.Where(q => q.Id != questionVM.Id && q.AnswerType != AnswerType.Text).ToList().ForEach(

                quest =>
                {
                    numbOfRows++;
                    if (surveyVM.Questions.Where(q => q.AnswerType != AnswerType.Text).Count() > 1)
                    {
                        ws.Cells[numbOfRows, 2].Style.Font.Size = 14;
                        ws.Cells[numbOfRows, 2].Style.Font.Bold = true;
                        ws.Cells[numbOfRows, 2, numbOfRows, questionVM.QuestionAnswers.Count + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        ws.Cells[numbOfRows, 2, numbOfRows, 2].Style.WrapText = true;
                    }
                    numbOfRows += quest.QuestionAnswers.Count;
                    ws.Cells[numbOfRows, 2, numbOfRows, questionVM.QuestionAnswers.Count + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    ws.Cells[numbOfRows - quest.QuestionAnswers.Count, 3, numbOfRows, quest.QuestionAnswers.Count + 3].Style.Font.Bold = true;
                    ws.Cells[numbOfRows - quest.QuestionAnswers.Count, 3, numbOfRows, quest.QuestionAnswers.Count + 3].Style.Font.Italic = true;
                }
            );
            }

            ws.Cells[ 2, 2, 2, questionVM.QuestionAnswers.Count + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells[ numbOfRows, 2, numbOfRows, questionVM.QuestionAnswers.Count + 3 ].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            
                ws.Cells[2, 2, numbOfRows, 2].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            if (surveyVM.Questions.Where(q => q.AnswerType != AnswerType.Text).Count() > 1)
            {
                ws.Cells[4, 3, numbOfRows, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            ws.Cells[ 2, questionVM.QuestionAnswers.Count+3, numbOfRows, questionVM.QuestionAnswers.Count+3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

            ws.Cells[2, 2, numbOfRows, questionVM.QuestionAnswers.Count + 3].Style.WrapText = true;

        }
        private void SetSurveyStyle(ExcelWorksheet ws)
        {
            ws.Cells["B2:N2"].Merge = true;
            ws.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            
            ws.Cells["B2:N2"].Style.Font.Size = 18;
            //border
            ws.Cells["B2:N2"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B2:N2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B7:N7"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B2:B6"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["N2:N6"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B3:N3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            ws.Cells["B6:N6"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            ws.Cells["B3:B5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            ws.Cells["L3:L6"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            //font
            ws.Cells["B3:N3"].Style.Font.Bold = true;
            ws.Cells["B6:N6"].Style.Font.Bold = true;
            ws.Cells["B3:B6"].Style.Font.Bold = true;
            ws.Cells["M3:M6"].Style.Font.Bold = true;
            ws.Cells["B3:N3"].Style.Font.Size = 12;
            ws.Cells["B6:N6"].Style.Font.Size = 12;
            ws.Cells["B3:B6"].Style.Font.Size = 12;
            ws.Cells["M3:M6"].Style.Font.Size = 12;

            ws.Cells["D3:D6"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells["F3:F6"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells["H3:H6"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells["J3:J6"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells["L3:L6"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells["N3:N6"].Style.Font.Color.SetColor(Color.CornflowerBlue);

            ws.Cells[3,3,6,14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            ws.Cells[2, 2, 6, 14].Style.WrapText = true;
            ws.Cells["D3:D6"].Style.Numberformat.Format = "0.00";
            ws.Cells["F3:F6"].Style.Numberformat.Format = "0.00";
            ws.Cells["H3:H6"].Style.Numberformat.Format = "0.00";
            ws.Cells["J3:J6"].Style.Numberformat.Format = "0.00";
            ws.Cells["L3:L6"].Style.Numberformat.Format = "0.00";
            ws.Cells["N3:N6"].Style.Numberformat.Format = "0.00";
            
        }
        public void CreateUserWorksheet(ExcelPackage e, ApplicationUser user, Survey survey, List<UserAnswer> answers, int index)
        {
            ExcelWorksheet ws = null; 
            int row = 4;
            if (!survey.Anonymous)
            {
                ws = e.Workbook.Worksheets.Add((index + 1) + "." +  user.FirstName + " " + user.LastName);
                ws.Cells[2, 2].Value = "Ime i prezime";
                ws.Cells[2, 3].Value = user.FirstName + " " + user.LastName;
                ws.Cells[3, 2].Value = "Godina rođenja";
                ws.Cells[3, 3].Value = user.Birthday == null ? "" : user.Birthday.ToString();
                ws.Cells[4, 2].Value = "Grad";
                ws.Cells[4, 3].Value = user.City;
                ws.Cells[5, 2].Value = "Pol";
                ws.Cells[5, 3].Value = user.Gender == Gender.MALE ? "Muško" : "Žensko";
                ws.Cells[6, 2].Value = "Naziv ankete";
                ws.Cells[6, 3].Value = survey.Name;
                ws.Cells[7, 2].Value = "Redni broj pitanja";
                ws.Cells[7, 3].Value = "Naziv pitanja";
                row = 8;
                ws.Cells[2, 2, 2, 4].Style.Font.Size = 18;
                ws.Row(2).Height = 30;
                //-----
                ws.Cells[6, 2, 6, 4].Style.Font.Size = 16;
                ws.Row(6).Height = 24;
                ws.Cells[6, 2, 6, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[7, 2, 7, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            else
            {
                ws = e.Workbook.Worksheets.Add("Korisnik " + ( index + 1));
                ws.Cells[2, 2].Value = "Naziv ankete";
                ws.Cells[2, 3].Value = survey.Name;
                ws.Cells[3, 2].Value = "Redni broj pitanja";
                ws.Cells[3, 3].Value = "Naziv pitanja";
                ws.Cells[2, 2, 2, 4].Style.Font.Size = 18;
                ws.Row(2).Height = 30;
                ws.Cells[3, 2, 3, 4].Style.Font.Size = 16;
                ws.Row(3).Height = 24;
                ws.Cells[3, 2, 3, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            
            
            
            survey.Questions.ToList().ForEach(q =>
            {

                
                ws.Cells[row, 2].Value = q.OrdinalNumber;
                ws.Cells[row, 3].Value = q.QuestionText;
                ws.Cells[row, 2, row, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells[row, 2, row, 4].Style.Font.Size = 14;
                if (q.AnswerType == AnswerType.Text)
                {
                    var answer = answers.Where(ua => ua.QuestionId == q.Id).FirstOrDefault();
                    ws.Cells[row, 4].Value = answer == null ? "" : answer.Answer.AnswerText;
                    ws.Cells[row, 4].Style.Font.Color.SetColor(Color.Red);
                    ws.Cells[row, 4].Style.Font.Italic = true;
                    ws.Cells[row, 4].Style.Font.Bold = true;
                }
                else
                {
                    row++;
                    q.QuestionAnswers.ToList().ForEach(qa =>
                    {
                        ws.Cells[row, 4].Value = qa.AnswerText;
                        if(qa.Users.Select(ua => ua.UserId).Contains(user.Id))
                        {
                            ws.Cells[row, 4].Style.Font.Color.SetColor(Color.Red);
                            ws.Cells[row, 4].Style.Font.Italic = true;
                            ws.Cells[row, 4].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 4].Style.Font.Size = 14;
                        }
                        row++;
                    });
                    
                }
                row++;
            });
            row--;
            ws.Cells[2, 2, row, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            ws.Cells[2, 2, row, 4].Style.Font.Bold = true;
            ws.Cells.Style.WrapText = true;
            ws.Column(2).Width = 50;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 50;
        }
        private void SetQuestionInSurveyWorksheetStyle(ExcelWorksheet ws, SurveyResultViewModel surveyVM)
        {
            int numbOfRows = 9;
            surveyVM.Questions.Where(q => q.AnswerType != AnswerType.Text).ToList().ForEach(
                q =>
                {
                    numbOfRows++;
                    ws.Cells["B" + numbOfRows + ":B" + numbOfRows].Style.Font.Bold = true;
                    ws.Cells["B" + numbOfRows + ":B" + numbOfRows].Style.Font.Size = 12;
                    ws.Cells["C"+numbOfRows + ":C" + numbOfRows].Style.Font.Bold = true;
                    ws.Cells["C" + numbOfRows + ":C" + numbOfRows].Style.Font.Size = 12;
                    ws.Cells["D" + (numbOfRows + 1) + ":D" + (numbOfRows + q.QuestionAnswers.Count)].Style.Font.Bold = true;
                    ws.Cells["D" + (numbOfRows + 1) + ":D" + (numbOfRows + q.QuestionAnswers.Count)].Style.Font.Size = 12;
                    ws.Cells["F" + (numbOfRows + 1) + ":F" + (numbOfRows + q.QuestionAnswers.Count)].Style.Font.Color.SetColor(Color.CornflowerBlue);
                    ws.Cells["G" + (numbOfRows + 1) + ":G" + (numbOfRows + q.QuestionAnswers.Count)].Style.Font.Color.SetColor(Color.CornflowerBlue);
                    ws.Cells["H" + (numbOfRows + 1) + ":H" + (numbOfRows + q.QuestionAnswers.Count)].Style.Font.Color.SetColor(Color.CornflowerBlue);
                    ws.Cells["F" + (numbOfRows + 1) + ":F" + (numbOfRows + q.QuestionAnswers.Count)].Style.Numberformat.Format = "0.00";
                    ws.Cells["G" + (numbOfRows + 1) + ":G" + (numbOfRows + q.QuestionAnswers.Count)].Style.Numberformat.Format = "0.00";
                    ws.Cells["H" + (numbOfRows + 1) + ":H" + (numbOfRows + q.QuestionAnswers.Count)].Style.Numberformat.Format = "0.00";
                    numbOfRows += q.QuestionAnswers.Count;
                    ws.Cells["B" + numbOfRows + ":H" + numbOfRows].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                });
            ws.Cells["B9:H9"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B9:H9"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B9:B" + numbOfRows].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["H9:H" + numbOfRows].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            ws.Cells["B"+numbOfRows+":H" + numbOfRows].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
           

            ws.Cells["F9:H9"].Style.Font.Color.SetColor(Color.CornflowerBlue);
            ws.Cells[9, 2, numbOfRows, 8].Style.WrapText = true;

            ws.Cells["B9:H9"].Style.Font.Bold = true;
            ws.Cells["B9:H9"].Style.Font.Size = 12;

        }
    }

    
}
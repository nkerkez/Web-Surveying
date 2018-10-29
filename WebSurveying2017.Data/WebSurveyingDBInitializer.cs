
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.Data
{
    class WebSurveyingDBInitializer : DropCreateDatabaseIfModelChanges<WebSurveyingContext>
    {

        //protected override void Seed(WebSurveyingContext context)
        //{
        //    //-----------------USERS--------------------------------------------------------

            
        //    ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser,MyRole, int, MyLogin, MyUserRole, MyClaim>(context));
        //    ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<MyRole, int, MyUserRole>(context));

        //    UserExtend nikola = new UserExtend();
        //    UserExtend david = new  UserExtend();
        //    UserExtend jelena = new UserExtend();
        //    UserExtend marija = new UserExtend();

        //    /*
        //    ApplicationUser nikola = new ApplicationUser("Nikola", "Kerkez", "kerkezn1@gmail.com");
        //    ApplicationUser david = new ApplicationUser("David", "Kerkez", "kerkezd2@gmail.com");
        //    ApplicationUser jelena = new ApplicationUser("Jelena", "Kerkez", "kerkezj3@gmail.com");
        //    ApplicationUser marija = new ApplicationUser("Marija", "Kerkez", "kerkezm4@gmail.com");
        //    */

        //    ApplicationUser user1 = new ApplicationUser()
        //    {
        //        Email = "nikola@gmail.com",
        //        UserName = "kerkeznikola",
        //        User = nikola,
        //        FirstName = "Nikola",
        //        LastName = "Kerkez",
        //        City = "Novi Sad",
        //        Gender = Gender.MALE,
        //        Birthday = DateTime.Now
        //    };
        //    ApplicationUser user2 = new ApplicationUser()
        //    {
        //        Email = "david@gmail.com",
        //        UserName = "kerkezdavid",
        //        User = david,
        //        FirstName = "David",
        //        LastName = "Kerkez",
        //        City = "Beograd",
        //        Gender = Gender.MALE,
        //        Birthday = DateTime.Now
        //    };
        //    ApplicationUser user3 = new ApplicationUser()
        //    {
        //        Email = "jelena@gmail.com",
        //        UserName = "kerkezjelena",
        //        User = jelena,
        //        FirstName = "Jelena",
        //        LastName = "Kerkez",
        //        City = "Subotica",
        //        Gender = Gender.FEMALE,
        //        Birthday = DateTime.Now
        //    };
        //    ApplicationUser user4 = new ApplicationUser()
        //    {
        //        Email = "marija@gmail.com",
        //        UserName = "kerkezmarija",
        //        User = marija,
        //        FirstName = "Marija",
        //        LastName = "Kerkez",
        //        City = "Nova Pazova",
        //        Gender = Gender.FEMALE,
        //        Birthday = DateTime.Now
        //    };

        //    roleManager.Create(new MyRole() { Name = "User"});
        //    roleManager.Create(new MyRole() { Name = "Moderator" });
        //    roleManager.Create(new MyRole() { Name = "Admin" });
        //    manager.Create(user1, "1111111");
        //    manager.Create(user2, "1111111");
        //    manager.Create(user3, "1111111");
        //    manager.Create(user4, "1111111");

        //    var adminUser = manager.FindByName(user1.UserName);
            

        //    manager.AddToRole(adminUser.Id, "Admin");
        //    manager.AddToRole(manager.FindByName(user2.UserName).Id, "Moderator");
        //    manager.AddToRole(manager.FindByName(user3.UserName).Id, "Moderator");
        //    manager.AddToRole(manager.FindByName(user4.UserName).Id, "Moderator");






        //    //------------------CATEGORIES--------------------------------------------------

        //    Category sportC = new Category(1, "Sport");
        //    Category programmingC = new Category(2, "Programiranje");
        //    Category football = new Category(3, "Fudbal");
        //    Category realMadrid = new Category(4, "Real Madrid");
        //    football.SubCategories.Add(realMadrid);
        //    sportC.SubCategories.Add(football);
            
        //    //-----------------QUESTIONS AND ANSWERS----------------------------------------------------
        //    Question qOneForSport = new Question(1, "Vas omiljeni fudbalski klub?", AnswerType.Text, false, 0, 0);
        //    Question qTwoForSport = new Question(3, "Vas omiljeni kosarkaski klub?", AnswerType.Text, false, 0, 0);
        //    Question qThreeForSport = new Question(2, "Vas omiljeni fudbalski klub?", AnswerType.Text, false, 0, 0);
            

        //    Answer an1 = new Answer(15, "Milano");
        //    Answer an2 = new Answer(18, "Real Madrid");
        //    Answer an3 = new Answer(16, "Crvena zvezda");

            

        //    UserAnswer marijaAnswerOne = new UserAnswer();
        //    marijaAnswerOne.User = marija;
        //    marijaAnswerOne.Answer = an1;


        //    UserAnswer marijaAnswerTwo = new UserAnswer();
        //    marijaAnswerTwo.User = marija;
        //    marijaAnswerTwo.Answer = an2;

        //    UserAnswer marijaAnswerThree = new UserAnswer();
        //    marijaAnswerThree.User = marija;
        //    marijaAnswerThree.Answer = an3;

        //    qOneForSport.UserAnswers.Add(marijaAnswerOne);
        //    qTwoForSport.UserAnswers.Add(marijaAnswerTwo);
        //    qThreeForSport.UserAnswers.Add(marijaAnswerThree);

        //    Question qOneForProgramming = new Question(4, "Koliko imate godina?", AnswerType.Select, false, 1, 1);

        //    Answer _17to20 = new Answer(11, "17 - 20");
        //    Answer _21to25 = new Answer(12, "21 - 25");
        //    Answer _26to30 = new Answer(13, "26 - 30");
        //    Answer _31to35 = new Answer(14, "31 - 35");
        //    Answer _36 = new Answer(15, "36+");
        //    qOneForProgramming.QuestionAnswers.Add(_17to20);
        //    qOneForProgramming.QuestionAnswers.Add(_21to25);
        //    qOneForProgramming.QuestionAnswers.Add(_26to30);
        //    qOneForProgramming.QuestionAnswers.Add(_31to35);
        //    qOneForProgramming.QuestionAnswers.Add(_36);


        //    Question qTwoForProgramming = new Question(5, "Da li imate posao ili ste student?", AnswerType.Radio, false, 1, 1);
        //    Answer student = new Answer(1, "Student sam");
        //    Answer haveJob = new Answer(2, "Imam posao");
        //    Answer other = new Answer(3, "Nista");
        //    qTwoForProgramming.QuestionAnswers.Add(student);
        //    qTwoForProgramming.QuestionAnswers.Add(haveJob);
        //    qTwoForProgramming.QuestionAnswers.Add(other);

        //    Question qThreeForProgramming = new Question(6, "Koji programski jezik vam se svidja?", AnswerType.Radio, false, 1, 1);
        //    Answer java = new Answer(21, "JAVA");
        //    Answer c_sharp = new Answer(22, "C#");
        //    Answer python = new Answer(23, "PYTHON");
        //    Answer php = new Answer(24, "PHP");
        //    qThreeForProgramming.QuestionAnswers.Add(java);
        //    qThreeForProgramming.QuestionAnswers.Add(c_sharp);
        //    qThreeForProgramming.QuestionAnswers.Add(python);
        //    qThreeForProgramming.QuestionAnswers.Add(php);

        //    Question qFourForProgramming = new Question(7, "Nivo ENG jezika?", AnswerType.CheckBox, true, 1, 1);
        //    Answer a2 = new Answer(31, "A2");
        //    Answer b1 = new Answer(32, "B1");
        //    Answer b2 = new Answer(33, "B2");
        //    Answer c1 = new Answer(34, "C1");
        //    Answer c2 = new Answer(35, "C2");
        //    qFourForProgramming.QuestionAnswers.Add(a2);
        //    qFourForProgramming.QuestionAnswers.Add(b1);
        //    qFourForProgramming.QuestionAnswers.Add(b2);
        //    qFourForProgramming.QuestionAnswers.Add(c1);
        //    qFourForProgramming.QuestionAnswers.Add(c2);

        //    Question qFiveForProgramming = new Question(8, "OOP jezik koji znate?", AnswerType.MultiCheckBox, true, 1, 3);
        //    Answer _java = new Answer(41, "JAVA");
        //    Answer _c_sharp = new Answer(42, "C#");
        //    Answer _python = new Answer(43, "PYTHON");
        //    Answer _php = new Answer(44, "PHP");
        //    qFiveForProgramming.QuestionAnswers.Add(_java);
        //    qFiveForProgramming.QuestionAnswers.Add(_c_sharp);
        //    qFiveForProgramming.QuestionAnswers.Add(_python);
        //    qFiveForProgramming.QuestionAnswers.Add(_php);
        //    //-----------------ANSWERS------------------------------------------------------------


        //    /*
        //    UserAnswer jelenaAnswer = new UserAnswer();
        //    marijaAnswer.Question = qOneForProgramming;
        //    marijaAnswer.Answer = _36;
        //    marijaAnswer.User = jelena;
        //    */

        //    //-----------------SURVEYS------------------------------------------------------------

        //    Survey sport = new Survey(1, "Sport", "Anketa o sportu", true, DateTime.Now);

        //    sport.Public = true;
        //    sport.ResultAuthor = false;
        //    sport.CanPutComment = false;
        //    sport.Anonymous = false;

        //    sport.Author = nikola;
        //    sport.Questions.Add(qOneForSport);
        //    sport.Questions.Add(qTwoForSport);
        //    sport.Questions.Add(qThreeForSport);

        //    Survey programming = new Survey(2, "Programming", "Anketa o programiranju", true, DateTime.Now);

        //    programming.Public = true;
        //    programming.ResultAuthor = false;
        //    programming.CanPutComment = false;
        //    programming.Anonymous = false;

        //    programming.Author = david;
        //    programming.Questions.Add(qOneForProgramming);
        //    programming.Questions.Add(qTwoForProgramming);
        //    programming.Questions.Add(qThreeForProgramming);
        //    programming.Questions.Add(qFourForProgramming);
        //    programming.Questions.Add(qFiveForProgramming);

        //    //context.Surveys.Add(programming);
        //   // context.Surveys.Add(sport);

        //    sportC.Surveys.Add(sport);
        //    programmingC.Surveys.Add(programming);
        //    context.Categories.Add(sportC);
        //    context.Categories.Add(programmingC);

        //    UserSurvey us = new UserSurvey();
        //    us.User = marija;
        //    us.Survey = sport;

        //    context.UsersSurvays.Add(us);
            
        // //   context.UsersAnswers.Add(jelenaAnswer);
        //    context.SaveChanges();

           
            
        //    /*
        //    List<UserAnswer> answers = new List<UserAnswer>();

        //    answers.Add(marijaAnswerOne);
        //    answers.Add(marijaAnswerTwo);
        //    answers.Add(marijaAnswerThree);

        //    context.UsersAnswers.AddRange(answers);

        //    context.SaveChanges();
        //    */
            
        //    base.Seed(context);
        //}
    }
}

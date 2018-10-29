

using Microsoft.Practices.Unity;
using System.Web.Http;
using WebSurveying2017.Data.Infrastructure;
using WebSurveying2017.Data.Repositories;
using WebSurveying2017.Service.Service;
using WebSurveyingS2017.Service.Service;

namespace WebSurveying2017.App_Start
{
    public static class IoCConfigurator
    {
        public static Microsoft.Practices.Unity.IUnityContainer ConfigureUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            Register(container);

           // DependencyResolver.SetResolver(new UnityMvcDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new WebSurveying2017.Infrastructure.UnityWebApiDependencyResolver(container);

            return container;
        }

        public static void Register(IUnityContainer container)
        {
         //   container.RegisterType<ICoffeeSpotRepository, CoffeeSpotRepository>();
           
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerThreadLifetimeManager());
            //  container.RegisterType<IRepository<User>, Repository<User>>();
            //  container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<IDbFactory, DbFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserRoleRepository, UserRoleRepository>();
            container.RegisterType<IUserExtendRepository, UserExtendRepository>();
            container.RegisterType<ISurveyRepository, SurveyRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IUserAnswerRepository, UserAnswerRepository>();
            container.RegisterType<IUserCategoryRepository, UserCategoryRepository>();
            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IUserSurveyRepository, UserSurveyRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IAnswerRepository, AnswerRepository>();
            container.RegisterType<IUserNotificationRepository, UserNotificationRepository>();
            container.RegisterType<INotificationRepository, NotificationRepository>();
            container.RegisterType<IGroupRepository, GroupRepository>();
            container.RegisterType<IUserGroupRepository, UserGroupRepository>();
            container.RegisterType<ILikeOrDislikeCommentRepository, LikeOrDislikeCommentRepository>();
            container.RegisterType<IFavoriteSurveysRepository, FavoriteSurveysRepository>();
            container.RegisterType<IExcelFilesRepository, ExcelFilesRepository>();
            container.RegisterType<IRequestGroupRepository, RequestGroupRepository>();
            


            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserRoleService, UserRoleService>();
            container.RegisterType<IUserExtendService, UserExtendService>();
            container.RegisterType<ISurveyService, SurveyService>();
            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IUserAnswerService, UserAnswerService>();
            container.RegisterType<IUserGroupService, UserGroupService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IUserCategoryService, UserCategoryService>();
            container.RegisterType<IUserSurveyService, UserSurveyService>();
            container.RegisterType<IUserNotificationService, UserNotificationService>();
            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<ILikeOrDislikeCommentService, LikeOrDislikeCommentService>();
            container.RegisterType<IFavoriteSurveysService, FavoriteSurveysService>();
            container.RegisterType<IRequestGroupService, RequestGroupService>();
            container.RegisterType<IExcelFilesService, ExcelFilesService>();
            container.RegisterType<IQuestionService, QuestionService>();
            //  container.RegisterType(typeof(IRepository<>), typeof(Repository<>));


        }
    }
}
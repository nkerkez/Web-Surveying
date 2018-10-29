using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebSurveying2017.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            

            bundles.Add(new ScriptBundle("~/bundles/skripte").Include(
               "~/Scripts/jquery-3.2.1.js",
               "~/Scripts/jquery-ui-1.12.1.js",
               "~/Scripts/bootstrap.js"
           //"~/Scripts/Vendors/toastr.js",
           //"~/Scripts/Vendors/angular-route.js",
           //"~/Scripts/Vendors/angular-cookies.js",
           //"~/Scripts/Vendors/angular-validator.js",
           //"~/Scripts/Vendors/angular-base64.js",
           //"~/Scripts/Vendors/angular-file-upload.js",
           //"~/Scripts/Vendors/angucomplete-alt.min.js",
           ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                            "~/Scripts/angular.min.js",
                            "~/Scripts/ui-grid.js",
                            "~/Scripts/angular-material.js",
                            "~/Scripts/angular-material.min.js",
                            "~/Scripts/angular-animate.min.js",
                            "~/Scripts/angular-ui-router.js",
                            "~/Scripts/angular-aria.min.js",
                            "~/Scripts/angular-ui/ui-bootstrap.min.js",
                            "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                            "~/AngularClient/app.js",
                            "~/AngularClient/appConfig.js",
                            "~/AngularClient/Home/homeController.js",
                            "~/AngularClient/Survey/surveyService.js",
                            "~/AngularClient/Survey/searchSurveysComponent.js",
                            "~/AngularClient/Survey/surveyBarComponent.js",
                            "~/AngularClient/Survey/publicSurveysController.js",
                            "~/AngularClient/Survey/filledSurveysController.js",
                            "~/AngularClient/Survey/authorSurveysController.js",
                            "~/AngularClient/Survey/favoriteSurveysController.js",
                            "~/AngularClient/Survey/groupSurveysController.js",
                            "~/AngularClient/Category/categoryService.js",
                             "~/AngularClient/Category/categoryController.js",
                             "~/AngularClient/Category/addCategoryController.js",
                             "~/AngularClient/Category/updateCategoryController.js",
                            "~/AngularClient/Category/categoriesComponent.js",
                            "~/AngularClient/Category/choiceCategoryComponent.js",
                            "~/AngularClient/Survey/surveysComponent.js",
                            "~/AngularClient/Survey/surveyComponent.js",
                            "~/AngularClient/Survey/newSurveyController.js",
                            "~/AngularClient/Survey/surveyController.js",
                            "~/AngularClient/Survey/fillSurveyController.js",
                            "~/AngularClient/Survey/surveyResultController.js",
                            "~/AngularClient/UsersAnswers/UsersAnswersService.js",
                            "~/AngularClient/UsersAnswers/createUserAnswersService.js",
                            "~/AngularClient/Comment/postCommentComponent.js",
                            "~/AngularClient/Comment/commentsComponent.js",
                            "~/AngularClient/Comment/commentController.js",
                             "~/AngularClient/Comment/userCommentComponent.js",
                            "~/AngularClient/Comment/userCommentsController.js",
                            "~/AngularClient/Comment/commentsController.js",
                            "~/AngularClient/Comment/commentService.js",
                            "~/AngularClient/testController.js",
                            "~/AngularClient/Error/errorController.js",
                            "~/AngularClient/Delete/deleteController.js",
                            "~/AngularClient/Navbar/navbarController.js",
                            "~/AngularClient/Authentication/authService.js",
                            "~/AngularClient/Authentication/signUpController.js",
                            "~/AngularClient/Authentication/resetPasswordController.js",
                            "~/AngularClient/Authentication/authenticationController.js",
                            "~/AngularClient/User/userService.js",
                            "~/AngularClient/User/userController.js",
                            "~/AngularClient/User/userInfoComponent.js",
                            "~/AngularClient/User/searchUsersComponent.js",
                            "~/AngularClient/UserRole/userRoleService.js",
                            "~/AngularClient/Category/updateModeratorsController.js",
                            "~/AngularClient/UserCategory/userCategoryService.js",
                            "~/AngularClient/UserNotification/userNotificationService.js",
                            "~/AngularClient/Notification/notificationComponent.js",
                            "~/AngularClient/Notification/notificationService.js",
                            "~/AngularClient/Notification/notificationsController.js",
                            "~/AngularClient/LikeOrDislikeComment/likeOrDislikeCommentService.js",
                            "~/AngularClient/LikeOrDislikeComment/likeOrDislikeCommentController.js",
                            "~/AngularClient/LikeOrDislikeSurvey/likeOrDislikeSurveyService.js",
                            "~/AngularClient/LikeOrDislikeSurvey/likeOrDislikeSurveyController.js",
                            "~/AngularClient/LikeOrDislike/likeOrDislikeComponent.js",
                            "~/AngularClient/UserExtend/UserExtendService.js",
                            "~/AngularClient/HelpServices/helpService.js",
                            "~/AngularClient/Question/questionComponent.js",
                            "~/AngularClient/UserGroup/newGroupController.js",
                            "~/AngularClient/UserGroup/editGroupController.js",
                            "~/AngularClient/UserGroup/userGroupService.js",
                            "~/AngularClient/UserGroup/groupUsersController.js",
                            "~/AngularClient/Group/groupService.js",
                            "~/AngularClient/Group/myGroupsController.js",
                            "~/AngularClient/Group/groupMemberController.js",
                            "~/AngularClient/Group/groupsController.js",
                            "~/AngularClient/Group/groupComponent.js",
                            "~/AngularClient/Group/searchGroupsComponent.js",
                            "~/AngularClient/Group/chooseGroupsComponent.js",
                            "~/AngularClient/FavoriteSurveys/favoriteSurveysService.js",
                            "~/AngularClient/Request/requestService.js",
                            "~/AngularClient/Request/requestsController.js",
                            "~/AngularClient/Request/requestComponent.js",
                            "~/AngularClient/Validation/surveyValidation.js",
                            "~/AngularClient/UserSurvey/userSurveyService.js",
                            "~/AngularClient/UserSurvey/surveyUsersController.js",
                            "~/AngularClient/Question/questionService.js",
                            "~/AngularClient/Question/questionResultController.js",
                            "~/AngularClient/Excel/excelFilesController.js",
                            "~/AngularClient/Excel/excelService.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/site.css",
                "~/Content/ui-grid.css",
                "~/Content/angular-material.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/ui-bootstrap-csp.css"
            //"~/content/css/toastr.css",
            ));
        }
    }
}
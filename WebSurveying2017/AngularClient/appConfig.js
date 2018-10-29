(function () {

    'use strict';
    angular.module('WebSurveying2017').config(['$qProvider', '$stateProvider', '$urlRouterProvider', '$httpProvider', '$locationProvider', function ($qProvider, $stateProvider, $urlRouterProvider, $httpProvider, $locationProvider) {

        $qProvider.errorOnUnhandledRejections(false);

        $urlRouterProvider.otherwise('/home/3/1/10');
        
        $stateProvider
            .state('home', {
                url: '/home/{state}/{page}/{size}',
                views: {
                    'logo': { templateUrl: "AngularClient/Logo/logo.html" },
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'
                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'homeController',
                        controllerAs: 'ctrl'
                    }
                }
            }).state('publicSurveys', {
                url: '/surveys/public/{state}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'publicSurveysController',
                        controllerAs: 'ctrl'
                    }
                }
            }).state('searchAllSurveys', {
                url: '/surveys/all/{state}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'homeController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('searchPublicSurveys', {
                url: '/surveys/public/{state}/search/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'publicSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('authorSurveys', {
                url: '/surveys/author/{state}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'authorSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('searchAuthorSurveys', {
                url: '/surveys/author/{state}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'authorSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('favoriteSurveys', {
                url: '/surveys/favorite/{state}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'favoriteSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('searchFavoriteSurveys', {
                url: '/surveys/favorite/{state}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'favoriteSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('groupSurveys', {
                url: '/surveys/group/{state}/{id}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'groupSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('searchGroupSurveys', {
                url: '/surveys/group/{id}/{state}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'groupSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('filledSurveys', {
                url: '/surveys/filled/{state}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'filledSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('searchFilledSurveys', {
                url: '/surveys/filled/{state}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},

                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Home/home.html',
                        controller: 'filledSurveysController',
                        controllerAs: 'ctrl'

                    }

                }


            }).state('category',
            {
                url: '/category/{categoryId}/all={flag}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Category/category.html',
                        controller: 'categoryController',
                        controllerAs: 'ctrl'

                    }
                }
            }).state('createCategory',
            {
                url: '/category/create',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Category/addOrUpdateCategory.html',
                        controller: 'addCategoryController',
                        controllerAs: 'ctrl'

                    }
                }
            }).state('updateCategory',
            {
                url: '/category/update/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Category/addOrUpdateCategory.html',
                        controller: 'updateCategoryController',
                        controllerAs: 'ctrl'

                    }
                }
            }).state('searchActiveSurveysWithCategory',
            {
                url: '/category/{categoryId}/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Category/category.html',
                        controller: 'categoryController',
                        controllerAs: 'ctrl'

                    }
                }
            }).state('survey',
            {
                url: '/surveys/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/survey.html',
                        controller: 'surveyController',
                        controllerAs: 'surveyCtrl'

                    }
                }
            }).state('fillsurvey',
            {
                url: '/survey/fill/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/survey.html',
                        controller: 'fillSurveyController',
                        controllerAs: 'surveyCtrl'

                    }
                }
            }).state('newsurvey',
            {
                url: '/survey/create',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/newSurvey.html',
                        controller: 'newSurveyController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('createNewUsingOld',
            {
                url: '/survey/create/old/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/newSurvey.html',
                        controller: 'newSurveyController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('updateSurvey',
            {
                url: '/survey/update/{flag}/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/newSurvey.html',
                        controller: 'newSurveyController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('surveyresult',
            {
                url: '/survey/result/{id}/{userId}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Survey/surveyResult.html',
                        controller: 'surveyResultController',
                        controllerAs: 'ctrl'


                    }
                },
               
            }).state('surveyusers',
            {
                url: '/survey/users/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/UserSurvey/surveyUsers.html',
                        controller: 'surveyUsersController',
                        controllerAs: 'ctrl'


                    }
                },

            }).state('surveyComments',
            {
                url: '/survey/comments/{id}/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Comment/surveyComments.html',
                        controller: 'commentsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('userComments',
            {
                url: '/user/comments/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Comment/userComments.html',
                        controller: 'userCommentsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('comment',
            {
                url: '/comment/{surveyId}/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Comment/surveyComments.html',
                        controller: 'commentController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('login',
            {
                url: '/login',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Authentication/login.html',
                        controller: 'authenticationController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('signup',
            {
                url: '/signup',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Authentication/signUp.html',
                        controller: 'signUpController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('resetPassword',
            {
                url: '/resetpassword/{token}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Authentication/resetPassword.html',
                        controller: 'resetPasswordController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('user',
            {
                url: '/users/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/User/user.html',
                        controller: 'userController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('groupUsers',
            {
                url: '/users/group/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/UserGroup/groupUsers.html',
                        controller: 'groupUsersController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('updatemoderators',
            {
                url: '/categories/update/moderators/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Category/updateModerators.html',
                        controller: 'updateModeratorsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('notifications',
            {
                url: '/notifications',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Notification/notifications.html',
                        controller: 'notificationsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('likeordislikecommentlist',
            {
                url: '/likeordislike/list/comment/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/LikeOrDislikeComment/likeOrDislikeCommentList.html',
                        controller: 'likeOrDislikeCommentController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('likeordislikesurveylist',
            {
                url: '/likeordislike/list/survey/{id}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/LikeOrDislikeSurvey/likeOrDislikeSurveyList.html',
                        controller: 'likeOrDislikeSurveyController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('newGroup',
            {
                url: '/group/create/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/UserGroup/newGroup.html',
                        controller: 'newGroupController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('editGroup',
            {
                url: '/group/{id}/update/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/UserGroup/newGroup.html',
                        controller: 'editGroupController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('groups',
            {
                url: '/groups/all/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'groupsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('searchGroups',
            {
                url: '/groups/all/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'groupsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('myGroups',
            {
                url: '/groups/my/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'myGroupsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('searchMyGroups',
            {
                url: '/groups/my/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'myGroupsController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('groupMember',
            {
                url: '/groups/member/{page}/{size}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'groupMemberController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('searchGroupMember',
            {
                url: '/groups/member/{page}/{size}/{queryString}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Group/groups.html',
                        controller: 'groupMemberController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('requests',
                {
                    url: '/requests/{groupId}',
                    views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                        'navbar': {
                            templateUrl: 'AngularClient/Navbar/navbar.html',
                            controller: 'navbarController',
                            controllerAs: 'nCtrl'

                        },
                        'content': {
                            templateUrl: 'AngularClient/Request/requests.html',
                            controller: 'requestsController',
                            controllerAs: 'ctrl'


                        }
                    }
            }).state('error',
            {
                url: '/error/{code}',
                views: { 'logo' : { templateUrl : "AngularClient/Logo/logo.html"},
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Error/error.html',
                        controller: 'errorController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('question',
            {
                url: '/questions/{id}',
                views: {
                    'logo': { templateUrl: "AngularClient/Logo/logo.html" },
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Question/questionResult.html',
                        controller: 'questionResultController',
                        controllerAs: 'ctrl'


                    }
                }
            }).state('excelFiles',
            {
                url: '/files/survey/{id}',
                views: {
                    'logo': { templateUrl: "AngularClient/Logo/logo.html" },
                    'navbar': {
                        templateUrl: 'AngularClient/Navbar/navbar.html',
                        controller: 'navbarController',
                        controllerAs: 'nCtrl'

                    },
                    'content': {
                        templateUrl: 'AngularClient/Excel/excelFiles.html',
                        controller: 'excelFilesController',
                        controllerAs: 'ctrl'


                    }
                }
            });
        
        $httpProvider.interceptors.push(['$q', '$rootScope', '$injector', function ($q, $rootScope, $injector) {
            return {
                request: function (config) {
                    var token = localStorage.getItem('token');
                   // console.log(token)
                    if (token) {
                        // config.headers['X-AUTH-TOKEN'] = authToken;
                    //    console.log(token)
                        config.headers.Authorization = 'Bearer' + token;
                    }
                    return config;
                },
                responseError: function (error) {
                    if (error.status === 401 || error.status === 403) {
                        var stateService = $injector.get('$state');

                        stateService.go('error', { code: error.status });
                    }
                    return $q.reject(error);
                }
            }
        }]);
    }]);


})();
(function(angular){
    'use-strict'

    angular.module('WebSurveying2017').controller('surveyController', surveyCtrl);

    function surveyCtrl($uibModal, $uibModalStack, $scope, surveyService, $stateParams, helpService, usersAnswersService, favoriteSurveysService, categoryService, createUserAnswersService, usersAnswersService, $state) {

        var ctrl = this;
        ctrl.state = "view";
        ctrl.helpService = helpService;
        ctrl.comment = {};
        $scope.service = surveyService;
        ctrl.resetSurvey = function () {
            
            usersAnswersService.resetSurvey(ctrl.survey.Id).then(
                function (response) {
                    $scope.message = "Uspešno ste resetovali anketu.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                },
                function (response) {
                    $scope.message = "Resetovanje ankete nije izvršeno. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }
        ctrl.postFavoriteSurvey = function () {
            favoriteSurveysService.postFavoriteSurvey(ctrl.survey).then(
                function (response)
                {
                    $scope.message = "Uspešno ste dodali anketu na listu omiljenih anketa.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                },
                function (response)
                {
                    $scope.message = "Dodavanje ankete na listu omiljenih anketa nije izvršeno. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }

        ctrl.changeState = function () {
            console.log('aaa')
            surveyService.changeStateOfSurvey(ctrl.survey).then(
                function (response) {

                    $scope.message = "Uspešno ste promenili stanje ankete.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                   
                },
                function (response) {
                    $scope.message = "Promena stanja ankete nije izvršena. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }

        ctrl.deleteSurvey = function () {

            $scope.text = "Da li ste sigurni da želite da obrišete anketu : " + ctrl.survey.Name + " ?"
            $scope.Id = ctrl.survey.Id;
            var modalInstance = $uibModal.open(
                {
                    templateUrl: 'AngularClient/Delete/deleteModal.html',
                    controller: 'deleteController',
                    controllerAs: 'deleteCtrl',
                    scope: $scope,
                    backdrop: false
                });
        }

        ctrl.download = function () {

            window.open('http://localhost:49681/api/surveys/download');
            /*
            surveyService.download().then(
                function (response) {
               //     $state.go('user', { id: 1 });
                    },
                function (response) {
                    console.log('error');
                });
            */
        }

        ctrl.createUserAnswers = function (fillAgain) {

            //  var userAnswers = createUserAnswersService.createUserAnswers(fillAgain, ctrl.survey, ctrl.questionWithNewAnswers);
            var userAnswers = createUserAnswersService.createObj(ctrl.survey.Questions);
            //console.log(createUserAnswersService.errorMessage);
            console.log(userAnswers);
            usersAnswersService.postUserAnswers(userAnswers, ctrl.survey.Id).then(
                function (response) {

                    $scope.message = "Uspešno ste popunili anketu.";
                    $scope.image = "../AngularClient/successfillout.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                    
                },
                function (response) {
                    if (response.data.ModelState)
                        ctrl.errors = response.data.ModelState;
                }
            );
            

        }

        ctrl.extendSurvey = function (questions) {

            questions.forEach(function (question) {

                //text
                if (question.AnswerType === 1)
                {
                    question.userAnswer = {
                        "QuestionId": null,
                        "AnswerText": ''
                    }
                }
                else if (question.AnswerType === 3 || question.AnswerType === 5 || question.AnswerType === 6)
                {
                    question.userAnswer = [];

                    
                }
                else
                {
                    question.userAnswer = {};
                }

            });
        };

        ctrl.showCommentForm = function () {
            if (ctrl.showForm === false) {
                ctrl.comment = {

                    Text: '',
                    SurveyId: ctrl.survey.Id

                };
                ctrl.showForm = true;

            }
            else ctrl.showForm = false;
        }

        ctrl.saveFile = function () {
            surveyService.saveFile($stateParams.id).then(
                function (response) {
                    $scope.message = "Uspešno ste eksportovali rezultate ankete.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('excelFiles', { id: $stateParams.id });
                    }, 2666);
                    
                },
                function (response) {
                    $scope.message = "eksportovanje ankete nije izvršena. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }
        ctrl.changeCategory = function () {
            ctrl.initCats('SET');
        }

        ctrl.initCats = function (operation) {


            categoryService.getCategories().then(
                function (response) {
                    ctrl.categories = response.data;

                    ctrl.openModalForCategories(operation);

                },
                function (error) {

                });

        };

  


        ctrl.openModalForCategories = function (operation) {


            var modalInstance = $uibModal.open({

                templateUrl: 'AngularClient/Category/categoriesModal.html',
                controller: function () {


                    var choiceCtrl = this;
                    choiceCtrl.state = 'VIEW';
                    choiceCtrl.rootCategories = ctrl.categories;
                    choiceCtrl.onClick = function (category) {

                        if (category == null) {
                            ctrl.survey.CategoryId = null;
                        }
                        else {
                            ctrl.survey.CategoryId = category.Id;
                        }

                        surveyService.changeCategory(ctrl.survey).then(
                            function (response) {

                                $scope.message = "Uspešno ste izmenili kategoriju.";
                                $scope.image = "../AngularClient/successs.png";
                                ctrl.openModal();

                                setTimeout(function () {
                                    $uibModalStack.dismissAll();
                                    $state.go('home', { page: 1, size: 10, state: 3 });
                                }, 2666);
                                
                            },
                            function (response) {
                                $scope.message = "Izmena kategorije nije izvršena. Došlo je do greške.";
                                $scope.image = "../AngularClient/negative.png";
                                ctrl.openModal();
                                setTimeout(function () {
                                    $uibModalStack.dismissAll();
                                }, 2666);
                            }
                        );



                    }
                },
                controllerAs: 'vm',
                backdrop: false
            });

        };

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }

        surveyService.getSurvey($stateParams.id).then(
            function (response) {

                ctrl.survey = response.data;
                console.log(ctrl.survey);
                ctrl.extendSurvey(ctrl.survey.Questions);
                
                
            },
            function (error) {
            });
    };

})(angular);
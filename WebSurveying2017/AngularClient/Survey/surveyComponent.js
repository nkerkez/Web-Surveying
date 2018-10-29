(function (angular) {

    function surveyCompController(surveyService, usersAnswersService,
        createUserAnswersService, categoryService, likeOrDislikeSurveyService, favoriteSurveysService,  $state, $uibModal, $scope, $uibModalStack, $timeout, $rootScope, helpService) {
        var ctrl = this;

        
        ctrl.showAccept = false;
        ctrl.showReject = false;
        ctrl.oldQuestions = [];
        ctrl.showForm = false;
        ctrl.updatedQuestions = [];
     

        ctrl.helpService = helpService;
        

        ctrl.comment = {};
        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }

        ctrl.removeFromFavoriteSurveys = function () {
            favoriteSurveysService.removeFromFavoriteSurveys(ctrl.survey.Id).then(
                function (response)
                {
                    $scope.message = "Uspešno ste uklonili anketu sa listu omiljenih anketa.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                    
                },
                function (response)
                {
                    $scope.message = "Brisanje ankete sa liste omiljenih anketa nije izvršeno. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        };
        // open form for posting comment delete this
        ctrl.showCommentForm = function () {
            if (ctrl.showForm === false)
            {
                ctrl.comment = {

                    Text: '',
                    SurveyId: ctrl.survey.Id

                };
                ctrl.showForm = true;
                
            }
            else ctrl.showForm = false;
        }
        ctrl.addButtonsAndCopyOldValues = function (question) {
            //IF USER FILL AGAIN SURVEY ADD BUTTONs FOR POSTING NEW ANSWERS
            
            question.showButtons = false;
            //COPY OLD VALUES
            angular.copy(ctrl.survey.Questions, ctrl.oldQuestions);
                
        };

        ctrl.initStyle = function(question) {
            //SET ALL QUESTION UNCLICKABLE
            if(ctrl.survey.fillAgain != null)
                question.myStyle = { 'border': 'solid 2px blue' ,  'pointer-events': 'none' };
        };

        ctrl.show = function (value) {
            if (value === 'accept') {
                ctrl.showAccept = true;
                ctrl.showReject = false;
            }
            else if (value === 'reject') {
                ctrl.showAccept = false;
                ctrl.showReject = true;
            }
        }

        ctrl.removeUndifined = function (array) {
            
            for (var i = 0; i < array.length; i++) {
                if (array[i] === undefined)
                    array.splice(i,1);
            }
            
        };
        
        ctrl.updateSurvey = function () {

            ctrl.survey.Groups = helpService.initGroups(ctrl.survey.Groups);
         //   var updatedQuestions = helpService.initUpdatedQuestions(ctrl.oldSurvey, ctrl.survey);
            surveyService.putSurvey(ctrl.survey).then(
                function (response) {
                    $state.go('home',{  page : 1 , size : 10 });
                },
                function (error) {

                });
            
        }
        ctrl.removeUserAnswerObj = function () {
            for (var i = 0; i < ctrl.survey.Questions.length; i++) {
                if(ctrl.survey.Questions[i].userAnswer != null)
                    delete ctrl.survey.Questions[i].userAnswer;
            }
        };

        

        ctrl.reject = function () {

            ctrl.removeUserAnswerObj();
            ctrl.survey.State = 1;



            if (ctrl.survey.Correction != null) {
                surveyService.changeStateOfSurvey(ctrl.survey).then(
                    function (response) {
                        $state.go('home', { page: 1, size: 10 });
                    },
                    function (error) {
                        console.log('error');
                    });
            }
            
        }
        ctrl.acceptRequest = function () {
            
            if (ctrl.survey.CategoryId == null)
                return;
            
            ctrl.removeUserAnswerObj();
            ctrl.survey.State = 2;
            
            surveyService.changeStateOfSurvey(ctrl.survey).then(
                function (response) {
                    $state.go('home', { page: 1, size: 10 });
                },
                function (error) {
                    console.log('error');
                });
            
            
        };
       ctrl.mouseOver = function (question) {
            if ($(".multiSelect option:selected").length > 2)
                question.userAnswer = [];
        }

        
        
        /*
        ctrl.initUserAnswerForCB = function (question) {
            console.log(question);
            if (question.AnswerType === 5) {
                question.MaxNumbOfAnswers = 1;
            }
            if(ctrl.state === 'view')
                for (var i = 0; i < question.QuestionAnswers.length; i++)
                    question.userAnswer[i] = undefined;
        }
        */
       
        ctrl.delete = function (question) {
            ctrl.onDelete({ question: question });
        };

        ctrl.edit = function (question) {
            ctrl.onEdit({ question: question });
        };

        

        ctrl.putNewAnswer = function (question) {
            question.myStyle = { 'border': 'solid 2px green'};
            question.showButtons = true;
            console.log(question.myStyle);

            
        };

        ctrl.cancel = function (question) {

            question.myStyle = { 'border': 'solid 2px blue' ,  'pointer-events': 'none' };
            // SET OLD VALUES AND CHANGE CSS
            ctrl.oldQuestions.map(function (quest) {
                if (quest.Id == question.Id) {
                    question.userAnswer = angular.copy(quest.userAnswer, question.userAnswer);
                  //  question.userAnswer = quest.userAnswer;
                    return;

                }
            });

            question.showButtons = false;
        };
        ctrl.ok = function (question) {
            //CHANGE CSS
            question.myStyle = { 'border': 'solid 2px blue', 'pointer-events': 'none' };
            ctrl.survey.questionWithNewAnswers.push(question);
            question.showButtons = false;
        };
    }
    angular.module('WebSurveying2017').component('surveyComponent', {
        templateUrl: 'AngularClient/Survey/surveyPreview.html',
        bindings: {
            survey: '<',
            onEdit: '&',
            onDelete: '&',
            state: '<',
            oldSurvey: '<'

        },
        controller: surveyCompController
    })

})(angular);
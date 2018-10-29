(function(angular){

    angular.module('WebSurveying2017').controller('newSurveyController', ctlr);

    function ctlr(surveyService, $stateParams, groupService, helpService , surveyValidation, $state, $uibModal, $uibModalStack, $scope ) {

        var ctrl = this;
        ctrl.state = 'add';
        ctrl.questionState = 'add';
        ctrl.questionReference = {};
        ctrl.myGroups;
        ctrl.ordinalNumbers = [];

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }

        ctrl.createSurvey = function () {

            ctrl.survey.Groups = helpService.initGroups(ctrl.survey.Groups);
            if ($stateParams.flag == null) {
                surveyService.addSurvey(ctrl.survey).then(
                    function (response) {

                        $scope.message = "Uspešno ste dodali anketu.";
                        $scope.image = "../AngularClient/surveypic.png";
                        ctrl.openModal();

                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                            $state.go('home', { page: 1, size: 10, state: 3 });
                        }, 2666);
                        
                    },
                    function (error) {
                        ctrl.errors = error.data.ModelState;
                    }
                );
            }
            else {
                if ($stateParams.flag != null) {
                    surveyService.putSurvey(ctrl.survey).then(
                        function (response) {
                            $scope.message = "Uspešno ste izmenili anketu.";
                            $scope.image = "../AngularClient/successs.png";
                            ctrl.openModal();

                            setTimeout(function () {
                                $uibModalStack.dismissAll();
                                $state.go('home', { page: 1, size: 10, state: 3 });
                            }, 2666);
                        },
                        function (error) {
                            ctrl.errors = error.data.ModelState;
                        }
                    );
                }
            }

        };
        ctrl.initSurvey = function () {
            ctrl.survey = {
                Questions: [],
                Groups: [],
                Public: true,
                Anonymous: true,
                ResultAuthor: true
            };
        }
        
        ctrl.initQuestion = function () {
            ctrl.question = {
                AnswerType: 1,
                MinNumbOfAnswers: 1,
                MaxNumbOfAnswers : 1,
                QuestionAnswers: [],
                Required: false,
                OrdinalNumber: ctrl.survey.Questions.length + 1
            };
            ctrl.ordinalNumbers.push(ctrl.survey.Questions.length + 1);
            if (ctrl.survey.Id !== null) {
                ctrl.question.SurveyId = ctrl.survey.Id;
            }
        };

        //prepraviti
        ctrl.initGroups = function () {
            groupService.getForAuthorAndMember().then(
                function (response) {
                    ctrl.myGroups = response.data;
                },
                function (response) {

                }
            );
        };

        (function () {
            ctrl.initGroups();
            if ($stateParams.id !== undefined) {
                surveyService.getSurvey($stateParams.id).then(
                    function (response) {
                        
                        
                        ctrl.survey = response.data;
                        ctrl.initQuestion();
                        ctrl.state = 'update';
                        if ($stateParams.flag == null) {
                            ctrl.survey.Id = 0;
                            angular.forEach(ctrl.survey.Questions, function (quest) {
                                angular.forEach(quest.QuestionAnswers, function (answer) {
                                    answer.Id = 0;
                                });
                                quest.Id = 0;
                            });
                        }

                        helpService.fromGroupToSurveyGroup(ctrl.myGroups, ctrl.survey.Groups);
                     },
                    function (response) {
                        console.log(response);
                    });
            }
            else {
                ctrl.initSurvey();    

                ctrl.initQuestion();
            }
        })();
        //[1,2,3,4,5]
        //3 [1,2, 3, ++, ++, ++]
        ctrl.addQuestion = function () {
            
            if ((ctrl.errorMessage = surveyValidation.validateQuestion(ctrl.question)) == null) {
                var questionClone = angular.extend({}, ctrl.question);
                if (ctrl.question.OrdinalNumber <= ctrl.survey.Questions.length)
                    ctrl.changeOrdinalNumbers();
                ctrl.survey.Questions.push(questionClone);
                ctrl.initQuestion();
            }
            
        };

        ctrl.changeOrdinalNumbersAfterUpdate = function (oldON, newON) {

            if (oldON != newON) {
                if (oldON < newON) {
                    angular.forEach(ctrl.survey.Questions, function (question) {
                        if (question.OrdinalNumber <= newON && question.OrdinalNumber > oldON)
                            question.OrdinalNumber--;
                    }
                    );
                }
                else {
                    angular.forEach(ctrl.survey.Questions, function (question) {
                        if (question.OrdinalNumber >= newON && question.OrdinalNumber < oldON)
                            question.OrdinalNumber++;
                    }
                    );
                }
            
            }
        }
        ctrl.changeOrdinalNumbers = function ()
        {
            angular.forEach(ctrl.survey.Questions, function (question) {
                if (ctrl.question.OrdinalNumber <= question.OrdinalNumber)
                    question.OrdinalNumber++;
            });
        }

        ctrl.addAnswer = function () {
            ctrl.question.QuestionAnswers.push({});
        };

        ctrl.changeType = function (question, type) {

            if (type === 1) {
                question.QuestionAnswers = [];
                question.MinNumbOfAnswers = 1;
                question.MaxNumbOfAnswers = 1;
            }

            if (type === 4 || type === 5 || type === 2)
            {
                question.MinNumbOfAnswers = 1;
                question.MaxNumbOfAnswers = 1;
            }

            question.AnswerType = type;
        };

        ctrl.edit = function (question) {
            ctrl.ref = question;
            ctrl.questionState = 'update';

            ctrl.updateOrdNumbList(ctrl.survey.Questions.length);
            //dodelio referencu kako bi znao sta menjam
            ctrl.questionReference = question;
            ctrl.question = {};
            //kopirao vrednosti u objekat
            angular.copy(question, ctrl.question);
         };
       
        ctrl.editQuestion = function () {

          //  ctrl.validateQuestion();
            if ((ctrl.errorMessage = surveyValidation.validateQuestion(ctrl.question)) == null) {
                // vrednosti iz objekta prebacio u objekat koji referencira pitanje koje se menja
                ctrl.changeOrdinalNumbersAfterUpdate(ctrl.questionReference.OrdinalNumber, ctrl.question.OrdinalNumber);
                ctrl.questionReference = angular.copy(ctrl.question, ctrl.questionReference);
                ctrl.initQuestion();
                ctrl.questionState = 'add';
            }
        };
        ctrl.updateOrdNumbList = function (maxOrdNumber) {
            angular.forEach(ctrl.ordinalNumbers, function (ordinalNumber) {

                if (ordinalNumber > maxOrdNumber) {
                    console.log(maxOrdNumber);
                    var index = ctrl.ordinalNumbers.indexOf(ordinalNumber);
                    ctrl.ordinalNumbers.splice(index, 1);
                }
            });
            
        }
        ctrl.updateOrdinalNumbers = function (ordinalNumber) {
            angular.forEach(ctrl.survey.Questions, function (q) {
                if (q.OrdinalNumber > ordinalNumber)
                    q.OrdinalNumber--;
            });
        }
        ctrl.delete = function (question) {
            //ord [1,2,3,4,5,6]
            //[1,2,3,4,5]
            var ordinalNumber = question.OrdinalNumber;
            var index = ctrl.survey.Questions.indexOf(question);
            if (index !== -1) {
                var maxOrdNumber = ctrl.survey.Questions.length;
                ctrl.updateOrdNumbList(maxOrdNumber);
                ctrl.question.OrdinalNumber = maxOrdNumber;
                ctrl.survey.Questions.splice(index, 1);
                ctrl.updateOrdinalNumbers(ordinalNumber, maxOrdNumber);
                
                
            }
        };

        ctrl.deleteAnswer = function (answer) {
            var index = ctrl.question.QuestionAnswers.indexOf(answer);
            if (index !== -1)
                ctrl.question.QuestionAnswers.splice(index, 1);
        };

        

    }
})(angular);
(function (angular) {
    'use-strict'

    angular.module('WebSurveying2017').controller('fillSurveyController', surveyCtrl);

    function surveyCtrl(surveyService, $stateParams, createUserAnswersService, usersAnswersService, $scope, $uibModal, $uibModalStack) {

        var ctrl = this;
        ctrl.state = "view";

        

        ctrl.extendSurvey = function (questions) {
          //  console.log(questions.length)
            for (var i = 0; i < questions.length; i++) {


                console.log(questions[i])

                if (questions[i].AnswerType === 1) {

                    if (questions[i].UserAnswers.length == 0) {
                        questions[i].userAnswer = ctrl.createNewObj();
                    }
                    else {
                        questions[i].userAnswer = ctrl.returnUserAnswerForTextType(questions[i]);

                    }
                }
                else {

                    if (questions[i].AnswerType == 3 || questions[i].AnswerType == 5 || questions[i].AnswerType == 6) {

                        questions[i].userAnswer = [];

                        if (questions[i].AnswerType == 3) {
                            ctrl.returnUserAnswerForMultiSelectType(questions[i]);
                        }

                        else {

                            ctrl.returnUserAnswerForCboxes(questions[i]);
                        }


                    }
                    else {
                        if (questions[i].UserAnswers.length > 0)
                            questions[i].userAnswer = String(questions[i].UserAnswers[0].Id);
                    }
                }

                

            }
            console.log('aaaaaaaaaaaa')
            console.log(ctrl.survey)
        }

        ctrl.createUserAnswers = function (fillAgain) {


            var flag = false;
            angular.forEach(ctrl.survey.Questions, function (quest) {

                if (quest.showButtons === true) {
                    flag = true;
                    ctrl.errors = {};
                    ctrl.errors[quest.QuestionText] = ["Potvrdite izmenu pitanja"];
                    console.log(ctrl.errors);
                    return;
                }
            });
            if (flag)
                return;
            var userAnswers = createUserAnswersService.createObj(ctrl.survey.questionWithNewAnswers);
            usersAnswersService.putUserAnswers(userAnswers, ctrl.survey.Id).then(
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
    
        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }


        ctrl.returnUserAnswerForCboxes = function (question) {
            for (var j = 0; j < question.QuestionAnswers.length; j++) {
                var questionAnswerId = question.QuestionAnswers[j].Id;
                var userAnswersIDs = question.UserAnswers.map(function (_userAnswer) {

                    return _userAnswer.Id;
                });
                if (userAnswersIDs.includes(questionAnswerId)) {

                    question.userAnswer[j] = questionAnswerId;
                }
                else {

                    question.userAnswer[j] = undefined;
                }
            }
        }
        ctrl.returnUserAnswerForMultiSelectType = function (question)
        {
            question.UserAnswers.forEach(
                item => {
                    question.userAnswer.push(String(item.Id));
                }
            );
        }
        ctrl.createNewObj = function () {
            return obj = {
                "QuestionId": null,
                "AnswerText": ''
            };
        }

        ctrl.returnUserAnswerForTextType = function (question) {
            return obj = {
                "QuestionId": null,
                "AnswerText": question.UserAnswers[0].AnswerText
            }
        }
         
        surveyService.getSurveyWithUserAnswers($stateParams.id).then(
            function (response) {

                ctrl.survey = response.data;
                console.log(ctrl.survey)
                ctrl.survey.fillAgain = true;
                ctrl.survey.questionWithNewAnswers = [];
                ctrl.extendSurvey(ctrl.survey.Questions);


            },
            function (error) {
            });
    };

})(angular);
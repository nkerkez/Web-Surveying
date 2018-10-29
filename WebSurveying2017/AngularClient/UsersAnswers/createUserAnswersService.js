(function (angular) {

    angular.module('WebSurveying2017').service('createUserAnswersService', createUAS);

    function createUAS(usersAnswersService, $state) {

        var ctrl = this;
        ctrl.errorMessage = null;
        ctrl.createUserAnswers = function (fillAgain, survey, questionWithNewAnswers)
        {
            // IF USER FILL SURVEY FOR A FIRST TIME
            if (fillAgain == null) {
                console.log(survey);
                var userAnswers = ctrl.createObj(survey.Questions);
                console.log(userAnswers)
                usersAnswersService.postUserAnswers(userAnswers, survey.Id).then(
                    function (response) {
                        $state.go('home', { page: 1, size: 10 });
                    },
                    function (response) {
                        console.log(response)
                        ctrl.errorMessage = "Greska !"
                    }
                );

            }
            // IF USER FILL SURVEY AGAIN
            else {
                var flag = false;
                angular.forEach(survey.Questions, function (quest) {
                    
                    if (quest.showButtons === true) {
                        flag = true;
                        ctrl.errorMessage = "Potvrdite izmenu pitanja"
                        return;
                    }
                });
                if (flag)
                    return ctrl.errorMessage;
                var userAnswers = ctrl.createObj(questionWithNewAnswers);
                usersAnswersService.putUserAnswers(userAnswers, survey.Id).then(
                        function (response) {
                            $state.go('home', { page: 1, size: 10 });
                        },
                        function (error) {
                            ctrl.errorMessage = "Greska !"
                        }
                 );

            }
        }
        ctrl.createObj = function (questions) {
           

            var retVal = [];
            for (var i = 0; i < questions.length; i++) {
                if (questions[i].userAnswer == null) {
                    continue;
                }
                if (questions[i].userAnswer instanceof Array) {
                    for (var j = 0; j < questions[i].userAnswer.length; j++) {
                        console.log(questions[i].userAnswer[j]);
                        if (questions[i].userAnswer[j] !== undefined) {
                            
                            retVal.push({
                                "AnswerId": questions[i].userAnswer[j],
                                "QuestionId": questions[i].Id

                            });
                        }
                        
                    }

                }
                else {
                    if (questions[i].AnswerType === 1 && questions[i].userAnswer.AnswerText !== '') {
                        retVal.push({
                            "Answer": questions[i].userAnswer,
                            "QuestionId": questions[i].Id

                        });
                    }
                    else if (questions[i].AnswerType !== 1 && Object.keys(questions[i].userAnswer).length > 0) {
                        var flag = false;
                        angular.forEach(questions[i].QuestionAnswers, function (qa) {
                            console.log(qa.Id, questions[i].userAnswer);
                            if (qa.Id == questions[i].userAnswer)
                                flag = true;
                        });
                        if (flag) {
                            retVal.push({
                                "AnswerId": questions[i].userAnswer,
                                "QuestionId": questions[i].Id

                            });
                        }
                    }
                }
             }
                    
            
            return retVal;
        }
    }
})(angular);
(function (angular) {

    function questionController(helpService, $rootScope, $state) {
        var ctrl = this;


        ctrl.numbOfNotUndefined = function (array) {
            var retVal = 0;
            for (var i = 0; i < array.length; i++) {
                if (array[i] !== undefined)
                    retVal++;
            }
            return retVal;

        };

        ctrl.className = function (parentIndex, index) {
            return parentIndex + '' + index;
        }

        ctrl.provera = function (question, answer, index, parentIndex) {

            var count = ctrl.numbOfNotUndefined(question.userAnswer);
            var className = parentIndex + '' + index;


            if ((question.AnswerType === 6 || question.AnswerType === 5)) {

                if (count > question.MaxNumbOfAnswers) {
                    //   question.userAnswer[question.userAnswer.length - 1] = undefined;

                    angular.forEach(question.userAnswer, function (ua) {

                        var _index = question.userAnswer.indexOf(ua);
                        if (ua != undefined && ua === question.QuestionAnswers[index].Id) {
                            question.userAnswer[index] = undefined;

                            document.getElementsByClassName(className)[0].checked = false;
                            return;
                        }

                    });
                }

            }
            
        }
        ctrl.multiSelectClick = function (question, index, parentIndex) {

            var className = parentIndex + '' + index;
            var count = question.userAnswer.length;

            if (question.AnswerType === 3 && question.MaxNumbOfAnswers > 1) {
                 if (count > question.MaxNumbOfAnswers) {
                     angular.forEach(question.userAnswer, function (ua) {
                        var _index = question.userAnswer.indexOf(ua);

                        if (ua == question.QuestionAnswers[index].Id) {
                            question.userAnswer.splice(_index, 1);
                            var s = document.getElementsByClassName(className)[0];
                            document.getElementsByClassName(className)[0].selected = false;
                            return;
                        }
                    });
                }
                 return;
            }
        }
    }

    angular.module('WebSurveying2017').component('questionComponent', {
        templateUrl: 'AngularClient/Question/question.html',
        bindings: {
            question: '<'

        },
        controller: questionController
    })

})(angular);
(function (angular) {

    angular.module('WebSurveying2017').service('surveyValidation', svService);

    function svService() {
        var instance = this;

        instance.validateQuestion = function (question) {
            var errorMessage = null;
            var questionTextTrim = "";
            if (question.QuestionText != null)
                questionTextTrim =  question.QuestionText.trim();
            if (question.QuestionText === null || questionTextTrim === "")
                return errorMessage = "Tekst pitanja ne sme biti prazan !";
            if (question.AnswerType === 1) {
                question.QuestionAnswers = [];
                return null;
            }
            if (question.AnswerType !== 1 && question.QuestionAnswers.length < 2) {


                return errorMessage = "Pitanja mora imati bar dva ponudjena odgovora !";
            }
            else if (question.AnswerType === 3 || question.AnswerType == 6) {
                if (question.MinNumbOfAnswers < 1) {
                    return errorMessage = "Minimalan broj izabranih odgovora mora biti veći od 0";
                }
                else if (question.MinNumbOfAnswers > question.QuestionAnswers.length) {
                    return errorMessage = "Minimalan broj izabranih odgovora mora biti manji ili jednak broju ponuđenih odgovora";
                }
                else if (question.MinNumbOfAnswers > question.MaxNumbOfAnswers) {
                    return errorMessage = "Maksimalan broj izabranih odgovora mora biti veći ili jednak od minimalnog broja ponuđenih odgovora";
                }
                else if (question.MaxNumbOfAnswers < 2) {
                    return errorMessage = "Maksimalan broj ponudjenenih odgovora mora biti veći od 1";
                }
                else if (question.MaxNumbOfAnswers > question.QuestionAnswers.length) {
                    return errorMessage = "Maksimalan broj izabranih odgovora mora biti manji ili jednak broju ponuđenih odgovora";
                }
                

            }

            for (var i = 0; i < question.QuestionAnswers.length; i++) {
                var answerTrim = "";
                if (question.QuestionAnswers[i].AnswerText != null) {
                    answerTrim = question.QuestionAnswers[i].AnswerText.trim();
                }
                if (question.QuestionAnswers[i].AnswerText === null || answerTrim === "")
                    return errorMessage = "Ponudjeni odgovor ne sme biti prazan";
            }

            return null;
        };
        

        return instance;
    };

})(angular);
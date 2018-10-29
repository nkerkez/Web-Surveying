(function (angular) {

    angular.module('WebSurveying2017').service('helpService', helpService);

    function helpService() {

        var instance = this;

        instance.removeUserAnswerObj = function (survey) {
            for (var i = 0; i < survey.Questions.length; i++) {
                if (survey.Questions[i].userAnswer != null)
                    delete survey.Questions[i].userAnswer;
            }
        };

        instance.contains = function (list, id) {
            
            var retVal = false;
            
            angular.forEach(list, function (_id) {

                if (_id == id) {
                    retVal = true;

                }

            });
         //   console.log(list, id, retVal);
            return retVal;

        }

       
        
        instance.initUpdatedQuestions = function (oldSurvey, survey) {
            var updatedQuestions = [];
            angular.forEach(oldSurvey.Questions, function (oldQuestion) {
                var isUpdated = true;
                angular.forEach(survey.Questions, function (question) {

                    if (angular.equals(oldQuestion, question)) {
                        console.log('----EQUALS---');
                        isUpdated = false;
                    }
                });

                if (isUpdated) {
                    updatedQuestions.push(oldQuestion.Id);
                }
            });

            return updatedQuestions;
        };

        instance.initGroups = function (groups)
        {
            var retVal = []
            angular.forEach(groups, function (group) {
                retVal.push(
                    {
                        GroupId: group.Id
                    });
            });
            return retVal;
        }
        instance.fromGroupToSurveyGroup = function (group, surveyGroup) {
            for (var j = 0; j < surveyGroup.length; j++) {
                for (var i = 0; i < group.length; i++) {
                    if (surveyGroup[j].GroupId == group[i].Id) {
                        surveyGroup[j] = group[i];
                    }
                }
            }
        }
        instance.fromQueryStringToObj = function (queryString) {
            var qs = decodeURIComponent(queryString);
            var retVal = {};
            var array = qs.split("?")[1].split("&");
            angular.forEach(array, function (obj) {
                var prop = obj.split("=")[0];
                var value = obj.split("=")[1];
                if (retVal[prop] != null && !(Array.isArray(retVal[prop]))) {
                    retVal[prop] = [retVal[prop], value]
                }
                else if (Array.isArray(retVal[prop])) {
                    retVal[prop].push(value);
                }
                else
                    retVal[prop] = value;
            });
            return retVal;
        }
        return instance;
        
    }
})(angular);
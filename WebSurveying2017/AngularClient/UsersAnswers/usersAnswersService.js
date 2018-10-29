(function (angular) {

    angular.module('WebSurveying2017').factory('usersAnswersService', usAnService);

    usAnService.$inject = ['$http'];
    function usAnService($http) {

        var url = '/api/usersanswers';
        var service = {
            postUserAnswers: postUserAnswers,
            putUserAnswers: putUserAnswers,
            resetSurvey: resetSurvey
        };

        function postUserAnswers(userAnswers, id) {

            return $http({
                'method': 'POST',
                'data': userAnswers,
                'url': url + '/' + id
            });
        }

        function putUserAnswers(userAnswers, id) {

            return $http({
                'method': 'PUT',
                'data': userAnswers,
                'url': url + '/' + id
            });
        }

        function resetSurvey(surveyId) {

            return $http({
                'method': 'DELETE',
                'url': url + '/survey/' + surveyId
            });
        }
        return service;
    }
})(angular);
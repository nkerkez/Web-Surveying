(function (angular) {

    angular.module('WebSurveying2017').factory('userSurveyService', usService);

    usService.$inject = ['$http'];
    function usService($http) {
        var url = '/api/userssurvey';
        var service = {
            getUsers : getUsers
        };

        function getUsers(surveyId) {

            return $http({
                'method': 'GET',
                'url': url + '/' + surveyId
            });
        }
        return service

    }

}) (angular);
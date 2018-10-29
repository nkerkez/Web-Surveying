(function (angular) {
    'use-strict'

    angular.module('WebSurveying2017').factory('favoriteSurveysService', fsService);

    fsService.$inject = ['$http'];

    function fsService($http) {
        var url = 'api/favoriteSurveys';
        var service = {
            postFavoriteSurvey,
            removeFromFavoriteSurveys : removeFromFavoriteSurveys
        };

        function removeFromFavoriteSurveys(surveyId)
        {
            return $http(
                {
                    method: 'DELETE',
                    url: url + '/' + surveyId

                });
        }

        function postFavoriteSurvey(data) {
            return $http(
                {
                    method: 'POST',
                    url: url,
                    data: data
                });
        };


        return service;
    }

})(angular);
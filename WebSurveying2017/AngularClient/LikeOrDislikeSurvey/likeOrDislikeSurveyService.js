(function (angular) {
    'use-strict'

    angular.module('WebSurveying2017').factory('likeOrDislikeSurveyService', lodService);

    lodService.$inject = ['$http'];

    function lodService($http) {
        var url = 'api/LikeOrDislikeSurvey';
        var service = {
            likeOrDislike: likeOrDislike,
            getLikesForSurvey: getLikesForSurvey,
            getDislikesForSurvey: getDislikesForSurvey
        };

        function getLikesForSurvey(id) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/like/' + id
                
                });
        };
        function getDislikesForSurvey(id) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/dislike/' + id

                });
        
        };

        function likeOrDislike(data) {
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
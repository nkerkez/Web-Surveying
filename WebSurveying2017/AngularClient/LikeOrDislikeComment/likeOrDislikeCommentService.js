(function (angular) {
    'use-strict'

    angular.module('WebSurveying2017').factory('likeOrDislikeCommentService', lodService);

    lodService.$inject = ['$http'];

    function lodService($http) {
        var url = 'api/LikeOrDislikeComment';
        var service = {
            likeOrDislike: likeOrDislike,
            putLikeOrDislike: putLikeOrDislike,
            getLikesForComment: getLikesForComment,
            getDislikesForComment: getDislikesForComment
        };

        function putLikeOrDislike(data) {
            return $http(
                {
                    method: 'PUT',
                    url: url + '/' + data.CommentId,
                    data: data
                });
        };
        function likeOrDislike(data) {
            return $http(
                {
                    method: 'POST',
                    url: url,
                    data : data
                });
        };
        function getLikesForComment(id) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/like/' + id
                    
                });
        }
        function getDislikesForComment(id) {
            return $http(
                {
                    method: 'GET',
                    url: url + '/dislike/' + id

                });
        }

       
        return service;
    }

})(angular);
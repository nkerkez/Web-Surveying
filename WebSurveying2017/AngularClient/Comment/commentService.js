(function ()
{
    'use-strict'

    angular.module('WebSurveying2017').factory('commentService', commentService);

    commentService.$inject = ['$http'];

    function commentService($http) {

        var url = 'api/comments';

        var service = {
            postComment: postComment,
            getComments: getComments,
            getComment: getComment,
            changeState: changeState,
            putComment: putComment,
            getCommentsForUser: getCommentsForUser,
            deleteEntity: deleteEntity
        };

        function deleteEntity(id) {
            return $http(
                {
                    method: 'DELETE',
                    url: url + '/' + id

                });


        }

        function postComment(comment) {
           return  $http(
                {
                    method: 'POST',
                    url: url,
                    data: comment
                });
        };

        function getComment(id) {
            return $http({
                method: 'GET',
                url: url + '/' + id
            });
        }

        function getComments(surveyId, page, size) {
            return $http({
                method: 'GET',
                url: url + '/survey/' + surveyId + '/' + page + '/' + size
            });
        }
        function getCommentsForUser(page, size) {
            return $http({
                method: 'GET',
                url: url + '/user/' + page + '/' + size
            });
        }
        function changeState(comment) {
            return $http({
                method: 'PUT',
                url: url + '/changestate/' + comment.Id,
                data: comment
            });
        };

        function putComment(comment) {
            return $http({
                method: 'PUT',
                url: url + '/' + comment.Id,
                data: comment
                });
        };

        
        return service;
    }

})();
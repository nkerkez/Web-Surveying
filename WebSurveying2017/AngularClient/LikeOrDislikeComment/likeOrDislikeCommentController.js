(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').controller('likeOrDislikeCommentController', ctrl);

    function ctrl(likeOrDislikeCommentService, $stateParams) {

        var ctrl = this;

        ctrl.users = [];
        ctrl.flagLike = true;
        ctrl.viewLikes = function () {
            likeOrDislikeCommentService.getLikesForComment($stateParams.id).then(
                function (response) {

                    ctrl.users = response.data;
                    ctrl.flagLike = true;
                },
                function (error) {

                }
            );
        };

        ctrl.viewDislikes = function () {
            likeOrDislikeCommentService.getDislikesForComment($stateParams.id).then(
                function (response) {
                    ctrl.flagLike = false;
                    ctrl.users = response.data;

                },
                function (error) {

                }
            );
        };
        (function () {

            ctrl.viewLikes();

        })();
    }
})(angular);
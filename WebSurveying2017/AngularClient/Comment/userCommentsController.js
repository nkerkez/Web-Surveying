(function(angular) {

    angular.module('WebSurveying2017').controller('userCommentsController', userCommentsCtrl);

    function userCommentsCtrl(commentService, $stateParams, $state) {
        var ctrl = this;

        (function () {
            commentService.getCommentsForUser( $stateParams.page, $stateParams.size).then(
                function(response) {
                    ctrl.comments = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    ctrl.size = response.data.Size;
                    console.log(ctrl.comments.length)
                },
                function(response) {
                    alert('greska')
                });
        })();

        ctrl.pageChanged = function () {

            $state.go("userComments", { "page": ctrl.currentPage, "size": $stateParams.size }, { reload: true });


        }

    };
})(angular);
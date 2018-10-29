(function () {
    'use-strict'

    function postCommentController(commentService, $uibModalStack, $uibModal, $scope, $state, $rootScope) {

        var ctrl = this;
        
        console.log('aaaaaaaaaaa')
        console.log(ctrl.comment);

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }

        ctrl.postComment = function () {

            console.log(ctrl.comment);


            if (ctrl.comment.Id != null) {
                commentService.putComment(ctrl.comment).then(
                    function (response) {
                        $scope.message = "Uspešno ste izmenili komentar.";
                        $scope.image = "../AngularClient/postcomment.png";
                        ctrl.openModal();

                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                            $state.go('comment', { surveyId: ctrl.comment.SurveyId, id: ctrl.comment.Id  });
                        }, 2666);
                    },
                    function (response) {
                        ctrl.errors = response.data.ModelState;
                    });
            }
            else {

                commentService.postComment(ctrl.comment).then(
                    function (response) {
                        $scope.message = "Uspešno ste napisali komentar.";
                        $scope.image = "../AngularClient/postcomment.png";
                        ctrl.openModal();

                        setTimeout(function () {
                            $uibModalStack.dismissAll();
                            $state.go('userComments', { page: 1, size: 10 });
                        }, 2666);
                    },
                    function (response) {
                        ctrl.errors = response.data.ModelState;
                    });
            }
        }
    };

    angular.module('WebSurveying2017').component('postCommentComponent',
        {
            templateUrl: 'AngularClient/Comment/postComment.html',
            bindings: {
                comment : '<'
                
            },

            controller : postCommentController

        });
})();
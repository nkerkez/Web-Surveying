(function () {
    'use-strict'

    function commentsCtrl($uibModal, $rootScope, likeOrDislikeCommentService, $state, commentService, $scope) {
        var ctrl = this;
        ctrl.comment = {};


        $scope.service = commentService;

        ctrl.contains = function (catList, catId) {
            console.log(catList, catId);
            var retVal = false;

            angular.forEach(catList, function (id) {

                if (id == catId)
                {
                    retVal = true;
                    
                }

            });
            return retVal;

        }

        ctrl.viewLikes = function (comment) {
            console.log('------a---------');
            console.log(comment.Id);
            $state.go('likeordislikecommentlist', { id: comment.Id });
        };

        


        ctrl.showHide = function (comment) {
            if (comment.showSubs == true)
                comment.showSubs = false;
            else
                comment.showSubs = true;
        };

        ctrl.updateComment = function (comment) {
            var modalInstance = $uibModal.open({

                templateUrl: "AngularClient/Comment/commentModal.html",
                controller: function () {
                    var modalCtrl = this;

                    modalCtrl.comment = angular.copy(comment, modalCtrl.comment);
                },
                controllerAs: 'ctrl',
                backdrop: false
            });
        };
        ctrl.postComment = function (parentId) {
            
            ctrl.comment.Text = '';
            ctrl.comment.ParentId = parentId;
            ctrl.comment.UserId = 1;
            ctrl.comment.SurveyId = ctrl.survey.Id;
            /*
            if (rootId == null)
                ctrl.comment.RootId = parentId;
            else
                ctrl.comment.RootId = rootId;
            */
            var modalInstance = $uibModal.open({

                templateUrl: "AngularClient/Comment/commentModal.html",
                controller: function () {
                    var modalCtrl = this;


                    modalCtrl.comment = ctrl.comment;
                },
                controllerAs : 'ctrl',
                backdrop : false
            });
        }


        ctrl.deleteComment = function (comment) {

            $scope.text = "Da li ste sigurni da želite da obrišete komentar : " + comment.Text + " ?"
            $scope.Id = comment.Id;
            var modalInstance = $uibModal.open(
                {
                    templateUrl: 'AngularClient/Delete/deleteModal.html',
                    controller: 'deleteController',
                    controllerAs: 'deleteCtrl',
                    scope: $scope,
                    backdrop: false
                });
        }

        ctrl.acceptOrReject = function (comment, isAccept) {

            if (isAccept) {
                comment.CommentState = 2
            }
            else
            {
                comment.CommentState = 1;
            }
            
            commentService.changeState(comment).then(
                function (response) {
                    console.log('-----ok------');
                },
                function (error) {
                    console.log('-----error------');
                });

        };

        
    }

    angular.module('WebSurveying2017').component('commentsComponent',
        {
            templateUrl: 'AngularClient/Comment/comments.html',
            bindings: {
                parentComments: '<',
                survey: '<',
                commentId : '<'
            },
            controller:
                commentsCtrl
        });

})();
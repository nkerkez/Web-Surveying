(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').controller('notificationsController', nCtrl);

    function nCtrl(userNotificationService, $state, notificationService) {

        var ctrl = this;
        ctrl.notifications = [];
        (function () {

            userNotificationService.getAllForUser().then(
                function (response) {
                    ctrl.notifications = response.data;
                    console.log(response.data);
                },
                function (error) {

                }
            );
        })();

        ctrl.clickOnNotification = function (n, onlyRead) {

            console.log(n);
            // for moderaor
            var obj = {
                NotificationId: n.Id,
                IsRead: true
            }
            console.log(obj)
            userNotificationService.readNotification(obj).then(
                function (response) {
                    if (!onlyRead) {
                        if (n.Operation == 5) {
                            $state.go('surveyresult', { id: n.SurveyId, userId: n.UserId });
                        }
                        else if (n.CommentId == null && n.SurveyId > 0) {

                            $state.go('survey', { id: n.SurveyId });
                        }
                        else if (n.CommentId > 0 && n.SurveyId > 0) {
                            $state.go('comment', { id: n.CommentId });
                        }
                    }
                    else
                    {
                        ctrl.showAll();
                    }
                
                },
            function (error) {

            });


    }

    ctrl.showAll = function () {
        userNotificationService.getAllForUser().then(
            function (response) {
                ctrl.notifications = response.data;
                console.log(response.data);
            },
            function (error) {

            }
        );
    };

    ctrl.showPersonal = function () {
        userNotificationService.getPersonalForUser().then(
            function (response) {
                ctrl.notifications = response.data;
                console.log(response.data);
            },
            function (error) {

            }
        );
    };

    ctrl.showModerator = function () {
        userNotificationService.getModeratorForUser().then(
            function (response) {
                ctrl.notifications = response.data;
                console.log(response.data);
            },
            function (error) {

            }
        );
    }
}
})(angular);
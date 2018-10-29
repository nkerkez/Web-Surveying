(function (angular) {

    angular.module('WebSurveying2017').controller('myGroupsController', myGroupsCtrl);

    function myGroupsCtrl(groupService, $stateParams, $state) {

        var ctrl = this;

        (function () {

            var s = '';
            if ($stateParams.queryString != null)
                s = $stateParams.queryString;

            groupService.getForUser(s, $stateParams.page, $stateParams.size).then(
                function (response) {
                    ctrl.groups = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    ctrl.size = response.data.Size;
                },
                function (response) {

                }
            );
        })();

        ctrl.searchGroups = function (queryString) {
            console.log(queryString);
            $state.go('searchMyGroups', { queryString: queryString, page: 1, size: 10 });

        }

        ctrl.updateName = function (group, name) {
            group.Name = name;

            groupService.updateGroup(group).then(
                function (response) {
                    console.log('ok');
                },
                function (response) {
                    console.log('error');
                }
            );
        }

    }

})(angular);
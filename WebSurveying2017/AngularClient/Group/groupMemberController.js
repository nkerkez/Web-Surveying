(function (angular) {

    angular.module('WebSurveying2017').controller('groupMemberController', groupMemberCtrl);

    function groupMemberCtrl(groupService, $stateParams, $state) {

        var ctrl = this;

        (function () {

            var s = '';
            if ($stateParams.queryString != null)
                s = $stateParams.queryString;

            groupService.getForMember(s, $stateParams.page, $stateParams.size).then(
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
            $state.go('searchGroupMember', { queryString: queryString, page: 1, size: 10 });

        }

       

    }

})(angular);
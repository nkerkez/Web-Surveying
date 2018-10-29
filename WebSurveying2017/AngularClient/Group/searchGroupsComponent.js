(function (angular) {

    function searchGroupsCmpCtrl($stateParams, helpService) {

        var ctrl = this;
        (function () {
            if ($stateParams.queryString != null && $stateParams.queryString != '') {
                ctrl.obj = helpService.fromQueryStringToObj($stateParams.queryString);
            }
            else {
                ctrl.obj = {
                    Name: '',
                    AuthorFirstName: '',
                    AuthorLastName: ''
                }
            }

        })();
        ctrl.searchGroupsClick = function () {
           
            ctrl.queryString = '';

            ctrl.queryString = $('#searchGroupsForm').serialize();
            ctrl.queryString = '?' + ctrl.queryString;
            ctrl.searchGroups({ 'queryString': ctrl.queryString });
        };
    }

    angular.module('WebSurveying2017').component('searchGroupsComponent', {

        templateUrl: 'AngularClient/Group/searchGroups.html',
        bindings: {

            searchGroups : '&'
        },
        controller : searchGroupsCmpCtrl

    });

})(angular);
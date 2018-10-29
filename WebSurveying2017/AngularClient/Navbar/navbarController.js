(function (angular) {

    angular.module('WebSurveying2017').controller('navbarController', nbCtrl);


    function nbCtrl($rootScope, $uibModal, userExtendService, categoryService, $state, surveyService) {

        
        
        var ctrl = this;
        ctrl.categories = [];

        var id = localStorage.getItem('id');
        console.log(localStorage)
        ctrl.username = localStorage.getItem('username');

        $rootScope.role = localStorage.getItem('role');
        if (ctrl.username != null)
            $rootScope.isAuth = true;
        else
            $rootScope.isAuth = false;
        console.log('-------------OK-------------------');

        ctrl.download = function ()
        {

            window.open('http://localhost:49681/api/surveys/download');
           
        }

        userExtendService.get(id).then(
            function (response) {
                console.log(response.data);
                $rootScope.loggedInUser = response.data;
            },
            function (error)
            {

            }
        );
        
        ctrl.createCategory = function () {
            $state.go('createCategory');
        }
        ctrl.createGroup = function () {
            ctrl.createGroup = function () {
                $state.go('newGroup', { page : 1, size : 10});
            }
        };

        


    };

})(angular);
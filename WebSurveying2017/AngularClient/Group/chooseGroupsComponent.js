(function (angular) {


    'use-strict'

    function chooseGroupsController() {
        var ctrl = this;

        
        

    }

    angular.module('WebSurveying2017').component('chooseGroupsComponent',
        {
            templateUrl: 'AngularClient/Group/chooseGroups.html',
            bindings: {
                myGroups: "<",
                survey: "<"
            },
            controller: chooseGroupsController
        }
    );
})(angular);
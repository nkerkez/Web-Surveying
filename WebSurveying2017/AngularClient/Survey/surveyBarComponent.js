(function (angular) {

    function sbController(groupService, $state, $rootScope, $stateParams, $scope) {

        var ctrl = this;
        
        ctrl.state = parseInt($stateParams.state);
       
        ctrl.groupId = null;
        
        (function () {

            if (localStorage.getItem('username') != null) {

                groupService.getForAuthorAndMember().then(
                    function (response) {

                        ctrl.groups = response.data;
                        

                    },
                    function (response) {
                        console.log('error');
                    }
                );

            }
        })();

        ctrl.$onInit = function () {

            
            ctrl.groupId = ctrl.typeOfSurveys == 'group' ? parseInt($stateParams.id) : null;
            
            
            
        }
        ctrl.onInit = function () {
            console.log(ctrl.groupId, ctrl.typeOfSurveys);
        }
       
        ctrl.all = function () {
            $state.go('home', { page: 1, size: 10, state: $stateParams.state });
        };
        ctrl.public = function () {
            $state.go('publicSurveys', { page: 1, size: 10, state: $stateParams.state });
        }

        ctrl.changeValue = function () {
            if (ctrl.groupId != null)
                $state.go('groupSurveys', { id: ctrl.groupId, page: 1, size: 10, state: $stateParams.state }, { reload: true });
                
        }

        ctrl.changeState = function () {
            console.log(ctrl.state);
            $stateParams.state = ctrl.state;
            ctrl.changeStateValue({state : ctrl.state});
        }
        
    }

    angular.module('WebSurveying2017').component('surveyBarComponent',
        {
            templateUrl: 'AngularClient/Survey/surveyBar.html',
            controller : sbController,
            bindings: {
                typeOfSurveys: '<',
                changeStateValue : '&'
            }
        });
})(angular);
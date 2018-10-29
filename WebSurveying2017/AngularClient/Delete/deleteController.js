(function (angular) {
    angular.module('WebSurveying2017').controller('deleteController', dCtrl);

    function dCtrl($stateParams, $scope, $uibModalStack, $uibModal, $state) {
        var ctrl = this;

        ctrl.text = $scope.text;
        
        ctrl.delete = function () {
            console.log('deleting')
            $scope.service.deleteEntity($scope.Id).then(
                function (response) {
                    $scope.message = "Uspešno brisanje.";
                    $scope.image = "../AngularClient/deleted.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('home', { page: 1, size: 10, state: 3 });
                    }, 2666);
                },
                function (response) {
                    $scope.message = "Neuspešno brisanje";
                    for (var error in response.data.ModelState) {
                        $scope.message += " " + response.data.ModelState[error];
                    }
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                });
        }
        ctrl.deleteEntity = function (flag) {

            if (flag) {
                ctrl.delete();
                $uibModalStack.dismissAll();
            }
            else {
                $uibModalStack.dismissAll();
            }
        }

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs: 'successCtrl'


            });
        }
    }
})(angular);
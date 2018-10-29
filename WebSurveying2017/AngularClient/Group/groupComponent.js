(function (angular) {

    function groupCompCtrl(groupService, requestService, $scope, $uibModal, $uibModalStack, $state) {
        var ctrl = this;
        $scope.service = groupService;
        ctrl.updateName = function (group, name) {
            group.Name = name;

            groupService.updateGroup(group).then(
                function (response) {
                    $scope.message = "Uspešno ste izmenili naziv grupe.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('groups', { page: 1, size: 10 }, { reload: true });
                    }, 2666);
                },
                function (response) {
                    $scope.message = "Naziv grupe nije izmenjen. Naziv mora biti jedinstven.";
                    for (var m in response.data.ModelState) {
                        $scope.message+=" "+ response.data.ModelState[m] + "."
                    }
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 3666);
                }
            );

        }
        ctrl.deleteGroup = function () {

            $scope.text = "Da li ste sigurni da želite da obrišete grupu pod nazivom " + ctrl.group.Name + " ?"
            $scope.Id = ctrl.group.Id;
            var modalInstance = $uibModal.open(
                {
                    templateUrl: 'AngularClient/Delete/deleteModal.html',
                    controller: 'deleteController',
                    controllerAs: 'deleteCtrl',
                    scope: $scope,
                    backdrop: false
                });
        }
        ctrl.sendRequest = function () {
            var obj = {
                GroupId: ctrl.group.Id,
                
            };

            requestService.postRequest(obj).then(
                function (response) {
                    $scope.message = "Uspešno ste poslali zahtev za pristup grupi.";
                    $scope.image = "../AngularClient/groupadd.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('groups', { page: 1, size: 10 }, { reload: true });
                    }, 2666);
                },
                function (response) {
                    $scope.message = "Zahtev za pristup grupi nije moguće poslati.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                });
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
    angular.module('WebSurveying2017').component('groupComponent', {
        templateUrl: 'AngularClient/Group/group.html',
        bindings: {
            group : '<'
        },
        controller : groupCompCtrl

    })

})(angular)
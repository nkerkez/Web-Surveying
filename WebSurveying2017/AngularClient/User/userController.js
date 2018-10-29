(function (angular) {

    'use-strict'

    angular.module('WebSurveying2017').controller('userController', userCtrl);

    function userCtrl($state, $stateParams, $rootScope, userService, categoryService, userCategoryService, authService, userRoleService, $uibModal, $uibModalStack, $scope) {

        var ctrl = this;
        ctrl.showModCats = false;
        
        ctrl.userId = $stateParams.id;
      
       // ctrl.isMyProfile = $stateParams.id == $rootScope.loggedInUser.id;
        (function () {
            console.log($stateParams.id, $rootScope.loggedInUser)
            userService.getUser($stateParams.id).then(
                function (response) {
                    ctrl.user = response.data;
                },
                function (response)
                {

                }
                
            );

        })();
        ctrl.update = function () {

            userService.putUser(ctrl.user).then(
                function (response) {
                    
                    $scope.message = "Uspešno ste izmenili Vaš profil.";
                    $scope.image = "../AngularClient/Correct.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('user', { id: ctrl.user.Id }, { reload: true });
                    }, 2666);
                  
                },
                function (response) {
                    ctrl.errors = response.data.ModelState;
                }
            );
        }
        ctrl.resetPassword = function (obj) {
           
            authService.resetPassword(obj).then(
                function (response) {
                    $scope.message = "Uspešno ste izmenili Vašu šifru.";
                    $scope.image = "../AngularClient/Correct.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        localStorage.removeItem('token');
                        localStorage.removeItem('username');
                        localStorage.removeItem('role');
                        localStorage.removeItem('id');
                        $state.go('login');
                    }, 2666);
                   
                },
                function (response) {
                    $scope.message = "Resetovanje šifre nije izvršeno. Šifre se ne poklapaju";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }


        ctrl.changeRole = function (roleName)
        {
           
            userRoleService.updateUserRole(ctrl.user.Id, roleName).then(
                function (response) {
                    $scope.message = "Uspešno ste izmenili ulogu korisnika.";
                    $scope.image = "../AngularClient/Correct.png";
                    ctrl.openModal();

                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('user', { id: ctrl.user.Id }, { reload: true });
                    }, 2666);
                },
                function (response) {
                    $scope.message = "Promena uloge nije uspešno izvršena. Došlo je do greške.";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                    }, 2666);
                }
            );
        }

        ctrl.openModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'AngularClient/Success/success.html',
                scope: $scope,
                controller: function () { },
                controllerAs : 'successCtrl'
                

            });
        }
        

    }

})(angular);
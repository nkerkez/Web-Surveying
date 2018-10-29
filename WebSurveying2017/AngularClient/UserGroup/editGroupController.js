(function (angular) {

    angular.module('WebSurveying2017').controller('editGroupController', editGroupCtrl);


    function editGroupCtrl(userService, $stateParams, $state, userGroupService, $uibModal, $uibModalStack, $scope) {
        var ctrl = this;
        ctrl.users = [];
        ctrl.usersInGroup = JSON.parse(localStorage.getItem('usersInGroup'));
        ctrl.groupId = $stateParams.id;
        (function () {

            userService.getUsersForGroup($stateParams.id).then(
                function (response) {
                    ctrl.usersInGroup = response.data;
                    localStorage.setItem('usersInGroup', JSON.stringify(ctrl.usersInGroup));
                    console.log(ctrl.user);
                },
                function () {

                }
            );
            userService.searchUsers($stateParams.queryString, $stateParams.page, $stateParams.size).then(
                function (response) {
                    console.log(response.data.Models);
                    ctrl.users = response.data.Models;
                    ctrl.currentPage = response.data.CurrentPage;
                    ctrl.totalItems = response.data.Count;
                    ctrl.size = response.data.Size;


                },
                function (error) {
                    console.log(error);
                }
            );
            

        })();

        ctrl.group = {
            Name: '',
            UserGroupList: []
        }

        ctrl.createGroup = function () {
            angular.forEach(ctrl.usersInGroup, function (user) {
                ctrl.group.UserGroupList.push({ 'UserId': user.Id , 'GroupId': $stateParams.id });
            });
            
            console.log(ctrl.group);

            userGroupService.editMembers($stateParams.id, ctrl.group.UserGroupList).then(
                function (response) {


                    localStorage.setItem('usersInGroup', JSON.stringify([]));

                    $scope.message = "Uspešno ste izmenili grupu.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('myGroups', { page: 1, size: 10 });
                    }, 2666);


                },
                function (response) {
                    ctrl.group.UserGroupList = [];
                    $scope.message = "Izmena grupe nije izvršena. Došlo je do greške";
                    $scope.image = "../AngularClient/negative.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        ctrl.errors = response.data.ModelState;
                    }, 2666);
                });
        }


        ctrl.pageChanged = function () {
            console.log('aaaa')
            $state.go("editGroup", { id: $stateParams.id, "page": ctrl.currentPage, "size": ctrl.size, queryString: $stateParams.queryString });


        }

        ctrl.removeFromGroup = function (user) {
            for (var i = 0; i < ctrl.usersInGroup.length; i++) {
                if (ctrl.usersInGroup[i].Id == user.Id) {
                    ctrl.usersInGroup.splice(i, 1);
                }
            }

            localStorage.setItem('usersInGroup', JSON.stringify(ctrl.usersInGroup));
        }
        ctrl.addUser = function (user) {
            ctrl.usersInGroup = JSON.parse(localStorage.getItem('usersInGroup'));

            if (ctrl.usersInGroup == null) {
                ctrl.usersInGroup = [];
            }
            var temp = 0;
            angular.forEach(ctrl.usersInGroup, function (_user) {
                if (_user.Id == user.Id)
                    temp = 1;
            });
            if (temp == 1)
                return;
            ctrl.usersInGroup.push(user);
            localStorage.setItem('usersInGroup', JSON.stringify(ctrl.usersInGroup));

        }
        ctrl.searchUsers = function (queryString) {
            $state.go('editGroup', { "page": 1, "size": $stateParams.size, queryString: queryString, id: $stateParams.id }, { notify: true });
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
    };

})(angular);
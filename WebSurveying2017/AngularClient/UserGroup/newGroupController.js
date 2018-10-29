
(function (angular) {

    angular.module('WebSurveying2017').controller('newGroupController', newGroupCtrl);


    function newGroupCtrl(userService, $stateParams, $state, userGroupService, $state, $uibModal, $uibModalStack, $scope) {
        var ctrl = this;
        ctrl.users = [];
        ctrl.usersInGroup = JSON.parse(localStorage.getItem('usersInGroup'));
        
        (function () {

           
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
                ctrl.group.UserGroupList.push({ 'UserId' : user.Id });
            });
           
            

            userGroupService.postGroup(ctrl.group).then(
                function (response) {
                    localStorage.setItem('usersInGroup', JSON.stringify([]));
                    ctrl.usersInGroup = [];
                    $scope.message = "Uspešno ste kreirali grupu.";
                    $scope.image = "../AngularClient/successs.png";
                    ctrl.openModal();
                    setTimeout(function () {
                        $uibModalStack.dismissAll();
                        $state.go('myGroups', { page: 1, size: 10 });
                    }, 2666);
                    
                },
                function (response) {
                    ctrl.errors = response.data.ModelState;
                    ctrl.group.UserGroupList = [];
                });
        }
        ctrl.removeFromGroup = function (user)
        {
            console.log(user)
            for (var i = 0; i < ctrl.usersInGroup.length; i++){
                if (ctrl.usersInGroup[i].Id == user.Id)
                {
                    ctrl.usersInGroup.splice(i, 1);
                }
            }
            localStorage.setItem('usersInGroup', JSON.stringify(ctrl.usersInGroup));
        }

        ctrl.pageChanged = function () {
            console.log('bbbbb')
            $state.go("newGroup", { "page": ctrl.currentPage, "size": $stateParams.size, queryString: $stateParams.queryString });


        }


        ctrl.addUser = function (user)
        {
            ctrl.usersInGroup = JSON.parse(localStorage.getItem('usersInGroup'));

            if (ctrl.usersInGroup == null)
            {
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
        ctrl.searchUsers = function (queryString)
        {
            console.log(queryString);
            $state.go('newGroup', { "page": 1, "size": $stateParams.size, queryString: queryString }, { notify: true});
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
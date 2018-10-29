(function (angular) {

    function searchUsersController($stateParams, helpService) {
        var ctrl = this;

        (function () {
            ctrl.obj = {};
            ctrl.obj.Birthday = new Date(2004, 1, 1);

            ctrl.minDate = new Date(
                ctrl.obj.Birthday.getFullYear() - 100,
                ctrl.obj.Birthday.getMonth(),
                ctrl.obj.Birthday.getDate()
            );

            ctrl.maxDate = new Date(
                ctrl.obj.Birthday.getFullYear(),
                ctrl.obj.Birthday.getMonth() + 10,
                ctrl.obj.Birthday.getDate()
            );
            if ($stateParams.queryString != null && $stateParams.queryString != '') {

                ctrl.obj = helpService.fromQueryStringToObj($stateParams.queryString);
                ctrl.BirthdayFrom = ctrl.obj.BirthdayFrom;
                ctrl.BirthdayTo = ctrl.obj.BirthdayTo;
                
               
            }
            else {
                ctrl.obj = {
                    FirstName: '',
                    LastName: '',
                    City: '',
                    BirthdayFrom: '',
                    BirthdayTo : ''
                }
            }

        })();

        ctrl.initCheckBoxes = function (prop, identifikator) {
            if (ctrl.obj[prop]== null)
                return false;
            if (!Array.isArray(ctrl.obj[prop]))
                if (ctrl.obj[prop] == identifikator)
                    return true;
                else
                    return false;
            else {
                if (ctrl.obj[prop].includes(identifikator))
                    return true;
                else return false
            }
        }
        ctrl.changeDate = function (flag) {

            

            if (ctrl.BirthdayFrom != undefined && flag)
                ctrl.obj.BirthdayFrom = ctrl.BirthdayFrom.toISOString();
            if (ctrl.BirthdayTo != undefined && !flag)
                ctrl.obj.BirthdayTo = ctrl.BirthdayTo.toISOString();
           
        };
       
        ctrl.search = function () {
            ctrl.queryString = '';
            
            ctrl.queryString = $('#searchUsersForm').serialize();
            ctrl.queryString = '?' + ctrl.queryString;
            ctrl.searchUsers({ queryString: ctrl.queryString });
        }
    }

    angular.module('WebSurveying2017').component('searchUsersComponent',
        {
            templateUrl: 'AngularClient/User/searchUsers.html',
            bindings:
            {
                searchUsers : '&'
            },
            controller : searchUsersController
        });
})(angular);
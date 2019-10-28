var app = angular.module('ManageTestModule', []);

app.service('ManageTestService', function ($http, $location) {

    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"
    }
    else {

        this.AppUrl = "/api/"
    }
    this.GetSessionIdForTestReset = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageTest/GetSessionIdForTestReset');
    };
    this.UpdateSessionIdForTestReset = function (Obj) {
        return $http.post(this.AppUrl + '/ManageTest/UpdateSessionIdForTestReset' ,Obj);
    }; 
   
});
app.controller('ManageTestController', function ($scope, $http, $location, CityService, $rootScope, $filter, ManageTestService, helperService) {
    
    $scope.init = function () {
        ManageTestService.GetSessionIdForTestReset().then(function success(success) {
            console.log(success.data);
            $scope.SessionIdForTestReset = success.data;
            
        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };
    
    $scope.Update = function (pt) {
        console.log(pt);
        swal({
            title: 'Warning',
            text: "You are going to allow test for same day \n for Session ID - " + pt.SessionID,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            //  cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            ManageTestService.UpdateSessionIdForTestReset(pt).then(function success(success) {

                if (success.data.indexOf("Success") !== -1) {
                    swal("", success.data, "success");
                    $scope.init();
                }
                else {
                    swal("", success.data, "error");
                }
            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });
        });
    };
    
    $scope.init();

});
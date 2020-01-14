var app = angular.module('RegistrationInfoModule', []);

app.service('RegistrationInfoService', function ($http) {
    this.GetData = function (mspin) {
        return $http.get('api/RegistrationInfo/GetData?mspin=' + mspin);
    };

    //this.EditData = function (mspin) {
    //    return $http.post('api/RegistrationInfo/EditData?mspin=', + mspin);
    //};

    this.Reset = function (Obj) {
        return $http.post('api/RegistrationInfo/Reset?Registration_Id=', Obj);
    };
});


app.controller('RegistrationInfoController', function (RegistrationInfoService, $scope, $window) {
    $scope.ShowTable = false;
    $scope.GetData = function (mspin) {
        $scope.ShowTable = false;
        RegistrationInfoService.GetData(mspin)
            .then(function (success) {
                console.log(success);
                if (success.data != null) {
                    $scope.RegistrationDetail = success.data;
                    $scope.ShowTable = true;
                }
                else { swal("", "No Record Found", "info");}
            },
                function error(error) {
                    console.log(error);
                });
    };
    //$scope.showConfirmbox = function () {
    //    if ($window.Confirm("Would you like to reset your data?"))
    //        $scope.Result = "Yes";
    //    else
    //        $scope.Result = "No";
    //}
    //$scope.EditData = function (mspin) {
    //    console.log(mspin);
    //    $scope.mspin = mspin;
    //}

    //$scope.Update = function (mspin) {
    //    myService.EditData(mspin).then(function (success) {
    //        console.log(success.mspin);
    //        $scope.GetData();
    //    })
    //}

    $scope.Reset = function () {
        $scope.RegistrationDetail.IsActive = false;
        RegistrationInfoService.Reset($scope.RegistrationDetail).then(function (success) {
            console.log(success.data);
            $scope.GetData();
        })
    };
});

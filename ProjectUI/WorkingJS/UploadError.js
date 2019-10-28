var app = angular.module('UploadError', []);

app.service('UploadErrorService', function ($http, $location) {
    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.GetUploadTimeErrors = function () {
        return $http.get(this.AppUrl + 'Error/GetUploadError/');
    };
    this.UpdateErrorRecord = function (Obj) {
        return $http.post(this.AppUrl +'Error/UpdateErrorRecord/',Obj)
    };
});

app.controller('UploadErrorController', function (UploadErrorService, $http, $scope,$location) {
    $scope.ShowTable = false;
    $scope.init = function () {
        UploadErrorService.GetUploadTimeErrors().then(function (success) { 
            if (success.data != null) {
                $scope.ShowTable = true;
                $scope.ErrorList = success.data;
            }
            else
                $scope.ShowTable = false;
        },
            function (error) { });
    };
    $scope.UpdateErrorRecord = function () {
        UploadErrorService.UpdateErrorRecord().then(function (success) {
            if (success.data.indexOf('Success') != -1)
                swal("", success.data, "success");
            else
                swal("", success.data, "error");
        },
            function (error) { });
    };

    $scope.init();
});
var app = angular.module('VendorMasterModule', []);

app.service('VendorMasterService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.GetVendorList = function () {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/Vendor/GetVendorList');
    };
    this.AddVendorList = function (Data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Vendor/AddVendorList', Data);
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/Vendor/GetFacultyList?Agency_Id=' + Agency_Id);
    };
});

app.controller('VendorMasterController', function ($scope, $http, $location, VendorMasterService, $uibModal, $rootScope, InitFactory) {

    $scope.init = function () {
        $scope.ShowTable = false;
        VendorMasterService.GetVendorList().then(function success(data) {
            $scope.VendorList = data.data;
            if ($scope.VendorList != null) {
                $scope.ShowTable = true;
            }

        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.AddVendor = function (VendorDetails) {

        VendorMasterService.AddVendorList(VendorDetails).then(function success(data) {
            if (data.data.indexOf('Success') != -1) {
                swal("", data.data, "success");
                $scope.init();
            }
            else {
                swal("", data.data, "error");
            }
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.init();
});

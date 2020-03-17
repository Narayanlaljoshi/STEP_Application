var app = angular.module('VendorTrainierMasterModule', []);

app.service('VendorTrainierMasterService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.GetVendorTrainerList = function (User_Id) {
        return $http.get(this.AppUrl + '/Vendor/GetVendorTrainerList?User_Id=' + User_Id);
    };
    this.UpdateVendorTrainerList = function (Obj) {
        return $http.post(this.AppUrl + '/Vendor/UpdateVendorTrainerList', Obj);
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/Vendor/GetFacultyList?Agency_Id=' + Agency_Id);
    };
});

app.factory('InitFactory', function (VendorTrainierMasterService) {

    return {
        init: function () {

            VendorTrainierMasterService.GetVendorTrainerList($rootScope.session.User_Id).then(function success(data) {
                
                console.log("Get Region List", data);


            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});

app.controller('VendorTrainierMasterController', function ($scope, $http, $location, VendorTrainierMasterService,  $uibModal, $rootScope, InitFactory) {

    $scope.TrainerList = [];
    
    $scope.init = function () {
        VendorTrainierMasterService.GetVendorTrainerList($rootScope.session.User_Id).then(function success(data) {
            $scope.TrainerList = data.data;
            console.log("Get Region List", data);

        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.AddRow = function () {
        $scope.TrainerList.push({
            Id: 0,
            Vendor_Id: null,
            TrainerCode: null,
            TrainerName: '',
            TrainerMobile: '',
            TrainerEmail: '',
            IsActive: true,
            CreationDate: new Date(),
            ModifiedDate: '',
            CreatedBy: $rootScope.session.User_Id,
            ModifiedBy: $rootScope.session.User_Id,
            VendorName: $rootScope.session.UserName
        });

        console.log($scope.TrainerList);
    };
    $scope.RemoveRow = function (index) {
        $scope.TrainerList.splice(index, 1);
    };
    $scope.EditRow = function (pt) {
        pt.Id = 0;
    };
    $scope.UpdateTrainerList = function () {

        $scope.IsValid = true;

        angular.forEach($scope.TrainerList, function (v1, key) {
            v1.VendorName = $rootScope.session.UserName;
            $scope.Count = 0;
            angular.forEach($scope.TrainerList, function (v2, key) {
                if (v1.TrainerCode === v2.TrainerCode) {
                    $scope.Count++;
                }
            });
            if ($scope.Count > 1) {
                $scope.IsValid = false;
            }
        });
        if ($scope.IsValid) {
            VendorTrainierMasterService.UpdateVendorTrainerList($scope.TrainerList).then(function success(data) {
                if (data.data.indexOf('Success') != -1) {
                    $scope.init();
                    swal("", data.data, "success");
                }
                else {
                    swal("", data.data, "error");
                }
                console.log("Get Region List", data);

            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            swal("", "Duplicate Trainer Code is not allowed", "error");
        }
    };


    $scope.init();
});

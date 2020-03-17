var app = angular.module('VendorManageSessionModule', []);

app.service('VendorManageSessionService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.GetSessionList_StepManager = function (Filter) {
        return $http.post(this.AppUrl + '/Vendor/GetSessionList_StepManager/', Filter);
    };
    this.GetActiveTrainerForVendor = function (UserName) {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/Vendor/GetActiveTrainerForVendor?UserName=' + UserName);
    };
    this.UpdateFaculty = function (Obj) {
        return $http.post(this.AppUrl + '/Vendor/UpdateFaculty' ,Obj);
    };
    this.GetProgramList = function (UserName, Month) {
        return $http.get(this.AppUrl + '/ProgramMaster/GetProgram', {});
    };
});

app.factory('InitFactory', function (AttendanceReportService) {

    return {
        init: function () {

            AttendanceReportService.GetActiveTrainerForVendor().then(function success(data) {
                $scope.TrainerList= data.data;
                console.log("Get Region List", data);
                return AttendanceReportService.GetSessionList_StepManager();
            }).then(function success(RetData) {
                $scope.SessionList = RetData.data;
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});

app.controller('VendorManageSessionController', function ($scope, $http, $location, VendorManageSessionService, $uibModal, $rootScope, InitFactory) {
    $scope.MonthList = [{ Name: 'Jan', Month: 1 }, { Name: 'Feb', Month: 2 }, { Name: 'Mar', Month: 3 }, { Name: 'Apr', Month: 4 }, { Name: 'May', Month: 5 },
    { Name: 'Jun', Month: 6 }, { Name: 'Jul', Month: 7 }, { Name: 'Aug', Month: 8 }, { Name: 'Sep', Month: 9 }, { Name: 'Oct', Month: 10 },
    { Name: 'Nov', Month: 11 }, { Name: 'Dec', Month: 12 }];
    var date = new Date('01-Jan-2019');
    var year = date.getFullYear();
    var month = new Date().getMonth();
    $scope.Filter = {
        Month: month + 1,
        ProgramCode: null,
        UserName: $rootScope.session.UserName
    };
    $scope.Month = month + 1;
    $scope.ProgramList = [];
    $scope.init = function () {
        VendorManageSessionService.GetActiveTrainerForVendor($rootScope.session.UserName).then(function success(data) {
            $scope.TrainerList = data.data;
            console.log("Get Region List", data);
            return VendorManageSessionService.GetSessionList_StepManager($scope.Filter);
        }).then(function success(RetData) {
            $scope.SessionList = RetData.data;
            return VendorManageSessionService.GetProgramList();
        }).then(function (successPrgData) {
            $scope.ProgramList = successPrgData.data;
            var Obj = { ProgramCode: null, ProgramName: 'All' };
            $scope.ProgramList.splice(0, 0, Obj);
        }, function error(errorData) {
            console.log("Error in loading data from EDB", errorData);
        });
    };
    $scope.GetSessionList_StepManager = function () {
        VendorManageSessionService.GetSessionList_StepManager($rootScope.session.UserName, $scope.Month).then(function success(RetData) {
            
            $scope.SessionList = RetData.data;
        }, function error(errorData) {
            console.log("Error in loading data from EDB", errorData);
        });
    };
    $scope.UpdateFaculty = function (pt) {
        VendorManageSessionService.UpdateFaculty(pt).then(function success(data) {
            //$scope.TrainerList = data.data;
            if (data.data.indexOf('Success')!= -1)
            {
                swal("", "Updated Successfully!", "success");
                $scope.init();
            }
            else {
                swal("", data.data, "error");
            }
            console.log("Get Region List", data);
           
        }, function error(errorData) {
            swal("", errorData.data, "error");
            console.log("Error in loading data from EDB", errorData);
        });
    };
    $scope.init();
});

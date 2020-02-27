var app = angular.module('CloseCourse_DMSModule', []);

app.service('CloseCourse_DMSService', function ($http, $location) {
    this.AppUrl = "";
    this.MSPin = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') !== -1) {

        this.AppUrl = "/CST/api/";

    }
    else {
        this.AppUrl = "/api/";
    }
    this.UploadAttendance = function (EndDate) {
        return $http.get(this.AppUrl + '/Automation/PushAttendanceReport?EndDate=' + EndDate);
    };
    this.UploadScore = function (EndDate) {
        return $http.get(this.AppUrl + '/Automation/PushMarksReportAsperDMS?EndDate=' + EndDate);
    };

});


app.controller('CloseCourse_DMSController', function ($scope, $http, $filter, $location, CloseCourse_DMSService, AttendanceReportDMSService, $uibModal, $rootScope, InitFactory) {

    $scope.ShowReport = false;
    $scope.ReportInput = {
        Agency_Id: null,
        ProgramId: null,
        AgencyCode: null,
        Faculty_Id: null,
        SessionID: null,
        StartDate: null,
        EndDate: null

    };
    $scope.UploadAttendance = function () {

        console.log('inside init');
        if (!$scope.ReportInput.EndDate) {
            swal("Please Select End date");
        }
        else {
            CloseCourse_DMSService.UploadAttendance($scope.ReportInput.EndDate).then(function success(data) {
                if (data.data.indexOf('Success')) { swal("", data.data, "success"); }
                else {
                    swal("", data.data, "error");
                }
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
    $scope.UploadScore = function () {

        console.log('inside init');
        if (!$scope.ReportInput.EndDate) {
            swal("Please Select End date");
        }
        else {
            CloseCourse_DMSService.UploadScore($scope.ReportInput.EndDate).then(function success(data) {
                if (data.data.indexOf('Success')) { swal("", data.data, "success"); }
                else {
                    swal("", data.data, "error");
                }
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});


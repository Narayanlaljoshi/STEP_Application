var app = angular.module('TestResetModule', []);

app.service('TestResetService', function ($http, $location) {

    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api";
    }
    else {
        this.AppUrl = "/api";
    }

    this.UpdateSaveRegion = function (data) {
        return $http.post(this.AppUrl + '/Region/UpdateRegion', data);
    };

    this.GetCurrentSessionIDsForReset = function () {

        return $http.get(this.AppUrl + '/TestReset/GetCurrentSessionIDsForReset');
    };

    this.ResetForWholeSession = function (pt) {

        return $http.post(this.AppUrl + '/TestReset/ResetForWholeSession', pt);
    };


});
app.controller('TestResetController', function ($scope, $http, $location, TestResetService, $rootScope) {

    $scope.init = function () {

        TestResetService.GetCurrentSessionIDsForReset().then(function success(data) {
            $scope.CurrentSessions = data.data;
            $scope.showRegionGrid = true;
            console.log("Get Region List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.ResetForSession = function (pt) {
        swal({
            title: 'Warning',
            text: "it will reset the test for all the candidates of Session ID - " + pt.SessionID + " for the day - " + pt.Day,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            //  cancelButtonColor: '#d33',
            confirmButtonText: 'OK'
        }).then((result) => {
            TestResetService.ResetForWholeSession(pt).then(function success(data) {
                if (data.data.indexOf('Success') != -1) {
                    $scope.init();
                    swal("", data.data, "success");
                }
                else { swal("", data.data, "error"); }
                console.log("Get Region List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        });
    };

    $scope.ResetForCandidate = function (pt) {

        swal("Sorry", "It's not available as of now.", "info");
        return false;
    };

    $scope.init();
});
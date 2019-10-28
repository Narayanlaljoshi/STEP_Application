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
        return $http.post(this.AppUrl + '/Region/UpdateRegion', data, {});
    };

    this.GetCurrentSessionIDsForReset = function () {

        return $http.get(this.AppUrl + '/TestReset/GetCurrentSessionIDsForReset');
    };

    this.ResetForWholeSession = function (pt) {

        return $http.post(this.AppUrl + '/TestReset/ResetForWholeSession',pt);
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
    };

    $scope.ResetForCanddaite = function () {

        if (!$scope.RegionDetailData.RegionCode) {
            swal('Please enter Region Name');

            return false;
        }


        if (!$scope.RegionDetailData.RegionName) {
            swal('Please enter Region Name');

            return false;
        }



        //swal({
        //    title: "Are you sure?", text: "You are about update Region?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        $scope.RegionDetailData.IsActive = true;
        //    $scope.RegionDetailData.ModifiedBy = $rootScope.session.data.User_Id;
        TestResetService.UpdateSaveRegion($scope.RegionDetailData).then(function success(retdata) {

            if (retdata.data.indexOf("Success") !== -1) {
                swal(retdata.data);
            }
            else {
                swal(retdata.data);
            }

            $scope.init();



        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    }
    
    $scope.init();
});
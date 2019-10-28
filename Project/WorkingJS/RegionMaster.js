var app = angular.module('RegionModule', []);

app.service('RegionService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api"

    }
    else {

        this.AppUrl = "/api"
    }
    this.AddSaveRegion = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Region/AddRegion', data, {});
    };


    this.UpdateSaveRegion = function (data) {
        return $http.post(this.AppUrl + '/Region/UpdateRegion', data, {});
    };

    this.GetRegionList = function () {

        return $http.get(this.AppUrl + '/Region/GetRegionList');
    };

    this.GetZoneList = function () {

        return $http.get(this.AppUrl + '/Zone/GetZoneList');
    };


});
app.controller('RegionController', function ($scope, $http, $location, RegionService, $rootScope) {

    //$scope.usedID = 0;



    $scope.RegionDetailData = {

        Region_Id: null,
        RegionCode: null,
        RegionName: null,

        Zone_Id: null,


        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: null,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null

    }


    $scope.RegionData = [];
    $scope.ProductCategory = '';



    $scope.init = function () {
        console.log('inside init');
        $scope.showRegionGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.RegionData = [];
        $scope.showUpdataForm = [];



        RegionService.GetRegionList().then(function success(data) {
            $scope.RegionData = data.data;
            console.log("Get Region List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
            });

        RegionService.GetZoneList().then(function success(data) {
            $scope.ZoneData = data.data;
            console.log("Get Zone List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

    $scope.AddRegion = function () {


        $scope.showRegionGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.RegionDetailData.RegionName = '';
        $scope.RegionDetailData.RegionCode = '';

    };


    $scope.AddSaveRegion = function () {
       
        if (!$scope.RegionDetailData.RegionName) {
            swal('Please enter Region Name');

            return false;
        }


        //if (!$scope.RegionDetailData.RegionCode) {
        //    swal('Please enter Region Code');
        //    return false;
        //}

        //swal({
        //    title: "Are you sure?", text: "You are about update Region ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        console.log("pppp:", $scope.RegionDetailData);

        RegionService.AddSaveRegion($scope.RegionDetailData).then(function success(retdata) {

            if (retdata.data.indexOf("Success") !== -1) {
                swal(retdata.data);
                $scope.init();
            }
            else {
                swal(retdata.data);
            }



        }, function error(data) {
            swal("Error in loading data from EDB");
        });
        //});
    }



    $scope.UpdateTransaction = function (pt) {

        // $scope.ProductCategory.ProductCategory = pt;

        $scope.RegionDetailData = pt;

        $scope.showRegionGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.RegionDetailData);

    };



    $scope.UpdateSaveRegion = function () {

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
        RegionService.UpdateSaveRegion($scope.RegionDetailData).then(function success(retdata) {

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



    $scope.DeleteTransaction = function (obj) {

        //swal({
        //    title: "Are you sure?", text: "You want to delete this Region?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //  function () {
        obj.IsActive = false,
            //  obj.ModifiedBy = $rootScope.session.data.User_Id,
            RegionService.UpdateSaveRegion(obj).then(function success(retdata) {

                if (retdata.data.indexOf("Success") !== -1) {
                    swal(retdata.data);
                    $scope.init();
                }
                else {
                    swal(retdata.data);
                }





            }, function error(data) {
                console.log("Error in loading data from EDB");
            })

        //});
    }

    $scope.init();
});
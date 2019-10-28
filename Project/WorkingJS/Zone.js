var app = angular.module('ZoneModule', []);

app.service('ZoneService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }
    this.AddSaveZone = function (data) {
 //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Zone/AddZone', data, {});
    };
    

    this.UpdateSaveZone = function (data) {
        return $http.post(this.AppUrl + '/Zone/UpdateZone', data, {});
    };

    this.GetZoneList = function () {     
    
        return $http.get(this.AppUrl + '/Zone/GetZoneList');       
    };

});
app.controller('ZoneController', function ($scope, $http, $location, ZoneService, ProgramTestCalenderService, $rootScope) {

    //$scope.usedID = 0;

   

    $scope.ZoneDetailData = {

        Zone_Id: null,
        ZoneCode: null,
        ZoneName: null,
       // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: null,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null

    }
   

    $scope.ZoneData = [];
    $scope.ProductCategory = '';



    $scope.init = function () {
        console.log('inside init');
        $scope.showZoneGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.ZoneData = [];
        $scope.showUpdataForm = [];

        console.log(ProgramTestCalenderService.id);

        ZoneService.GetZoneList().then(function success(data) {
            $scope.ZoneData = data.data;
            console.log("Get Zone List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.AddZone = function () {


        $scope.showZoneGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.ZoneDetailData.ZoneName = '';
        $scope.ZoneDetailData.ZoneCode = '';

    };


    $scope.AddSaveZone = function () {
        console.log("fdfdfdf");
       
        if (!$scope.ZoneDetailData.ZoneName) {
            swal('Please enter Zone Name');

            return false;
        }


        //if (!$scope.ZoneDetailData.ZoneCode) {
        //    swal('Please enter Zone Code');
        //    return false;
        //}

        //swal({
        //    title: "Are you sure?", text: "You are about update Zone ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

                console.log("pppp:",$scope.ZoneDetailData);

                ZoneService.AddSaveZone($scope.ZoneDetailData).then(function success(retdata) {

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

        $scope.ZoneDetailData = pt;

        $scope.showZoneGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.ZoneDetailData);

    };



    $scope.UpdateSaveZone = function () {

        if (!$scope.ZoneDetailData.ZoneCode) {
            swal('Please enter Zone Name');

            return false;
        }


        if (!$scope.ZoneDetailData.ZoneName) {
            swal('Please enter Zone Name');

            return false;
        }

       

        //swal({
        //    title: "Are you sure?", text: "You are about update Zone?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

             $scope.ZoneDetailData.IsActive = true;
            //    $scope.ZoneDetailData.ModifiedBy = $rootScope.session.data.User_Id;
                ZoneService.UpdateSaveZone($scope.ZoneDetailData).then(function success(retdata) {

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
        //    title: "Are you sure?", text: "You want to delete this Zone?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
          //  function () {
                obj.IsActive = false,
                  //  obj.ModifiedBy = $rootScope.session.data.User_Id,
                    ZoneService.UpdateSaveZone(obj).then(function success(retdata) {

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
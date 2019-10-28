var app = angular.module('DealerOutletModule', []);

app.service('DealerOutletService', function ($http) {
    this.AddSaveDealerOutlet = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/DealerOutlet/AddDealerOutlet', data, {});
    };


    this.UpdateSaveDealerOutlet = function (data) {
        return $http.post(this.AppUrl + '/DealerOutlet/UpdateDealerOutlet', data, {});
    };

    this.GetDealerOutletList = function () {

        return $http.get(this.AppUrl + '/DealerOutlet/GetDealerOutletList');
    };

    this.GetRegionList = function () {

        return $http.get(this.AppUrl + '/Region/GetRegionList');
    };

    this.GetDealerGroupList = function () {

        return $http.get(this.AppUrl + '/DealerGroup/GetDealerGroupList');
    };
    //this.GetCityList = function () {

    //    return $http.get(this.AppUrl + '/City/GetCityList');
    //};
    this.GetCityList = function (id) {

        console.log(id);

        return $http.get(this.AppUrl + '/DealerOutlet/GetCity?id=' + id);
    };

    this.GetChannelList = function () {

        return $http.get(this.AppUrl + '/Channel/GetChannelList');
    };


});
app.controller('DealerOutletController', function ($scope, $http, $location, DealerOutletService, $rootScope) {

    //$scope.usedID = 0;



    $scope.DealerOutletDetailData = {

        DealerOutlet_Id : null,
        DealerCode : null,
        OutletCode : null,
        OutletName : null,
        OutletAddress : null,
        ChannelName : null,
        Channel_Id : null,
        City_Id : null,
        CityName : null,
        DealerGroup_Id : null,
        DealerGroupName : null,
        DealerGroupCode : null,
        Region_Id : null,
        RegionName : null,
        RegionCode : null,

     
        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: null,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null

    }


    $scope.DealerOutletData = [];
    



    $scope.init = function () {
        console.log('inside init');
        $scope.showDealerOutletGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.DealerOutletData = [];
        $scope.showUpdataForm = [];



        DealerOutletService.GetDealerOutletList().then(function success(data) {
            $scope.DealerOutletData = data.data;
            console.log("Get DealerOutlet List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

        DealerOutletService.GetRegionList().then(function success(data) {
            $scope.RegionData = data.data;
            console.log("Get Region Data List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
            });

        DealerOutletService.GetDealerGroupList().then(function success(data) {
            $scope.DealerGroupData = data.data;
            console.log("Get DealerGroup List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });


        DealerOutletService.GetChannelList().then(function success(data) {
            $scope.ChannelData = data.data;
            console.log("Get Channel List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };



    $scope.GetCity = function (dt)
    {
        console.log(dt);

        DealerOutletService.GetCityList(dt).then(function success(data) {
            $scope.CityData = data.data;
            console.log("Get City List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };





    $scope.AddDealerOutlet = function () {


        $scope.showDealerOutletGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.DealerOutletDetailData.DealerOutletName = '';
        $scope.DealerOutletDetailData.DealerOutletCode = '';

    };


    $scope.AddSaveDealerOutlet = function () {



        //if (!$scope.DealerOutletDetailData.DealerOutletName) {
        //    swal('Please enter DealerOutlet Name');

        //    return false;
        //}


        //if (!$scope.DealerOutletDetailData.DealerOutletCode) {
        //    swal('Please enter DealerOutlet Code');
        //    return false;
        //}

        //swal({
        //    title: "Are you sure?", text: "You are about update DealerOutlet ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        console.log("pppp:", $scope.DealerOutletDetailData);

        DealerOutletService.AddSaveDealerOutlet($scope.DealerOutletDetailData).then(function success(retdata) {

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

        $scope.DealerOutletDetailData = pt;

        $scope.showDealerOutletGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.DealerOutletDetailData);

    };



    $scope.UpdateSaveDealerOutlet = function () {



        if (!$scope.DealerOutletDetailData.DealerOutletName) {
            swal('Please enter DealerOutlet Name');

            return false;
        }


        if (!$scope.DealerOutletDetailData.Region_Id) {
            swal('Please enter Region Name');

            return false;
        }


        //swal({
        //    title: "Are you sure?", text: "You are about update DealerOutlet?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        $scope.DealerOutletDetailData.IsActive = true;
        //    $scope.DealerOutletDetailData.ModifiedBy = $rootScope.session.data.User_Id;
        DealerOutletService.UpdateSaveDealerOutlet($scope.DealerOutletDetailData).then(function success(retdata) {

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
        //    title: "Are you sure?", text: "You want to delete this DealerOutlet?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //  function () {
        obj.IsActive = false,
            //  obj.ModifiedBy = $rootScope.session.data.User_Id,
            DealerOutletService.UpdateSaveDealerOutlet(obj).then(function success(retdata) {

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



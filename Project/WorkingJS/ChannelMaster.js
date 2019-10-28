var app = angular.module('ChannelModule', []);

app.service('ChannelService', function ($http) {
    this.AddSaveChannel = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Channel/AddChannel', data, {});
    };


    this.UpdateSaveChannel = function (data) {
        return $http.post(this.AppUrl + '/Channel/UpdateChannel', data, {});
    };

    this.GetChannelList = function () {

        return $http.get(this.AppUrl + '/Channel/GetChannelList');
    };

});
app.controller('ChannelController', function ($scope, $http, $location, ChannelService, $rootScope) {

    //$scope.usedID = 0;



    $scope.ChannelDetailData = {

        Channel_Id: null,
        ChannelCode: null,
        ChannelName: null,
        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null

    }


    $scope.ChannelData = [];
    $scope.ProductCategory = '';



    $scope.init = function () {
        console.log('inside init');
        $scope.showChannelGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.ChannelData = [];
        $scope.showUpdataForm = [];



        ChannelService.GetChannelList().then(function success(data) {
            $scope.ChannelData = data.data;
            console.log("Get Channel List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.AddChannel = function () {


        $scope.showChannelGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.ChannelDetailData.ChannelName = '';
        $scope.ChannelDetailData.ChannelCode = '';

    };


    $scope.AddSaveChannel = function () {
        console.log("fdfdfdf");

        if (!$scope.ChannelDetailData.ChannelName) {
            swal('Please enter Channel Name');

            return false;
        }


        //if (!$scope.ChannelDetailData.ChannelCode) {
        //    swal('Please enter Channel Code');
        //    return false;
        //}

        //swal({
        //    title: "Are you sure?", text: "You are about update Channel ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        console.log("pppp:", $scope.ChannelDetailData);

        ChannelService.AddSaveChannel($scope.ChannelDetailData).then(function success(retdata) {

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

        $scope.ChannelDetailData = pt;

        $scope.showChannelGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.ChannelDetailData);

    };



    $scope.UpdateSaveChannel = function () {

        if (!$scope.ChannelDetailData.ChannelCode) {
            swal('Please enter Channel Name');

            return false;
        }


        if (!$scope.ChannelDetailData.ChannelName) {
            swal('Please enter Channel Name');

            return false;
        }



        //swal({
        //    title: "Are you sure?", text: "You are about update Channel?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        $scope.ChannelDetailData.IsActive = true;
        //    $scope.ChannelDetailData.ModifiedBy = $rootScope.session.data.User_Id;
        ChannelService.UpdateSaveChannel($scope.ChannelDetailData).then(function success(retdata) {

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
        //    title: "Are you sure?", text: "You want to delete this Channel?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //  function () {
        obj.IsActive = false,
            //  obj.ModifiedBy = $rootScope.session.data.User_Id,
            ChannelService.UpdateSaveChannel(obj).then(function success(retdata) {

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
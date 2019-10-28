var app = angular.module('DealerGroupModule', []);

app.service('DealerGroupService', function ($http) {
    this.AddSaveDealerGroup = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/DealerGroup/AddDealerGroup', data, {});
    };


    this.UpdateSaveDealerGroup = function (data) {
        return $http.post(this.AppUrl + '/DealerGroup/UpdateDealerGroup', data, {});
    };

    this.GetDealerGroupList = function () {

        return $http.get(this.AppUrl + '/DealerGroup/GetDealerGroupList');
    };

});
app.controller('DealerGroupController', function ($scope, $http, $location, DealerGroupService, $rootScope) {

    //$scope.usedID = 0;



    $scope.DealerGroupDetailData = {

        DealerGroup_Id: null,
        DealerGroupCode: null,
        DealerGroupName: null,
        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: null,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null

    }


    $scope.DealerGroupData = [];
   



    $scope.init = function () {
        console.log('inside init');
        $scope.showDealerGroupGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.DealerGroupData = [];
        $scope.showUpdataForm = [];



        DealerGroupService.GetDealerGroupList().then(function success(data) {
            $scope.DealerGroupData = data.data;
            console.log("Get DealerGroup List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.AddDealerGroup = function () {


        $scope.showDealerGroupGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.DealerGroupDetailData.DealerGroupName = '';
        $scope.DealerGroupDetailData.DealerGroupCode = '';

    };


    $scope.AddSaveDealerGroup = function () {
        console.log("fdfdfdf");

        if (!$scope.DealerGroupDetailData.DealerGroupName) {
            swal('Please enter DealerGroup Name');

            return false;
        }

        if (!$scope.DealerGroupDetailData.DealerGroupCode) {
            swal('Please enter Dealer Group Code');

            return false;
        }

        //if (!$scope.DealerGroupDetailData.DealerGroupCode) {
        //    swal('Please enter DealerGroup Code');
        //    return false;
        //}

        //swal({
        //    title: "Are you sure?", text: "You are about update DealerGroup ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        console.log("pppp:", $scope.DealerGroupDetailData);

        DealerGroupService.AddSaveDealerGroup($scope.DealerGroupDetailData).then(function success(retdata) {

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

        $scope.DealerGroupDetailData = pt;

        $scope.showDealerGroupGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.DealerGroupDetailData);

    };



    $scope.UpdateSaveDealerGroup = function () {

        if (!$scope.DealerGroupDetailData.DealerGroupCode) {
            swal('Please enter DealerGroup Name');

            return false;
        }


        if (!$scope.DealerGroupDetailData.DealerGroupName) {
            swal('Please enter DealerGroup Name');

            return false;
        }



        //swal({
        //    title: "Are you sure?", text: "You are about update DealerGroup?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        $scope.DealerGroupDetailData.IsActive = true;
        //    $scope.DealerGroupDetailData.ModifiedBy = $rootScope.session.data.User_Id;
        DealerGroupService.UpdateSaveDealerGroup($scope.DealerGroupDetailData).then(function success(retdata) {

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
        //    title: "Are you sure?", text: "You want to delete this DealerGroup?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //  function () {
        obj.IsActive = false,
            //  obj.ModifiedBy = $rootScope.session.data.User_Id,
            DealerGroupService.UpdateSaveDealerGroup(obj).then(function success(retdata) {

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
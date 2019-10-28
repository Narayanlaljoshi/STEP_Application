var app = angular.module('RtcMasterModule', []);

app.service('RtcMasterService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }


    this.GetAgencyList = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/RtcMaster/GetAgencyList', {});
    };


    this.UpdateSaveRegion = function (data) {
        return $http.post(this.AppUrl + '/RtcMaster/AddUpdateAgency', data, {});
    };

    this.GetFacultyDetailsList = function (Agency_Id) {

        return $http.get(this.AppUrl + '/RtcMaster/GetFacultyDetailsList?Agency_Id=' + Agency_Id);
    };

    this.UpdateFacultyDetails = function (Obj) {

        return $http.post(this.AppUrl + '/RtcMaster/SubmitFacultyList', Obj);
    };
    this.DeleteFaculty = function (Obj) {

        return $http.post(this.AppUrl + '/RtcMaster/DeleteFaculty', Obj);
    };


    this.AgencyList = [];
});

app.factory('InitFactory', function (RtcMasterService) {

    return {
        init: function () {


            RtcMasterService.GetAgencyList().then(function success(data) {
                RtcMasterService.AgencyList = data.data;
                console.log("Get Region List", data);


            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }



    };


});
app.controller('RtcMasterController', function ($scope, $http, $location, RtcMasterService, $uibModal, $rootScope, InitFactory) {

    //$scope.usedID = 0;
    $scope.DetailData = {
        AgencyName: null,
        //AgencyName: null,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: $rootScope.session.User_Id,
        ModifiedDate: null
    };


    $scope.RegionDetailData = {

        Region_Id: null,
        RegionCode: null,
        RegionName: null,

        Zone_Id: null,


        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: $rootScope.session.User_Id,
        ModifiedDate: null

    };


    $scope.RegionData = [];
    $scope.ProductCategory = '';


    $scope.InitializeArrayData = function () {

        InitFactory.init();
        $scope.init();
    };

    //$scope.$on('$locationChangeStart', function (event, next, current) {
    //    // Here you can take the control and call your own functions:
    //    console.log(current);
    //    console.log(next);
    //    alert('Sorry ! Back Button is disabled');
    //    // Prevent the browser default action (Going back):
    //    event.preventDefault();
    //});


    //InitFactory.init();

    $scope.init = function () {
        console.log('inside init');
        $scope.showRegionGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.RegionData = [];
        $scope.showUpdataForm = [];
        
        RtcMasterService.GetAgencyList().then(function success(data) {
            $scope.AgencyList = data.data;
            console.log("Get Region List", data);
        }, function error(data) {
            console.log("Error in loading data from EDB","error");
        });

       
        //$scope.AgencyList = RtcMasterService.AgencyList;
        //console.log($scope.AgencyList);
    };

    $scope.AddRegion = function () {

        $scope.showRegionGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        //$scope.ProductCategory = [];
        $scope.DetailData = {
            AgencyName: null,
            //AgencyName: null,
            IsActive: true,
            CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
            CreationDate: null,
            ModifiedBy: $rootScope.session.User_Id,
            ModifiedDate: null
        };

    };

    $scope.UpdateSaveRegion = function () {
        //if (!$scope.RegionDetailData.AgencyCode) {
        //    swal('Error', 'Please enter Agency Code', 'error');
        //    return false;
        //}

        //if (!$scope.RegionDetailData.AgencyName) {
        //    swal('Please enter Agency Name');

        //    return false;
        //}

        console.log("pppp:", $scope.RegionDetailData);

        RtcMasterService.UpdateSaveRegion($scope.RegionDetailData).then(function success(retdata) {

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
        $scope.DetailData = pt;

        $scope.showRegionGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;

        console.log("UpdateTransaction", $scope.DetailData);
    };

    $scope.UpdateSaveRegion = function () {
        if (!$scope.DetailData.AgencyCode) {
            swal('Error','Please enter Agency Code','error');
            return false;
        }

        if (!$scope.DetailData.AgencyName) {
            swal('Error', 'Please enter Agency Name','error');

            return false;
        }

        if (!$scope.DetailData.RTMName) {
            swal('Error', 'Please enter RTM Name', 'error');

            return false;
        }
        if (!$scope.DetailData.RTMCode) {
            swal('Error', 'Please enter RTM Code', 'error');

            return false;
        }
        if (!$scope.DetailData.Mobile) {
            swal('Error', 'Please enter Mobile number', 'error');

            return false;
        }


        console.log($scope.DetailData.AgencyCode);
        console.log($scope.DetailData.AgencyName);
        $scope.DetailData.IsActive = true;

        RtcMasterService.UpdateSaveRegion($scope.DetailData).then(function success(retdata) {

            if (retdata.data.indexOf("Success") !== -1) {
               // swal(retdata.data);
                swal("","RTC Updated Successfully", "success")
            }
            else {
              //  swal(retdata.data);
                swal(retdata.data, "success");
            }

            $scope.init();



        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    }

    $scope.DeleteTransaction = function (obj) {

        swal({
            title: 'Are you sure?',
            text: "You are going to delete!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
           
                obj.IsActive = false,
                    //  obj.ModifiedBy = $rootScope.session.data.User_Id,
                    RtcMasterService.UpdateSaveRegion(obj).then(function success(retdata) {

                        if (retdata.data.indexOf("Success") !== -1) {
                            swal("",retdata.data,"success");
                            $scope.init();
                        }
                        else {
                            swal(retdata.data);
                        }
                    }, function error(data) {
                        console.log("Error in loading data from EDB");
                    })

                //});
          

        });
    };

    $scope.AddFaculties = function (pt) {
        console.log("Popup working");
        RtcMasterService.AgencyDetails = pt;

        $uibModal.open({
            templateUrl: 'Faculty.html',
            controller: 'FacultyController',
            windowClass: 'app-modal-window',
            backdrop: 'static',
            size: 'lg'
        }).result.then(
            function () {

            },
            function () {

            }
            );

    }

    $scope.init();
});

app.controller('FacultyController', function ($scope, $rootScope, RtcMasterService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.SaveValue = function () {
        console.log("hi");
        obj = {
            RevisitDate: $scope.RevisitDate,

            Remarks: $scope.Remarks,
        };
        console.log(obj);
        ApprovalMatrixService.SaveEmployee(obj)
            .then(function (success) {
                console.log(success);

            },
            function (error) {
                console.log(error);

            });
    };
    $scope.AgencyDetails = RtcMasterService.AgencyDetails;

    $scope.init = function () {
        RtcMasterService.GetFacultyDetailsList($scope.AgencyDetails.Agency_Id).then(function success(retdata) {

            $scope.FacultyDetailsList = retdata.data;
            console.log($scope.FacultyDetailsList);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };
    $scope.AddRowToFacultyList = function (pt) {
        $scope.FacultyDetailsList.push({
            Agency_Id: $scope.AgencyDetails.Agency_Id,
            CreatedBy: $rootScope.session.User_Id,
            FacultyCode:null,
            FacultyName: null,
            Email :null,
            Faculty_Id:null,
            IsActive: true,
            Mobile: null,
            ModifiedBy: $rootScope.session.User_Id,
        });
    }
    $scope.UpdateFacultyList = function () {
        RtcMasterService.UpdateFacultyList($scope.AgencyDetails.Agency_Id).then(function success(retdata) {

            $scope.FacultyDetailsList = retdata.data;
            console.log($scope.FacultyDetailsList);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    }

    $scope.SpliceRowToFacultyList = function (pt,index) {
        if (pt.Faculty_Id && $scope.FacultyDetailsList.length != 1) {
            RtcMasterService.DeleteFaculty(pt).then(function success(retdata) {
                swal("", retdata.data, "success");
                $scope.FacultyDetailsList.splice(index);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

        }
        else {
            if ($scope.FacultyDetailsList.length == 1) {
                swal("", "Atleast one faculty should be there", "error");
            }
            else {
                $scope.FacultyDetailsList.splice(index);
            }
        }

    }
    $scope.SubmitFacultyList = function () {
        var Count = parseInt(0);
        $scope.KeepGoing = true;
        angular.forEach($scope.FacultyDetailsList, function (value1, key1) {
            console.log(value1.FacultyCode, value1.FacultyName, value1.Mobile, value1.Email);
            console.log("!(value1.Email)", !(value1.Email));
            Count = parseInt(0);
            angular.forEach($scope.FacultyDetailsList, function (value2, key2) {
                if (!value1.FacultyCode || !value1.FacultyName || !value1.Mobile || !value1.Email) {
                    $scope.KeepGoing = false;
                }
                if (value1.FacultyCode == value2.FacultyCode) {
                    Count = parseInt(Count) + 1;
                }               
                if (parseInt(Count) > 1) {
                    $scope.KeepGoing = false;                   
                }
            });
        });
        if ($scope.KeepGoing == true) {
            RtcMasterService.UpdateFacultyDetails($scope.FacultyDetailsList).then(function success(retdata) {
                //$scope.FacultyDetailsList = retdata.data;
                swal("Success", retdata.data, "success");
                $scope.cancel();
                InitFactory.init();
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            swal("Error", "Duplicate entries and Empty fields are not allowed", "error");
            return false;

        }
    }
    $scope.init();


});
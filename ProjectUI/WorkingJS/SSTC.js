var app = angular.module('SSTCModule', []);

app.service('SSTCService', function ($http, $location) {

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

    this.GetFacultyDetailsList = function (Agency_Id, UserName) {

        return $http.get(this.AppUrl + '/RtcMaster/GetFacultyDetailsList_External?Agency_Id=' + Agency_Id + '&UserName=' + UserName);
    };

    this.UpdateFacultyDetails = function (Obj) {

        return $http.post(this.AppUrl + '/RtcMaster/SubmitFacultyList_External', Obj);
    };
    this.DeleteFaculty = function (Obj) {

        return $http.post(this.AppUrl + '/RtcMaster/DeleteFaculty', Obj);
    };


    this.AgencyList = [];
});

app.factory('InitFactory', function (SSTCService) {

    return {
        init: function () {


            SSTCService.GetAgencyList().then(function success(data) {
                SSTCService.AgencyList = data.data;
                console.log("Get Region List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }



    };


});
app.controller('SSTCController', function ($scope, $http, $location, SSTCService, $uibModal, $rootScope, InitFactory) {

    $scope.init = function () {
        SSTCService.GetFacultyDetailsList($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(retdata) {

            $scope.FacultyDetailsList = retdata.data;
            console.log($scope.FacultyDetailsList);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };
    $scope.AddRowToFacultyList = function (pt) {
        $scope.FacultyDetailsList.push({
            Agency_Id: $rootScope.session.Agency_Id,
            CreatedBy: $rootScope.session.User_Id,
            FacultyCode: null,
            FacultyName: null,
            Email: null,
            Faculty_Id: null,
            IsActive: true,
            Mobile: null,
            ModifiedBy: $rootScope.session.User_Id
        });
    };
    $scope.UpdateFacultyList = function () {
        SSTCService.UpdateFacultyList($rootScope.session.Agency_Id).then(function success(retdata) {

            $scope.FacultyDetailsList = retdata.data;
            console.log($scope.FacultyDetailsList);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.SpliceRowToFacultyList = function (pt, index) {
        if (pt.Faculty_Id && $scope.FacultyDetailsList.length != 1) {
            SSTCService.DeleteFaculty(pt).then(function success(retdata) {
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

    };
    $scope.SubmitFacultyList = function () {
        var Count = parseInt(0);
        $scope.KeepGoing = true;
        angular.forEach($scope.FacultyDetailsList, function (value1, key1) {
            value1.Role_Id = $rootScope.session.Role_Id;
            value1.ParentFaculty_Id = $rootScope.session.User_Id;
            value1.ParentFaculty_Code = $rootScope.session.UserName;
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
            SSTCService.UpdateFacultyDetails($scope.FacultyDetailsList).then(function success(retdata) {
                //$scope.FacultyDetailsList = retdata.data;
                if (retdata.data.indexOf('Success') != -1) {
                    swal("", retdata.data, "success");
                    $scope.init();
                }
                else { swal("Error", retdata.data, "error");}
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            swal("Error", "Duplicate entries and Empty fields are not allowed", "error");
            return false;

        }
    };
    $scope.init();
});
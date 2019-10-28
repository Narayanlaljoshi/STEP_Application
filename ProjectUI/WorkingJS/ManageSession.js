var app = angular.module('ManageSessionModule', []);

app.service('ManageSessionService', function ($http, $location) {

    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetFacultyList = function (Agency_Id) {       
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    }; 
    this.GetFacultyList_External = function (Agency_Id, UserName) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList_External?Agency_Id=' + Agency_Id + '&UserName=' + UserName);
    }; 
    this.GetSessionList = function (Agency_Id) {        
        return $http.get(this.AppUrl + '/ManageSession/GetSessionList?Agency_Id=' + Agency_Id);
    }; 
    this.UpdateFaculty = function (Obj) {
        return $http.post(this.AppUrl + '/ManageSession/UpdateFaculty', Obj);
    }; 
    this.GetAgencyList = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/RtcMaster/GetAgencyList', {});
    };
    this.GetSessionIDListForAgency = function (Agency_Id) {

        return $http.get(this.AppUrl + '/RtcMaster/GetSessionIDListForAgency?Agency_Id=' + Agency_Id);
    };
});
app.controller('ManageSessionController', function ($scope, $http, $location, CityService, $rootScope, ManageSessionService, helperService) {
    console.log($rootScope.session.Agency_Id);
    $scope.init = function () {  

        if ($rootScope.session.Role_Id==1){

            ManageSessionService.GetAgencyList($rootScope.session.Agency_Id).then(function success(success) {

                $scope.RTMList = success.data;

            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });

        }
        else if ($rootScope.session.Role_Id == 5) {
            ManageSessionService.GetFacultyList_External($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(success) {

                $scope.FacultyList = success.data;

                return ManageSessionService.GetSessionList($rootScope.session.Agency_Id);

            }).then(function success(success) {

                $scope.SessionList = success.data;

            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });
        }
        else {
            ManageSessionService.GetFacultyList($rootScope.session.Agency_Id).then(function success(success) {

                $scope.FacultyList = success.data;

                return ManageSessionService.GetSessionList($rootScope.session.Agency_Id);

            }).then(function success(success) {

                $scope.SessionList = success.data;

            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });
        }
    };
    $scope.GetFacultyList = function (Agency_Id) {

        ManageSessionService.GetFacultyList(Agency_Id).then(function success(success) {

            $scope.FacultyList = success.data;

            return ManageSessionService.GetSessionList(Agency_Id);

        }).then(function success(success) {

            $scope.SessionList = success.data;

        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };
    $scope.UpdateFaculty = function (pt) {
        console.log(pt);
        pt.User_Id = $rootScope.session.User_Id;
        ManageSessionService.UpdateFaculty(pt).then(function success(success) {

            if (success.data.indexOf("Success") !== -1) {
                swal("", success.data, "success");
                $scope.init();
            }
            else {
                swal("", success.data,"error");
            }
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.init();

});
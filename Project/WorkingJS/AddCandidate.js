var app = angular.module('FacultyAgencyLevelModule', []);

app.service('FacultyAgencyLevelService', function ($http, $location) {

    this.SelectedSessionIds = "";
    this.Session = "";
    this.AppUrl = "";
    this.MSPin = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }
    this.GetSessionIdList = function (Agency_Id, FacultyCode) {
        return $http.get(this.AppUrl + '/FacultyAgencyLevel/GetSessionIdList?Agency_Id=' + Agency_Id + '&FacultyCode=' + FacultyCode);
    };

    this.GetFacultyProgramdetails = function (FacultyCode) {
        return $http.get(this.AppUrl + '/FacultyAgencyLevel/GetFaciltyProgramdetails?FacultyCode=' + FacultyCode);
    };
    //this.SaveInitiatedTestDetails = function (Obj) {
    //    return $http.post(this.AppUrl + '/Test/SaveInitiatedTestDetails', Obj);
    //};
    this.SaveTestInitiationDetails = function (Obj) {
        return $http.post(this.AppUrl + '/Test/SaveTestInitiationDetails', Obj);
    };
    this.GetStudentPostTestScores = function (MSpin) {
        return $http.get(this.AppUrl + '/Test/GetStudentPostTestScores/?MSpin=' + MSpin);
    };
    this.GetSessionWiseStudentsList = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/GetSessionWiseStudentsList', Obj);
    };
});

//app.factory('InitFactory', function (FacultyAgencyLevelService) {

//    return {
//        init: function () {
//            RtcMasterService.GetAgencyList().then(function success(data) {
//                RtcMasterService.AgencyList = data.data;
//                console.log("Get Region List", data);
//            }, function error(data) {
//                console.log("Error in loading data from EDB");
//            });
//        }
//    };
//});

app.controller('FacultyAgencyLevelController', function ($scope, $http, $location, FacultyAgencyLevelService, $uibModal, $rootScope, InitFactory, $filter) {

    $scope.SelectedSessionIds = [];

    $scope.TestInitiateDay = null;
    $scope.init = function () {
        $scope.TestInitiateDay = null;
        console.log($scope.SelectedSessionIds.length);
        $scope.EligiblForTestInitiate = true;
        console.log("FacultyProgramdetails");
        FacultyAgencyLevelService.GetSessionIdList($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(success) {
            $scope.SessionIdList = success.data;
            console.log("FacultyProgramdetails", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.GetSelectedSessionIds = function (pt) {

        $scope.IsValidSessionId = true;
        if (pt.IsChecked == true) {
            if ($scope.SelectedSessionIds.length == 0) {
                pt.CreatedBy = $rootScope.session.User_Id;
                $scope.SelectedSessionIds.push(pt);
                $scope.TestInitiateDay = pt.day;
            }
            else {
                angular.forEach($scope.SelectedSessionIds, function (value, key) {
                    console.log(pt.StartDate == value.StartDate, pt.StartDate, value.StartDate, pt.EndDate == value.EndDate, pt.EndDate, value.EndDate);
                    if (pt.ProgramId == value.ProgramId && pt.StartDate == value.StartDate && pt.EndDate == value.EndDate && pt.day == value.day) {
                        pt.CreatedBy = $rootScope.session.User_Id;
                        $scope.SelectedSessionIds.push(pt);
                    }
                    else {
                        $scope.IsValidSessionId = false;
                        pt.IsChecked = false;
                    }
                });
                if ($scope.IsValidSessionId == false) {
                    swal("Invalid Selection.", "Selected Session Ids do not meet desired conditions either of : Start Date, End Date ,Test Day , Program Code", "warning");
                }
            }
        }
        else {
            $scope.SelectedSessionIds.splice($scope.SelectedSessionIds.findIndex(x => x.SessionID == pt.SessionID), 1);
            if ($scope.SelectedSessionIds.length == 0) {

                $scope.TestInitiateDay = null;
            }
        }
    }


    $scope.InitiatTest = function () {

        swal({
            title: 'Are you sure?',
            text: "You are going to initiate test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
            FacultyAgencyLevelService.SaveTestInitiationDetails($scope.SelectedSessionIds).then(function success(success) {
                if (success.data.indexOf("Success") != -1) {
                    swal("", success.data, "success");
                    $scope.init();
                }
                else
                    swal("", success.data, "error");
                console.log("FacultyProgramdetails", success.data);
            }, function error(Error) {

                console.log("Error in loading data from EDB");
            });
        });
    };


  

    $scope.ViewResult = function (Session) {
        FacultyAgencyLevelService.Session = Session;
        $uibModal.open({
            templateUrl: 'partial/ShowStudentMarks.html',
            controller: 'ShowStudentMarksController',
            windowClass: 'app-modal-window',
            backdrop: 'static',
            size: 'lg'
        }).result.then(
            function () {

            },
            function () {

            }
            );
    };

    

    $scope.init();
});

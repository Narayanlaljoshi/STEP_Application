var app = angular.module('SSTCCourseClosureModule', []);

app.service('SSTCCourseClosureService', function ($http, $location) {

    this.AppUrl = "";

    this.SessionIdList = [];

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }

    this.GetCandidatesListForSSTC = function (SessionId, Day) {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/SSTC/GetCandidatesListForSSTC?SessionId=' + SessionId + '&Day=' + Day);
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
    this.CloseSSTCCourse = function (Obj) {
        return $http.post(this.AppUrl + '/SSTC/CloseSSTCCourse', Obj);
    };
    this.GetSessionIDListForFaculty = function (Agency_Id, FacultyCode) {
        return $http.get(this.AppUrl + '/RtcMaster/GetSessionIDListForFaculty?Agency_Id=' + Agency_Id + '&FacultyCode=' + FacultyCode);
    };
    this.GetDaySequenceSSTC = function (SessionId) {
        return $http.get(this.AppUrl + '/SSTC/GetDaySequenceSSTC?SessionId=' + SessionId);
    };
    this.AgencyList = [];
});

app.factory('InitFactory', function (SSTCCourseClosureService) {
    return {
        init: function () {
            SSTCCourseClosureService.GetAgencyList().then(function success(data) {
                SSTCCourseClosureService.AgencyList = data.data;
                console.log("Get Region List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});
app.controller('SSTCCourseClosureController', function ($scope, $http, $location, FacultyAgencyLevelService, SSTCCourseClosureService, $uibModal, $rootScope, InitFactory) {
    $scope.ShowTable = false;
    $scope.init = function () {
        SSTCCourseClosureService.GetSessionIDListForFaculty($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(success) {

            $scope.SessionList = success.data;
            angular.forEach($scope.SessionList, function (value, key) {
                console.log(new Date(value.EndDate) < new Date());
                if (new Date(value.EndDate )< new Date())
                {
                    alert("There are course closure pending");
                }
            });
            SSTCCourseClosureService.SessionIdList = success.data;
        }, function error(error) {
            console.log("Error in loading data from EDB");
        });
    };
    
    $scope.CloseSSTCCourse = function (pt) {
        pt.CreatedBy = $rootScope.session.User_Id;
        swal({
            title: 'Are you sure?',
            text: "You are going to close the course!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Close it!'
        }).then((result) => {
            SSTCCourseClosureService.CloseSSTCCourse(pt).then(function success(success) {
                if (success.data.indexOf('Success!=-1')) {
                    swal("", success.data, "success");
                    $scope.init();
                }
                else { swal("", success.data, "error"); }
            }, function error(error) {
                console.log("Error in loading data from EDB");
            });
        });
    };
    $scope.AddCandidate = function () {
        //FacultyAgencyLevelService.Session = ;
        $uibModal.open({
            templateUrl: 'partial/AddCandidate.html',
            controller: 'AddCandidateSSTCController',
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

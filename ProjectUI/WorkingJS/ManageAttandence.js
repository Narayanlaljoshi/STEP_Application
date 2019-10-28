var app = angular.module('ManageAttandenceModule', []);

app.service('ManageAttandenceService', function ($http, $location) {

    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageAttandence/GetFacultyList?Agency_Id=' + Agency_Id);
    };
    this.GetSessionList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageAttandence/GetSessionList?Agency_Id=' + Agency_Id);
    };
    this.GetCandidateList = function (SessionId) {
        return $http.get(this.AppUrl + '/RtcMaster/GetCandidateList?SessionId=' + SessionId);
    };
    this.GetCandidateListForSSTC = function (SessionId, day) {
        return $http.get(this.AppUrl + '/RtcMaster/GetCandidateListForSSTC?SessionId=' + SessionId+'&Day='+day);
    };
    this.GetAgencyList = function (data) {
        return $http.get(this.AppUrl + '/RtcMaster/GetAgencyList', {});
    };
    this.GetSessionIDListForAgency = function (Agency_Id) {
        return $http.get(this.AppUrl + '/RtcMaster/GetSessionIDListForAgency?Agency_Id=' + Agency_Id);
    };
    this.GetSessionIDListForFaculty = function (Agency_Id, FacultyCode) {
        return $http.get(this.AppUrl + '/RtcMaster/GetSessionIDListForFaculty?Agency_Id=' + Agency_Id + '&FacultyCode=' + FacultyCode);
    };
    this.UpdateAttendance = function (Obj) {
        return $http.post(this.AppUrl + '/RtcMaster/UpdateAttendance', Obj);
    };
    this.Update_InsertIntoTblAttendance_SSTC = function (Obj) {
        return $http.post(this.AppUrl + '/SSTC/Update_InsertIntoTblAttendance_SSTC', Obj);
    };
});
app.controller('ManageAttandenceController', function ($scope, $http, $location, $rootScope, ManageAttandenceService, SSTCMarksService) {
    console.log($rootScope.session.Agency_Id);
    $scope.CandidateList = [];
    $scope.ShowTable = false;
    $scope.SessionIdd = '';
    $scope.init = function () {

        if ($rootScope.session.Role_Id == 1) {

            ManageAttandenceService.GetAgencyList($rootScope.session.Agency_Id).then(function success(success) {

                $scope.RTMList = success.data;

            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });

        }
        else if ($rootScope.session.Role_Id == 6)
        {
            ManageAttandenceService.GetSessionIDListForFaculty($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(success) {

                $scope.SessionList = success.data;
                angular.forEach($scope.SessionList, function (value, key) {
                    console.log(new Date(value.EndDate) < new Date());
                    if (new Date(value.EndDate) < new Date()) {
                        alert("There are course closure pending");
                    }
                });
            }, function error(error) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            ManageAttandenceService.GetSessionIDListForAgency($rootScope.session.Agency_Id).then(function success(success) {

                $scope.SessionList = success.data;
            }, function error(error) {
                console.log("Error in loading data from EDB");
            });
        }
    };
    $scope.GetDaySequenceSSTC = function (SessionID) {
        SSTCMarksService.GetDaySequenceSSTC(SessionID).then(function success(success) {
            $scope.DaySequence = success.data;
        }, function error(error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetSessionList = function (AgenCy_Id) {
        //pt.User_Id = $rootScope.session.User_Id;
        ManageAttandenceService.GetSessionIDListForAgency(AgenCy_Id).then(function success(success) {

            $scope.SessionList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetCandidateList = function (SessionID) {
        $scope.SessionIdd = SessionID;
        //pt.User_Id = $rootScope.session.User_Id;
        $scope.CandidateList = [];
        ManageAttandenceService.GetCandidateList(SessionID).then(function success(success) {
            $scope.ShowTable = true;
            $scope.CandidateList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetCandidateListForSSTC = function (SessionID,day) {
        $scope.SessionIdd = SessionID;
        
        //pt.User_Id = $rootScope.session.User_Id;
        $scope.CandidateList = [];
        ManageAttandenceService.GetCandidateListForSSTC(SessionID,day).then(function success(success) {
            $scope.ShowTable = true;
            $scope.CandidateList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.UpdateAttendance = function () {
        //pt.User_Id = $rootScope.session.User_Id;
        ManageAttandenceService.UpdateAttendance($scope.CandidateList).then(function success(success) {

            if (success.data.indexOf('Success') != -1) {
                $scope.GetCandidateList($scope.SessionIdd);
                swal("", success.data, "success");
            }
            else {
                swal("", success.data, "error");
            }
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.Update_InsertIntoTblAttendance_SSTC = function () {
        //pt.User_Id = $rootScope.session.User_Id;
        var index = $scope.DaySequence.findIndex(record => record.SessionId === $scope.SessionID);
        var Obj = {
            CandidateList: $scope.CandidateList,
            Date: $scope.DaySequence[index].Date,
            Day: $scope.Day,
            SessionID: $scope.SessionID
        };
        ManageAttandenceService.Update_InsertIntoTblAttendance_SSTC(Obj).then(function success(success) {

            if (success.data.indexOf('Success') != -1) {
                $scope.GetCandidateListForSSTC($scope.SessionID,$scope.Day);
                swal("", success.data, "success");
            }
            else {
                swal("", success.data, "error");
            }
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.init();

});
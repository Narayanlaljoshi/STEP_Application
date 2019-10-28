var app = angular.module('SSTCMarksModule', []);

app.service('SSTCMarksService', function ($http, $location) {

    this.AppUrl = "";

    this.SessionIdList = [];

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1)
    {
        this.AppUrl = "/CST/api/";
    }
    else
    {
        this.AppUrl = "/api/";
    }

    this.GetCandidatesListForSSTC = function (SessionId,Day) {
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
    this.UpDateMarksForSSTC = function (Obj) {
        return $http.post(this.AppUrl + '/SSTC/UpDateMarksForSSTC', Obj);
    };
    this.GetSessionIDListForFaculty = function (Agency_Id, FacultyCode) {
        return $http.get(this.AppUrl + '/RtcMaster/GetSessionIDListForFaculty?Agency_Id=' + Agency_Id + '&FacultyCode=' + FacultyCode);
    };
    this.GetDaySequenceSSTC = function (SessionId) {
        return $http.get(this.AppUrl + '/SSTC/GetDaySequenceSSTC?SessionId=' + SessionId);
    };
    this.AgencyList = [];
});

app.factory('InitFactory', function (SSTCMarksService) {

    return {
        init: function () {


            SSTCMarksService.GetAgencyList().then(function success(data) {
                SSTCMarksService.AgencyList = data.data;
                console.log("Get Region List", data);


            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }



    };


});
app.controller('SSTCMarksController', function ($scope, $http, $location, FacultyAgencyLevelService, SSTCMarksService, $uibModal, $rootScope, InitFactory) {
    $scope.ShowTable = false;
    $scope.init = function () {
        SSTCMarksService.GetSessionIDListForFaculty($rootScope.session.Agency_Id, $rootScope.session.UserName).then(function success(success) {

            $scope.SessionList = success.data;
            angular.forEach($scope.SessionList, function (value, key) {
                console.log(new Date(value.EndDate) < new Date());
                if (new Date(value.EndDate) < new Date()) {
                    alert("There are course closure pending");
                }
            });
            SSTCMarksService.SessionIdList = success.data;
        }, function error(error) {
            console.log("Error in loading data from EDB");
            });
    };
    $scope.GetCandidatesListForSSTC = function () {
        SSTCMarksService.GetCandidatesListForSSTC($scope.Filter.SessionId, $scope.Filter.Day).then(function success(success) {

            $scope.CandidateList = success.data;
            $scope.ShowTable = true;
        }, function error(error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetDaySequenceSSTC = function () {
        SSTCMarksService.GetDaySequenceSSTC($scope.Filter.SessionId).then(function success(success) {
            $scope.DaySequence = success.data;
        }, function error(error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.UpDateMarksForSSTC = function () {
        angular.forEach($scope.CandidateList, function (value, key) {
            value.CreatedBy = $rootScope.session.User_Id;
        });
        SSTCMarksService.UpDateMarksForSSTC($scope.CandidateList).then(function success(success) {
            if (success.data.indexOf('Success!=-1'))
            {
                swal("", success.data,"success");
            }
            else { swal("", success.data, "error");}
        }, function error(error) {
            console.log("Error in loading data from EDB");
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
app.controller('AddCandidateSSTCController', function ($scope, $rootScope, SSTCMarksService, FacultyAgencyLevelService, $uibModalInstance, InitFactory) {
    $scope.SessionIdList = SSTCMarksService.SessionIdList;
    $scope.Candidates = [{
        Name: null,
        SessionID: null,
        Designation: null,
        OrgType: null,
        Organization: null,
        Location: null,
        MobileNo: null,
        DateofBirth: null
    }];

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.AddRow = function () {
        $scope.Candidates.push({
            Name: null,
            Designation: null,
            OrgType: null,
            Organization: null,
            Location: null,
            MobileNo: null,
            DateofBirth: null
        });

    };
    $scope.DeleteRow = function (Index) {
        if (Index != 0) {
            console.log(Index);
            $scope.Candidates.splice(Index, 1);

        }
    };
    console.log($scope.SessionIdList);

    $scope.ValidateSession = function (CandidateDetails) {
        $scope.KeepGoing = true;
        angular.forEach($scope.SessionIdList, function (value, key) {
            console.log(value.SessionID == CandidateDetails.SessionID, " ", value.SessionID, " ", CandidateDetails.SessionID)
            if (value.SessionID == CandidateDetails.SessionID) {
                if (value.Day + 1 != 1) {
                    $scope.KeepGoing = false;
                    CandidateDetails.SessionID = null;
                    console.log("Its Working", $scope.KeepGoing);
                    return false;
                }
            }
            else {
                console.log("Its Working", $scope.KeepGoing);
                //continue;
            }
        });
        if ($scope.KeepGoing) { }
        else {
            swal("", "You Can not Add candidate against selected session id as day-1 test has already been conducted!", "warning");
        }
    };

    $scope.Submit = function (CandidateDetails) {
        $scope.KeepGo = true;
        angular.forEach($scope.Candidates, function (value, key) {
            $scope.Count = 0;
            value.SessionID = $scope.Candidate.SessionID;
            value.CreatedBy = $rootScope.session.User_Id;
            if (!value.Name || !value.Designation || !value.OrgType || !value.Organization || !value.Location || !value.MobileNo || !value.DateofBirth) {
                $scope.KeepGo = false;
            }
            angular.forEach($scope.Candidates, function (value1, key1) {
                if (value.Name == value1.Name && value.MobileNo == value1.MobileNo) {
                    $scope.Count++;
                    console.log($scope.Count);
                }

            });
            if ($scope.Count == 2) { $scope.KeepGo = false; }
        });
        if ($scope.KeepGo == true) {
            swal({
                title: '',
                text: "You are going to add candidates",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                //  cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then((result) => {
                //console.log("$scope.SessionIdList[$scope.Index]", $scope.SessionIdList[$scope.Index]);           
                FacultyAgencyLevelService.AddCandidate($scope.Candidates).then(function success(retdata) {

                    if (retdata.data.indexOf("Success") != -1) {
                        if (swal("", retdata.data, "success"))
                            $scope.cancel();
                    }
                    else {
                        swal("", retdata.data, "error");
                    }
                    console.log($scope.SessionWiseStudentsList);
                }, function error(data) {
                    console.log("Error in loading data from EDB");
                });
            });
        }
        else {
            swal("", "Duplicate or empty fields are not allowed ", "error");
        }

    };

    //$scope.init();


});

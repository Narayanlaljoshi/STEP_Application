var app = angular.module('FacultyAgencyLevelModule', []);

app.service('FacultyAgencyLevelService', function ($http, $location) {
    this.TestInitiateDay = null;
    this.InitiateTheory = false;
    this.SelectedSessionIds = "";
    this.SessionIdList = [];
    this.CandidateTestDetails = [];
    this.Session = [];
    this.AppUrl = "";
    this.MSPin = "";
    this.CurrentTestEvaluationType_Id = 1;
    this.CandidateDetails = null;
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
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
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/GetSessionWiseStudentsList' ,Obj);
    }; 
    this.AddCandidate = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/AddCandidate', Obj);
    };
    this.getFacultyName = function (UserName) {
        return $http.get(this.AppUrl + '/FacultyAgencyLevel/GetFacultyName?UserName=' + UserName);
    }; 
    this.SaveTestInitiationDetailsWithAttendance = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel_Mobile/SaveTestInitiationDetailsWithAttendance', Obj);
    }; 
    this.ResetStudentLogin = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/ResetStudentLogin', Obj);
    };
    this.GetEligibleCandidatesForEvaluation = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/GetEligibleCandidatesForEvaluation', Obj);
    };
    this.InitiateTestForEvaluation_Practical = function (Obj) {
        return $http.post(this.AppUrl + '/Test/InitiateTestForEvaluation_Practical', Obj);
    };
    this.CheckIfAnyTestIsGoingOn = function (Obj) {
        return $http.post(this.AppUrl + '/Test/CheckIfAnyTestIsGoingOn', Obj);
    };
    this.ClosePracticalSession = function (Obj) {
        return $http.post(this.AppUrl + '/Test/ClosePracticalSession', Obj);
    };
    this.GetMarksReport_Practical = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetMarksReport_Practical', Obj);
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
    FacultyAgencyLevelService.CurrentTestEvaluationType_Id = 1;
    $scope.ShowInitiateButton = false;
    $scope.InitiateTheory = false;
    FacultyAgencyLevelService.InitiateTheory = false;
    $scope.TestInitiateDay = null;
    $scope.NextTestEvaluationType_Id = null;
    //FacultyAgencyLevelService.TestInitiateDay = null;
    $rootScope.init = function () {
        $scope.SelectedSessionIds = [];
        $scope.TestInitiateDay = null;
        $scope.ShowInitiateButton = false;
        //FacultyAgencyLevelService.TestInitiateDay = null;
        console.log($scope.SelectedSessionIds.length);
        $scope.EligiblForTestInitiate = true;
        console.log("FacultyProgramdetails");
        FacultyAgencyLevelService.GetSessionIdList($rootScope.session.Agency_Id,$rootScope.session.UserName).then(function success(success) {
            $scope.SessionIdList = success.data;
            FacultyAgencyLevelService.SessionIdList = success.data;
            console.log("FacultyProgramdetails", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
            });
        FacultyAgencyLevelService.getFacultyName($rootScope.session.UserName).then(function success(success) {
            $scope.FacultyName = success.data;
            console.log(success.data);

        }, function error(Error) {
            console.log("Error in loading data from DB");
        });
    };

    $scope.GetSelectedSessionIds = function (pt) {
        $scope.IsValidSessionId = true;
        $scope.IsExixts = false;
        if (pt.IsChecked == true) {
            if ($scope.SelectedSessionIds.length == 0) {
                $scope.ProgramType_Id = pt.ProgramType_Id;
                pt.CreatedBy = $rootScope.session.User_Id;
                $scope.SelectedSessionIds.push(pt);
                $scope.TestInitiateDay = pt.day + 1;
                FacultyAgencyLevelService.TestInitiateDay = pt.day + 1;
                $scope.NextTestEvaluationType_Id = pt.NextTestEvaluationType_Id;
                FacultyAgencyLevelService.CurrentTestEvaluationType_Id= pt.CurrentTestEvaluationType_Id;
                
                if (pt.NextTestEvaluationType_Id == 1) {
                    $scope.ShowInitiateButton = true;
                    $scope.InitiateTheory = true; 
                    FacultyAgencyLevelService.InitiateTheory = true;
                }
                else {
                    $scope.InitiateTheory = false;
                    FacultyAgencyLevelService.InitiateTheory = false;
                    $scope.ShowInitiateButton = true;
                }
                //FacultyAgencyLevelService.TestInitiateDay = pt.day + 1;
            }
            else {
                angular.forEach($scope.SelectedSessionIds, function (value, key) {
                    console.log(pt.StartDate == value.StartDate, pt.StartDate, value.StartDate, pt.EndDate == value.EndDate, pt.EndDate, value.EndDate);
                    if (pt.ProgramId == value.ProgramId && pt.StartDate == value.StartDate && pt.EndDate == value.EndDate && pt.day == value.day && pt.NextTestEvaluationType_Id == value.NextTestEvaluationType_Id) {
                        pt.CreatedBy = $rootScope.session.User_Id;
                        if (pt.SessionID == value.SessionID) {
                            $scope.IsExixts = true;
                        }
                    }
                    else {
                        $scope.IsValidSessionId = false;
                        pt.IsChecked = false;
                    }
                });
                if ($scope.IsExixts == false && $scope.IsValidSessionId == true) {
                    $scope.SelectedSessionIds.push(pt);
                }
                if ($scope.IsValidSessionId == false) {
                    swal("Invalid Selection.", "Selected Session Ids do not meet desired conditions either of : Start Date, End Date ,Test Day , Program Code", "warning");
                }
            }
        }
        else {
            $scope.SelectedSessionIds.splice($scope.SelectedSessionIds.findIndex(x => x.SessionID == pt.SessionID), 1);
            if ($scope.SelectedSessionIds.length == 0) {
                FacultyAgencyLevelService.CurrentTestEvaluationType_Id = 1;
                FacultyAgencyLevelService.TestInitiateDay = null;
                $scope.TestInitiateDay = null; $scope.ShowInitiateButton = false;
            }
        }
    };

    $scope.InitiatTestWithAttendance = function () {
        $scope.Eligible = true;
        $scope.NotEligibleSessionID = '';
        var CurrentDate = $filter('date')(new Date(), 'dd-MM-yyyy');
            console.log(FacultyAgencyLevelService.SelectedSessionIds.length);
            if ($scope.SelectedSessionIds .length != 0)
            {
                
                angular.forEach($scope.SelectedSessionIds, function (value,Key) {
                    var TestIntiateDate = $filter('date')(value.TestIniateDate, 'dd-MM-yyyy');
                    console.log(value.ProgramType_Id == 1);
                    if (CurrentDate == TestIntiateDate && value.ProgramType_Id == 1) {
                        if (value.SameDayTestInitiation == false) {
                            $scope.Eligible = false;
                            $scope.NotEligibleSessionID = value.SessionID;
                        }
                    }
                    else {
                        //
                    }
                });

                if ($scope.Eligible == false ) {
                    swal("", "You can not Initiate multiple tests on same day for session Id : " + $scope.NotEligibleSessionID, "error");
                    //$scope.SelectedSessionIds = [];
                    return false;
                }
                else {
                    FacultyAgencyLevelService.TestInitiateDay = $scope.TestInitiateDay;
                    FacultyAgencyLevelService.SelectedSessionIds = $scope.SelectedSessionIds;
                    $uibModal.open({
                        templateUrl: 'InitiateTest.html',
                        controller: 'InitiateTestController',
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
            }
            else {

                swal("", "Please select the session ids for which you want to initiate test", "warning")
            }
        

    };

    $scope.InitiatTest = function () {
        console.log($scope.SelectedSessionIds);
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
                    $scope.SelectedSessionIds = [];
                    $rootScope.init();
                }
                else
                    swal("", success.data, "error");
                console.log("FacultyProgramdetails", success.data);
            }, function error(Error) {

                console.log("Error in loading data from EDB");
            });
        });
    };


    $scope.ShowPostTestsScore = function (pt) {
        FacultyAgencyLevelService.MSPin = pt.MSPIN;
        $uibModal.open({
            templateUrl: 'TestRecords.html',
            controller: 'TestRecordsController',
            windowClass: 'app-modal-window',
            backdrop: 'static',
            size: 'md'
        }).result.then(
            function () {

            },
            function () {

            }
            );
    };
    $scope.ShowTestCode = function (pt) {
        if (!pt.TestCode) {
            swal("", "No Test code available", "info");
        }
        else {
            swal("", "Test code is = " + pt.TestCode, "info");
        }

    };
    $scope.ViewResult = function () {
        console.log();
        //FacultyAgencyLevelService.Session = Session;
        FacultyAgencyLevelService.SelectedSessionIds = [];
        FacultyAgencyLevelService.SelectedSessionIds = $scope.SelectedSessionIds;
        if (FacultyAgencyLevelService.SelectedSessionIds.length != 0) {
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

                });
        }
        else {

            swal("", "Please select the session ids for which you want to see the result list", "warning");
        }
    };
    $scope.AddCandidate = function () {
        //FacultyAgencyLevelService.Session = ;
        $uibModal.open({
            templateUrl: 'partial/AddCandidate.html',
            controller: 'AddCandidateController',
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
    $scope.ShowCandidates = function () {
        console.log(FacultyAgencyLevelService.SelectedSessionIds);
        FacultyAgencyLevelService.SelectedSessionIds = [];
        FacultyAgencyLevelService.SelectedSessionIds = $scope.SelectedSessionIds;
        console.log(FacultyAgencyLevelService.SelectedSessionIds.length);
        if (FacultyAgencyLevelService.SelectedSessionIds.length != 0) {
           
            $uibModal.open({
                templateUrl: 'StudentsList.html',
                controller: 'StudentsListController',
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
        else {

            swal("","Please select the session ids for which you want to see the candidates list","warning")
        }
    };

    $rootScope.init();
});

app.controller('TestRecordsssController', function ($scope, $rootScope, FacultyAgencyLevelService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.MSPin = FacultyAgencyLevelService.MSPin;
    $scope.init = function () {
        FacultyAgencyLevelService.GetStudentPostTestScores($scope.MSPin).then(function success(retdata) {
            $scope.StudentPostTestScores = retdata.data;
            console.log($scope.StudentPostTestScores);
        }, function error(data) {
            console.log("Error in loading data from EDB", data);
        });
        // });
    };

    $scope.init();


});


app.controller('StudentsListController', function ($scope, $rootScope, $filter,FacultyAgencyLevelService, $uibModalInstance, InitFactory) {
    $scope.CurrentTestEvaluationType_Id = FacultyAgencyLevelService.CurrentTestEvaluationType_Id;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    console.log(FacultyAgencyLevelService.SelectedSessionIds);
    $scope.MSPin = FacultyAgencyLevelService.MSPin;
    $scope.init = function () {
        console.log(FacultyAgencyLevelService.SelectedSessionIds);
        FacultyAgencyLevelService.GetSessionWiseStudentsList(FacultyAgencyLevelService.SelectedSessionIds).then(function success(retdata) {

            $scope.SessionWiseStudentsList = retdata.data;
            angular.forEach($scope.SessionWiseStudentsList, function(value, key){

                if ($filter('date')(new Date(), 'dd-MM-yyyy') == $filter('date')(value.LoginDateTime, 'dd-MM-yyyy')) {
                    if (value.Status_Id == 1) {
                        value.RowColor = 'rgb(118, 132, 232)';
                    }
                    else {
                        value.RowColor = '#9ad89a';
                    }
                   
                }
                else {
                    value.RowColor = '#ffffff';
                }

            });
            console.log($scope.SessionWiseStudentsList);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };

    $scope.ResetStudentLogin = function (pt)
    {
        console.log(pt);
       
        swal({
            title: 'Are you sure?',
            text: "You are going to Reset password",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result) {

                FacultyAgencyLevelService.ResetStudentLogin(pt).then(function success(retdata) {
                    if (retdata.data.indexOf('Success') != -1)
                    {
                        swal("", retdata.data, "success");
                        $scope.init();
                    }

                }, function error(data) {
                    swal("", retdata.data, "error");
                });


                //swal("Poof! Your imaginary file has been deleted!", {
                //    icon: "success",
                //});
            } else {
                //swal("Your imaginary file is safe!");
            }
        });
    }

    $scope.InitiatPracticalTest = function (pt) {
        console.log("AFHASGH");
        FacultyAgencyLevelService.CandidateTestDetails = pt;
        console.log(FacultyAgencyLevelService.CandidateTestDetails);
        swal({
            title: 'Are you sure?',
            text: "You are going to initiate practical test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
            window.open('./PracticalScreen.html#/log=' + pt.MSPIN+'&asd=' + pt.Day, '_blank');
            //window.location = './PracticalScreen.html#/log=' + pt.MSPIN;//+ '&xud=' + success.data.User_Id;
            //window.location.assign('#/PracticalQuestion');// + '&xud=' + success.data.User_Id;

        });
    };


    $scope.init();
});

app.controller('AddCandidateController', function ($scope, $rootScope, FacultyAgencyLevelService, $uibModalInstance, InitFactory) {
    $scope.SessionIdList = FacultyAgencyLevelService.SessionIdList;
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
                if (value.day+1 != 1) {
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
            if (!value.Name || !value.Designation || !value.OrgType || !value.Organization || !value.Location || !value.MobileNo || !value.DateofBirth)
            {
                $scope.KeepGo = false;
            }
            angular.forEach($scope.Candidates, function (value1, key1) {
                if (value.Name == value1.Name && value.MobileNo==value1.MobileNo){
                    $scope.Count++;
                    console.log($scope.Count);
                }

            });
            if ($scope.Count == 2) { $scope.KeepGo = false;}
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
            swal("","Duplicate or empty fields are not allowed ","error");
        }
       
    };

    //$scope.init();


});

app.controller('InitiateTestController', function ($scope, $rootScope, FacultyAgencyLevelService, $uibModalInstance, $uibModal, InitFactory) {
    $scope.InitiateTheory=FacultyAgencyLevelService.InitiateTheory;
    $scope.TestInitiateDay = FacultyAgencyLevelService.TestInitiateDay;

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.ProgramType_Id = FacultyAgencyLevelService.SelectedSessionIds[0].ProgramType_Id;
    $scope.EvaluationTypeId = FacultyAgencyLevelService.SelectedSessionIds[0].NextTestEvaluationType_Id;
    //$scope.MSPin = FacultyAgencyLevelService.MSPin;
    $scope.init = function () {
        console.log(FacultyAgencyLevelService.SelectedSessionIds);
        $scope.SelectedSessionIds=FacultyAgencyLevelService.SelectedSessionIds;
        if ($scope.EvaluationTypeId == 1) {
            FacultyAgencyLevelService.GetSessionWiseStudentsList(FacultyAgencyLevelService.SelectedSessionIds).then(function success(retdata) {

                $scope.SessionWiseStudentsList = retdata.data;
                console.log($scope.SessionWiseStudentsList);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            var Obj = {
                Day: $scope.SelectedSessionIds[0].Day + 1,
                EvaluationType: $scope.SelectedSessionIds[0].NextTestEvaluationType,
                EvaluationTypeId: $scope.SelectedSessionIds[0].NextTestEvaluationType_Id,
                ProgramId: $scope.SelectedSessionIds[0].ProgramId,
                ProgramTestCalenderId: $scope.SelectedSessionIds[0].ProgramTestCalenderId,
                TestCode: $scope.SelectedSessionIds[0].TestCode
            };
            FacultyAgencyLevelService.GetEligibleCandidatesForEvaluation(Obj).then(function success(success) {
                $scope.Candidates = success.data;
                if ($scope.Candidates.length != 0) {
                    $scope.CandidatesSection = true;
                }
                else { $scope.CandidatesSection = false; }
                console.log("TestCodes", success.data);
            }, function error(Error) {
                console.log("Error in loading data from EDB");
            });
        }
        // });
    };

    $scope.GetEligibleCandidatesForEvaluation = function (Obj) {
        console.log(Obj);
        $scope.CurrentTestCode = Obj.TestCode;
        if (Obj.EvaluationTypeId == 2) {
            $scope.IsPractical = true;

        } else {
            $scope.IsPractical = false;
        }
        $scope.EligibleCandidatesCriteria = Obj;
        FacultyAgencyLevelService.GetEligibleCandidatesForEvaluation(Obj).then(function success(success) {
            $scope.Candidates = success.data;
            if ($scope.Candidates.length != 0) {
                $scope.CandidatesSection = true;
            }
            else { $scope.CandidatesSection = false; }
            console.log("TestCodes", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.ViewResult = function (pt) {
        console.log(pt);
        FacultyAgencyLevelService.CandidateDetails = pt;
        console.log("Popup is working");
        $uibModal.open({
            templateUrl: 'ViewResultPractical.html',
            controller: 'ViewResultPractical',
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
    $scope.InitiatPracticalTest = function (pt) {
        console.log(pt);
        $scope.IsTestInitiated = false;
        swal({
            title: 'Are you sure?',
            text: "You are going to initiate practical test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
            //window.open('./PracticalScreen.html#/log=' + pt.MSPIN + '&bus=' + pt.Day, '_blank');
            //$scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
            FacultyAgencyLevelService.CheckIfAnyTestIsGoingOn(pt).then(function success(success) {
                if (success.data.indexOf('Success') != -1) {
                    FacultyAgencyLevelService.InitiateTestForEvaluation_Practical(pt).then(function success(success) {
                        if (success.data.indexOf('Success') != -1) {
                            $scope.IsTestInitiated = true;
                            console.log(success.data, $scope.IsTestInitiated);
                            if ($scope.IsTestInitiated == true) {
                                window.open('./Timer.html#/log=' + pt.MSPIN + '&bus=' + pt.Day, '_blank');
                            }
                            $scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
                        }
                        else { swal("", "Somthing Happened Wrong, Please try again", "error"); }
                    }, function error(Error) {
                        console.log("Error in loading data from EDB");
                    });
                }
                else {
                    swal("", success.data, "warning");
                }
            }, function error(Error) {
                console.log("Error in loading data from EDB");
            });
        });
    };

    $scope.SaveTestInitiationDetailsWithAttendance = function () {
        //console.log(FacultyAgencyLevelService.SelectedSessionIds);
        angular.forEach($scope.SessionWiseStudentsList, function (value, key) {
            value.Day = $scope.TestInitiateDay;

        });
        var Obj = {
            SessionIDList: FacultyAgencyLevelService.SelectedSessionIds,
            StudentsList: $scope.SessionWiseStudentsList
        };
        swal({
            title: 'Are you sure?',
            text: "You are going to initiate test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
            FacultyAgencyLevelService.SaveTestInitiationDetailsWithAttendance(Obj).then(function success(retdata) {

                if (retdata.data.indexOf("Success") != -1) {
                    swal("", retdata.data, "success");
                    $scope.SelectedSessionIds = [];
                    $scope.InitiateTheory = false;
                    $rootScope.init();
                }
                else
                    swal("", retdata.data, "error");
                console.log("FacultyProgramdetails", success.data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        });
    };

    $scope.ClosePractical = function () {
        swal({
            title: 'Are you sure, You are going to Close Practical?',
            text: "Is it completed!!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Close it!'
        }).then((result) => {
            console.log(FacultyAgencyLevelService.SelectedSessionIds);
            FacultyAgencyLevelService.ClosePracticalSession(FacultyAgencyLevelService.SelectedSessionIds).then(function success(retdata) {
                if (retdata.data.indexOf("Error") != -1) {
                    swal("", retdata.data, "error");
                }
                else {
                    swal({
                        title: 'Practical Session Closed.',
                        text: "",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3c8dbc',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Ok'
                    }).then((result) => {
                        $scope.SelectedSessionIds = [];
                        $scope.InitiateTheory = false;
                        $scope.TestInitiateDay = null;
                        $scope.ShowInitiateButton = false;
                        $rootScope.init();
                        $scope.cancel();
                    });
                }
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        });
    };

    $scope.init();
});

app.controller('ViewResultPractical', function ($scope, $rootScope, FacultyAgencyLevelService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    console.log(FacultyAgencyLevelService.CandidateDetails);
    $scope.CandidateDetails = FacultyAgencyLevelService.CandidateDetails;
    $scope.init = function () {
        FacultyAgencyLevelService.GetMarksReport_Practical($scope.CandidateDetails).then(function success(retdata) {
            $scope.StudentPracticalScores = retdata.data;
            console.log($scope.StudentPracticalScores);
        }, function error(data) {
            console.log("Error in loading data from EDB", data);
        });
    };
    $scope.init();
});
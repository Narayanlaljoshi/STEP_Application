var app = angular.module('EvaluationModule', ['QuestionBankPractical']);

app.service('EvaluationService', function ($http, $location) {

    this.AppUrl = "";
    this.ProgramId = null;
    this.CandidateDetails = null;
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetProgramsList_Evaluation = function () {
        return $http.get(this.AppUrl + '/FacultyAgencyLevel/GetProgramsList_Evaluation');
    };
    this.GetTestCodes = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/GetTestCodes',Obj);
    };
    this.GetEligibleCandidatesForEvaluation = function (Obj) {
        return $http.post(this.AppUrl + '/FacultyAgencyLevel/GetEligibleCandidatesForEvaluation', Obj);
    };
    this.InitiateTestForEvaluation_Practical = function (Obj) {
        return $http.post(this.AppUrl + '/Test/InitiateTestForEvaluation_Practical', Obj);
    };
    this.InitiateTestForEvaluation_Theory = function (Obj) {
        return $http.post(this.AppUrl + '/Test/InitiateTestForEvaluation_Theory', Obj);
    };
    this.CheckIfAnyTestIsGoingOn = function (Obj) {
        return $http.post(this.AppUrl + '/Test/CheckIfAnyTestIsGoingOn', Obj);
    };
    this.List_CheckIfAnyTestIsGoingOn = function (Obj) {
        return $http.post(this.AppUrl + '/Test/List_CheckIfAnyTestIsGoingOn', Obj);
    };
    
    this.GetMarksReport_Practical = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetMarksReport_Practical', Obj);
    };
});
app.controller('EvaluationController', function ($scope, $http, $location, $rootScope, $uibModal,EvaluationService) {
    $scope.TestCodeSection = false;
    $scope.CandidatesSection = false;
    $scope.IsPractical = false;
    $scope.Init = function () {
        EvaluationService.GetProgramsList_Evaluation().then(function success(success) {
            $scope.ProgramsList = success.data;
            console.log("FacultyProgramdetails", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetTestCodes = function (ProgramId) {
        $scope.Candidates = [];
        console.log(ProgramId);
        var Obj = {
            ProgramId: ProgramId
        };
        EvaluationService.GetTestCodes(Obj).then(function success(success) {
            $scope.TestCodes = success.data;
            if ($scope.TestCodes.length!=0) {
                $scope.TestCodeSection = true;
            }
            else { $scope.TestCodeSection = false; }
            console.log("TestCodes", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
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
        EvaluationService.GetEligibleCandidatesForEvaluation(Obj).then(function success(success) {
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

    $scope.InitiatPracticalTest = function (pt) {
        pt.UserId = $rootScope.session.User_Id;
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
            EvaluationService.CheckIfAnyTestIsGoingOn(pt).then(function success(success) {
                if (success.data.indexOf('Success') != -1) {
            EvaluationService.InitiateTestForEvaluation_Practical(pt).then(function success(success) {
                if (success.data.indexOf('Success') != -1) {
                    $scope.IsTestInitiated = true;
                    console.log(success.data, $scope.IsTestInitiated);
                    if ($scope.IsTestInitiated == true) {
                        window.open('./Timer.html#/log=' + pt.MSPIN + '&bus=' + pt.Day, '_blank');
                    }
                    $scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
                }
                else if (success.data.indexOf('Warning')!=-1) { swal("", "Somthing Happened Wrong, Please try again", "error"); } 
                else { swal("", "Somthing Happened Wrong, Please try again", "error");}
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
    $scope.ResumePracticalTest = function (pt) {
        pt.UserId = $rootScope.session.User_Id;
        console.log(pt);
        $scope.IsTestInitiated = false;
        swal({
            title: 'Are you sure?',
            text: "You are going to resume practical test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Resume it!'
        }).then((result) => {
            //window.open('./PracticalScreen.html#/log=' + pt.MSPIN + '&bus=' + pt.Day, '_blank');
            //$scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
            pt.UserId = $rootScope.session.User_Id;
                    EvaluationService.InitiateTestForEvaluation_Practical(pt).then(function success(success) {
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
                
        });
    };
    $scope.InitiateTestForEvaluation_Practical = function (pt) {
        console.log(pt);
        //debugger
        EvaluationService.CheckIfAnyTestIsGoingOn(pt).then(function success(success) {
            if (success.data.indexOf('Success') != -1) {
                EvaluationService.InitiateTestForEvaluation_Practical(pt).then(function success(success) {
                    $scope.Candidates = success.data;
                    $scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
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
        
    };
    //window.open('./PracticalScreen.html#/log=' + pt.MSPIN, '_blank');
    $scope.InitiateTestForEvaluation_Theory = function () {
        console.log($scope.Candidates);
        swal({
            title: 'Are you sure?',
            text: "You are going to Initiate Theory Test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
            EvaluationService.List_CheckIfAnyTestIsGoingOn($scope.Candidates).then(function success(success) {
                if (success.data.indexOf('Success') != -1) {
                    EvaluationService.InitiateTestForEvaluation_Theory($scope.Candidates).then(function success(success) {
                        if (success.data.indexOf('Success') != -1) {
                            swal("", success.data, "success");
                            $scope.GetEligibleCandidatesForEvaluation($scope.EligibleCandidatesCriteria);
                        }
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
    $scope.Init();
    $scope.ViewResult = function (pt) {
        console.log(pt);
        EvaluationService.CandidateDetails = pt;
        console.log("Popup is working");
        $uibModal.open({
            templateUrl: 'ViewResultsEvaluation.html',
            controller: 'ViewResultsEvaluation',
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
});


app.controller('ViewResultsEvaluation', function ($scope, $rootScope, EvaluationService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    console.log(EvaluationService.CandidateDetails);
    $scope.CandidateDetails = EvaluationService.CandidateDetails;
    $scope.init = function () {
        EvaluationService.GetMarksReport_Practical($scope.CandidateDetails).then(function success(retdata) {
            $scope.StudentPracticalScores = retdata.data;
            console.log($scope.StudentPracticalScores);
        }, function error(data) {
            console.log("Error in loading data from EDB", data);
        });
    };
    $scope.init();
});
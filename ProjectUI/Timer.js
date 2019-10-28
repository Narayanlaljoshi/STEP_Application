var app = angular.module('TimerModule', ['ngLoadingSpinner', 'ngCookies']);

app.service('PracticalService', function ($http, $location) {

    this.prevPage;
    this.currentPage = 1;
    this.RemainingTime;
    this.AppUrl = "";
    this.TimerStoped = false;
    this.ShowNextbutton = true;
    this.qt = {};
    //console.log($location.absUrl());
    this.Minutes = 0;
    this.Seconds = 0;
    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.IsStarted = false;
    this.StudentTestQuestions = [];
    this.QuestionVariable = {};

    this.GetStudenttestDetails = function (MSPin) {
        return $http.get(this.AppUrl + '/Test/GetStudenttestDetails?MSPin=' + MSPin);
    };
    this.SaveTestResponse_Practical = function (Obj) {
        return $http.post(this.AppUrl + '/Test/SaveTestResponse_Practical/', Obj);
    };
    this.SaveCompleteTestResponse_Practical = function (Obj) {
        return $http.post(this.AppUrl + 'Test/SaveCompleteTestResponse_Practical/', Obj);
    }
    this.SaveInitiatedTestDetails = function (Obj) {
        return $http.post(this.AppUrl + '/Test/SaveInitiatedTestDetails', Obj);
    };
    this.GetStudentTestQuestions = function (ProgramTestCalenderId) {
        return $http.get(this.AppUrl + '/Test/GetStudentTestQuestions?ProgramTestCalenderId=' + ProgramTestCalenderId);
    };

    this.Get_Check = function (MSPin, Day) {
        ////console.log("MSPin",MSPin);
        ////console.log("Day",Day);

        return $http.get(this.AppUrl + '/Test/Get_Check?MSPin=' + MSPin + '&day=' + Day);
    };
    this.GetPrgramLanguage = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetLanguageWithoutEnglish?id=' + id);
    };

    this.GetStudentQuestionFormatedList = function (data) {
        return $http.post(this.AppUrl + '/Test/GetStudentQuestionFormatedList_Practical', data, {});
    };
    this.GetSetSequenceByProgramTestCalanderId = function (id) {
        return $http.get(this.AppUrl + '/Test/GetSetSequenceByProgramTestCalanderId?Id=' + id);
    };
    this.GetServiceTypes = function () {
        return $http.get(this.AppUrl + '/ServiceType/GetServiceTypes/');
    };
});
app.run(function ($window, $rootScope) {
    $rootScope.online = navigator.onLine;
    $window.addEventListener("offline", function () {
        $rootScope.$apply(function () {
            $rootScope.online = false;
        });
    }, false);

    $window.addEventListener("online", function () {
        $rootScope.$apply(function () {
            $rootScope.online = true;
        });
    }, false);
});

app.config(function ($httpProvider, $locationProvider) {
    $locationProvider.hashPrefix('');
});
app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
app.controller("ControllerName", function ($scope, $timeout, $location, $cookies, PracticalService, $rootScope, $interval) {
    console.log("Timer Controller is working");
    //var queCount1 = angular.element(document.getElementById("examTime"));
    $scope.NumberofQuestionsViewed = [{ Id: 0 }];
    $scope.ShowSubmitButton = true;
    $scope.Set_Id = null;
    $scope.UserName = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&'));
    $scope.Day = $location.absUrl().substring($location.absUrl().indexOf('&') + 5, $location.absUrl().length);
    //$scope.CandidateTestDetails.MSPIN;
    console.log($scope.UserName, "$scope.Day");
    console.log($scope.Day, "$scope.Day");
    $scope.ShowStartButton = true;
    $scope.ShowDirective = false;
    $scope.ShowNextbutton = true;
    $scope.currentPage = 0;
    $scope.IsTimeOver = false;
    var countDowner, countDown = null;
    var hours = null;
    var minutes = null;
    var seconds = null;
    $scope.init = function () {
        var Cookiess = $cookies.get('StudentDetails');
        //console.log($location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().length));
        $scope.dirOptions = {};
        //$scope.CandidateTestDetails = FacultyAgencyLevelService.CandidateTestDetails;
        //$scope.UserName = $scope.CandidateTestDetails.MSPIN;
        $scope.UserName = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&'));
        //$scope.User_Id = $location.absUrl().substring($location.absUrl().indexOf('&') + 5, $location.absUrl().length);
        PracticalService.GetStudenttestDetails($scope.UserName).then(function success(success) {
            if (success.data != null) {
                $scope.StudentTestDetails = success.data;
                PracticalService.StudentTestDetail = $scope.StudentTestDetails;
                console.log($scope.StudentTestDetails);
                //$scope.Duration = $scope.StudentTestDetails.TestDuration;
                
                $scope.ShowStartButton = true;
                $scope.ShowQuestions = false;
                PracticalService.GetSetSequenceByProgramTestCalanderId($scope.StudentTestDetails.ProgramTestCalenderId).then(function success(data) {
                    $scope.SetSequence = data.data;
                    return PracticalService.GetServiceTypes();
                }).then(function success(results) {
                    $scope.ServiceTypeList = results.data;
                    $scope.Position = $scope.ServiceTypeList[$scope.StudentTestDetails.Position_Id].ServiceType + ' : ' + $scope.ServiceTypeList[$scope.StudentTestDetails.Position_Id].Position;
                }, function error(data) {
                    console.log("Error in loading data from EDB");
                });
            }
            else {
                swal({
                    title: 'No Test Assigned Yet !',
                    text: "",
                    type: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3c8dbc',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'OK!'
                }).then((result) => {
                    $scope.ShowStartButton = false;
                    if (result)
                        window.top.close();
                    else
                        window.top.close();
                });
            }

        }, function error(Error) {
            ////console.log("Error in loading data from EDB");
        });


    };
    $scope.ServiceType = function () {
        PracticalService.ServiceType().then(function success(data) {
            $scope.ServiceTypeList = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.getClass = function (pt) {

        if (pt.Catagory == $scope.SelectedCatagory) {
            return 'active';
        }

    };
    $scope.SelectCatagory = function (Category) {
        $scope.SelectedCatagory = Category;
    };

    $scope.$watch('online', function (newStatus) {
        if (newStatus == false) {
            alert("No internet connection");
            //$scope.stopTimer();
        }
        //while (newStatus == false) {
        //    alert("No internet connection");
        //}

    });
    //Note: Only those configs are functional which is documented at: http://www.codeproject.com/Articles/860024/Quiz-Application-in-AngularJs
    // Others are work in progress.
    $scope.GetPosition = function (Position_Id) {
        console.log(Position_Id);
        $scope.Position = $scope.ServiceTypeList[Position_Id].ServiceType + ' : ' + $scope.ServiceTypeList[Position_Id].Position;
    };
    $scope.GetStudentTestQuestions = function (pt) {
        console.log($scope.ExtendedTime);
        if (!$scope.Set_Id) {
            swal("Error", "Please select any Set", "error");
            return false;
        }
        swal({
            title: 'Are you sure?',
            text: "You are going to start test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, start it!'
        }).then((result) => {

            PracticalService.QuestionVariable = {
                ProgramTestCalenderId: $scope.StudentTestDetails.ProgramTestCalenderId,
                LangId: 1,
                MSPIN: $scope.StudentTestDetails.MSPIN,
                SessionID: $scope.StudentTestDetails.SessionID,
                Day: $scope.StudentTestDetails.DayCount,
                UserId: $scope.User_Id,
                ProgramId: $scope.StudentTestDetails.ProgramId,
                TypeOfTest: $scope.StudentTestDetails.TypeOfTest,
                ProgramType_Id: $scope.StudentTestDetails.ProgramType_Id,
                Set_Id: $scope.Set_Id,
                ExtendedTime: $scope.ExtendedTime,
                Position_Id: $scope.Position_Id,
                TimetoExtend: $scope.ExtendedTime,
                EvaluationType_Id: $scope.StudentTestDetails.EvaluationTypeId
            };
            console.log(PracticalService.QuestionVariable);
            PracticalService.GetStudentQuestionFormatedList(PracticalService.QuestionVariable).then(function success(success) {

                if (success.data == null) {
                    swal("", "Question Set Not Generated !", "error");
                }
                else {
                    $scope.CompleteObject = success.data;
                    $scope.StudentLanguageQuestion = success.data.StudentLanguageQuestion;
                    $scope.SelectedCatagory = $scope.StudentLanguageQuestion[0].Catagory;
                    PracticalService.StudentTestQuestions = success.data.StudentLanguageQuestion;
                    if (PracticalService.StudentTestQuestions != null) {
                        $scope.Duration = success.data.StudentTestDetails.TestDuration;
                        $scope.time = success.data.StudentTestDetails.TestDuration;
                        $scope.ShowQuestions = true;
                        $scope.ShowStartButton = false;
                        $scope.mode = 'quiz';
                        $scope.QuestionsResponse = success.data.StudentLanguageQuestion;
                        $scope.totalItems = PracticalService.StudentTestQuestions.length;
                        $scope.currentPage = 1;
                        countDowner, countDown = $scope.time;
                        //hours = Math.floor(countDown / 3600);
                        minutes = Math.floor((countDown) / 60);
                        seconds = Math.floor(countDown % 60);
                        $scope.countDowner();
                    }
                }
            }, function error(Error) {

            });
        });
    };
    $scope.Submit = function () {
        $scope.AnsweredAllQuestion = true;
        //angular.forEach(PracticalService.StudentTestQuestions, function (value, key) {
        //    //value.
        //    if (!value.AnswerGiven) {
        //        $scope.AnsweredAllQuestion = false;
        //    }
        //});
        console.log(parseInt(minutes * 60) + parseInt(seconds));
        $scope.CompleteObject.StudentTestDetails.RemainingTime = parseInt(minutes * 60) + parseInt(seconds);
        console.log($scope.CompleteObject);
        if ($scope.AnsweredAllQuestion == false) {

            swal({
                title: 'Are you sure?',
                text: "You have not answered all questions!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then((result) => {
                //$scope.CompleteObject.StudentTestDetails.CreatedBy = $rootScope.SessionID.User_Id;
                $scope.CompleteObject.StudentTestDetails.Status_Id = 1;
                $scope.CompleteObject.StudentLanguageQuestion = $scope.StudentLanguageQuestion;

                //var Obj = {
                //    StudentTestDetails: PracticalService.QuestionVariable,
                //    StudentLanguageQuestion: PracticalService.StudentTestQuestions
                //};
                $scope.CompleteObject.StudentTestDetails.RemainingTime = (minutes * 60) + seconds;
                console.log($scope.CompleteObject);
                PracticalService.SaveCompleteTestResponse_Practical($scope.CompleteObject).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted test successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        console.log("window.top.close();");
                        if (result)
                            window.top.close();
                        else
                            window.top.close();
                        //window.location = './Stlogin.html';
                    });
                }, function error(Error) {
                    ////console.log("Error in loading data from EDB");
                });
            });
        }
        else {
            swal({
                title: 'Are you sure?',
                text: "You are going to submit test!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Submit it!'
            }).then((result) => {

                $scope.CompleteObject.StudentTestDetails.Status_Id = 1;
                $scope.CompleteObject.StudentLanguageQuestion = $scope.StudentLanguageQuestion;

                //var Obj = {
                //    StudentTestDetails: PracticalService.QuestionVariable,
                //    StudentLanguageQuestion: PracticalService.StudentTestQuestions
                //};
                $scope.CompleteObject.StudentTestDetails.RemainingTime = PracticalService.RemainingTime;
                PracticalService.SaveCompleteTestResponse_Practical($scope.CompleteObject).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted test successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result)
                            window.top.close();
                        else
                            window.top.close();
                    });
                }, function error(Error) {
                });
            });
        }

    };

    //$scope.time = 1;// queCount1.val();
    $scope.PauseTime = 1;
    $scope.IsTimePaused = false;
    $scope.PausedMinutes = 0;
    $scope.PausedSeconds = 0;
    //var CountDownPauseTime = $scope.PauseTime * 60;

    //var mminutes =null;
    //var sseconds = null;

    var secondsToStr = ('00' + minutes).substr(-2) + 'm : ' + ('00' + seconds).substr(-2) + 's';

    $scope.countDowner = function () {
        $scope.clock = minutes + "" + seconds;
        if (seconds <= 0) {
            seconds = 60;
            minutes--;
        }
        seconds--;
        if (seconds == 0 && minutes == 0) {
            if ($scope.IsTimeOver == true) {
                $scope.CompleteObject.StudentTestDetails.Status_Id = 1;
                $scope.CompleteObject.StudentLanguageQuestion = $scope.StudentLanguageQuestion;
                PracticalService.SaveCompleteTestResponse_Practical($scope.CompleteObject).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted test successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result)
                            window.top.close();
                        else
                            window.top.close();
                    });
                }, function error(Error) {
                });
            }
            else {
                $scope.IsTimeOver = true;
                swal("", "Time Over, now you have 3 minutes more to submit the test", "warning");
                minutes = 3;
            }
            //$scope.CompleteObject.StudentTestDetails.Status_Id = 1;
            //$scope.CompleteObject.StudentLanguageQuestion = $scope.StudentLanguageQuestion;
            //var Obj = {
            //    StudentTestDetails: PracticalService.QuestionVariable,
            //    StudentLanguageQuestion: PracticalService.StudentTestQuestions
            //};
            $scope.CompleteObject.StudentTestDetails.RemainingTime = PracticalService.RemainingTime;
            //PracticalService.SaveCompleteTestResponse_Practical($scope.CompleteObject).then(function success(success) {
            //    swal({
            //        title: 'Thanks',
            //        text: "You have submitted test successfully",
            //        type: 'success',
            //        showCancelButton: false,
            //        confirmButtonColor: '#3085d6',
            //        cancelButtonColor: '#d33',
            //        confirmButtonText: 'OK'
            //    }).then((result) => {
            //        if (result)
            //            window.top.close();
            //        else
            //            window.top.close();
            //    });
            //}, function error(Error) {
            //});
            console.log("Time Up");
        }

        $scope.countDown_text = secondsToStr;
        $scope.countDown_text1 = ('00' + minutes).substr(-2);
        $scope.countDown_text2 = ('00' + seconds).substr(-2);
        $timeout($scope.countDowner, 1000);
    };

    //$scope.PauseTimerTill = function () {
    //    console.log(mminutes == 0);
    //    $scope.clock1 = mminutes + "" + sseconds;
    //    if (mminutes == 0) {
    //        console.log(mminutes);
    //        $scope.countDown_text2 = ('00' + sseconds).substr(-2);
    //    }
    //    else {
    //        if (sseconds <= 0) {
    //            sseconds = 60;
    //            mminutes--;
    //            $scope.countDown_text1 = ('00' + mminutes).substr(-2);
    //            $scope.countDown_text2 = ('00' + sseconds).substr(-2);
    //        }

    //    }

    //    sseconds--;



    //    $timeout($scope.PauseTimerTill, 1000);
    //};

    $scope.PauseTime = function () {
        $scope.clock = null;
        $scope.IsTimePaused = true;
        $scope.PausedMinutes = minutes;
        $scope.PausedSeconds = seconds;
        minutes = null;
        seconds = null;
        console.log($scope.PausedMinutes, $scope.PausedSeconds);
    };
    $scope.ResumeTime = function () {
        //$scope.clock = null;
        $scope.IsTimePaused = false;
        minutes = $scope.PausedMinutes;
        seconds = $scope.PausedSeconds;
    };
    //$scope.countDowner();
});  
var app = angular.module('PracticalModule', ['ngLoadingSpinner', 'ngCookies']);

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
        return $http.post(this.AppUrl + '/Test/SaveTestResponse_Practical/' , Obj);
    };
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

app.controller('PracticalController', function ($scope, $http, $location, $cookies, PracticalService, $rootScope) {
    
    $scope.UserName = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&'));
    $scope.Day = $location.absUrl().substring($location.absUrl().indexOf('&') + 5, $location.absUrl().length);
    //$scope.CandidateTestDetails.MSPIN;
    console.log($scope.UserName, "$scope.Day");
    console.log($scope.Day, "$scope.Day");
    $scope.ShowStartButton = true;
    $scope.ShowDirective = false;
    $scope.ShowNextbutton = true;
    $scope.currentPage = 0;
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
                $scope.Duration = $scope.StudentTestDetails.TestDuration;
                $scope.ShowStartButton = true;
                $scope.ShowQuestions = false;
                //PracticalService.GetPrgramLanguage($scope.StudentTestDetails.ProgramId).then(function success(data) {
                //    $scope.ProgramLanguageArray = data.data;
                //}, function error(data) {
                //    console.log("Error in loading data from EDB");
                //});
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
                    //window.location.assign = "./Stlogin.html";
                    //swal("", "No Test Assigned Yet !", "warning");
                });
            }

        }, function error(Error) {
            ////console.log("Error in loading data from EDB");
        });


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
    
    $scope.GetStudentTestQuestions = function (pt) {

        //if (!pt) {
        //    swal("Error", "Please select any language", "error");
        //    return false;
        //}
        swal({
            title: 'Are you sure?',
            text: "You are going to start test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, start it!'
        }).then((result) => {

            //PracticalService.GetStudenttestDetails($scope.UserName).then(function success(success) {
            //    if (success.data != null) {
            //        $scope.StudentTestDetails = success.data;
            //        PracticalService.StudentTestDetail = $scope.StudentTestDetails;
            //        PracticalService.IsStarted = true;
            //        $scope.ShowStartButton = true;
            //        $scope.ShowQuestions = false;
            //    }
            //    else {
            //        $scope.ShowStartButton = true;
            //        //swal("Error", "No Test Assigned Yet !", "Error");
            //        swal({
            //            title: 'Thanks',
            //            text: "No Test Assigned Yet !",
            //            type: 'error',
            //            showCancelButton: false,
            //            confirmButtonColor: '#3085d6',
            //            cancelButtonColor: '#d33',
            //            confirmButtonText: 'OK'
            //        }).then((result) => {
            //            if (result)
            //                window.top.close();
            //            else
            //                window.top.close();
            //        });
            //    }
                PracticalService.QuestionVariable = {
                    ProgramTestCalenderId: $scope.StudentTestDetails.ProgramTestCalenderId,
                    LangId: 1,
                    MSPIN: $scope.StudentTestDetails.MSPIN,
                    SessionID: $scope.StudentTestDetails.SessionID,
                    Day: $scope.StudentTestDetails.DayCount,
                    UserId: $scope.User_Id,
                    ProgramId: $scope.StudentTestDetails.ProgramId,
                    TypeOfTest: $scope.StudentTestDetails.TypeOfTest,
                    ProgramType_Id: $scope.StudentTestDetails.ProgramType_Id
                };


                PracticalService.GetStudentQuestionFormatedList(PracticalService.QuestionVariable).then(function success(success) {
                   
                    PracticalService.StudentTestQuestions = success.data.StudentLanguageQuestion;
                    if (success.data == null) {
                        swal("", "Question Set Not Generated !", "error");
                    }
                    else {
                        if (PracticalService.StudentTestQuestions != null) {
                            $scope.Duration = $scope.StudentTestDetails.TestDuration;
                            $scope.dirOptions.directiveFunction();
                            $scope.QuestionsResponse = PracticalService.StudentTestQuestions;
                            $scope.ShowQuestions = true;
                            $scope.ShowStartButton = false;
                            $scope.mode = 'quiz';
                            $scope.totalItems = PracticalService.StudentTestQuestions.length;
                            //$scope.questions = PracticalService.StudentTestQuestions;
                            $scope.currentPage = 1;
                            $scope.qt = PracticalService.StudentTestQuestions[0];
                            PracticalService.qt = $scope.qt;
                            console.log("$scope.questions",$scope.questions);
                            
                        }
                    }
                }, function error(Error) {
                    ////console.log("Error in loading data from EDB");
                });
        //    }, function error(Error) {
        //        ////console.log("Error in loading data from EDB");
        //    });
        });
    };
    $scope.GoToNext = function (index) {
       
        PracticalService.SaveTestResponse_Practical($scope.qt).then(function success(success) {
           
        }, function error(Error) {
            ////console.log("Error in loading data from EDB");
        });

        if (index > 0 && index <= $scope.totalItems) {
            $scope.currentPage = index;
            $scope.qt = PracticalService.StudentTestQuestions[$scope.currentPage];
            $scope.mode = 'quiz';
        }
        if ($scope.currentPage == $scope.totalItems) {
            console.log($scope.totalItems);
            $scope.ShowNextbutton = false;
        }
        else {
            $scope.ShowNextbutton = true;
        }
        PracticalService.StudentTestQuestions[$scope.currentPage] = $scope.qt;
        
        //console.log($scope.currentPage,Index, $scope.totalItems-1);
        //if (Index == 0) {
        //    $scope.currentPage = 1;
        //    $scope.ShowNextbutton = true;
        //    console.log("$scope.currentPage", $scope.currentPage, "Index", Index);
        //    $scope.qt = PracticalService.StudentTestQuestions[$scope.currentPage];
        //    PracticalService.qt = $scope.qt;
        //}
        //else if (Index > 0 && Index < $scope.totalItems - 1) {
        //    $scope.currentPage = Index + 1;
        //    $scope.ShowNextbutton = true;
        //    console.log("$scope.currentPage", $scope.currentPage, "Index", Index);
        //    $scope.qt = PracticalService.StudentTestQuestions[$scope.currentPage];
        //    PracticalService.qt = $scope.qt;
        //}
        //else if (Index == $scope.totalItems - 1) {
        //    $scope.currentPage = Index + 1;
        //    $scope.ShowNextbutton = false;
        //    console.log("$scope.currentPage", $scope.currentPage, "Index", Index);
        //    $scope.qt = PracticalService.StudentTestQuestions[$scope.currentPage];
        //    PracticalService.qt = $scope.qt;
        //}
        //else {
        //    $scope.currentPage = 1;
        //    $scope.ShowNextbutton = false;
        //    console.log("$scope.currentPage", $scope.currentPage, "Index", Index);
        //    $scope.qt = PracticalService.StudentTestQuestions[0];
        //    PracticalService.qt = $scope.qt;
        //}
        
    };
    $scope.GoToPrevious = function () {
        PracticalService.SaveTestResponse_Practical($scope.qt).then(function success(success) {

        }, function error(Error) {
            ////console.log("Error in loading data from EDB");
        });
        PracticalService.StudentTestQuestions[$scope.currentPage] = $scope.qt;
        $scope.currentPage = $scope.currentPage - 1;
        $scope.qt = PracticalService.StudentTestQuestions[$scope.currentPage];
        if ($scope.currentPage > 0 && index <= $scope.totalItems) {
            $scope.currentPage = $scope.currentPage;
            //$scope.mode = 'quiz';
        }
        if ($scope.currentPage == $scope.totalItems) {
            //console.log($scope.totalItems);
            $scope.ShowNextbutton = false;
        }
        else {
            $scope.ShowNextbutton = true;
        }
    };
    $scope.Submit = function () {

        $scope.AnsweredAllQuestion = true;
        //angular.forEach(PracticalService.StudentTestQuestions, function (value, key) {
        //    //value.
        //    if (!value.AnswerGiven) {
        //        $scope.AnsweredAllQuestion = false;
        //    }
        //});
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
                $scope.qt.Status_Id = 1;
                
                //var Obj = {
                //    StudentTestDetails: PracticalService.QuestionVariable,
                //    StudentLanguageQuestion: PracticalService.StudentTestQuestions
                //};

                PracticalService.SaveTestResponse_Practical($scope.qt).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted test successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        //$cookies.remove('LogKey');
                        //$cookies.get('LogKey');
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
                $scope.qt.Status_Id = 1;
               
                PracticalService.SaveTestResponse_Practical($scope.qt).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted test successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        //$cookies.remove('LogKey');
                        //$cookies.get('LogKey');
                        window.top.close();
                        //window.location = './Stlogin.html';
                    });
                    //window.location = './Stlogin.html';
                }, function error(Error) {
                    ////console.log("Error in loading data from EDB");
                });
            });
        }

    };

    $scope.init();
});

app.filter('counterValue', function () {
    return function (value) {
        value = parseInt(value);
        if (!isNaN(value) && value >= 0 && value < 10)
            return "0" + valueInt;
        return "";
    };
});

app.directive('timer', function ($timeout, $http, $compile, $cookies, PracticalService) {
    return {

        restrict: 'E',
        scope: {
            interval: '=', //don't need to write word again, if property name matches HTML attribute name
            startTimeAttr: '=?startTime', //a question mark makes it optional
            countdownAttr: '=?countdown',
            options: '='//what unit?
        },
        template: '<div><p>' +
            
            '<p>{{ minutes }}:' +
            '{{ seconds }}  ' +
            
            '</p>' + '</p>',

        
        link: function (scope, elem, attrs) {

            //Properties
            scope.startTime = scope.startTimeAttr;//? new Date(scope.startTimeAttr) : new Date();
            var countdown = (scope.countdownAttr && parseInt(scope.countdownAttr, 10) > 0) ? parseInt(scope.countdownAttr, 10) : 60; //defaults to 60 seconds
           
            function tick() {

                //How many milliseconds have passed: current time - start time
                scope.millis = new Date() - scope.startTime;
                //var TimerStoped = false;
                if (countdown > 0) {
                    //if (scope.minutes < 10) { scope.minutes = '0' + scope.minutes }
                    scope.millis = countdown * 1000;
                    countdown--;
                   
                    PracticalService.RemainingTime = countdown;


                }
                else if (countdown <= 0) {
                    if (PracticalService.TimerStoped == false) {
                        scope.stop();
                        PracticalService.TimerStoped = true;
                    }
                    
                }
                else if (seconds < 10) {


                };

                
                scope.seconds = Math.floor((scope.millis / 1000) % 60);
                if (scope.seconds < 10) {
                    scope.seconds = '0' + scope.seconds;
                }
                
                scope.minutes = Math.floor(((scope.millis / (1000)) / 60) % 60);
                if (scope.minutes < 10) {
                    scope.minutes = '0' + scope.minutes;
                }

                scope.$emit('timer-tick', {
                    intervalId: scope.intervalId,
                    millis: scope.millis
                });

                scope.$apply();

            }

            angular.extend(scope.options, {
                directiveFunction: function () {
                    
                    start();
                }
            });
            scope.stop = function () {
                PracticalService.qt.Status_Id = 1;
                PracticalService.SaveTestResponse_Practical(PracticalService.qt).then(function success(success) {
                    swal({
                        title: 'Thanks',
                        text: "You have submitted practical successfully",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        $cookies.remove('LogKey');
                        window.top.close();// = './Stlogin.html';
                    });
                }, function error(Error) {
                    
                });
            };

            
            function start() {
                //resetInterval();
                scope.intervalId = setInterval(tick, scope.interval);
            }

            scope.resume = function () {
                scope.stoppedTime = null;
                scope.startTime = new Date() - (scope.stoppedTime - scope.startTime);
                start();
            };
            if (PracticalService.IsStarted == true) {
                start(); //start timer automatically
            }

            scope.$watch('PracticalService.IsStarted', function (oldValue, newValue) {
                ////console.log(oldValue);

            }, true);

            //Watches
            scope.$on('time-start', function () {
                start();
            });

            scope.$on('timer-resume', function () {
                scope.resume();
            });

            scope.$on('timer-stop', function () {
                scope.stop();
            });

            //Cleanup
            elem.on('$destroy', function () {
                resetInterval();
            });

        }
    };
});
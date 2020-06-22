var app = angular.module('StudentModule', ['ngLoadingSpinner', 'ngCookies']);

app.service('StudentService', function ($http, $location) {

    this.prevPage;
    this.currentPage = 1;
    this.RemainingTime;
    this.AppUrl = "";
    this.TimerStoped = false;
    //this.ShowNextbutton = true;
    console.log($location.absUrl());
    this.Minutes = 0;
    this.Seconds = 0;
    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }
    this.IsStarted = false;
    this.StudentTestQuestions = [];
    this.QuestionVariable = {};

    this.GetStudenttestDetails = function (MSPin) {
        return $http.get(this.AppUrl + '/Test/GetStudenttestDetails?MSPin=' + MSPin);
    };
    this.SaveInitiatedTestDetails = function (Obj) {
        return $http.post(this.AppUrl + '/Test/SaveInitiatedTestDetails', Obj);
    };
    this.GetStudentTestQuestions = function (ProgramTestCalenderId) {
        return $http.get(this.AppUrl + '/Test/GetStudentTestQuestions?ProgramTestCalenderId=' + ProgramTestCalenderId);
    };
    this.SaveTestResponse = function (Obj) {
        //console.log("Submit Working", Obj);
        return $http.post(this.AppUrl + '/Test/SaveTestResponse/', Obj);
    };
    this.SaveTestResponseComplete = function (Obj) {
        //console.log("Submit Working", Obj);
        return $http.post(this.AppUrl + '/Test/SaveTestResponseComplete/', Obj);
    };

    this.Get_Check = function (MSPin, Day) {
        //console.log("MSPin",MSPin);
        //console.log("Day",Day);

        return $http.get(this.AppUrl + '/Test/Get_Check?MSPin=' + MSPin + '&day=' + Day);
    };
    this.GetPrgramLanguage = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetLanguageWithoutEnglish?id=' + id);
    };

    this.GetStudentQuestionFormatedList = function (data) {
        return $http.post(this.AppUrl + '/Test/GetStudentQuestionFormatedList', data, {});
    };
});

//app.factory('InitFactory', function (StudentService) {

//    return {
//        init: function () {
//            RtcMasterService.GetAgencyList().then(function success(data) {
//                RtcMasterService.AgencyList = data.data;
//                //console.log("Get Region List", data);
//            }, function error(data) {
//                //console.log("Error in loading data from EDB");
//            });
//        }
//    };
//});
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
app.controller('StudentController', function ($scope, $http, $location, $cookies, StudentService, helperService, $rootScope, $window) {
    $scope.dirOptions = {};
    //StudentService.QuestionVariable = {};
    $scope.quizName = 'data/csharp.js';
    $scope.UserName = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&') - 1);
    $scope.User_Id = $location.absUrl().substring($location.absUrl().indexOf('&') + 5, $location.absUrl().length);
    //console.log($scope.User_Id );
    $scope.ShowStartButton = true;
    $scope.ShowDirective = false;
    $scope.ShowNextbutton = true;
    //window.onpopstate = function (e) { window.history.forward(1); }
    //$scope.$on('$locationChangeStart', function (event, next, current) {
    //    // Here you can take the control and call your own functions:
    //    console.log(current);
    //    console.log(next);
    //    alert('Sorry ! Back Button is disabled');
    //    // Prevent the browser default action (Going back):
    //    event.preventDefault();
    //});
    var DTL = JSON.parse(localStorage.getItem('StudentTestQuestions'));

    $scope.init = function () {
        if (DTL != null) {
            $scope.Duration = DTL.StudentTestDetails.RemainingTime;
            console.log("$scope.Duration", $scope.Duration);
        }
        var Cookiess = $cookies.get('StudentDetails');
        //window.localStorage.setItem('StudentTestQuestions', JSON.stringify(StudentService.StudentTestQuestions));
        var Details = JSON.parse(localStorage.getItem('StudentTestQuestions'));
        console.log(DTL);
        $scope.dirOptions = {};
        $scope.UserName = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&'));
        $scope.User_Id = $location.absUrl().substring($location.absUrl().indexOf('&') + 5, $location.absUrl().length);
        StudentService.GetStudenttestDetails($scope.UserName).then(function success(success) {
            if (success.data != null) {
                //window.localStorage.setItem('UserName', success.data);
                window.localStorage.setItem('StudenttestDetails', JSON.stringify(success.data));
                //var Details = JSON.parse(localStorage.getItem('StudenttestDetails'));
                //var as = window.localStorage.getItem('UserName');
                //console.log(Details);
                if (success.data.ErrorMessage != null) {
                    swal({
                        title: success.data.ErrorMessage,
                        text: "",
                        type: 'warning',
                        showCancelButton: false,
                        confirmButtonColor: '#3c8dbc',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK!'
                    }).then((result) => {
                        //$scope.ShowStartButton = false;
                        window.location.assign('./Stlogin.html');
                        //swal("", "No Test Assigned Yet !", "warning");
                    });
                }
                else {
                    $scope.StudentTestDetails = success.data;
                    
                    StudentService.StudentTestDetail = $scope.StudentTestDetails;
                    $scope.Duration = $scope.StudentTestDetails.TestDuration;//parseInt($scope.StudentTestDetails.TestDuration * 60);
                    console.log("$scope.Duration", $scope.Duration);
                    $scope.ShowStartButton = true;
                    $scope.ShowQuestions = false;
                    if (DTL != null) {
                        if ($scope.StudentTestDetails.MSPIN != DTL.StudentTestDetails.MSPIN || DTL.StudentTestDetails.DayCount != $scope.StudentTestDetails.MSPIN.DayCount || DTL.StudentTestDetails.SessionID != $scope.StudentTestDetails.MSPIN.SessionID) {
                            DTL = null;
                            window.localStorage.removeItem('StudentTestQuestions');
                        }
                        else {

                            $scope.Duration = DTL.StudentTestDetails.RemainingTime;
                            console.log("$scope.Duration", $scope.Duration);
                        }
                    }
                    console.log($scope.StudentTestDetails.ProgramId);

                    StudentService.GetPrgramLanguage($scope.StudentTestDetails.ProgramId).then(function success(data) {
                        $scope.ProgramLanguageArray = data.data;
                        console.log("Program Language Array List", $scope.ProgramLanguageArray);
                    }, function error(data) {
                        console.log("Error in loading data from EDB");
                    });
                }
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
                    //$scope.ShowStartButton = false;
                    window.location.assign('./Stlogin.html');
                    //swal("", "No Test Assigned Yet !", "warning");
                });
            }
        }, function error(Error) {
            //console.log("Error in loading data from EDB");
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
    $scope.defaultConfig = {
        'allowBack': true,
        'allowReview': true,
        'autoMove': false,  // if true, it will move to next question automatically when answered.
        'duration': 0,  // indicates the time in which quiz needs to be completed. post that, quiz will be automatically submitted. 0 means unlimited.
        'pageSize': 1,
        'requiredAll': false,  // indicates if you must answer all the questions before submitting.
        'richText': false,
        'shuffleQuestions': false,
        'shuffleOptions': false,
        'showClock': false,
        'showPager': true,
        'theme': 'none'
    };

    $scope.goTo = function (index) {
        StudentService.prevPage = $scope.currentPage - 1;
        console.log(StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.prevPage]);
        if (index > 0 && index <= $scope.totalItems) {
            $scope.currentPage = index;
            $scope.mode = 'quiz';
        }
        if ($scope.currentPage == $scope.totalItems) {
            console.log($scope.totalItems);
            $scope.ShowNextbutton = false;
        }
        else {
            $scope.ShowNextbutton = true;
        }
        StudentService.currentPage = $scope.currentPage;
        StudentService.QuestionVariable.Status_Id = 3;
        StudentService.QuestionVariable.RemainingTime = StudentService.RemainingTime;

        var Obj = {
            StudentTestDetails: StudentService.QuestionVariable,
            StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.prevPage]
        };
        StudentService.StudentTestQuestions.StudentTestDetails = StudentService.QuestionVariable;
        StudentService.StudentTestQuestions.StudentLanguageQuestion = $scope.questions;//StudentService.StudentTestQuestions.StudentLanguageQuestion;
        /*API call to save question response*/
        window.localStorage.setItem('StudentTestQuestions', JSON.stringify(StudentService.StudentTestQuestions));
        console.log(JSON.parse(localStorage.getItem('StudentTestQuestions')));

        //StudentService.SaveTestResponse(Obj).then(function success(success) {
        //    console.log("saving data at every second is working");
        //}, function error(error) {
        //});
    };

    $scope.onSelect = function (question, option) {
        if (question.QuestionTypeId == 1) {
            question.Options.forEach(function (element, index, array) {
                if (element.Id != option.Id) {
                    element.Selected = false;
                    //question.Answered = element.Id;
                }
            });
        }
        if ($scope.config.autoMove == true && $scope.currentPage < $scope.totalItems) {
            //$scope.currentPage++;
        }
    };

    $scope.onSubmit = function () {
        var answers = [];
        $scope.questions.forEach(function (q, index) {
            answers.push({ 'QuizId': $scope.quiz.Id, 'QuestionId': q.Id, 'Answered': q.Answered });
        });
        // Post your data to the server here. answers contains the questionId and the users' answer.
        //$http.post('api/Quiz/Submit', answers).success(function (data, status) {
        //    alert(data);
        //});
        //console.log($scope.questions);
        $scope.mode = 'result';
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.questions.length / $scope.itemsPerPage);
    };

    //If you wish, you may create a separate factory or service to call loadQuiz. To keep things simple, i have kept it within controller.
    $scope.loadQuiz = function (file) {
        $http.get(file)
            .then(function (res) {
                $scope.quiz = res.data.quiz;
                $scope.config = helper.extend({}, $scope.defaultConfig, res.data.config);
                $scope.questions = res.data.questions;// $scope.config.shuffleQuestions ? helper.shuffle(res.data.questions) : res.data.questions;
                $scope.totalItems = $scope.questions.length;
                $scope.itemsPerPage = $scope.config.pageSize;
                $scope.currentPage = 1;
                StudentService.currentPage = $scope.currentPage;
                $scope.mode = 'quiz';
                if ($scope.config.shuffleOptions)
                    $scope.shuffleOptions();

                $scope.$watch('currentPage + itemsPerPage', function () {
                    var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
                        end = begin + $scope.itemsPerPage;
                    $scope.filteredQuestions = $scope.questions.slice(begin, end);
                });
            });
    };

    $scope.shuffleOptions = function () {
        $scope.questions.forEach(function (question) {
            question.Options = helper.shuffle(question.Options);
        });
    };

    //$scope.loadQuiz($scope.quizName);

    $scope.isAnswered = function (pt) {
        var answered = 'Not Answered';
        console.log($scope.questions[index].AnswerGiven, "index=", index);
        // $scope.questions[index].(function (element, index, array) {
        //angular.forEach($scope.questions, function (value, key) {
        //    value.AnswerGiven
        //});
        if (pt.AnswerGiven != null) {
            console.log(pt.AnswerGiven);
            //answered = 'Answered';
            return "answered";
        }
        else { return "unanswered"; }
        //});
        //return answered;
    };

    $scope.isCorrect = function (question) {
        var result = 'correct';
        question.Options.forEach(function (option, index, array) {
            if (helper.toBool(option.Selected) != option.IsAnswer) {
                result = 'wrong';
                return false;
            }
        });
        return result;
    };

    $scope.GetStudentTestQuestions = function (pt) {
        if (!pt) {
            swal("Error", "Please select any language", "error");
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

            StudentService.GetStudenttestDetails($scope.UserName).then(function success(success) {
                if (success.data != null) {
                    $scope.StudentTestDetails = success.data;
                    if (DTL != null) {
                        $scope.StudentTestDetails = DTL.StudentTestDetails;
                    }
                    StudentService.StudentTestDetail = $scope.StudentTestDetails;
                    StudentService.IsStarted = true;
                    $scope.ShowStartButton = true;
                    $scope.ShowQuestions = false;
                }
                else {
                    $scope.ShowStartButton = true;
                    swal("Error", "No Test Assigned Yet !", "Error");
                }
                StudentService.QuestionVariable = {
                    ProgramTestCalenderId: $scope.StudentTestDetails.ProgramTestCalenderId,
                    LangId: pt,
                    MSPIN: $scope.StudentTestDetails.MSPIN,
                    SessionID: $scope.StudentTestDetails.SessionID,
                    Day: $scope.StudentTestDetails.Day ? $scope.StudentTestDetails.Day : $scope.StudentTestDetails.DayCount,
                    UserId: $scope.User_Id,
                    ProgramId: $scope.StudentTestDetails.ProgramId,
                    TypeOfTest: $scope.StudentTestDetails.TypeOfTest
                };
                console.log(StudentService.QuestionVariable);

                StudentService.GetStudentQuestionFormatedList(StudentService.QuestionVariable).then(function success(success) {
                    //StudentService.GetStudentTestQuestions($scope.StudentTestDetails.ProgramTestCalenderId).then(function success(success) {
                    StudentService.StudentTestQuestions = success.data;
                    if (success.data == null) {
                        swal("", "Question Set Not Generated !", "error");
                    }
                    else {
                        if (StudentService.StudentTestQuestions != null) {
                            StudentService.Get_Check($scope.StudentTestDetails.MSPIN, StudentService.QuestionVariable.Day).then(function success(success) {
                                $scope.Check = success.data;
                                console.log($scope.Check == null);
                                if ($scope.Check != null) {
                                    swal("Thanks", "You have already taken this test or You have exceed the time limit", "warning");
                                }

                                else {
                                    $scope.Duration = $scope.StudentTestDetails.TestDuration;
                                    console.log("$scope.Duration", $scope.Duration);
                                    $scope.dirOptions.directiveFunction();
                                    //console.log(StudentService.StudentTestQuestions);
                                    $scope.ShowQuestions = true;
                                    $scope.ShowStartButton = false;
                                    var config = {
                                        allowBack: false,
                                        autoMove: true,
                                        showPager: false,
                                        shuffleQuestions: false
                                    };
                                    $scope.mode = 'quiz';
                                    $scope.quiz = $scope.QuestionsResponse;
                                    console.log('Initiating Shuffle');
                                    $scope.config = helperService.extend({}, $scope.defaultConfig, config);
                                    $scope.questions = StudentService.StudentTestQuestions.StudentLanguageQuestion;
                                    $scope.QuestionsResponse = StudentService.StudentTestQuestions;
                                    console.log($scope.QuestionsResponse);
                                    if (DTL != null) {
                                        //var  = window.localStorage.getItem('StudentTestQuestions');
                                        $scope.questions = DTL.StudentLanguageQuestion;
                                        $scope.QuestionsResponse = DTL;
                                        $scope.Duration = DTL.StudentTestDetails.RemainingTime;
                                        console.log("$scope.Duration", $scope.Duration);
                                    }
                                    //$scope.config.shuffleQuestions ? helperService.shuffle(StudentService.StudentTestQuestions.StudentLanguageQuestion) : StudentService.StudentTestQuestions.StudentLanguageQuestion;
                                    //$scope.questions = $scope.QuestionsResponse;
                                    console.log('Ended Shuffle');
                                    $scope.totalItems = $scope.questions.length;
                                    $scope.itemsPerPage = $scope.config.pageSize;
                                    $scope.currentPage = 1;
                                    window.localStorage.setItem('StudentTestQuestions', JSON.stringify(StudentService.StudentTestQuestions));
                                    //console.log(JSON.parse(localStorage.getItem('StudentTestQuestions')));
                                    if ($scope.config.shuffleOptions)
                                        $scope.shuffleOptions();
                                    angular.forEach(StudentService.StudentTestQuestions, function (value, key) {
                                        value.MSPIN = StudentService.StudentTestDetail.MSPIN;
                                        value.Day = StudentService.StudentTestDetail.DayCount;
                                        value.TypeOfTest = StudentService.StudentTestDetail.TypeOfTest;
                                        value.SessionID = StudentService.StudentTestDetail.SessionID;
                                        value.User_Id = $scope.User_Id;
                                    });
                                    $scope.$watch('currentPage + itemsPerPage', function () {
                                        var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
                                            end = begin + $scope.itemsPerPage;
                                        $scope.filteredQuestions = $scope.questions.slice(begin, end);
                                    });
                                }
                            }, function error(Error) {
                                //console.log("Error in loading data from EDB");
                            });
                        }
                    }
                }, function error(Error) {

                    //console.log("Error in loading data from EDB");
                });

            }, function error(Error) {
                //console.log("Error in loading data from EDB");
            });
        });
    };
    //$scope.init();

    $scope.Submit = function () {

        $scope.AnsweredAllQuestion = true;
        angular.forEach(StudentService.StudentTestQuestions.StudentLanguageQuestion, function (value, key) {
            //value.
            if (!value.AnswerGiven) {
                $scope.AnsweredAllQuestion = false;
            }
        });
        if ($scope.AnsweredAllQuestion == false) {
            console.log('./Feedback.html?ProgramId=' + StudentService.StudentTestDetail.ProgramId + '&SessionID=' + StudentService.StudentTestDetail.SessionID + '&MSPIN=' + StudentService.StudentTestDetail.MSPIN);
            swal({
                title: 'Are you sure?',
                text: "You have not answered all questions!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then((result) => {
                StudentService.QuestionVariable.Status_Id = 1;
                //var Obj = {
                //    StudentTestDetails: StudentService.QuestionVariable,
                //    StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion
                //};
                var Obj = {
                    StudentTestDetails: StudentService.QuestionVariable,
                    //StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.currentPage - 1]
                    StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion
                };

                console.log(Obj);
                StudentService.SaveTestResponseComplete(Obj).then(function success(success) {
                    if (success.data.indexOf('Success') != -1) {

                        swal({
                            title: 'Thanks',
                            text: "You have submitted test successfully",
                            type: 'success',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            window.localStorage.removeItem('StudentTestQuestions');
                            $cookies.remove('LogKey');
                            $cookies.get('LogKey');
                            if (StudentService.StudentTestDetail.Duration == StudentService.StudentTestDetail.DayCount) {
                                $scope.ProgramId = StudentService.StudentTestDetail.ProgramId;
                                $scope.SessionID = StudentService.StudentTestDetail.SessionID;
                                $scope.MSPIN = StudentService.StudentTestDetail.MSPIN;
                                console.log('./Feedback.html?ProgramId=' + $scope.ProgramId + '&SessionID=' + $scope.SessionID + '&MSPIN=' + $scope.MSPIN);
                                window.location = './Feedback.html?ProgramId=' + $scope.ProgramId + '&SessionID=' + $scope.SessionID + '&MSPIN=' + $scope.MSPIN;
                            }
                            else {
                                window.location = './Stlogin.html';
                            }
                        });
                    }
                    else {
                        swal("", success.data, "error");
                    }
                }, function error(Error) {
                    //console.log("Error in loading data from EDB");
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
                StudentService.QuestionVariable.Status_Id = 1;
                //var Obj = {
                //    StudentTestDetails: StudentService.QuestionVariable,
                //    StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion
                //};
                console.log("StudentService.currentPage", StudentService.currentPage);
                console.log(StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.currentPage - 1]);
                var Obj = {
                    StudentTestDetails: StudentService.QuestionVariable,
                    //StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.currentPage - 1]
                    StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion
                };
                StudentService.SaveTestResponseComplete(Obj).then(function success(success) {
                    if (success.data.indexOf('Success') != -1) {
                        swal("", success.data, "success");
                        window.localStorage.removeItem('StudentTestQuestions');
                        window.location = './Stlogin.html';
                    }
                    else {
                        swal("", success.data, "error");
                    }

                }, function error(Error) {
                    //console.log("Error in loading data from EDB");
                });
            });
        }

    };
    //$scope.init();

    $scope.blink = true;
    $scope.fontSize = {};
    $scope.timerColor = {};
    $scope.deadlineMillis = 0;
    $scope.timerRunning = false;

    $scope.timeOver = function () {
        $scope.timerColor.color = $scope.blink ? 'blinking-end' : 'end';
    };

    $scope.changeSize = function (value) {
        $scope.fontSize = { 'font-size': value + 'px' };
    };

    $scope.startTimer = function (deadline) {
        $scope.$broadcast('timer-start');
        $scope.timerRunning = true;
        $scope.deadlineMillis += deadline * 1000 * 60;
    };

    $scope.stopTimer = function () {
        $scope.$broadcast('timer-stop');
        $scope.timerColor = {};
        $scope.deadlineMillis = 500;
        $scope.timerRunning = false;
    };

    $scope.$on('timer-tick', function (event, data) {
        if ($scope.timerRunning && data.millis >= $scope.deadlineMillis) {
            $scope.$apply($scope.timeOver);
        }
    });

    $scope.$on('$routeChangeStart', function (scope, next, current) {
        if (next.$$route.controller != "Your Controller Name") {
            alert("gfdgfd");
            $("#yourModel").show();
        }
    });


    //$scope.$watch('StudentService.Seconds', ['StudentService', function (newValue, oldValue) {
    //  console.log("$watch is working")

    //}]);
});

app.filter('counterValue', function () {
    return function (value) {
        value = parseInt(value);
        if (!isNaN(value) && value >= 0 && value < 10)
            return "0" + valueInt;
        return "";
    };
});
app.directive('timer', function ($timeout, $http, $compile, $cookies, StudentService) {
    return {

        restrict: 'E',
        scope: {
            interval: '=', //don't need to write word again, if property name matches HTML attribute name
            startTimeAttr: '=?startTime', //a question mark makes it optional
            countdownAttr: '=?countdown',
            options: '='//what unit?
        },
        template: '<div><p>' +
            //    Time ends in : {{ hours } } hour < span > s</span >, ' +
            //'
            '<p>{{ minutes }}:' +
            '{{ seconds }}  ' +
            //'<span data-ng-if="millis">, milliseconds: {{millis}}</span>
            '</p>' + '</p>',

        //'<p>Interval ID: {{ intervalId  }}<br>' +
        //'Start Time: {{ startTime | date:"mediumTime" }}<br>' +
        //'Stopped Time: {{ stoppedTime || "Not stopped" }}</p>' +
        //'</p>' +
        //'<button data-ng-click="resume()" data-ng-disabled="!stoppedTime">Resume</button>' +
        //'<button data-ng-click="stop()" data-ng-disabled="stoppedTime">Stop</button>'


        link: function (scope, elem, attrs) {

            //Properties
            scope.startTime = scope.startTimeAttr;//? new Date(scope.startTimeAttr) : new Date();
            var countdown = (scope.countdownAttr && parseInt(scope.countdownAttr, 10) > 0) ? parseInt(scope.countdownAttr, 10) : 60; //defaults to 60 seconds
            //console.log(StudentService.IsStarted);
            function tick() {

                //How many milliseconds have passed: current time - start time
                scope.millis = new Date() - scope.startTime;
                //var TimerStoped = false;
                if (countdown > 0) {
                    //if (scope.minutes < 10) { scope.minutes = '0' + scope.minutes }
                    scope.millis = countdown * 1000;
                    countdown--;
                    //StudentService.Minutes = scope.minutes;
                    //StudentService.Seconds = scope.seconds;
                    /* saving details at every second*/
                    //StudentService.QuestionVariable.Status_Id = 3;
                    StudentService.RemainingTime = countdown;


                }
                else if (countdown <= 0) {
                    if (StudentService.TimerStoped == false) {
                        scope.stop();
                        StudentService.TimerStoped = true;
                    }
                    //console.log('Your time is up!');
                }
                else if (seconds < 10) {


                }

                //console.log('scope.millis', scope.millis);
                scope.seconds = Math.floor((scope.millis / 1000) % 60);
                if (scope.seconds < 10) {
                    scope.seconds = '0' + scope.seconds;
                }
                //else {
                //    scope.seconds = Math.floor((scope.millis / 1000) % 60);
                //}
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
                    // //console.log('function called');
                    start();
                }
            });
            scope.stop = function () {
                StudentService.QuestionVariable.Status_Id = 1;
                var Obj = {
                    StudentTestDetails: StudentService.QuestionVariable,
                    //StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion[StudentService.currentPage - 1]
                    StudentLanguageQuestion: StudentService.StudentTestQuestions.StudentLanguageQuestion
                };
                //$http.post(StudentService.AppUrl + '/Test/SaveTestResponse/', Obj).then(function (results) {
                //console.log(results);
                // window.location = './Stlogin.html';
                //});
                console.log(Obj);
                StudentService.SaveTestResponseComplete(Obj).then(function success(success) {
                    if (success.data.indexOf('Success') != -1) {

                        swal({
                            title: 'Thanks',
                            text: "You have submitted test successfully",
                            type: 'success',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            console.log(StudentService.StudentTestDetail.Duration == StudentService.StudentTestDetail.DayCount);
                            $cookies.remove('LogKey');
                            window.localStorage.removeItem('StudentTestQuestions');
                            if (StudentService.StudentTestDetail.Duration == StudentService.StudentTestDetail.DayCount) {
                                var ProgramId = StudentService.StudentTestDetail.ProgramId;
                                var SessionID = StudentService.StudentTestDetail.SessionID;
                                var MSPIN = StudentService.StudentTestDetail.MSPIN;
                                console.log('./Feedback.html?ProgramId=' + ProgramId + '&SessionID=' + SessionID + '&MSPIN=' + MSPIN);
                                window.location = './Feedback.html?ProgramId=' + ProgramId + '&SessionID=' + SessionID + '&MSPIN=' + MSPIN;
                            }
                            else {
                                window.location = './Stlogin.html';
                            }
                        });
                    }
                    else {
                        swal("", success.data, "error");
                    }
                }, function error(Error) {
                    //console.log("Error in loading data from EDB");
                });
            }

            //if not used anywhere, make it a regular function so you don't pollute the scope
            function start() {
                //resetInterval();
                scope.intervalId = setInterval(tick, scope.interval);
            }

            scope.resume = function () {
                scope.stoppedTime = null;
                scope.startTime = new Date() - (scope.stoppedTime - scope.startTime);
                start();
            }
            if (StudentService.IsStarted == true) {
                start(); //start timer automatically
            }

            scope.$watch('StudentService.IsStarted', function (oldValue, newValue) {
                //console.log(oldValue);

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
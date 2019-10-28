var app = angular.module('Feedback', ['ngLoadingSpinner']);

app.service('FeedbackService', function ($http,$location) {
    this.AppUrl = "";

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.GetQuestionsList = function (ProgramId, SessionID, MSPIN) {
        return $http.get(this.AppUrl + 'Feedback/GetFeedbackQuestion/?ProgramId=' + ProgramId + '&SessionID=' + SessionID + '&MSPIN=' + MSPIN);
    };
    this.CaptureFeedback = function (Obj) {
        return $http.post(this.AppUrl + 'Feedback/CaptureFeedback/', Obj);
    };
});

app.controller('FeedbackController', function (FeedbackService, $location,$scope,$window) {

    console.log('zhcbzcnzxnbc');

    $scope.ProgranId = $location.absUrl().substring($location.absUrl().indexOf('=') + 1, $location.absUrl().indexOf('&'));
    $scope.SessionID = $location.absUrl().substring($location.absUrl().indexOf('&') + 10, $location.absUrl().indexOf('MSPIN')-1);// $location.absUrl().length);
    $scope.MSPIN = $location.absUrl().substring($location.absUrl().indexOf('MSPIN') + 5, $location.absUrl().length);
    console.log($scope.ProgranId);
    console.log($scope.SessionID);
    console.log($scope.MSPIN);
    $scope.init = function () {
        FeedbackService.GetQuestionsList($scope.ProgranId, $scope.SessionID, $scope.MSPIN).then(function (success) { 
            $scope.QuestionsList = success.data;
            }, function (error) { });
    };
    $scope.Submit = function () {
        FeedbackService.CaptureFeedback($scope.QuestionsList).then(function (success) {

            if (success.data.indexOf('Success') != -1) {
                console.log(success.data.indexOf('Success') != -1);
                //window.location.absUrl = "http://localhost:4756/Stlogin.html";
                $window.location.href = '/Stlogin.html';
            }
            else {

            }
        }, function (error) { });
    };
    $scope.init();
});
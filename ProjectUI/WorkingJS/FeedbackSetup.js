var app = angular.module('FeedbackModule', []);
app.service('FeedbackService', function ($http, $location) {
    this.AppUrl = "";
    
    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }

    this.UploadFeedback = function (fd) {
        return $http.post(this.AppUrl+"/Feedback/UploadExcel", fd, {

            ransformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
    this.GetPrgramLanguage = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetLanguage?id=' + id);
    };
});


app.controller("FeedbackController", function ($scope, $rootScope, AttendanceReportDMSService, FeedbackService) {
    $scope.init = function () {
        AttendanceReportDMSService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
            console.log("$scope.ReportFilter", $scope.ReportFilter);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetProgrmaLanuages = function (ProgramId) {
        FeedbackService.GetPrgramLanguage(ProgramId).then(function success(data) {
            $scope.LanuagesList = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.UploadFeedbackFile = function () {
        console.log($scope.ReportInput);
        console.log($rootScope.session.UserName);
        if (($scope.ReportInput === null) || ($scope.ReportInput === undefined)) {
            swal("Error", "Please Select Program", 'error');
            return false;
        }
        if(!$scope.ReportInput.LanguageMaster_Id)
        {
            swal('','Please Select Language for which you want uoload the questions','error')
        }
        else {
            var obj = {
                UserId: $rootScope.session.User_Id,
                ProgramId: $scope.ReportInput.ProgramId,
                LanguageMaster_Id: $scope.ReportInput.LanguageMaster_Id
            };
            console.log(obj);
            if (($scope.File === null) || ($scope.File == undefined)) {
                swal("Error", "Please Upload Feedback File", 'error');
                return false;
            }
            else {
                var fd = new FormData();
                fd.append('file', $scope.File);
                fd.append('data', angular.toJson(obj));
                console.log($scope.File);
                console.log(fd);
                FeedbackService.UploadFeedback(fd).then(function (success) {
                    console.log(success.data);
                    swal("Success", success.data, 'success');
                }, function (error) {
                    console.log(error);
                    swal("Error", "Please upload correct file", 'error');
                });
            }

        }
    }

    $scope.init();
}); 
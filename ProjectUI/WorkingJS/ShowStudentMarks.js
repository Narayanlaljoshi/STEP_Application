var app = angular.module('ShowStudentMarksModule', []);

app.service('ShowStudentMarksService', function ($http, $location) {
    this.AppUrl = "";
    this.MSPin = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }
    this.GetMarksReport = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetMarksReport', data);
    };
    this.GetReportFilter = function () {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetReportFilter', {});
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    };
    this.GetStudentPostTestScores = function (MSpin) {
        return $http.get(this.AppUrl + '/Test/GetStudentPostTestScores/?MSpin=' + MSpin);
    }; 
    this.GetMarksReportForFaculty = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetMarksReportForFaculty', data);
    };
});


app.controller('ShowStudentMarksController', function ($scope, $http, $location, ShowStudentMarksService, $uibModal, $rootScope, $uibModalInstance, InitFactory, FacultyAgencyLevelService)
{

    $scope.init = function () {
        console.log('inside init');
        var ReportInput = {
            Agency_Id: null,
            ProgramId: null,
            AgencyCode: null,
            Faculty_Id: null,
            SessionID: null,
            StartDate: null,
            EndDate: null
        };
        ShowStudentMarksService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
            console.log("$scope.ReportFilter", $scope.ReportFilter);
            return ShowStudentMarksService.GetMarksReport(FacultyAgencyLevelService.SelectedSessionIds);
        }).then(function success(data) {
            $scope.MarksReport = data.data;
            return ShowStudentMarksService.GetMarksReportForFaculty(FacultyAgencyLevelService.SelectedSessionIds);
        }).then(function success(data) {
            $scope.MarksReportDayWise= data.data;
            console.log("GetMarksReportForFaculty", data.data);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };
    $scope.GetFacultyList = function (Agency_Id) {
        console.log("ng-Change Working", Agency_Id);
        ShowStudentMarksService.GetFacultyList(Agency_Id).then(function success(success) {

            $scope.ReportFilter.FacultyList = success.data;

        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };
    $scope.ResetFilters = function (ReportInput) {
        $scope.ReportInput = null;
        $scope.init();
    };
    $scope.GetReport = function (ReportInput) {
        console.log('inside init');
        //var ReportInput = {
        //    Agency_Id: null,
        //    ProgramId: null,
        //    AgencyCode: null,
        //    Faculty_Id: null,
        //    SessionID: null,
        //    StartDate: null,
        //    EndDate: null

        //};
        ShowStudentMarksService.GetMarksReport(ReportInput).then(function success(data) {
            $scope.GetAttendanceReport = data.data;
            console.log("$scope.GetAttendanceReport", $scope.GetAttendanceReport);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

    $scope.ShowPostTestsScore = function (pt) {
        ShowStudentMarksService.MSPin = pt.MSPIN;
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

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.exportExcel = function () {

        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        var table = document.getElementById("searchResult");
        var filters = $('.ng-table-filters').remove();

        //pass html table id (e.g.-'Sp_ContractorReport' in this)

        var ctx = { worksheet: name || 'Colour Details', table: DayWiseReport.innerHTML };
        $('.ng-table-sort-header').after(filters);
        var url = uri + base64(format(template, ctx));
        var a = document.createElement('a');
        a.href = url;

        //Name the Excel like 'GroupWiseReport.xls' in this

        a.download = 'DayWiseReport.xls';
        a.click();

    }
    $scope.init();
});

app.controller('TestRecordsController', function ($scope, $rootScope, ShowStudentMarksService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.MSPin = ShowStudentMarksService.MSPin;
    $scope.init = function () {
        ShowStudentMarksService.GetStudentPostTestScores($scope.MSPin).then(function success(retdata) {

            $scope.StudentPostTestScores = retdata.data;
            console.log($scope.StudentPostTestScores);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };

    $scope.init();


});

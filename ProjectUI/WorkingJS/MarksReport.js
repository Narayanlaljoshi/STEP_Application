var app = angular.module('MarksReportModule', []);

app.service('MarksReportService', function ($http, $location) {
    this.AppUrl = "";
    this.MSPin = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetMarksReport = function (data) {
        console.log(data);
        return $http.post(this.AppUrl + '/Reports/GetMarksReportForAdmin', data);
    };
    this.GetReportFilter = function () {
        return $http.post(this.AppUrl + '/Reports/GetReportFilter', {});
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    };
    this.GetStudentPostTestScores = function (MSpin) {
        return $http.get(this.AppUrl + '/Test/GetStudentPostTestScores/?MSpin=' + MSpin);
    };
});


app.controller('MarksReportController', function ($scope, $http, $location, MarksReportService, $uibModal, $rootScope, InitFactory) {
    $scope.ReportInput = {};
    $scope.currentPage = 1;
    $scope.ShowReport = false;
    $scope.init = function () {
       
        console.log('inside init');
        if ($rootScope.session.RoleName == 'HOAdmin') {
            console.log($rootScope.session.RoleName);
            $scope.ReportInput = {
                Agency_Id: null,
                ProgramId: null,
                AgencyCode: null,
                Faculty_Id: null,
                SessionID: null,
                StartDate: null,
                EndDate: null
            };
        }
        else if ($rootScope.session.RoleName == 'AgencyAdmin') {
            console.log($rootScope.session.RoleName);
            $scope.ReportInput = {
                Agency_Id: $rootScope.session.Agency_Id,
                ProgramId: null,
                AgencyCode: null,
                Faculty_Id: null,
                SessionID: null,
                StartDate: null,
                EndDate: null

            };

        }
        else if ($rootScope.session.RoleName == 'Faculty') {

            console.log($rootScope.session.RoleName);
            $scope.ReportInput = {
                Agency_Id: $rootScope.session.Agency_Id,
                ProgramId: null,
                AgencyCode: null,
                Faculty_Id: null,
                SessionID: null,
                StartDate: null,
                EndDate: null,
                FacultyCode: $rootScope.session.UserName

            };

        }
        
        MarksReportService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
            console.log("$scope.ReportFilter", $scope.ReportFilter);
        
        }, function error(data) {
            console.log("Error in loading data from EDB");
            });
        if ($rootScope.session.RoleName == 'HOAdmin') {
            console.log($rootScope.session.RoleName);
        }
        else if ($rootScope.session.RoleName == 'AgencyAdmin') {
            //$scope.GetFacultyList($rootScope.session.Agency_Id);
            //$scope.ReportFilter.FacultyList = [];
            MarksReportService.GetFacultyList($rootScope.session.Agency_Id).then(function success(success) {
                $scope.ReportFilter.FacultyList = success.data;
            }, function error(Error) {
                console.log("Error in loading data from EDB");
            });
            console.log($rootScope.session.RoleName);
        }
        else if ($rootScope.session.RoleName == 'Faculty') {
            //$scope.ReportInput.Faculty_Id
            console.log($rootScope.session.RoleName);
        }
    };
    $scope.GetFacultyList = function (Agency_Id) {
        console.log("ng-Change Working", Agency_Id);
        MarksReportService.GetFacultyList(Agency_Id).then(function success(success) {
            $scope.ReportFilter.FacultyList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.ResetFilters = function (ReportInput) {
        $scope.ReportInput = null;
        $scope.ShowReport = true;
        $scope.init();
    };
    $scope.GetReport = function (ReportInput) {
        $scope.ReportInput = ReportInput;
        if (!$scope.ReportInput.StartDate)
        {
            swal("", "Please Select Start Date", "error");
            return false;
        }
        if ($rootScope.session.RoleName == 'HOAdmin') {
            console.log($rootScope.session.RoleName);
            
        }
        else if ($rootScope.session.RoleName == 'AgencyAdmin') {
            console.log($rootScope.session.RoleName);            
            $scope.ReportInput.Agency_Id = $rootScope.session.Agency_Id;               
        }
        else if ($rootScope.session.RoleName == 'Faculty') {

            console.log($rootScope.session.RoleName);
            $scope.ReportInput.FacultyCode= $rootScope.session.UserName;
        }
       
        MarksReportService.GetMarksReport($scope.ReportInput).then(function success(data) {
            $scope.MarksReport = data.data;
            $scope.ShowReport = true;
            console.log("$scope.GetAttendanceReport", $scope.GetAttendanceReport);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

    $scope.ShowPostTestsScore = function (pt) {
        MarksReportService.MSPin = pt.MSPIN;
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

    };

    $scope.init();
});


var app = angular.module('MarksReportAsPerDMSModule', []);

app.service('MarksReportAsPerDMSService', function ($http, $location) {
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
        return $http.post(this.AppUrl + '/Reports/GetMarksReportAsperDMS', data);
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
});


app.controller('MarksReportAsPerDMSController', function ($scope, $http, $filter, $location, MarksReportAsPerDMSService, AttendanceReportDMSService, $uibModal, $rootScope, InitFactory) {

    $scope.ShowReport = false;
    $scope.ReportInput = {
        Agency_Id: null,
        ProgramId: null,
        AgencyCode: null,
        Faculty_Id: null,
        SessionID: null,
        StartDate: null,
        EndDate: null

    };
    $scope.init = function () {
       
        console.log('inside init');
        $scope.ReportInput = {
            Agency_Id: null,
            ProgramId: null,
            AgencyCode: null,
            Faculty_Id: null,
            SessionID: null,
            StartDate: null,
            EndDate: null

        };

        
        
        MarksReportAsPerDMSService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
            console.log("$scope.ReportFilter", $scope.ReportFilter);
        //    return MarksReportAsPerDMSService.GetMarksReport(ReportInput);
        //}).then(function success(data) {
        //    $scope.MarksReport = data.data;
        //    console.log("$scope.MarksReport", $scope.MarksReport);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetFacultyList = function (Agency_Id) {
        console.log("ng-Change Working", Agency_Id);
        MarksReportAsPerDMSService.GetFacultyList(Agency_Id).then(function success(success) {

            $scope.ReportFilter.FacultyList = success.data;

        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };

    $scope.GetSessionId = function (ProgramId) {
        console.log(ProgramId);
        AttendanceReportDMSService.GetSessionList(ProgramId).then(function success(success) {
            console.log(success);
            $scope.SessionList = success.data;
        });
    };
    $scope.ResetFilters = function (ReportInput) {
        $scope.ReportInput = null;
        $scope.SessionList = null;
        $scope.init();
    };
    $scope.GetReport = function (ReportInput) {
        //$scope.MarksReport = [];
        console.log('inside init');
        //if (!$scope.ReportInput.EndDate) {
        //    swal("", "Select End date", "error");
        //    return false;
        //}
        //var ReportInput = {
        //    Agency_Id: null,
        //    ProgramId: null,
        //    AgencyCode: null,
        //    Faculty_Id: null,
        //    SessionID: null,
        //    StartDate: null,
        //    EndDate: null
        //};
        MarksReportAsPerDMSService.GetMarksReport(ReportInput).then(function success(data) {
            $scope.MarksReport = data.data;

            angular.forEach($scope.MarksReport, function (value, key) {
                value.StartDate = $filter('date')(value.StartDate, 'dd-MMM-yy');
                if (value.IsPresentInPreTest == 0)
                { value.PreTest_MarksObtained = '0' }
                if (value.IsPresentInPostTest == 0)
                { value.PostTest_MarksObtained = '0' }
            });
            $scope.data = {};
            $scope.data.exportFilename = 'ScoreSheet.csv';
            $scope.data.displayLabel = 'Export';
            $scope.data.myHeaderData = {

                FacultyCode: 'Faculty',
                ProgramCode: 'Program',
                SessionID: 'Session',
                StartDate: 'Start Date',
                MSPIN: 'MSPIN',
                Name:'Name',
                PreTest_MarksObtained: 'Pre Test Score',
                PostTest_MarksObtained: 'Post Test Score'
            };
            $scope.data.myInputArray = $scope.MarksReport;

            $scope.ShowReport = true;
            console.log("$scope.GetAttendanceReport", $scope.GetAttendanceReport);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

  
    $scope.exportExcel = function () {

        //var uri = 'data:application/vnd.ms-excel;base64,'
        //    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        //    , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        //    , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        //var table = document.getElementById("searchResult");
        //var filters = $('.ng-table-filters').remove();

        ////pass html table id (e.g.-'Sp_ContractorReport' in this)

        //var ctx = { worksheet: name || 'Colour Details', table: MarksReportDMS.innerHTML };
        //$('.ng-table-sort-header').after(filters);
        //var url = uri + base64(format(template, ctx));
        //var a = document.createElement('a');
        //a.href = url;

        ////Name the Excel like 'GroupWiseReport.xls' in this

        //a.download = ' MarksReportDMS.xls';
        //a.click();
        alasql.fn.datetime = function (dateStr) {

            var date = $filter('date')(dateStr, 'dd-MMM-yyyy');
            return date;
        };
        alasql.promise('SELECT Co_id,FacultyCode,ProgramCode,SessionID,datetime(StartDate) StartDate,MSPIN,Name,PreTest_MarksObtained PreTest,PostTest_MarksObtained PostTest INTO CSV("MarksReportDMS.csv",{headers:true}) FROM ? ', [$scope.MarksReport]).then(function (data) {
            console.log('Data saved');
        }).catch(function (err) {
            console.log('Error:', err);
        });

    }

    $scope.init();
});


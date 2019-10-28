var app = angular.module('AttendanceReportDMSModule', ['tld.csvDownload']);

app.service('AttendanceReportDMSService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') !== -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetAttendanceReport = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetAttendanceReport', data);
    };
    this.GetReportFilter = function () {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetReportFilter', {});
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    };

    this.GetSessionList = function (ProgramId) {
        return $http.get(this.AppUrl + '/ManageSession/GetSessionListByProgramId?ProgramId=' + ProgramId);
    };
});

app.factory('InitFactory', function (AttendanceReportDMSService) {
    return {
        init: function () {
            AttendanceReportDMSService.GetAgencyList().then(function success(data) {
                AttendanceReportDMSService.AgencyList = data.data;
                console.log("Get Region List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});

app.controller('AttendanceReportDMSController', function ($scope, $http, $filter, $location, AttendanceReportDMSService, $uibModal, $rootScope, InitFactory) {
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
        AttendanceReportDMSService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
            console.log("$scope.ReportFilter", $scope.ReportFilter);
        //    return AttendanceReportDMSService.GetAttendanceReport(ReportInput);
        //}).then(function success(data) {
        //    $scope.AttendanceReportData = data.data;
        //    console.log("$scope.AttendanceReportData", $scope.AttendanceReportData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };
    $scope.GetFacultyList = function (Agency_Id) {
        console.log("ng-Change Working", Agency_Id);
        AttendanceReportDMSService.GetFacultyList(Agency_Id).then(function success(success) {

            $scope.ReportFilter.FacultyList = success.data;

        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };
    $scope.ResetFilters = function (ReportInput) {
        $scope.ReportInput = null;
        $scope.SessionList = null;
        $scope.init();
    };

    $scope.GetSessionId = function (ProgramId) {
        console.log(ProgramId);
        AttendanceReportDMSService.GetSessionList(ProgramId).then(function success(success) {
            console.log(success);
            $scope.SessionList = success.data;
        });
    };

    $scope.GetReport = function (ReportInput) {
        var PrgDtl = $filter('filter')($scope.ReportFilter.ProgramList, { 'ProgramId': ReportInput.ProgramId });
        var Data = $filter('filter')($scope.SessionList, { 'SessionID': ReportInput.SessionID });
        console.log('inside init', PrgDtl);
        //if (!$scope.ReportInput.EndDate) {
        //    swal("", "Select end date", "error");
        //    return false;
        ////}
        //var ReportInput = {
        //    Agency_Id: null,
        //    ProgramId: null,
        //    AgencyCode: null,
        //    Faculty_Id: null,
        //    SessionID: null,
        ReportInput.ProgramType_Id = 1;//PrgDtl[0].ProgramType_Id;
        //ReportInput.StartDate = Data[0].StartDate;
        //ReportInput.EndDate = Data[0].EndDate;
        //};
        console.log(ReportInput);
        AttendanceReportDMSService.GetAttendanceReport(ReportInput).then(function success(data) {
            $scope.AttendanceReportData = data.data;
            angular.forEach($scope.AttendanceReportData, function (value, key) {
                value.StartDate = $filter('date')(value.StartDate, 'dd-MMM-yy');
            });
            $scope.data = {};
            $scope.data.exportFilename = 'AttendanceReportDMS.csv';
            $scope.data.displayLabel = 'Export';
            $scope.data.myHeaderData = {
                FacultyCode: 'Faculty',
                ProgramCode: 'Program',
                SessionID: 'Session',
                StartDate: 'Start Date',
                MSPIN: 'MSPIN',
                Day: 'Day',
                P_A: 'Attendance'
            };
            $scope.data.myInputArray = $scope.AttendanceReportData;
            $scope.ShowReport = true;
            console.log("$scope.GetAttendanceReport", $scope.AttendanceReportData);
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

        //var ctx = { worksheet: name || 'Colour Details', table: AttendanceReportDMS.innerHTML };
        //$('.ng-table-sort-header').after(filters);
        //var url = uri + base64(format(template, ctx));
        //var a = document.createElement('a');
        //a.href = url;

        ////Name the Excel like 'GroupWiseReport.xls' in this

        //a.download = ' AttendanceReportDMS.xls';
        //a.click();
        alasql.fn.datetime = function (dateStr) {

            var date = $filter('date')(dateStr, 'dd-MMM-yyyy');
            return date;
        };
        alasql.promise('SELECT Co_id,FacultyCode,ProgramCode,SessionID,datetime(StartDate) StartDate,MSPIN,Day,P_A INTO csv("AttendanceReportDMS.csv",{headers:true}) FROM ? ', [$scope.AttendanceReportData]).then(function (data) {
            console.log('Data saved');
        }).catch(function (err) {
            console.log('Error:', err);
        });

    };

    $scope.init();
});

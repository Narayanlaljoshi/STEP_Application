var app = angular.module('AttendanceReportModule', []);

app.service('AttendanceReportService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetAttendanceReport = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetMarksReportForAdmin', data);
    };
    this.GetReportFilter = function () {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetReportFilter', {});
    };
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    }; 
    this.GetAgencyList = function () {
        return $http.get(this.AppUrl + '/RtcMaster/GetAgencyList', {});
    }; 
    this.GetProgramList_From_To_Date = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetProgramList_From_To_Date', Obj);
    }; 
    this.GetSessionList_ProgramWise = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetSessionList_ProgramWise', Obj);
    };
});

app.factory('InitFactory', function (AttendanceReportService) {
    return {
        init: function () {
            AttendanceReportService.GetAgencyList().then(function success(data) {
                AttendanceReportService.AgencyList = data.data;
                console.log("Get Region List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
});

app.controller('AttendanceReportController', function ($scope, $http, $location, AttendanceReportService, $uibModal, $rootScope, InitFactory) {
    $scope.ReportInput = {};
    $scope.currentPage = 1;
    $scope.ShowReport = false;
    $scope.init = function () {
        console.log('inside init');
        $scope.ShowReport = false;
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
        AttendanceReportService.GetAgencyList().then(function success(data) {
            $scope.AgencyList = data.data;
            console.log("Get Region List", data);


        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        AttendanceReportService.GetReportFilter().then(function success(data) {
            $scope.ReportFilter = data.data;
           console.log("$scope.ReportFilter", $scope.ReportFilter);
            return AttendanceReportService.GetAgencyList();
        }).then(function success(data) {
             AttendanceReportService.AgencyList = data.data;
            console.log("Get Region List", data);
        //    console.log("$scope.AttendanceReportData", $scope.AttendanceReportData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

    $scope.GetProgramList_From_To_Date = function () {

        $scope.ReportInput.Faculty_Id = null;
        $scope.ReportInput.SessionID = null;

        AttendanceReportService.GetProgramList_From_To_Date($scope.ReportInput).then(function success(success) {
            $scope.ReportFilter.ProgramList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.GetSessionList_ProgramWise = function () {
        AttendanceReportService.GetSessionList_ProgramWise($scope.ReportInput).then(function success(success) {
            $scope.ReportFilter.SessionList = success.data;
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.GetFacultyList = function (Agency_Id) {

        $scope.ReportInputProgramId = null;
        //$scope.ReportInputAgencyCode = null;
        $scope.ReportInput.Faculty_Id = null;
        $scope.ReportInput.SessionID = null;

        AttendanceReportService.GetFacultyList(Agency_Id).then(function success(success) {
            $scope.GetProgramList_From_To_Date();
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
        $scope.AttendanceReportData = [];
        $scope.ReportInput = ReportInput;
        if (!$scope.ReportInput.StartDate) {
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
            $scope.ReportInput.FacultyCode = $rootScope.session.UserName;
        }
        AttendanceReportService.GetAttendanceReport(ReportInput).then(function success(data) {
            $scope.AttendanceReportData = data.data;
            $scope.ShowReport = true;
            console.log($scope.AttendanceReportData);
            console.log("$scope.GetAttendanceReport", $scope.AttendanceReportData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

    };

    $scope.exportExcel = function () {

        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        var table = document.getElementById("searchResult");
        var filters = $('.ng-table-filters').remove();

        //pass html table id (e.g.-'Sp_ContractorReport' in this)

        var ctx = { worksheet: name || 'Colour Details', table: AttendanceDayWiseReport.innerHTML };
        $('.ng-table-sort-header').after(filters);
        var url = uri + base64(format(template, ctx));
        var a = document.createElement('a');
        a.href = url;

        //Name the Excel like 'GroupWiseReport.xls' in this

        a.download = 'AttendanceDayWiseReport.xls';
        a.click();

    };

    $scope.init();
});

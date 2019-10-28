var app = angular.module('MarksReport_VendorModule', []);

app.service('MarksReport_VendorService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    //this.GetAttendanceReport = function (data) {
    //    //       console.log("yahiooooooooooo");
    //    return $http.post(this.AppUrl + '/Reports/GetMarksReportForAdmin', data);
    //};
    this.GetActiveTrainerForVendor = function (UserName) {
        //       console.log("yahiooooooooooo");
        return $http.get(this.AppUrl + '/Vendor/GetActiveTrainerForVendor?UserName=' + UserName);
    };
    this.GetProgramList = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetProgramList', Obj);
    };
    this.GetVendorMarksReport = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetVendorMarksReport', Obj);
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

app.controller('MarksReport_VendorController', function ($scope, $filter, $http, $location, MarksReport_VendorService, $uibModal, $rootScope, InitFactory) {
    $scope.ReportInput = {
        ProgramId: null,
        Trainer_Id: null,
        SessionID: null,
        StartDate: null,
        EndDate: null,
        ManagerID: $rootScope.session.UserName
    };
    $scope.currentPage = 1;
    $scope.ShowReport = false;
    $scope.init = function () {
        MarksReport_VendorService.GetActiveTrainerForVendor($rootScope.session.UserName).then(function success(data) {
            $scope.TrainerList = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.ResetFilters = function (ReportInput) {
        $scope.ReportInput = {
            ProgramId: null,
            Trainer_Id: null,
            SessionID: null,
            StartDate: null,
            EndDate: null,
            ManagerID: $rootScope.session.UserName
        };
        $scope.init();
    };
    $scope.GetVendorMarksReport = function (ReportInput) {
        var index = $scope.RawProgramList.findIndex(x => x.ProgramId === ReportInput.ProgramId)
        ReportInput.ProgramType_Id = $scope.RawProgramList.w
        if (!$scope.ReportInput.ProgramId && !$scope.ReportInput.Trainer_Id && !$scope.ReportInput.SessionID && !$scope.ReportInput.StartDate && !$scope.ReportInput.EndDate) {
            swal("Warning", "Please Select Start Date and End Date", "warning");
        }
        else {
            MarksReport_VendorService.GetVendorMarksReport(ReportInput).then(function success(data) {
                $scope.MarksReport = data.data;
                $scope.ShowReport = true;

            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
    $scope.ProgramList = [];
    $scope.SessionIDList = [];
    $scope.GetProgramList = function (ReportInput) {
        MarksReport_VendorService.GetProgramList(ReportInput).then(function success(data) {
            if (ReportInput) {
                $scope.RawProgramList = data.data;
                angular.forEach($scope.RawProgramList, function (Key, Value) {
                    if ($scope.ProgramList.indexOf(Key) === -1) {
                        $scope.ProgramList.push(Key);
                        //console.log($scope.ProgramList);
                    }
                });
                $scope.ReportInput.ProgramId = null;
                console.log($scope.ReportInput);
            }
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.GetSessionIDList = function (ProgramID) {
        console.log(ProgramID);
        $scope.SessionIDList = [];
        angular.forEach($scope.RawProgramList, function (key, value) {
            if (key.ProgramID === ProgramID) {
                console.log(key.ProgramID === ProgramID);
                $scope.SessionIDList.push({
                    SessionID: key.SessionID
                });
            }

        });
        //$scope.SessionIDList = $filter('filter')($scope.ProgramList, { 'ProgramName': ProgramId });//$scope.ProgramList.find(x => x.ProgramId === ReportInput.ProgramId);

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

        a.download = 'MarksDayWiseReport_AS_ON_' + $filter('date')(new Date(), "MMM dd yyyy") + '.xls';
        a.click();

    };

    $scope.init();
});

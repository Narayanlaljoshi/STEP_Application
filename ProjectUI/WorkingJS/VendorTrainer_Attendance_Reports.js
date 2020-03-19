var app = angular.module('AttendanceReport_VendorModule', []);

app.service('AttendanceReport_VendorService', function ($http, $location) {
    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetSessionList = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetSessionList', data);
    };
    this.GetAttendanceReport = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetMarksReportForAdmin', data);
    };
    this.GetTrainerForFilter = function (Obj) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/Reports/GetTrainerForFilter' , Obj);
    };
    this.GetProgramList = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetProgramList' ,Obj);
    };
    this.GetVendorAttendanceReport = function (Obj) {
        return $http.post(this.AppUrl + '/Reports/GetVendorAttendanceReport', Obj);
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

app.controller('AttendanceReport_VendorController', function ($scope, $http, $location, $filter, AttendanceReport_VendorService, $uibModal, $rootScope, InitFactory) {
    $scope.ReportInput = {
       ProgramId:null,
        Trainer_Id:null,
        SessionID :null,
        StartDate:null,
        EndDate: null,
        ManagerID: $rootScope.session.UserName
    };
    $scope.currentPage = 1;
    $scope.ShowReport = false;
    $scope.init = function () {
        $scope.ReportInput.Trainer_Id = null;
        AttendanceReport_VendorService.GetTrainerForFilter($scope.ReportInput).then(function success(data) {
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
            ManagerID:$rootScope.session.UserName
        };
        $scope.init();
    };
    $scope.GetVendorAttendanceReport = function (ReportInput) {

        if (!$scope.ReportInput.ProgramId && !$scope.ReportInput.Trainer_Id && !$scope.ReportInput.SessionID && !$scope.ReportInput.StartDate && !$scope.ReportInput.EndDate) {
            swal("Warning", "Please Select Start Date and End Date", "warning");
        }
        else {
            AttendanceReport_VendorService.GetVendorAttendanceReport(ReportInput).then(function success(data) {
                $scope.AttendanceReportData = data.data;
                $scope.ShowReport = true;

            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
    };
    $scope.ProgramList = [];
    $scope.SessionIDList = [];
    $scope.GetProgramList = function (ReportInput) {
        $scope.init();
        $scope.ProgramList = [];
        AttendanceReport_VendorService.GetProgramList(ReportInput).then(function success(data) {
            if (ReportInput) {
                $scope.RawProgramList = data.data;
                angular.forEach($scope.RawProgramList, function (Key, Value) {
                    if ($scope.ProgramList.indexOf(Key)===-1)
                    {
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
    $scope.GetSessionIDList = function (ReportInput) {
        //console.log(ProgramID);
        $scope.SessionIDList = [];
        //angular.forEach($scope.RawProgramList, function (key, value) {
        //    if (key.ProgramID === ProgramID) {
        //        console.log(key.ProgramID === ProgramID);
        //        $scope.SessionIDList.push({
        //            SessionID: key.SessionID
        //        });
        //    }
           
        //});
        AttendanceReport_VendorService.GetSessionList(ReportInput).then(function success(data) {
            $scope.SessionIDList = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
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

        var ctx = { worksheet: name || 'Colour Details', table: AttendanceDayWiseReport.innerHTML };
        $('.ng-table-sort-header').after(filters);
        var url = uri + base64(format(template, ctx));
        var a = document.createElement('a');
        a.href = url;

        //Name the Excel like 'GroupWiseReport.xls' in this

        a.download = 'AttendanceDayWiseReport_AS_ON_' + $filter('date')(new Date(), "MMM dd yyyy")+'.xls';
        a.click();

    };

    $scope.init();
});

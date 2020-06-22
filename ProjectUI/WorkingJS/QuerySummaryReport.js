var app = angular.module('Queries_SummaryModule', []);

app.service('Queries_SummaryService', function ($http, $location) {

    this.GetReport = function (User_Id) {
        return $http.get('api/Queries/GetSummaryReport?User_Id=' + User_Id,{ });
    };
});
app.controller('Queries_SummaryController', function ($scope, $http, $location, $window, Queries_SummaryService, $rootScope) {
    console.log("Queries_SummaryController");

    //uploader = new FileUploader();
    ////
    //uploader.filters.push({
    //    'name': 'enforceMaxFileSize',
    //    'fn': function (item) {
    //        return item.size <= 10485760; // 10 MiB to bytes
    //    }
    //});
    $scope.init = function () {
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        Queries_SummaryService.GetReport($rootScope.session.User_Id).then(function (success) {
            $scope.SummaryList = success.data;
        }, function (error) { });
    };

    $scope.exportExcel = function () {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        var table = document.getElementById("SummaryTable");
        var filters = $('.ng-table-filters').remove();
        //pass html table id (e.g.-'Sp_ContractorReport' in this)
        var ctx = { worksheet: name || 'Colour Details', table: SummaryTable.innerHTML };
        $('.ng-table-sort-header').after(filters);
        var url = uri + base64(format(template, ctx));
        var a = document.createElement('a');
        a.href = url;
        //Name the Excel like 'GroupWiseReport.xls' in this
        a.download = 'Summary.xls';
        a.click();
    };

    $scope.init();
    
});
var app = angular.module('Queries_AdminModule', []);

app.service('Queries_AdminService', function ($http, $location) {

    this.GetAllQueries = function (User_Id) {
        return $http.get('api/Queries/GetAllQueries?User_Id=' + User_Id, {});
    };
    this.GetStatusList = function () {
        return $http.get('api/Queries/GetStatusList/', {});
    };
    this.UpdateQuery = function (Obj) {
        return $http.post('api/Queries/UpdateQuery', Obj);
    };
    this.GetEmployeeRequestDetails = function (Query_Id) {
        return $http.get('/api/Queries/GetEmployeeDetails?Query_Id=' + Query_Id);
    };

    //this.GetRequestStatusDetails = function (id) {
    //    return $http.get('/api/InProcessRequest/GetRequestStatusDetails?id=' + id);
    //};

    this.GeCurrentStatus = function (Query_Id) {
        return $http.get('/api/Queries/GetCurrentStatus?Query_Id=' + Query_Id);
    };

    this.GetFutureStatus = function (Query_Id) {
        return $http.get('/api/Queries/GetFutureStatus?Query_Id=' + Query_Id);
    };
});
app.controller('Queries_AdminController', function ($scope, $http, $location, $uibModal, Queries_AdminService, $rootScope) {
    console.log("Queries_AdminController");

    $scope.init = function () {
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        console.log("inside init");
        
        Queries_AdminService.GetAllQueries($rootScope.session.User_Id).then(function success(data) {
            console.log(data);
            $scope.QueryList = data.data;

        }, function (error) {
            console.log(error);
        });
    };

    $scope.ViewDescription = function (pt) {
        $scope.QueryDetail = pt;
    };

    $scope.UpdateQuery = function (Id) {

        $uibModal.open({
            templateUrl: 'CloseQueryModal.html',
            controller: 'CloseQueryModalController',
            windowClass: 'app-modal-window',
            backdrop: 'static',
            resolve: {
                Query_Id: function () {
                    return Id;
                }
            }
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

    $scope.ViewStatus = function (id) {
        // alert(id);
        $rootScope.RequestID = id;
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'partial/MyModelPopUp.html',
            controller: 'ModalInstanceCtrl',
            size: 'lg',
            resolve: {
                items: function () {
                    return id;
                }
            }
        });
    };

    $scope.init();

    $scope.$on('query-closed', function (event, args) {
        $scope.init();
    });
});

app.controller('CloseQueryModalController', function ($scope, $rootScope, Queries_AdminService, Query_Id, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    Queries_AdminService.GetStatusList().then(function success(data) {
        console.log(data);
        $scope.StatusList = data.data;
    }, function (error) {
        console.log(error);
    });
    $scope.UpdateQuery = function () {
        var obj = {
            Remarks: $scope.Remarks,
            User_Id: $rootScope.session.User_Id,
            Query_Id: Query_Id.Id,
            Status_Id: $scope.Status_Id
        };
        console.log(obj);
        Queries_AdminService.UpdateQuery(obj).then(function (data) {
            $scope.cancel();
            swal("Success", data.data, "success");
            $rootScope.$broadcast('query-closed');
        }, function (error)
        {
            console.log(error);
        });
    };


});

app.controller('ModalInstanceCtrl', function ($scope, $rootScope, $uibModalInstance, Queries_AdminService, $uibModal) {
    // alert( $rootScope.RequestID );

    $scope.PendingStatusData = {};
    $scope.grid = {};
    $scope.initStatus1 = function () {
        $scope.EmployeeDetails = [];
        //console.log(InProcessRequestJsService.RequestID);
        Queries_AdminService.GetEmployeeRequestDetails($rootScope.RequestID).then(function success(retdata) {
            if (retdata != null) {
                console.log(retdata);
                $scope.EmployeeDetails = retdata.data;
            }
            else {
                console.log('Error case');
            }
        }, function error(retdata) {
            console.log(retdata);
        });
        var neeObject = {};

        //Queries_AdminService.GetRequestStatusDetails($rootScope.RequestID).then(function success(retdata) {
        //    //debugger
        //    if (retdata != null) {
        //        $scope.grid = retdata.data;
        //    }
        //    else {
        //        console.log('Error case');
        //    }
        //}, function error(retdata) {
        //});
        Queries_AdminService.GeCurrentStatus($rootScope.RequestID).then(function success(retdata) {
            //debugger
            if (retdata != null) {
                $scope.PendingStatusData = retdata.data;
            }
            else {
                console.log('Error case');
            }
        }, function error(retdata) {
            console.log(FutureStat);
        });
        Queries_AdminService.GetFutureStatus($rootScope.RequestID).then(function success(retdata) {
            //debugger
            if (retdata != null) {
                $scope.FutureStatus = retdata.data;
            }
            else {
                console.log('Error case');
            }
        }, function error(retdata) {
            console.log(Futretdata);
        });
    };

    $scope.initStatus1();
    $scope.confirm = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});
var app = angular.module('Queries_AdminModule', []);

app.service('Queries_AdminService', function ($http, $location) {

    this.GetAllQueries = function (User_Id) {
        return $http.get('api/Queries/GetAllQueries?User_Id=' + User_Id, {});
    };

    this.CloseQuery = function (Obj) {
        return $http.post('api/Queries/CloseQuery', Obj, {});
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


    $scope.CloseQuery = function (Id) {

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

    $scope.init();
    $scope.$on('query-closed', function (event, args) {

        $scope.init();
    });


});

app.controller('CloseQueryModalController', function ($scope, $rootScope, Queries_AdminService, Query_Id, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.CloseQuery = function () {
        let obj = {
            Remarks: $scope.Remarks,
            User_Id: $rootScope.session.User_Id,
            Query_Id: Query_Id
        }
        Queries_AdminService.CloseQuery(obj).then(function (data) {

            $scope.cancel();
            swal("Success", data.data, "success");

            $rootScope.$broadcast('query-closed');
        }, function (error) {
            console.log(error);
        });
    };


});
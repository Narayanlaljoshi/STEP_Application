var app = angular.module('ManageNominationModule', []);

app.service('ManageNominationService', function ($http, $location) {

    this.AppUrl = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }
    this.GetFacultyList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetFacultyList?Agency_Id=' + Agency_Id);
    };
    this.GetSessionList = function (Agency_Id) {
        return $http.get(this.AppUrl + '/ManageSession/GetSessionList?Agency_Id=' + Agency_Id);
    };
    this.UpdateFaculty = function (Obj) {
        return $http.post(this.AppUrl + '/ManageSession/UpdateFaculty', Obj);
    }; 
    this.GetNominationsList = function () {
        return $http.get(this.AppUrl + '/Nomination/GetNominationsList', {});
    }; 
    this.UpdateNominationsList = function (Obj) {
        return $http.post(this.AppUrl + '/Nomination/UpdateNominationsList', Obj);
    };
});
app.controller('ManageNominationController', function ($scope, $http, $location, CityService, $rootScope, $filter, ManageNominationService, helperService) {
    console.log($rootScope.session.Agency_Id);
    $scope.init = function () {
        ManageNominationService.GetNominationsList().then(function success(success) {

            $scope.NominationsList = success.data;
            //$scope.eqpCustFields[i].Value = $filter('date')(new Date(dateValue), 'yyyy-MM-dd');

            angular.forEach($scope.NominationsList, function (value, key) {

                value.EndDate = $filter('date')(new Date(value.EndDate), 'dd-MMM-yyyy');
                console.log(value.EndDate);
                //this.push(key + ': ' + value);
            });



        //    return ManageNominationService.GetSessionList($rootScope.session.Agency_Id);

        //}).then(function success(success) {

        //    $scope.SessionList = success.data;

        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };

    $scope.UpdateFaculty = function (pt) {
        pt.User_Id = $rootScope.session.User_Id;
        ManageNominationService.UpdateFaculty(pt).then(function success(success) {

            if (success.data.indexOf("Success") !== -1) {
                swal("", success.data, "success");
                $scope.init();
            }
            else {
                swal("", success.data, "error");
            }



        }, function error(Error) {

            console.log("Error in loading data from EDB");

        });
    };


    $scope.UpdateNominationsList = function (pt) {
        pt.ModifiedBy = $rootScope.session.User_Id;
        swal({
            title: 'Warning',
            text: "it will affect all the candidates End date, who are mapped to this session id",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            //  cancelButtonColor: '#d33',
            confirmButtonText: 'OK'
        }).then((result) => {
            ManageNominationService.UpdateNominationsList(pt).then(function success(success) {

                if (success.data.indexOf("Success") !== -1) {
                    swal("", success.data, "success");
                    $scope.init();
                }
                else {
                    swal("", success.data, "error");
                }
            }, function error(Error) {

                console.log("Error in loading data from EDB");

            });
        });
    };


    $scope.init();

});
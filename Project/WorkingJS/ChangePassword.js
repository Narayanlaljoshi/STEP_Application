var app = angular.module('ChangePasswordModule', ['ngLoadingSpinner', 'ui.bootstrap', 'ngLoadingSpinner', 'ngStorage']);
app.service('ChangePasswordService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }

    
    this.ChangePassword = function (UserName, Password) {
        console.log(Password);
        return $http.post(this.AppUrl + '/Login/ChangePassword/?UserName=' + UserName + '&Password=' + Password);
    };
   
});
app.controller('ChangePasswordController', function (ChangePasswordService, $scope, $rootScope) {

    var user = $rootScope.session.UserName;
    $scope.init = function () {
        console.log("In Change Password Controller " + user);

    };


    $scope.UpdatePassword = function () {
        console.log("In Update function " + $scope.CPassword);

        if (!$scope.CPassword) {
            swal('Error', 'Password can not empty', 'error');
            return false;
        }
        ChangePasswordService.ChangePassword(user, $scope.CPassword).then(function (success) {
            console.log(success.data);
            if (success.data == 'success') {
                swal('Success', 'Your password has been Changed', 'success');
            }
            else {
                swal('Error', 'Password not Reset', 'error');
                return false;
            }

        });
    };


    $scope.init();
});

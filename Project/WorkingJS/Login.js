var app = angular.module('LoginModule', ['ngLoadingSpinner', 'ui.bootstrap', 'ngLoadingSpinner', 'ngStorage', 'ngCookies']);
app.service('LoginService', function ($http, $location) {
    this.AppUrl = "";
    //console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.authenticateLogin = function (UserName, Password) {
        return $http.get(this.AppUrl + 'Login/Login/?UserName=' + UserName + '&Password=' + Password);
    };
    this.AuthUser = function (UserName) {

        return $http.get(this.AppUrl + 'Login/AuthUser/?UserName=' + UserName);
    };

    this.LoginStudents = function (MSPIN, Password,Key) {

        return $http.get(this.AppUrl + 'Login/LoginStudent/?MSPIN=' + MSPIN + '&Password=' + Password +'&Key='+Key);
    };

});

app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
app.factory('crypt', function () {
    return {
        hash: function (value) {
            //var str = JSON.stringify(value);
            return CryptoJS.SHA1(value).toString();
        }
    };
});
app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.controller('LoginController', function ($scope, $http, $location, LoginService, $cookieStore, $uibModal, crypt, $cookies,  $localStorage ) {
    //console.log("insoide LoginController");

    $scope.InvalidUser = false;

    $scope.init = function () {
        sessionStorage.setItem("app", null);
        $scope.showForgotPassword = false;
    };

    $scope.ForgotPass = function () {
        console.log("In Forgot Password");
        if (!$scope.UserName) {
            swal('Error', 'UserName can not empty', 'error');
            return false;
        }
        LoginService.AuthUser($scope.UserName).then(function (success) {

            console.log(success.data);
            if (success.data != null) {
                if (success.data == 0) {
                    swal('Error', 'User does not exists', 'error');
                    return false;
                }
                else if (success.data==1){
                    swal('Success', 'Your crednetials has been sent to your registrated mail', 'success');
                    $scope.init();
                }
                else if (success.data == 2){
                    swal('Success', 'Your crednetials has been sent to your registrated mobile', 'success');
                    $scope.init();
                }
                else {
                    swal('Error', 'Error in reseting', 'error');
                }                

            }

        }, function (error) {
            swal('Error', 'Error in reseting', 'error');
        });

    }

    $scope.SetCookies = function (Obj) {
        console.log(Obj);
        $cookies.put("StudentDetails", Obj);
    };

    $scope.GetCookies = function () {
        $window.alert($cookies.get('StudentDetails'));
    };

    $scope.ClearCookies = function () {
        $cookies.remove('LogKey');
    };
    $scope.LoginStudent = function () {
        $scope.Key = null;
        console.log($scope.Mspin);
        console.log($scope.PasswordSt);
        if (!$scope.Mspin) {
            swal('Error', 'MSPIN can not empty', 'error');
            return false;
        }
        if (!$scope.PasswordSt) {
            swal('Error', 'Password can not empty and It Would Be in XXXXDDMMYYYY format', 'error');
            return false;
        }
        if ($cookies.get('StudentDetails')) {
            var StudentDetails = $cookies.get('StudentDetails');
            $scope.Key = StudentDetails.LoginKey;
        }
        else {

        }
        console.log("$cookies.get('LogKey')", $cookies.get('LogKey'));

        LoginService.LoginStudents($scope.Mspin, $scope.PasswordSt, $cookies.get('LogKey')).then(function (success) {
            console.log("success.data",success.data);
            //if (success.data.Status.indexOf('Success') != -1) {
            //    $cookies.put('LogKey', success.data.LogKey);
            //    LoginService.UserName = $scope.UserName;
            //    //$scope.SetCookies(success.data);
            //    //console.log("Cookies", $cookies.get('StudentDetails'));
            //    console.log("LogKey", $cookies.get('LogKey'));
            //    var Cookie = $cookies.get('Object');
            //    window.location = './StudentScreen.html#/log=' + $scope.Mspin + '&xud=' + success.data.User_Id;
            //}
            //else {
            //    swal("", success.data.Status, "error");
            //}
            if (success.data) {
                if (success.data.ErrorMsg!=null) {
                    swal("", success.data.ErrorMsg, "error");
                    return false;
                }
                if (success.data.AlreadyLoggedIn == true) {
                    swal("", "You have already logged in </br> Please contact your trainer.", "error");
                }
                else if (success.data.User_Id != 0) {
                    $cookies.put('LogKey', success.data.LogKey);
                    LoginService.UserName = $scope.UserName;
                    //$scope.SetCookies(success.data);
                    //console.log("Cookies", $cookies.get('StudentDetails'));
                    console.log("LogKey", $cookies.get('LogKey'));
                    var Cookie = $cookies.get('Object');
                    window.location = './StudentScreen.html#/log=' + $scope.Mspin + '&xud=' + success.data.User_Id;
                }
                else {
                    $scope.InvalidUser = true;
                }
            }
            else
            {
                swal("","You have already given this test </br> or </br> you have exceeded  the time limit.","warning");
            }
        }, function (error) {

        });
    };


    $scope.Login = function () {
        $scope.InvalidUser = false;
        //console.log($scope.UserName);
        //console.log($scope.Password);

        if (!$scope.UserName) {

            swal('Error', 'UserName can not empty', 'error');
            return false;
        }
        if (!$scope.Password) {
            swal('Error', 'Password can not empty', 'error');
            return false;
        }


        //var Password = crypt.hash($scope.Password);
        //window.location = './index.html#/';
        LoginService.authenticateLogin($scope.UserName, $scope.Password).then(function (success) {
            //console.log(success.data.User_Id);
          

            //console.log($scope.UserName);
            //console.log($scope.Password);
         
            if (success.data.User_Id != 0) {

                if (success.data.RoleName == 'Student') {


                    LoginService.UserName = $scope.UserName;
                    //console.log(LoginService.UserName);
                    //var Crypted =hash($scope.UserName).toString();
                    ////console.log(Crypted);
                   // window.location = './StudentScreen.html?UserName=' + $scope.UserName;
                    window.location = './StudentScreen.html#/log=' + $scope.UserName + '&xud=' + success.data.User_Id;
                    //window.location = './index.html#/St';
                    //window.location = './.html#/';

                }
                else {
                    sessionStorage.setItem("app", angular.toJson(success.data));

                    window.location = './index.html#/';
                }

            }
            else
            {
                $scope.InvalidUser = true;
                //swal("Please Check Your username and password");
            }

        }, function (error) {
            //console.log(error);
        });
    };


    $scope.init();
});

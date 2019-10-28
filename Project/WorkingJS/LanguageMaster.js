var app = angular.module('LanguageMasterModule', [])
app.service('LanguageMasterService',  function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }

    this.SaveFunction = function (obj) {
        return $http.post(this.AppUrl + '/LanguageMaster/SaveFunction', obj, {
        });
    };
    this.GetLanguage = function () {
        return $http.get(this.AppUrl + '/LanguageMaster/GetLanguage')
    };

    this.UpdateLanguage = function (data) {
        console.log(data);
        return $http.post(this.AppUrl + '/LanguageMaster/UpdateLanguage', data, {});
    };
    this.DeleteLanguage = function (obj) {
        console.log(obj);
        return $http.post(this.AppUrl + '/LanguageMaster/DeleteLanguage', obj, {})

    };
});
app.controller('LanguageMasterController', function (LanguageMasterService, $scope,$rootScope) {

    $scope.AddLanguage = false;
    $scope.GridLanguage = true;
    $scope.SaveLanguage = false;
    $scope.UpdateLanguage = false;
    $scope.BackLanguage = false;


    $scope.init = function () {

        $scope.AddLanguage = false;
        $scope.GridLanguage = true;
        $scope.SaveLanguage = false;
        $scope.UpdateLanguage = false;
        $scope.BackLanguage = false;


        LanguageMasterService.GetLanguage()
            .then(function (success) {
                if (success.data != null) {
                    $scope.LanguageArray = success.data

                }
            },
            function (error) {
                console.log(error.data);


            });

    };

    $scope.Save = function () {
        if (!$scope.Language) {
            swal("Error", "Language Cannot Be Empty", "error");
            return false;
        }

        var obj = {
            Language: $scope.Language,
            CreatedBy: $rootScope.session.User_Id
        };
        LanguageMasterService.SaveFunction(obj)
            .then(function (success) {
                //$scope.init();
                if (success.data == 1) {
                    swal("Save!", "Language saved successfully.", "success");
                    $scope.init();
                }
                else if (success.data == 2) {

                    swal("error", "Language already exist.", "error");

                }
            },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");

            });
    }


    $scope.UpdateL = function () {
        if (!$scope.Language) {
            swal("Error", "Language Cannot Be Empty", "error");
            return false;
        }
        var obj = {
            LanguageMasterId: $scope.LanguageMasterId,
            Language: $scope.Language,
            ModifiedBy: $rootScope.session.User_Id
        };
        LanguageMasterService.UpdateLanguage(obj)
            .then(function (success) {

                console.log("SUCCESS DATA", success.data);
                if (success.data == 1) {
                    swal("Success!", "Language Has Been Updated.", "success");
                    $scope.init();
                }
                else if (success.data == 2) {

                    swal("error", "Language already exist in database.", "error");
                    //$scope.init();
                }
            },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");

            });
    }


    $scope.AddAccount = function () {
        $scope.Language = "";
        $scope.AddLanguage = true;
        $scope.GridLanguage = false;
        $scope.SaveLanguage = true;
        $scope.UpdateLanguage = false;
        $scope.BackLanguage = true;

    }

    $scope.Back = function () {

        $scope.AddLanguage = false;
        $scope.GridLanguage = true;
        $scope.SaveLanguage = false;
        $scope.UpdateLanguage = false;
        $scope.BackLanguage = false;
    }

    $scope.Edit = function (pt) {


        $scope.Language = pt.Language;
        $scope.LanguageMasterId = pt.LanguageMasterId;
        $scope.AddLanguage = true;
        $scope.GridLanguage = false;
        $scope.SaveLanguage = false;
        $scope.UpdateLanguage = true;
        $scope.BackLanguage = true;

    }



    $scope.DeleteLanguage = function (LanguageMasterId) {

        swal({
            title: 'Are you sure?',
            text: "You are going to delete!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {


            var obj = {
                LanguageMasterId: LanguageMasterId,
                ModifiedBy: $rootScope.session.User_Id
            }
            console.log(LanguageMasterId);
            LanguageMasterService.DeleteLanguage(obj)
                .then(function (success) {
                    $scope.init();
                    console.log("SUCCESS DATA", success.data);
                    swal("Success!", "Language has been Deleted.", "success");
                },
                function (error) {
                    console.log(error.data);
                    swal("error", error.data, "error");

                });
        });
    }

    $scope.init();



});
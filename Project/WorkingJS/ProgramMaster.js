var app = angular.module('ProgramMasterModule', [])
app.service('ProgramMasterService', function ($http, $location) {


    this.ProgramDeatil = null;
    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }
    this.GetEmp = function () {
        return $http.get(this.AppUrl + '/ProgramMaster/GetProgram');
    };
    this.DeleteProgram = function (obj) {
        console.log(obj);
        return $http.post(this.AppUrl + '/ProgramMaster/DeleteProgram', obj, {});
    };
    this.UpdateProgram = function (data) {
        console.log(data);
        return $http.post(this.AppUrl + '/ProgramMaster/UpdateProgram', data, {});
    };
    this.SaveFunction = function (obj) {
        return $http.post(this.AppUrl + '/ProgramMaster/SaveFunction', obj, {});
    };
});


app.controller('ProgramMasterController', function (ProgramMasterService, $scope, LanguageMasterService, $rootScope) {
    $scope.DataProgram = true;
    $scope.PlusProgram = false;
    $scope.UpdateProgram = false;
    $scope.SaveProgram = false;
    $scope.BackProgram = false;
    $scope.LanguageArray = [];

    
    $scope.init = function () {
        $scope.BackProgram = false;
        $scope.DataProgram = true;
        $scope.PlusProgram = false;
        $scope.UpdateProgram = false;
        $scope.SaveProgram = false;
        $scope.SelectedLanguages = [];
        ProgramMasterService.GetEmp()
            .then(function (success) {
                console.log(success);
                if (success.data != null) {
                    $scope.ProgramArray = success.data;
                };
            },
            function (error) {
                console.log(error);

            });
        LanguageMasterService.GetLanguage()
            .then(function (success) {
                if (success.data != null) {
                    $scope.LanguageArray = success.data;

                }
            },
            function (error) {
                console.log(error.data);


            });
    };

    
    $scope.SaveFunction = function () {
        if (!$scope.ProgramCode) {
            swal("Error", "Program Code Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.ProgramName) {
            swal("Error", "Program Name Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.Duration) {
            swal("Error", "Duration Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.ProgramType_Id) {
            swal("Error", "Program Type Cannot Be Empty", "error");
            return false;
        }
        $scope.SelectedLanguagesArray = [];
        angular.forEach($scope.SelectedLanguages, function (value, key) {

            $scope.SelectedLanguagesArray.push({
                LanguageMaster_Id: value
            });
        });
        var obj = {
            //LanguageMasterId: $scope.LanguageMasterId,
            SelectedLanguages: $scope.SelectedLanguagesArray,
            ProgramCode: $scope.ProgramCode,
            ProgramName: $scope.ProgramName,
            Duration: $scope.Duration,
            CreatedBy: $rootScope.session.User_Id,
            ProgramType_Id: $scope.ProgramType_Id
        };
        console.log(obj);
        ProgramMasterService.SaveFunction(obj)
            .then(function (success) {

                if (success.data == 1) {
                    swal("Save!", "Program saved successfully.", "success");
                    $scope.init();
                }
                else if (success.data == 2) {

                    swal("error", "Program can not be saved.", "error");

                }
            },
                function (error) {
                    console.log(error.data);
                    swal("error", error.data, "error");

                });
    };


    $scope.UpdateFunction = function () {
        if (!$scope.SelectedLanguages) {
            swal("Error", "Language Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.ProgramCode) {
            swal("Error", "Program Code Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.ProgramName) {
            swal("Error", "Program Name Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.Duration) {
            swal("Error", "Duration Cannot Be Empty", "error");
            return false;
        }
        if (!$scope.ProgramType_Id) {
            swal("Error", "Program Type Cannot Be Empty", "error");
            return false;
        }
        $scope.SelectedLanguagesArray = [];
        angular.forEach($scope.SelectedLanguages, function (value, key) {

            $scope.SelectedLanguagesArray.push({
                LanguageMaster_Id: value
            });
        });

        console.log($scope.ProgramId);

        var obj = {
            SelectedLanguages: $scope.SelectedLanguagesArray,
            Language: $scope.Language,
            ProgramCode: $scope.ProgramCode,
            ProgramName: $scope.ProgramName,
            Duration: $scope.Duration,
            ProgramId: $scope.ProgramId,
            ModifiedBy: $rootScope.session.User_Id
        };
        console.log(obj);
        ProgramMasterService.UpdateProgram(obj)
            .then(function (success) {

                console.log("SUCCESS DATA", success.data);
                if (success.data == 1) {
                    swal("Save!", "Program  has been Updated.", "success");
                    $scope.init();
                }
                else if (success.data == 2) {

                    swal("error", "Program  can Not be Updated.", "error");

                }
            },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");

            });
    }


    $scope.DeleteProgram = function (ProgramId) {

        var obj = {
            ProgramId: ProgramId,
            ModifiedBy: $rootScope.session.User_Id
        };
        console.log(ProgramId);
        ProgramMasterService.DeleteProgram(obj)
            .then(function (success) {
                $scope.init();
                if (success.data.indexOf('error') == -1) {

                    swal("Save!", "program deleted successsfully.", "success");
                }
                else {


                    swal("Error!", "program can not be deleted.", "error");
                }
            },
                function (error) {
                    console.log(error.data);
                    swal("error", error.data, "error");

                });
    };
    

    $scope.AddProgram = function () {
        $scope.ProgramName = null;
        $scope.ProgramCode = null;
        $scope.Duration = 0;
        $scope.Language = [];
        $scope.ProgramId = null;
        $scope.ProgramType_Id = null;
        $scope.DataProgram = false;
        $scope.PlusProgram = true;
        $scope.UpdateProgram = false;
        $scope.SaveProgram = true;
        $scope.BackProgram = true;
    };


    $scope.Back = function () {
        $scope.init();
    };


    $scope.Edit = function (pt) {

        console.log(pt);

        $scope.SelectedLanguages = [];
        $scope.ProgramId = pt.ProgramId;
        $scope.ProgramName = pt.ProgramName;
        $scope.ProgramCode = pt.ProgramCode;
        $scope.Duration = pt.Duration;
        angular.forEach(pt.SelectedLanguages, function (value, key) {

            $scope.SelectedLanguages.push(value.LanguageMaster_Id);
        });

        $scope.DataProgram = false;
        $scope.PlusProgram = true;
        $scope.UpdateProgram = true;
        $scope.SaveProgram = false;
        $scope.BackProgram = true;

    };


    $scope.TestSetup = function (pt) {
        ProgramMasterService.ProgramDeatil = pt;
        //if (pt.ProgramType_Id == 1)
        //{
            window.location.assign('#/ProgramTestCalender');
        //}
        //if (pt.ProgramType_Id == 2) {
           // window.location.assign('#/ProgramTestCalender_Evaluation');
        //}
    };

    $scope.init();
});
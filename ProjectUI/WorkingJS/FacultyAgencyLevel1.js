var app = angular.module('FacultyAgencyLevelModule', []);

app.service('FacultyAgencyLevelService', function ($http, $location) {



    this.AppUrl = "";
    this.MSPin = "";
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }


    this.GetFacultyProgramdetails = function (FacultyCode) {
        return $http.get(this.AppUrl + '/FacultyAgencyLevel/GetFaciltyProgramdetails?FacultyCode=' + FacultyCode);
    };
    this.SaveInitiatedTestDetails = function (Obj) {
        return $http.post(this.AppUrl + '/Test/SaveInitiatedTestDetails', Obj);
    };
    this.GetStudentPostTestScores = function (MSpin) {
        return $http.get(this.AppUrl + '/Test/GetStudentPostTestScores/?MSpin=' + MSpin);
    };
});

//app.factory('InitFactory', function (FacultyAgencyLevelService) {

//    return {
//        init: function () {
//            RtcMasterService.GetAgencyList().then(function success(data) {
//                RtcMasterService.AgencyList = data.data;
//                console.log("Get Region List", data);
//            }, function error(data) {
//                console.log("Error in loading data from EDB");
//            });
//        }
//    };
//});

app.controller('FacultyAgencyLevelController', function ($scope, $http, $location, FacultyAgencyLevelService, $uibModal, $rootScope, InitFactory, $filter) {

    $scope.EligiblForTestInitiate = true;
    $scope.CurrentDate = $filter('date')(new Date(), "dd-MM-yyyy");
    $scope.init = function () {
        $scope.EligiblForTestInitiate = true;
        console.log("FacultyProgramdetails");
        FacultyAgencyLevelService.GetFacultyProgramdetails($rootScope.session.UserName).then(function success(success) {
            $scope.FacultyProgramdetails = success.data;
            $scope.LastDate = $filter('date')($scope.FacultyProgramdetails.FaciltyProgramdetails.LastTestDate, "dd-MM-yyyy");
            console.log($scope.LastDate, $scope.CurrentDate, $scope.LastDate == $scope.CurrentDate);
            if ($scope.LastDate == $scope.CurrentDate) {
                $scope.EligiblForTestInitiate = false;
            }
            console.log("FacultyProgramdetails", success.data);
        }, function error(Error) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.InitiatTest = function (pt) {


        //swal({
        //    title: "Are you sure?",
        //    text: "You are going to initiate the test!",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonClass: "btn-danger",
        //    confirmButtonText: "Yes, Initiate it!",
        //    closeOnConfirm: false
        //}, function () {

        closeOnConfirm: false

        console.log($rootScope.session);
        pt.User_Id = $rootScope.session.User_Id;
        console.log("FacultyProgramdetails");
        swal({
            title: 'Are you sure?',
            text: "You are going to initiate test!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3c8dbc',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, initiate it!'
        }).then((result) => {
        FacultyAgencyLevelService.SaveInitiatedTestDetails(pt).then(function success(success) {
            if (success.data.indexOf("Success") != -1) {
                swal("Success", success.data, "success");
                $scope.init();
            }
            else
                swal("Error", success.data, "error");
            console.log("FacultyProgramdetails", success.data);
        }, function error(Error) {

            console.log("Error in loading data from EDB");
        });
       });
    };


    $scope.ShowPostTestsScore = function (pt) {
        FacultyAgencyLevelService.MSPin = pt.MSPIN;
        $uibModal.open({
            templateUrl: 'TestRecords.html',
            controller: 'TestRecordsController',
            windowClass: 'app-modal-window',
            backdrop: 'static',
            size: 'md'
        }).result.then(
            function () {

            },
            function () {

            }
            );
    };



    $scope.init();
});

app.controller('TestRecordsController', function ($scope, $rootScope, FacultyAgencyLevelService, $uibModalInstance, InitFactory) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.MSPin = FacultyAgencyLevelService.MSPin;
    $scope.init = function () {
        FacultyAgencyLevelService.GetStudentPostTestScores($scope.MSPin).then(function success(retdata) {

            $scope.StudentPostTestScores = retdata.data;
            console.log($scope.StudentPostTestScores);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };

    $scope.init();


});
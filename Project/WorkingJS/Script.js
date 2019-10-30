﻿var app = angular.module('homeApp', ['ui.bootstrap', 'ngLoadingSpinner', 'angularUtils.directives.dirPagination',
    'ngAnimate', 'ngRoute', 'chart.js', 'angular.morris-chart', 'ZoneModule', 'RtcMasterModule', 'RegionModule', 'CityModule', 'ChannelModule', 'DealerGroupModule', 'DealerOutletModule', '720kb.datepicker',
    'ProgramTestCalenderModule', 'ProgramTestCalender_EvaluationModule', 'QuestionBankModule', 'NominationModule', 'LanguageMasterModule', 'ProgramMasterModule', 'ReportScoreSheetModule', 'ReportAttendanceSheetModule', 'FacultyAgencyLevelModule', 'StudentModule', 'AttendanceReportModule', 'ManageSessionModule', 'MarksReportModule', 'ShowStudentMarksModule', 'ManageNominationModule', 'ChangePasswordModule', 'MarksReportAsPerDMSModule', 'AttendanceReportDMSModule', 'ManageTestModule', 'ManageAttandenceModule', 'PracticalModule', 'EvaluationModule']);

app.factory('httpRequestInterceptor', function () {
    return {
        request: function (config) {
            var session = angular.fromJson(sessionStorage.getItem("app")) || {};
            console.log(session);
            // config.headers['Authorization'] = session.userAuthKey;
            if (session != null) {
                return config;
            }
            sessionStorage.setItem("app", null);
            //window.location.assign('./login.html');
            //console.log(config);
            return;
        },

        responseError: function (response) {
            if (response.status === 403 || response.status === 400) {
                var data = {};
                sessionStorage.setItem("app", null);
               // window.location.assign('./login.html');
            }
        }
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

app.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|local|data|chrome-extension):/);
    $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|local|data|chrome-extension):/);

}]);
app.config(function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'partial/Dashboard.html' //,
            //controller: 'DashboardController'
        })
      .when('/dashboard', {
          templateUrl: 'partial/Dashboard.html' //,
          //controller: 'DashboardController'
        })
        .when('/ProgramTestCalender_Evaluation', {
            templateUrl: 'partial/ProgramTestCalender_Evaluation.html' ,
            controller: 'ProgramTestCalender_EvaluationController'
        })
        .when('/attribute-analysis', {
            templateUrl: 'partial/attribute-analysis.html' //,
            //controller: 'DashboardController'
        })
        .when('/impact-analysis', {
            templateUrl: 'partial/impact-analysis.html' //,
            //controller: 'DashboardController'
        })
        .when('/sop-analysis', {
            templateUrl: 'partial/sop-analysis.html' //,
            //controller: 'DashboardController'
        })
        .when('/customer-voice', {
            templateUrl: 'partial/customer-voice.html' //,
            //controller: 'DashboardController'
        })
        .when('/ZoneMaster', {
            templateUrl: 'partial/ZoneMaster.html' ,
            controller: 'ZoneController'
        })

        .when('/RegionMaster', {
            templateUrl: 'partial/RegionMaster.html',
            controller: 'RegionController'
        })

        .when('/St', {
            templateUrl: 'partial/CityMaster.html',
            controller: 'CityController'
        })

        .when('/ChannelMaster', {
            templateUrl: 'partial/ChannelMaster.html',
            controller: 'ChannelController'
        })
        .when('/DealerGroupMaster', {
            templateUrl: 'partial/DealerGroupMaster.html',
            controller: 'DealerGroupController'
        })
        .when('/DealerOutletMaster', {
            templateUrl: 'partial/DealerOutletMaster.html',
            controller: 'DealerOutletController'
        })

        .when('/ProgramTestCalender', {
            templateUrl: 'partial/ProgramTestCalender.html',
            controller: 'ProgramTestCalenderController'
        })
        .when('/QuestionBank', {
            templateUrl: 'partial/QuestionBank.html',
            controller: 'QuestionBankController'
        })
        .when('/RtcMaster', {
            templateUrl: 'partial/RtcMaster.html',
            controller: 'RtcMasterController'
        })
        .when('/Nomination', {
            templateUrl: 'partial/Nomination.html',
            controller: 'NominationController'
		})
		.when('/LanguageMaster', {
			templateUrl: 'partial/LanguageMaster.html',
			controller: 'LanguageMasterController' 

		})
		.when('/ProgramMaster', {
			templateUrl: 'partial/ProgramMaster.html',
			controller: 'ProgramMasterController'
		}) 
		.when('/ReportScoreSheet', {
			templateUrl: 'partial/ReportScoreSheet.html',
			controller: 'ReportScoreSheetController'

			})
		.when('/ReportAttendanceSheet', {
			templateUrl: 'partial/ReportAttendanceSheet.html',
			controller: 'ReportAttendanceSheetController'

        })
        .when('/FacultyAgencyLevel', {
            templateUrl: 'partial/FacultyAgencyLevel.html',
            controller: 'FacultyAgencyLevelController'
        })
        .when('/Student', {
            templateUrl: 'partial/StudentScreen.html',
            controller: 'StudentController'
        })
        .when('/Report', {
            templateUrl: 'partial/AttendanceReport.html',
            controller: 'AttendanceReportController'
        })
        .when('/ManageSessions', {
            templateUrl: 'partial/ManageSession.html',
            controller: 'ManageSessionController'
        })
        .when('/MarksReport', {
            templateUrl: 'partial/MarksReport.html',
            controller: 'MarksReportController'
        })
        .when('/ManageNominations', {
            templateUrl: 'partial/ManageNominations.html',
            controller: 'ManageNominationController'
        })
        .when('/changePassword', {
            templateUrl: 'partial/ChangePassword.html',
            controller: 'ChangePasswordController' //
        })
        .when('/MrksDMS', {
            templateUrl: 'partial/MarksReportAsPerDMS.html',
            controller: 'MarksReportAsPerDMSController' //
        }) 
        .when('/ArDMS', {
            templateUrl: 'partial/AttendanceReportAsPerDMS.html',
            controller: 'AttendanceReportDMSController' //
        })
        .when('/ManageTest', {
            templateUrl: 'partial/ManageTest.html',
            controller: 'ManageTestController' //
        })
        .when('/ManageAttandence', {
            templateUrl: 'partial/ManageAttandence.html',
            controller: 'ManageAttandenceController' //
        })
        .when('/QuestionBankPractical', {
            templateUrl: 'partial/QuestionBankPractical.html',
            controller: 'QuestionBankPracticalController' //
        })
        .when('/PracticalQuestion', {
            templateUrl: 'partial/PracticalScreen.html',
            controller: 'PracticalController' //
        })
        .when('/Evaluation', {
            templateUrl: 'partial/Evaluation.html',
            controller: 'EvaluationController' //
        })
    .otherwise({
        redirectTo: "/"
        }); 

    $locationProvider.hashPrefix('');
    //$httpProvider.interceptors.push('httpRequestInterceptor');
});
app.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});

app.service('HomeService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }

});



app.controller('HomeController', function ($scope, HomeService, $http, $rootScope, $window, $location) {
    console.log("homecontroller ");
   
    $scope.init = function () {
        console.log("homeInit");
        $rootScope.session = angular.fromJson(sessionStorage.getItem("app"));
        $scope.Role = $rootScope.session.RoleName;
        if ($rootScope.session.RoleName == 'Faculty') {
            
            window.location.href = '#/FacultyAgencyLevel';
        }
        console.log($rootScope.session);
    };

    $scope.signout = function () {
        // debugger
        swal({
            title: 'Are you sure?',
            text: "You are going to logout!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'OK'
        }).then((result) => {
            var data = {};
            sessionStorage.setItem("app", angular.toJson(data));
            $rootScope.session = {};

            window.location = './login.html';
            console.log(window.location);
        });
    };

    $scope.init();

});
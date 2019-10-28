var WebApp = angular.module('myapp', ['ui.bootstrap', 'ngLoadingSpinner', 'angularUtils.directives.dirPagination',
    'ngAnimate', 'ngRoute', 'chart.js', 'angular.morris-chart', '720kb.datepicker',
    'webcam']);

WebApp.factory('httpRequestInterceptor', function () {
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
WebApp.directive('numbersOnly', function () {
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
WebApp.directive('fileModel', ['$parse', function ($parse) {
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

WebApp.config(function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'DocumentCapture.html' //,
            //controller: 'DashboardController'
        })        
        .otherwise({
            redirectTo: "/"
        });

    $locationProvider.hashPrefix('');
    //$httpProvider.interceptors.push('httpRequestInterceptor');
});
WebApp.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});

WebApp.service('WebAppService', function ($http, $location) {
       
    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api/";
    }
    else {
        this.AppUrl = "/api/";
    }
    this.SaveDocument = function (Obj) {
        return $http.post(this.AppUrl + '/DocumentCapture/SaveDocument/', Obj);
    };

});

WebApp.controller('mainController', function ($scope, HomeService, $http, $rootScope, $window, $location) {
 
    $scope.PictureBase64 = null;
    $scope.DocumentBase64 = null;
    var _video = null,
        patData = null;
    $scope.Init = function () {
        var array = location.search.substring(1).split('=');
        console.log(array);
    };

    $scope.patOpts = {
        x: 0,
        y: 0,
        w: 25,
        h: 25
    };

    // Setup a channel to receive a video property
    // with a reference to the video element
    // See the HTML binding in main.html
    $scope.channel = {};

    $scope.webcamError = false;
    $scope.onError = function (err) {
        $scope.$apply(
            function () {
                $scope.webcamError = err;
            }
        );
    };

    $scope.onSuccess = function () {
        // The video element contains the captured camera data
        _video = $scope.channel.video;
        $scope.$apply(function () {
            $scope.patOpts.w = _video.width;
            $scope.patOpts.h = _video.height;
            //$scope.showDemos = true;
        });
    };

    $scope.onStream = function (stream) {
        // You could do something manually with the stream.
    };

    $scope.TakePicture = function () {
        if (_video) {
            var patCanvas = document.querySelector('#snapshot');
            if (!patCanvas) return;

            patCanvas.width = _video.width;
            patCanvas.height = _video.height;
            var ctxPat = patCanvas.getContext('2d');

            var idata = getVideoData($scope.patOpts.x, $scope.patOpts.y, $scope.patOpts.w, $scope.patOpts.h);
            ctxPat.putImageData(idata, 0, 0);

            $scope.PictureBase64 = patCanvas.toDataURL();
            console.log($scope.PictureBase64);
            //sendSnapshotToServer(patCanvas.toDataURL());

            patData = idata;
        }
    };

    $scope.TakeDocument = function () {
        if (_video) {
            var patCanvas = document.querySelector('#snapshot1');
            if (!patCanvas) return;

            patCanvas.width = _video.width;
            patCanvas.height = _video.height;
            var ctxPat = patCanvas.getContext('2d');

            var idata = getVideoData($scope.patOpts.x, $scope.patOpts.y, $scope.patOpts.w, $scope.patOpts.h);
            ctxPat.putImageData(idata, 0, 0);

            $scope.DocumentBase64 = patCanvas.toDataURL();
            console.log($scope.DocumentBase64);
            //sendSnapshotToServer(patCanvas.toDataURL());

            patData = idata;
        }
    };

    ///**
    // * Redirect the browser to the URL given.
    // * Used to download the image by passing a dataURL string
    // **/
    $scope.downloadSnapshot = function downloadSnapshot(dataURL) {
        window.location.href = dataURL;
    };

    var getVideoData = function getVideoData(x, y, w, h) {
        var hiddenCanvas = document.createElement('canvas');
        hiddenCanvas.width = _video.width;
        hiddenCanvas.height = _video.height;
        var ctx = hiddenCanvas.getContext('2d');
        ctx.drawImage(_video, 0, 0, _video.width, _video.height);
        return ctx.getImageData(x, y, w, h);
    };

    ///**
    // * This function could be used to send the image data
    // * to a backend server that expects base64 encoded images.
    // *
    // * In this example, we simply store it in the scope for display.
    // */
    var sendSnapshotToServer = function sendSnapshotToServer() {
        $scope.snapshotData = imgBase64;
        var Obj = {
            Year: $scope.snapshotData
            //Sem: DocBase64,
        };
        console.log($scope.snapshotData);
    };


    $scope.SaveDocument = function () {
        console.log("hi");
        var Obj = {
            Picture: $scope.PictureBase64,
            Document: $scope.DocumentBase64
        };
        console.log(Obj);
        //var fd = new FormData();

        ////console.log("Obj", Obj);
        //fd.append('file', $scope.File1);
        //fd.append('data', angular.toJson($rootScope.session));
        //console.log($rootScope.session);
        ////fd.append('file', $scope.File1);
        WebAppService.SaveDocument(Obj).then(function (success) {
            console.log("SUCCESS DATA", success.data);
        },
            function (error) {
                console.log(error.data);
                //swal("error", error.data, "error");
            });
    };


});
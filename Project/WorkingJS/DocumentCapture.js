var WebApp = angular.module('myapp', ['webcam', 'ngRoute']);
WebApp.service('WebAppService', function ($http, $location) {
    this.AppUrl = "";
    this.MSPin = "";
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

    //this.UploadUserMatrix = function (fd) {
    //    console.log(fd);
    //    return $http.post(this.AppUrl + '/Nomination/UploadUserMatrix', fd, {
    //        transformRequest: angular.identity,
    //        headers: { 'Content-Type': undefined }

    //    });
    //};
});
WebApp.config(function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'partial/Dashboard.html' //,
            //controller: 'DashboardController'
        })
        .otherwise({
            redirectTo: "/DocumentCapture.html"
        });

    $locationProvider.hashPrefix('');
    //$httpProvider.interceptors.push('httpRequestInterceptor');
});
WebApp.controller('mainController', function ($scope, $location, WebAppService) {
    $scope.PictureBase64 = null;
    $scope.DocumentBase64 = null;
    $scope.MSPIN = null;
    $scope.array = [];
    var _video = null,
        patData = null;
    $scope.Init = function () {
        $scope.array =$location.absUrl().split('=');
        //var array = location.search.substring(1).split('=');
        console.log($scope.array);
        if ($scope.array.length == 2)
        {
            $scope.MSPIN = $scope.array[1];
        }
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
        console.log(err);
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
            Document: $scope.DocumentBase64,
            MSPIN: $scope.array[1]
        };
        console.log(Obj);
        //var fd = new FormData();
        ////console.log("Obj", Obj);
        //fd.append('file', $scope.File1);
        //fd.append('data', angular.toJson($rootScope.session));
        //console.log($rootScope.session);
        ////fd.append('file', $scope.File1);
        //swal({
        //    title: "Are you sure?", text: "You want to create Request ?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    showLoaderOnConfirm:true
        //}, function () {
        $scope.SuccessMessage = "";
        swal({
            title: "Are you sure?",
            text: "You are going to upload!",
            showCancelButton: true,

            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Continue",
            showLoaderOnConfirm: true,
            preConfirm: (login) => {
                return WebAppService.SaveDocument(Obj).then(function (success) {
                    console.log("SUCCESS DATA", success.data);
                    $scope.SuccessMessage = success.data;
                    //if (success.data.indexOf('Success') != -1) {
                    //    swal("", success.data, "success");
                    //}
                    //else {
                    //    swal("", "Data Not Saved", "error");
                    //}

                },
                    function (error) {
                        console.log(error.data);
                    });
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if ($scope.SuccessMessage.indexOf('Success') != -1) {
                swal("", $scope.SuccessMessage, "success");
            }
            else {
                swal("", "Data Not Saved", "error");
            }
        });
    };

    $scope.Init();

});



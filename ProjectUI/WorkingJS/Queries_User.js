var app = angular.module('Queries_UserModule', []);

app.service('Queries_UserService', function ($http, $location) {

    this.SubmitQuery = function (data) {

        return $http.post('api/Queries/SaveQuery', data, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
});
app.controller('Queries_UserController', function ($scope, $http, $location, $window, Queries_UserService, $rootScope) {
    console.log("Queries_UserController");

    //uploader = new FileUploader();
    ////...
    //uploader.filters.push({
    //    'name': 'enforceMaxFileSize',
    //    'fn': function (item) {
    //        return item.size <= 10485760; // 10 MiB to bytes
    //    }
    //});

    $scope.init = function () {
        $scope.QuerySubject = '';
        $scope.QueryBody = '';
        $scope.Attachment = {};
    };
    $scope.clear = function () {
        angular.element("input[type='file']").val(null);
        $scope.Attachment1 = '';
        $scope.Attachment2 = '';
        $scope.Attachment3 = '';
        $scope.Attachment4 = '';
        $scope.Attachment5 = '';
        console.log($scope.Attachment1);
    };
    $scope.SubmitQuery = function () {

        if (!$scope.QuerySubject) {
            swal("Error", "Query subject can not be empty", "error");
            return false;
        }
        if (!$scope.QueryBody) {
            swal("Error", "Query description can not be empty", "error");
            return false;
        }

        //console.log($scope.Attachment1.size / 1024);
        swal({
            title: "Are you sure?",
            text: "You want to submit",
            icon: "warning",
            buttons: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Submit",
            showCancelButton: true,

            closeOnConfirm: true,
        }).then((result) => {
            if (result) {
                let Obj = {
                    QuerySubject: $scope.QuerySubject,
                    QueryBody: $scope.QueryBody,
                    User_Id: $rootScope.session.User_Id,
                    AttachmentType: $scope.Attachment != null ? $scope.Attachment.type : ''

                };
                var fd = new FormData();
                console.log($scope.Attachment1.size / 1024);
                fd.append('attachment', $scope.Attachment1);
                fd.append('attachment', $scope.Attachment2);
                fd.append('attachment', $scope.Attachment3);
                fd.append('attachment', $scope.Attachment4);
                fd.append('attachment', $scope.Attachment5);
                fd.append('data', angular.toJson(Obj));

                console.log(fd);
                Queries_UserService.SubmitQuery(fd).then(function success(data) {
                    console.log(data);
                    if (data.data.indexOf('Success') != -1) {
                        swal("Success", data.data, "success");
                        //$window.location.reload();
                        $scope.QuerySubject = null;
                        $scope.QueryBody = null;
                        $scope.Attachment.Val(null);
                    }
                    else
                        swal("Error", data.data, "error");
                    
                }, function (error) {
                    console.log(error);
                });
            }
        });
    }
});
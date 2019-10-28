var app = angular.module('NominationModule', [])
app.service('NominationService', function ($http, $location) {



    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/"

    }
    else {

        this.AppUrl = "/api/"
    }

	this.UploadUserMatrix = function (fd) {
		console.log(fd);
        return $http.post(this.AppUrl + '/Nomination/UploadUserMatrix', fd, {
			transformRequest: angular.identity,
			headers: { 'Content-Type': undefined }

		});	
	};
});


app.controller('NominationController', function (NominationService, $scope, $rootScope) {

	$scope.UploadExel = function () {
		console.log("hi");
		//if (!$scope.File1) {
		//	swal("Error", "Please upload the excel", "error");
		//	return false;
		//}
		console.log($scope.File1);
		var fd = new FormData();
		//var Obj = {
		//	Year: $scope.Year,
		//	Sem: $scope.Sem,
		//};
		//console.log("Obj", Obj);
		fd.append('file', $scope.File1);
        fd.append('data', angular.toJson($rootScope.session));
        console.log($rootScope.session);
		//fd.append('file', $scope.File1);
		NominationService.UploadUserMatrix(fd)

			.then(function (success) {

				console.log("SUCCESS DATA", success.data);
				if (success.data.indexOf('Success') !=-1) {
                    swal("", success.data, "success");
                }
                else {
                    swal("", success.data, "error");
                }
				//else if (success.data == 2) {
				//	swal("error", "Excel sheet not in format!", "error");
				//}
				//else if (success.data == 1) {

				//	var selectedsem = "H1";
				//	if ($scope.Sem == 2) {
				//		selectedsem = "H2";

				//	}
				//	swal("error", "Your Data is Empty For " + $scope.Year + " and " + selectedsem, "error");
				//}
				


				//swal("Save!", "Your file has been Uploaded.", "success");
			},
			function (error) {
				console.log(error.data);
				swal("error", error.data, "error");
			});
	};

});
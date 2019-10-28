var app = angular.module('ReportScoreSheetModule',[])

app.service('ReportScoreSheetService', function ($http) {

	this.GetReportSheetData = function () {
		return $http.get(this.AppUrl + '/ReportScoreSheet/GetReportSheetData')
	};
});


app.controller('ReportScoreSheetController', function (ReportScoreSheetService, $scope) {


	$scope.init = function () {
		ReportScoreSheetService.GetReportSheetData()
			.then(function (success) {
				if (success.data != null) {
					$scope.ReportSheetArray = success.data

				}
			},
			function (error) {
				console.log(error.data);


			});


    }

    $scope.init();
});
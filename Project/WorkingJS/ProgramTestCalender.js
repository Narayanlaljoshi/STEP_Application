var app = angular.module('ProgramTestCalenderModule', ['QuestionBankPractical']);

app.service('ProgramTestCalenderService', function ($http, $location) {

    this.AppUrl = "";
    this.ProgramId = null;
    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {

        this.AppUrl = "/CST/api/";

    }
    else {

        this.AppUrl = "/api/";
    }

    this.AddSaveProgramTestCalender = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/ProgramTestCalender/AddProgramTestCalender', data, {});
    };


    this.UpdateProgramTestCalender = function (data) {
        return $http.post(this.AppUrl + '/ProgramTestCalender/UpdateProgramTestCalender', data, {});
    };

    this.GetProgramTestCalenderList = function (ProgramId) {

        return $http.get(this.AppUrl + '/ProgramTestCalender/GetProgramTestCalenderList?ProgramId=' + ProgramId);
    };



    this.GetProgramList = function () {

        return $http.get(this.AppUrl + '/ProgramTestCalender/GetProgramList');
    };


    this.UploadExcel = function (fd) {

        console.log(fd);

        return $http.post(this.AppUrl + '/ProgramTestCalender/UploadExcel', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }

        });
    };



});
app.controller('ProgramTestCalenderController', function ($scope, $http, $location, ProgramTestCalenderService, ProgramMasterService,$rootScope) {

    //$scope.usedID = 0;


    
    console.log("ProgramMasterService.ProgramId ", !(ProgramMasterService.ProgramDeatil));
    $scope.ProgramTestCalenderDetailData = {
        ProgramTestCalender_Id: null,
        DayCount: null,
        ProgramCode: null,
        ProgramId: null,
        ProgramName: null,
        TestCode: null,
        Marks_Question: null,
        Q_Bank: null,
        Total_Marks: null,
        TestDuration: null,
        ValidDuration: null,
        TotalNoQuestion: null,
        TypeOfTest: null,
        // CreatedBy: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: null,
        ModifiedDate: null
    };


    $scope.ProgramTestCalenderData = [];

    //$scope.AddQuestion = function () {

    //    $scope.QuestionShow = true;


    //    $scope.ProgramTestCalenderArray.push({
    //        DayCount: '',
    //        IsActive: true,
    //        DayCount: '',
    //        IsActive: true,

    //    });
    //};



    $scope.init = function () {
        console.log('inside init');
        $scope.showProgramTestCalenderGrid = true;
        $scope.showAddBulkButton = true;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.QuestionShow = false;
        $scope.showBulk = false;
        // $scope.ProgramTestCalenderDetailData.ProgramId = '';
        $scope.ProgramTestCalenderData = [];

        $scope.showUpdataForm = [];

        console.log($rootScope.session.RoleName);

        console.log("user id",$rootScope.session.User_Id);
   
        if (!(ProgramMasterService.ProgramDeatil)) {
            window.location.assign('#/ProgramMaster');
        }
        else {
            
            ProgramTestCalenderService.GetProgramTestCalenderList(ProgramMasterService.ProgramDeatil.ProgramId).then(function success(data) {
                $scope.ProgramTestCalenderData = data.data;
                $scope.ProgramTestCalenderDetailData = $scope.ProgramTestCalenderData;

                angular.forEach($scope.ProgramTestCalenderData, function (value, key) {
                    
                    if (value.TotalNoQuestion <= value.QuesAdded)
                    { value.RowColor = '#9cd6cb';} 
                    else { value.RowColor = '#d6a19c';}
                });
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });





            ProgramTestCalenderService.GetProgramList().then(function success(data) {
                $scope.ProgramData = data.data;
                console.log("Get Program Data List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

        }
    };

    $scope.BackToProgramMaster = function () {
        window.location.assign('#/ProgramMaster');
    };
    $scope.AddProgramTestCalender = function () {

        $scope.showProgramTestCalenderGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        $scope.QuestionShow = false;
        $scope.showBulk = false;
        $scope.ProgramTestCalenderDetailData.ProgramId = ProgramMasterService.ProgramDeatil.ProgramId;
        //$scope.ProgramTestCalenderDetailData = [];
        $scope.ProgramTestCalenderDetailData.ProgramTestCalenderName = '';
        $scope.ProgramTestCalenderDetailData.ProgramTestCalenderCode = '';
        if (ProgramMasterService.ProgramDeatil.ProgramType_Id==1) {
            if ($scope.ProgramTestCalenderDetailData.length == 0) {
                var Day = 1;
                while ($scope.ProgramTestCalenderDetailData.length < ProgramMasterService.ProgramDeatil.Duration) {
                    $scope.ProgramTestCalenderDetailData.push();
                    var Obj = {
                        ProgramTestCalender_Id: null,
                        DayCount: Day,
                        ProgramCode: null,
                        ProgramId: null,
                        ProgramName: null,
                        TestCode: null,
                        Marks_Question: null,
                        Q_Bank: null,
                        Total_Marks: null,
                        TestDuration: null,
                        ValidDuration: null,
                        TotalNoQuestion: null,
                        TypeOfTest: null,
                        // CreatedBy: $rootScope.session.data.Account_Id,
                        IsActive: true,
                        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
                        CreationDate: null,
                        ModifiedBy: null,
                        ModifiedDate: null
                    };
                    $scope.ProgramTestCalenderDetailData.push(Obj);
                    Day += 1;
                }

            }
        }
        else {
            console.log(ProgramMasterService.ProgramDeatil);
            window.location.assign('#/ProgramTestCalender_Evaluation');
        }
    };

    $scope.AddRow = function () {
        console.log($scope.ProgramTestCalenderDetailData.length);
        var Obj = {
            ProgramTestCalender_Id: null,
            DayCount: null,
            ProgramCode: null,
            ProgramId: null,
            ProgramName: null,
            TestCode: null,
            Marks_Question: null,
            Q_Bank: null,
            Total_Marks: null,
            TestDuration: null,
            ValidDuration: null,
            TotalNoQuestion: null,
            TypeOfTest: null,
            // CreatedBy: $rootScope.session.data.Account_Id,
            IsActive: true,
            CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
            CreationDate: null,
            ModifiedBy: null,
            ModifiedDate: null
        };
        if (ProgramMasterService.ProgramDeatil.Duration > $scope.ProgramTestCalenderDetailData.length) {
            $scope.ProgramTestCalenderDetailData.push(Obj);
        }
        else {
            swal("","You can not create tests more than the program duration","warning");
        }
    };


    $scope.DeleteRow = function (Index) {
        if (Index != 0) {
            console.log(Index);
            $scope.ProgramTestCalenderDetailData.splice(Index, 1);

        }
    };
    $scope.GetRowColor = function (pt) {
        if (pt.TotalNoQuestion > pt.QuesAdded) {
            return '#d6a19c';
        }
        else {
            return '#9cd6cb';
        }
    };
    
    $scope.AddSaveProgramTestCalender = function () {



        //if (!$scope.ProgramTestCalenderDetailData.ProgramId) {
        //    swal('Please enter Program');

        //    return false;
        //}


        //if (!$scope.ProgramTestCalenderDetailData.DayCount) {
        //    swal('Please enter Day Count');
        //    return false;
        //}
        //if (!$scope.ProgramTestCalenderDetailData.TypeOfTest) {
        //    swal('Please enter Type Of Test');
        //    return false;
        //}
        //if (!$scope.ProgramTestCalenderDetailData.TotalNoQuestion) {
        //    swal('Please enter Total No Question ');
        //    return false;
        //}
        //if (!$scope.ProgramTestCalenderDetailData.TestDuration) {
        //    swal('Please enter Test Duration ');
        //    return false;
        //}
        $scope.KeepGoing = true;
        angular.forEach($scope.ProgramTestCalenderDetailData, function (value, key) {
            $scope.Count = 0;
            value.ProgramId = ProgramMasterService.ProgramDeatil.ProgramId;
            console.log(value);
            if (!value.DayCount || !value.TestCode || !value.EvaluationTypeId || !value.TypeOfTest || !value.TotalNoQuestion || !value.TestDuration || !value.ValidDuration || (!value.Marks_Question && value.EvaluationTypeId == 1)) {
                console.log(value);
                $scope.KeepGoing = false;
                swal("", "Empty fields are not allowed", "error"); return false;
            }
            else {
                angular.forEach($scope.ProgramTestCalenderDetailData, function (value1, key1) {
                    if (value.DayCount == value1.DayCount) {
                        $scope.Count++;
                    }
                    else {
                        console.log("Its Working", $scope.KeepGoing);
                        //continue;
                    }
                });
            }
            if ($scope.Count > 1) { $scope.KeepGoing = false; }
        });
        if ($scope.KeepGoing == true) {
            swal({
                title: 'Are you sure?',
                text: "You are going to create tests!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes'
            }).then((result) => {
                console.log("pppp:", $scope.ProgramTestCalenderDetailData);

                ProgramTestCalenderService.AddSaveProgramTestCalender($scope.ProgramTestCalenderDetailData).then(function success(retdata) {

                    if (retdata.data.indexOf("Success") !== -1) {
                        swal("", retdata.data, "success");
                        $scope.init();
                    }
                    else {
                        swal(retdata.data);
                    }



                }, function error(data) {
                    swal("Error in loading data from EDB");
                });
            });
        }
        else {
            swal("", "Duplicate and Empty fields are not allowed ", "warning");
        }
    };



    $scope.UpdateTransaction = function (pt) {

        // $scope.ProductCategory.ProductCategory = pt;

        $scope.ProgramTestCalenderDetailData = pt;

        $scope.showProgramTestCalenderGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = true;
        $scope.showAddForm = false;
        $scope.QuestionShow = false;

        console.log("UpdateTransaction", $scope.ProgramTestCalenderDetailData);

    };

    $scope.UpdateSaveProgramTestCalender = function () {
        if (!$scope.ProgramTestCalenderDetailData.ProgramId) {
            swal('Please enter Program');
            return false;
        }

        if (!$scope.ProgramTestCalenderDetailData.DayCount) {
            swal('Please enter Day Count');
            return false;
        }
        if (!$scope.ProgramTestCalenderDetailData.TypeOfTest) {
            swal('Please enter Type Of Test');
            return false;
        }
        if (!$scope.ProgramTestCalenderDetailData.TotalNoQuestion) {
            swal('Please enter Total No Question ');
            return false;
        }
        if (!$scope.ProgramTestCalenderDetailData.TestDuration) {
            swal('Please enter Test Duration ');
            return false;
        }


        //swal({
        //    title: "Are you sure?", text: "You are about update ProgramTestCalender?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //    function () {

        $scope.ProgramTestCalenderDetailData.IsActive = true;
        //    $scope.ProgramTestCalenderDetailData.ModifiedBy = $rootScope.session.data.User_Id;
        ProgramTestCalenderService.UpdateProgramTestCalender($scope.ProgramTestCalenderDetailData).then(function success(retdata) {

            if (retdata.data.indexOf("Success") !== -1) {
                swal(retdata.data);
            }
            else {
                swal(retdata.data);
            }

            $scope.init();



        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        // });
    };



    $scope.DeleteTransaction = function (obj) {

        //swal({
        //    title: "Are you sure?", text: "You want to delete this ProgramTestCalender?",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Continue",
        //    closeOnConfirm: false
        //},
        //  function () {
        obj.IsActive = false,
            //  obj.ModifiedBy = $rootScope.session.data.User_Id,
            ProgramTestCalenderService.UpdateProgramTestCalender(obj).then(function success(retdata) {

                if (retdata.data.indexOf("Success") !== -1) {
                    swal(retdata.data);
                    $scope.init();
                }
                else {
                    swal(retdata.data);
                }





            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

        //});
    };


    $scope.AddTransaction = function () {

        $scope.showProgramTestCalenderGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showAddForm = false;
        $scope.showUpdataForm = false;
        $scope.QuestionShow = false;
        $scope.QuestionShow = false;

        $scope.showBulk = true;

    };


    $scope.UploadExcelfunction = function () {

        console.log("UploadExcel ex");


        if (!$scope.File1) {
            swal("Error", "Please upload the excel", "error");
            return false;
        }

        console.log("upload");
        var file = $scope.File1;

        console.log(file);
        var fd = new FormData();
        fd.append('file', file);


        ProgramTestCalenderService.UploadExcel(fd).then(function (success) {
            swal("Save!", "Your file has been Uploaded.", "success");
            $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };


    $scope.AddQuestionBank = function (pt) {
        console.log(pt);
        ProgramTestCalenderService.id = pt.ProgramTestCalenderId;
        ProgramTestCalenderService.ProgramName = pt.ProgramName;
        ProgramTestCalenderService.ProgramId = pt.ProgramId;
        ProgramTestCalenderService.Day = pt.DayCount;
        ProgramTestCalenderService.TypeOfTest = pt.TypeOfTest;

        //console.log(ProgramTestCalenderService);
        if (pt.EvaluationTypeId == 2)
        {
            window.location.href = '#/QuestionBankPractical/';
        }
        else {
            window.location.href = '#/QuestionBank/';
        }
        
        //$scope.ProgramTestCalenderDetailData = pt;
        //$scope.showProgramTestCalenderGrid = false;
        //$scope.showAddBulkButton = false;
        //$scope.showUpdataForm = false;
        //$scope.showAddForm = false;
        //$scope.QuestionShow = true;

    };

  
    $scope.init();
});



var app = angular.module('ProgramTestCalender_EvaluationModule', ['QuestionBankPractical']);

app.service('ProgramTestCalender_EvaluationService', function ($http, $location) {

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
app.controller('ProgramTestCalender_EvaluationController', function ($scope, $http, $location, QuestionBankService, LanguageMasterService, ProgramTestCalenderService, ProgramTestCalender_EvaluationService, ProgramMasterService, $rootScope) {

    //$scope.usedID = 0;
   // ProgramMasterService.ProgramDeatil

    console.log("ProgramMasterService.ProgramId ", ProgramMasterService.ProgramDeatil);
    //$scope.ProgramTestCalenderDetailData = {

    //    ProgramTestCalender_Id: null,
    //    DayCount: null,
    //    ProgramCode: null,
    //    ProgramId: null,
    //    ProgramName: null,
    //    TestCode: null,
    //    Marks_Question: null,
    //    Q_Bank: null,
    //    Total_Marks: null,
    //    TestDuration: null,
    //    ValidDuration: null,
    //    TotalNoQuestion: null,
    //    TypeOfTest: null,
    //    // CreatedBy: $rootScope.session.data.Account_Id,
    //    IsActive: true,
    //    CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
    //    CreationDate: null,
    //    ModifiedBy: null,
    //    ModifiedDate: null

    //};


    $scope.ProgramTestCalenderData = [];

   

    $scope.init = function () {
        console.log('inside init');
        $scope.showProgramTestCalenderGrid = true;
        $scope.BulkUpload = false;
        $scope.LanguageArray = [];
        // $scope.ProgramTestCalenderDetailData.ProgramId = '';
        $scope.ProgramTestCalenderData = [];
        //$scope.ProgramId = ProgramTestCalenderService.ProgramDeatil.ProgramId;
        $scope.showUpdataForm = [];
        console.log($rootScope.session.RoleName);

        console.log("user id", $rootScope.session.User_Id);
        //QuestionBankService.GetPrgramLanguage($scope.ProgramId).then(function success(data) {
        //    $scope.ProgramLanguageArray = data.data;
        //    console.log("Program Language Array List", $scope.ProgramLanguageArray);
        //}, function error(data) {
        //    console.log("Error in loading data from EDB");
        //});

        if (!(ProgramMasterService.ProgramDeatil)) {
            window.location.assign('#/ProgramMaster');
        }
        else {
            $scope.ProgramDeatil = ProgramMasterService.ProgramDeatil;
            ProgramTestCalender_EvaluationService.GetProgramTestCalenderList(ProgramMasterService.ProgramDeatil.ProgramId).then(function success(data) {
                //$scope.ProgramTestCalenderData = data.data;
                $scope.ProgramTestCalenderDetailData = data.data;

                //angular.forEach($scope.ProgramTestCalenderData, function (value, key) {
                //    if (value.TotalNoQuestion < value.QuesAdded) { value.RowColor = '#9cd6cb'; }
                //    else { value.RowColor = '#d6a19c'; }
                //});
                console.log("$scope.ProgramTestCalenderDetailData", $scope.ProgramTestCalenderDetailData.length);
               
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

            ProgramTestCalender_EvaluationService.GetProgramList().then(function success(data) {
                $scope.ProgramData = data.data;
                console.log("Get Program Data List", data);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
            $scope.AddProgramTestCalender();
        }
    };

    $scope.BackToProgramMaster = function () {
        window.location.assign('#/ProgramMaster');
    };
    $scope.BackToProgramTestCalender = function () {
        window.location.assign('#/ProgramTestCalender');
    }; 
    $scope.AddProgramTestCalender = function () {
        $scope.showProgramTestCalenderGrid = false;
        $scope.showAddBulkButton = false;
        $scope.showUpdataForm = false;
        $scope.showAddForm = true;
        $scope.QuestionShow = false;
        $scope.showBulk = false;
        console.log(!$scope.ProgramTestCalenderDetailData);
        if (!$scope.ProgramTestCalenderDetailData) {
            var Day = 1;
            $scope.ProgramTestCalenderDetailData = [];
            
            var Obj = {
                ProgramTestCalender_Id: null,
                DayCount: Day,
                ProgramCode: ProgramMasterService.ProgramDeatil.ProgramCode,
                ProgramId: ProgramMasterService.ProgramDeatil.ProgramId,
                ProgramName: ProgramMasterService.ProgramDeatil.ProgramName,
                TestCode: null,
                Marks_Question: null,
                Q_Bank: null,
                Total_Marks: null,
                TestDuration: null,
                ValidDuration: null,
                TotalNoQuestion: null,
                TypeOfTest: null,
                IsActive: true,
                CreatedBy: $rootScope.session.User_Id,
                CreationDate: null,
                ModifiedBy: null,
                ModifiedDate: null
            };
            $scope.ProgramTestCalenderDetailData.push(Obj);
            Day += 1;
            console.log("$scope.ProgramTestCalenderDetailData", $scope.ProgramTestCalenderDetailData);
        }

    };

    $scope.AddRow = function () {
        console.log($scope.ProgramTestCalenderDetailData.length);
        var Index = $scope.ProgramTestCalenderDetailData.length-1;
        var Obj = {
            ProgramTestCalender_Id: null,
            DayCount: $scope.ProgramTestCalenderDetailData.length+1,
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
        //if (ProgramMasterService.ProgramDeatil.Duration > $scope.ProgramTestCalenderDetailData.length) {
            $scope.ProgramTestCalenderDetailData.push(Obj);
        //}
        //else {
        //    swal("", "You can not create tests more than the program duration", "warning");
        //}
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
        //if (!$scope.ProgramTestCalenderDetailData.EvaluationTypeId) {
        //    swal('Please select Evaluation Type');
        //    return false;
        //}

        //if (!$scope.ProgramTestCalenderDetailData.TypeOfTest) {
        //    swal('Please enter Type Of Test');
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
            //console.log(value);
            //console.log(!value.TotalNoQuestion || !value.Marks_Question);
            //console.log(!value.PracticalDefaultMarks || !value.PracticalMaxMarks || !value.PracticalMinMarks);
            //console.log(!value.DayCount || !value.TestCode || !value.EvaluationTypeId || !value.TypeOfTest || !value.TestDuration || !value.ValidDuration);
            //if (value.TypeOfTest == 1) {
            //    console.log("");
            //    if (!value.TotalNoQuestion || !value.Marks_Question) $scope.KeepGoing = false;
            //}
            //else if (value.TypeOfTest == 2){
            //    console.log("");
            //    if (!value.PracticalDefaultMarks || !value.PracticalMaxMarks || !value.PracticalMinMarks) $scope.KeepGoing = false;
            //}
            if (!value.DayCount || !value.TestCode || !value.EvaluationTypeId || !value.TypeOfTest  || !value.ValidDuration) { console.log(value);
                console.log("");
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
            if ($scope.Count > 1) {
                console.log("");
                $scope.KeepGoing = false;
            }
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

                ProgramTestCalender_EvaluationService.AddSaveProgramTestCalender($scope.ProgramTestCalenderDetailData).then(function success(retdata) {

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
        ProgramTestCalender_EvaluationService.UpdateProgramTestCalender($scope.ProgramTestCalenderDetailData).then(function success(retdata) {

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
            ProgramTestCalender_EvaluationService.UpdateProgramTestCalender(obj).then(function success(retdata) {

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


        ProgramTestCalender_EvaluationService.UploadExcel(fd).then(function (success) {
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

        ProgramTestCalender_EvaluationService.id = pt.ProgramTestCalenderId;
        ProgramTestCalender_EvaluationService.ProgramName = pt.ProgramName;

        ProgramTestCalender_EvaluationService.ProgramId = pt.ProgramId;
        ProgramTestCalender_EvaluationService.Day = pt.DayCount;
        ProgramTestCalender_EvaluationService.TypeOfTest = pt.TypeOfTest;
        
        console.log(pt.EvaluationTypeId);
        if (pt.EvaluationTypeId == 2) {//window.location.assign('#/ProgramMaster');
            //console.log("QuestionBankPractical");
            window.location.href = 'http://localhost:52267/index.html#/QuestionBankPractical';
        }
        else {
            //console.log("QuestionBank");
            window.location.href = 'http://localhost:52267/index.html#/QuestionBank';
        }
    };

    $scope.init();
});



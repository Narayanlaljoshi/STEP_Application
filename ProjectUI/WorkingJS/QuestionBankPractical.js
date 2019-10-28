var app = angular.module('QuestionBankPractical', []);

app.service('QuestionBankPracticalService', function ($http, $location) {

    this.AppUrl = "";

    console.log($location.absUrl());

    if ($location.absUrl().indexOf('CST') != -1) {
        this.AppUrl = "/CST/api";
    }
    else {

        this.AppUrl = "/api";
    }
    this.AddSaveQuestionBank = function (data) {
        //       console.log("yahiooooooooooo");
        return $http.post(this.AppUrl + '/QuestionBank/AddQuestionBank', data, {});
    };

    this.UpdateSaveQuestionBank = function (data) {
        return $http.post(this.AppUrl + '/QuestionBank/UpdateQuestionBank', data, {});
    };

    this.UploadExcel = function (fd) {
        return $http.post(this.AppUrl + '/QuestionBank/UploadExcel_Practical', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.UpdateImage_Practical = function (fd) {                                //----------------------Add Question with image (if any) ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/UpdateImage_Practical', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
    this.GetSetSequenceList = function () {
        return $http.get(this.AppUrl + '/QuestionBank/GetSetSequenceList' );
    };
    this.GetQuestionBankList = function (id,Set_Id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetPracticalQuestionBankList/?id=' + id + '&Set_Id=' + Set_Id);
    };
    this.GetQuestionBankList1 = function (Obj) {
        return $http.post(this.AppUrl + '/QuestionBank/GetPracticalQuestionBankList',Obj);
    };
    this.GetQuestionBankTemplate = function (Obj) {
        return $http.post(this.AppUrl + '/QuestionBank/GetPracticalQuestionTemplate',Obj);
    };
    this.GetLanguageList = function () {
        return $http.get(this.AppUrl + '/LanguageMaster/GetLanguage');
    };
    this.GetPrgramLanguage = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetLanguage?id=' + id);
    };

    this.UploadLanguage = function (fd) {                                //----------------------upload excel against Question (LANGUAGE) ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/UploadLanguage', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.DeleteQuestion = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/DeleteQuestion?id=' + id);
    };

    this.DeleteAllSelected = function (Obj) {
        return $http.post(this.AppUrl + '/QuestionBank/DeleteQuestion', Obj);
    };

    this.GetQuestionFormatedList = function (data) {
        return $http.post(this.AppUrl + '/QuestionBank/GetQuestionFormatedList', data, {});
        //return $http.get(this.AppUrl + '/QuestionBank/GetQuestionFormatedList?id=' + id +'&LangId=' + LangId);
    };
    this.UpdateImage_Practical = function (fd) {                                //----------------------Add Question with image (if any) ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/UpdateImage_Practical', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
    this.BulkUpload_PracticalQuestions = function (fd) {                                //----------------------Add Question with image (if any) ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/BulkUpload_PracticalQuestions', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
    this.GetSetSequenceByProgramTestCalanderId = function (id) {
        return $http.get(this.AppUrl + '/Test/GetSetSequenceByProgramTestCalanderId?Id=' + id);
    };
});

app.controller('QuestionBankPracticalController', function ($scope, $http, $location, QuestionBankPracticalService, ProgramTestCalenderService, $rootScope) {
    $scope.ShowTable = true;
    $scope.BulkUpload = false;
    $scope.ImageUpload = false;
    $scope.LanguageWise = false;
    $scope.QuestionDetailData = {
        Id: 0,
        ProgramTestCalenderId: ProgramTestCalenderService.id,
        QuestionCode: null,
        Question: null,
        ActionA: null,
        ActionB: null,
        ActionC: null,
        ActionD: null,
        ActionE: null,
        ActionF: null,
        Image: null,
        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: $rootScope.session.User_Id,
        ModifiedDate: null
    };
    $scope.Filter = {
        Set_Id: '1',
        LanguageMaster_Id: 1,
        CreatedBy: '',
        ModifiedBy:''//Filter.LanguageMasterId
    };
    $scope.GetSetSequenceListsFor = function () {
        QuestionBankPracticalService.GetSetSequenceList().then(function success(data) {
            $scope.SetSequence = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
        QuestionBankPracticalService.GetSetSequenceByProgramTestCalanderId(ProgramTestCalenderService.id).then(function success(data) {
            $scope.SetSequenceWithTitle = data.data;
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.init = function () {
        $scope.ShowDeleteAllButton = false;
        console.log('inside init', ProgramTestCalenderService.id);
        console.log(ProgramTestCalenderService);
        $scope.TestType = ProgramTestCalenderService.TypeOfTest;
        $scope.Day = ProgramTestCalenderService.Day;
        $scope.ProgramName = ProgramTestCalenderService.ProgramName;
        $scope.ProgramId = ProgramTestCalenderService.ProgramId;
        $scope.ProgramCode = ProgramTestCalenderService.ProgramCode;

        $scope.ShowTable = true;
        $scope.BulkUpload = false;
        $scope.ImageUpload = false;
        $scope.Filter = {
            Set_Id: 1,
            LanguageMaster_Id: 1//Filter.LanguageMasterId
        };
        $scope.QuestionDetailData = {};

        if (ProgramTestCalenderService.id) {
            QuestionBankPracticalService.GetQuestionBankList(ProgramTestCalenderService.id,1).then(function success(data) {
                $scope.QuestionBankData = data.data;
                $scope.BulkUpload = false;
                console.log("Get Question Bank Service Data List", $scope.QuestionBankData);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

            QuestionBankPracticalService.GetLanguageList().then(function success(data) {
                $scope.LanguageArray = data.data;
                console.log("LanguageArray List", $scope.LanguageArray);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
            //--------------bulk language screen ------- dropdown-- //
            //QuestionBankPracticalService.GetSetSequenceByProgramTestCalanderId(ProgramTestCalenderService.id).then(function success(data) {
            //    $scope.SetSequenceWithTitle = data.data;
            //}, function error(data) {
            //    console.log("Error in loading data from EDB");
            //});
            QuestionBankPracticalService.GetPrgramLanguage($scope.ProgramId).then(function success(data) {
                $scope.ProgramLanguageArray = data.data;
                console.log("Program Language Array List", $scope.ProgramLanguageArray);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            window.location.href = '#/ProgramTestCalender/';
            console.log("hello");
        }
    };

    $scope.UploadLanguageExcelfunction = function (id) {
        console.log(id);

        console.log("UploadExcel ex");
        if (!id) {
            swal("Error", "Please Select language", "error");
            return false;
        }
        else if (!$scope.File2) {
            swal("Error", "Please upload the excel", "error");
            return false;
        }
        console.log($scope.File2);
        var fd = new FormData();
        var Obj = {
            LanguageMaster_Id: id,
            CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
            ModifiedBy: $rootScope.session.User_Id
        };
        console.log("Obj", Obj);
        fd.append('file', $scope.File2);
        fd.append('data', angular.toJson(Obj));
        console.log("upload");
        console.log(fd);
        QuestionBankPracticalService.UploadLanguage(fd).then(function (success) {
            swal("Save!", "Your file has been Uploaded.", "success");
            $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };

    $scope.UploadExcelfunction = function () {
        console.log("UploadExcel ex");
       
        if (!$scope.Set_Id) {
            swal("", "Please Select Set", "warning");
            return false;
        }
        if (!$scope.Set_Title)
        {
            swal("", "Please Enter Set Title", "warning");
            return false;
        }
        if (!$scope.TestDuration) {
            swal("", "Please Enter Test Duration", "warning");
            return false;
        }
        if (!$scope.File1) {
            swal("Error", "Please Select Excel File", "error");
            return false;
        } 
        console.log($scope.files);
        var fd = new FormData();
        var Obj = {
            ProgramTestCalenderId: ProgramTestCalenderService.id,
            CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
            ModifiedBy: $rootScope.session.User_Id,
            Set_Id: $scope.Set_Id,
            Set_Title: $scope.Set_Title,
            TestDuration: $scope.TestDuration
        };
        console.log("Obj", Obj);
        fd.append('file', $scope.File1);
        fd.append('data', angular.toJson(Obj));
        QuestionBankPracticalService.UploadExcel(fd).then(function (success) {
            if (success.data.indexOf('Success') != -1)
            { swal("Save!", "Your file has been Uploaded.", "success"); }
            else { swal("", success.data, "error");}
            $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };
    
    $scope.UploadLanguageWise = function () {
        console.log("click is working");
        $scope.ImageUpload = false;
        $scope.BulkUpload = false;
        $scope.ShowTable = false;
        $scope.LanguageWise = true;
    };

    $scope.UploadBulk = function () {
        console.log("click is working");
        $scope.ImageUpload = false;
        $scope.BulkUpload = true;
        $scope.ShowTable = false;
        $scope.LanguageWise = false;
    };

    $scope.BackToQuestionScreen = function (pt) {
        console.log("click is working");
        $scope.ImageUpload = false;
        $scope.BulkUpload = false;
        $scope.ShowTable = true;
    };

    $scope.UpdateImages = function (pt) {
        console.log("click is working");
        $scope.ImageUpload = true;
        $scope.BulkUpload = false;
        $scope.ShowTable = false;
        console.log($scope.ImageUpload, $scope.BulkUpload, $scope.ShowTable);
        $scope.QuestionDetails = pt;
        $scope.Files = {
            QuestionImage: null,
            ActionAImage: null,
            ActionBImage: null,
            ActionCImage: null,
            ActionDImage: null,
            ActionEImage: null
        };
    };

    $scope.UpdateImage_Practical = function () {
        console.log($scope.QuestionDetails);
        $scope.Continue = false;
        
        if ($scope.Files.QuestionImage) {
            $scope.Continue = true;
        }
        else if ($scope.Files.ActionAImage) {
            $scope.Continue = true;
        }
        else if ($scope.Files.ActionBImage) {
            $scope.Continue = true;
        }
        else if ($scope.Files.ActionCImage) {
            $scope.Continue = true;
        }
        else if ($scope.Files.ActionDImage) {
            $scope.Continue = true;
        }
        else if ($scope.Files.ActionEImage) {
            $scope.Continue = true;
        }
        if (!$scope.Continue) {
            swal("Error", "Please upload atleast one image", "error");
            return false;
        }
        console.log($scope.Files);
        var fd = new FormData();
        //var Obj = {

        //    ProgramTestCalenderId: ProgramTestCalenderService.id,
        //    CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        //    ModifiedBy: $rootScope.session.User_Id,
        //    Set_Id: $scope.Set_Id
        //};
        //console.log("Obj", Obj);
        fd.append('QuestionImage', $scope.Files.QuestionImage);
        fd.append('ActionAImage', $scope.Files.ActionAImage);
        fd.append('ActionBImage', $scope.Files.ActionBImage);
        fd.append('ActionCImage', $scope.Files.ActionCImage);
        fd.append('ActionDImage', $scope.Files.ActionDImage);
        fd.append('ActionEImage', $scope.Files.ActionEImage);
        fd.append('data', angular.toJson($scope.QuestionDetails));



        console.log("upload");
        // var file = $scope.File1;

        // console.log(file);
        //var fd = new FormData();
        //  fd.append('file', file);


        console.log(fd);

        QuestionBankPracticalService.UpdateImage_Practical(fd).then(function (success) {
            swal("Save!", "Your file has been Uploaded.", "success");
            $scope.Files = [];
            $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };

    $scope.UpdateTransaction = function (pt) {

        console.log(pt);
        $scope.QuestionDetailData = pt;
        console.log($scope.QuestionDetailData);
        console.log($scope.file);

        $scope.showAddBulkButton = false;

        $scope.showQuestionGrid = false;
        $scope.AddGrid = false;
        $scope.AddLanguageGrid = false;
        $scope.BulkGrid = false;

        $scope.UploadImageShow = true;


    };

    $scope.DownloadTemplate = function () {
        $scope.Filter.ProgramTestCalenderId = ProgramTestCalenderService.id;
        $scope.Filter.LanguageMaster_Id = null;
        QuestionBankPracticalService.GetQuestionBankTemplate($scope.Filter).then(function success(data) {
            $scope.QuestionBankTemplate = data.data;
            var mystyle = {
                headers: true,
            };

            alasql.promise('SELECT QuestionCode,Question,ActionA,ActionB,ActionC,ActionD,ActionE,ActionF,OtherLanguageQuestion,OtherLanguageActionA,OtherLanguageActionB,OtherLanguageActionC,OtherLanguageActionD,OtherLanguageActionE,OtherLanguageActionF INTO XLSX("PracticalQuestionTemplate.xlsx",?) FROM ?', [mystyle, $scope.QuestionBankTemplate]);

        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };

    $scope.updateImage = function () {
        console.log($scope.QuestionDetailData);
        var fd = new FormData();
        fd.append('file', $scope.Image);
        fd.append('data', angular.toJson($scope.QuestionDetailData));
        console.log(fd);
        QuestionBankPracticalService.UploadData(fd).then(function (success) {
            //  swal("Save!", "Your file has been Uploaded.", "success");
            swal({
                title: 'Success',
                text: "Your File uploaded Sucessfully !",
                type: 'success',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                //  cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then((result) => {
                // if (result.value) {

                $scope.Image = null;
                $scope.init();
                //window.location.reload();
                //}
            });
            // $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };

    $scope.DeleteTransaction = function (dt) {
        console.log(dt);
        // DetailId
        QuestionBankPracticalService.DeleteQuestion(dt.DetailId).then(function success(data) {

            console.log("Sucessfully Updated", data);
            $scope.init();
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });


    };

    $scope.BulkUpload_PracticalQuestions = function () {

        if (!$scope.LanguageQuestionExcel) { swal("", "Please Select File", "warning"); return false; }
        else if (!$scope.Filter.LanguageMaster_Id) { swal("", "Please Select Language", "warning"); return false; }

        $scope.Filter.CreatedBy = $rootScope.session.User_Id;
        $scope.Filter.ModifiedBy = $rootScope.session.User_Id;
        var fd = new FormData();
        fd.append('file', $scope.LanguageQuestionExcel);
        fd.append('data', angular.toJson($scope.Filter));
        console.log(fd);
        QuestionBankPracticalService.BulkUpload_PracticalQuestions(fd).then(function (success) {
            console.log(success.data.indexOf('Success') != -1);
            if (success.data.indexOf('Success') != -1) {
                swal({
                    title: 'Success',
                    text: "Your File uploaded Sucessfully !",
                    type: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    $scope.init();
                });
                // $scope.init();

            }
            else {
                swal("", success.data, "error");
            }
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
    };


    $scope.Export = function () {
        //console.log('inside init');
        alasql.fn.datetime = function (dateStr) {

            var date = $filter('date')(dateStr, 'MMMM dd, yyyy');
            return date;
        };

        alasql.promise('SELECT QuestionCode,Question,Answer1  A,Answer2  B,Answer3  C,Answer4  D,QueInOther  QueInOther,AnswerA,AnswerB,AnswerC,AnswerD INTO XLSX("QuestionLanguage.xlsx",{headers:true}) FROM ? ', [$scope.QuestionBankData]).then(function (data) {
            console.log('Data saved');
        }).catch(function (err) {
            console.log('Error:', err);
        });

    };

    $scope.ChangeFormat = function (pt) {
        // console.log(pt);
        $scope.Filter.ProgramTestCalenderId = ProgramTestCalenderService.id;
        QuestionBankPracticalService.GetQuestionBankList1($scope.Filter).then(function success(data) {
            $scope.QuestionBankData = data.data;
            $scope.BulkUpload = false;
            console.log("Get Question Bank Service Data List", $scope.QuestionBankData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });

        QuestionBankPracticalService.GetQuestionFormatedList(QuestionVariable).then(function success(data) {
            $scope.QuestionBankData = data.data;
            console.log("Get Question Bank Filtered Data List", $scope.QuestionBankData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.Back = function () {
        window.location.href = '#/ProgramTestCalender/';
    };
    $scope.init();
    $scope.GetSetSequenceListsFor();
});



















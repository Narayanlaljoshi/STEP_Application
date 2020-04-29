var app = angular.module('QuestionBankModule', []);

app.service('QuestionBankService', function ($http, $location) {
    
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
        //----------------------upload excel against Question ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/UploadExcel', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.UploadData = function (fd) {
        //----------------------Add Question with image (if any) ------------------//
        return $http.post(this.AppUrl + '/QuestionBank/UploadData', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };

    this.GetQuestionBankList = function (id, Set_Id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetQuestionBankList?id=' + id + '&Set_Id=' + Set_Id);
    };

    this.GetLanguageList = function () {
        return $http.get(this.AppUrl + '/LanguageMaster/GetLanguage');
    };
    this.GetSetSequenceList = function () {
        return $http.get(this.AppUrl + '/QuestionBank/GetSetSequenceList');
    };
    this.GetPrgramLanguage = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/GetLanguage?id=' + id);
    };

    this.UploadLanguage = function (fd) {
        return $http.post(this.AppUrl + '/QuestionBank/UploadLanguage', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    };
    
    this.DeleteQuestion = function (id) {
        return $http.get(this.AppUrl + '/QuestionBank/DeleteQuestion?id=' + id);
    };

    this.DeleteAllSelected = function (Obj) {
        return $http.post(this.AppUrl + '/QuestionBank/DeleteQuestion',Obj);
    };

    this.GetQuestionFormatedList = function (data) {
        return $http.post(this.AppUrl + '/QuestionBank/GetQuestionFormatedList', data, {});
    };
});
app.controller('QuestionBankController', function ($scope, $http, $location, QuestionBankService, ProgramTestCalenderService, $rootScope) {
    $scope.ShowDeleteAllButton = false;
    $scope.SelectAll = false;
    $scope.QuestionDetailData = {

        DetailId: 0,
        Question: null,
        QuestionCode: null,

        Answer1: null,
        Answer2: null,
        Answer3: null,
        Answer4: null,
        AnswerKey: null,
        Image: null,
        // Account_Id: $rootScope.session.data.Account_Id,
        IsActive: true,
        CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
        CreationDate: null,
        ModifiedBy: $rootScope.session.User_Id,
        ModifiedDate: null

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
        $scope.QuestionPaperType = ProgramTestCalenderService.QuestionPaperType;
        $scope.Set_Id = $scope.QuestionPaperType == 'QB' ? 0 : 1;
        $scope.showAddBulkButton = true;

        $scope.showQuestionGrid = true;
        $scope.AddGrid = false;
        $scope.AddLanguageGrid = false;

        $scope.BulkGrid = false;
        $scope.UploadImageShow = false;
        $scope.Image = null;


        $scope.File2 = null;
        $scope.showQuestionGrid = true;

        $scope.QuestionDetailData = {};

        if (ProgramTestCalenderService.id) {
            QuestionBankService.GetQuestionBankList(ProgramTestCalenderService.id, $scope.Set_Id).then(function success(data) {
                $scope.QuestionBankData = data.data;
                console.log("Get Question Bank Service Data List", $scope.QuestionBankData);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

            QuestionBankService.GetLanguageList().then(function success(data) {
                $scope.LanguageArray = data.data;
                console.log("LanguageArray List", $scope.LanguageArray);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
            //--------------bulk language screen ------- dropdown-- //
            QuestionBankService.GetPrgramLanguage($scope.ProgramId).then(function success(data) {
                $scope.ProgramLanguageArray = data.data;
                console.log("Program Language Array List", $scope.ProgramLanguageArray);
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });

            QuestionBankService.GetSetSequenceList().then(function success(data) {
                $scope.SetSequence = data.data;
            }, function error(data) {
                console.log("Error in loading data from EDB");
            });
        }
        else {
            window.location.href = '#/ProgramTestCalender/';
        }
    };
    $scope.GetQuestionBankList = function () {
        QuestionBankService.GetQuestionBankList(ProgramTestCalenderService.id, $scope.Set_Id).then(function success(data) {
            $scope.QuestionBankData = data.data;
            console.log("Get Question Bank Service Data List", $scope.QuestionBankData);
        }, function error(data) {
            console.log("Error in loading data from EDB");
        });
    };
    $scope.SelectAllQues = function () {
        if ($scope.SelectAll == true) {
            $scope.ShowDeleteAllButton = true;
            angular.forEach($scope.QuestionBankData, function (value, key) {
                value.IsChecked = true;
            });
        }
        else {
            $scope.ShowDeleteAllButton = false;
            angular.forEach($scope.QuestionBankData, function (value, key) {
                value.IsChecked = false;
            });
        }
    };
    $scope.SelectedQuestions = function () {

        $scope.IsAnySelected = false;
        angular.forEach($scope.QuestionBankData, function (value, key) {
            if (value.IsChecked == true) { $scope.IsAnySelected = true;}
        });
        if ($scope.IsAnySelected == true) {
            $scope.ShowDeleteAllButton = true;
        }
        else {
            $scope.ShowDeleteAllButton = false;
        }
    };
    $scope.DeleteAllSelected = function () {

        swal({
            title: 'Are you sure?',
            text: "You are going to delete questions",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            $scope.SelectedQuestions = [];
            angular.forEach($scope.QuestionBankData, function (value, key) {
                if (value.IsChecked == true) {
                    $scope.SelectedQuestions.push(value.DetailId);
                    console.log($scope.SelectedQuestions);
                }
            });

            QuestionBankService.DeleteAllSelected($scope.SelectedQuestions).then(function (success) {
                swal("Save!", "Questions have been added sucessfully.", "success");
                $scope.init();
            },
                function (error) {
                    console.log(error.data);
                    swal("error", error.data, "error");
                });
        });

    };
    $scope.AddQuestionBank = function () {
        $scope.QuestionDetailData = {
            Answer1:null,
            Answer2:null,
            Answer3:null,
            Answer4:null,
            AnswerKey:null,
            Question:null
        };
        $scope.showAddBulkButton = false;

        $scope.showQuestionGrid = false;
        $scope.AddGrid = true;
        $scope.AddLanguageGrid = false;
        $scope.BulkGrid = false;
        $scope.UploadImageShow = false;

    };


    $scope.AddBulk = function () {
        $scope.showAddBulkButton = false;

        $scope.showQuestionGrid = false;
        $scope.AddGrid = false;
        $scope.AddLanguageGrid = false;
        $scope.BulkGrid = true;

        $scope.UploadImageShow = false;

    };

    $scope.AddSaveQuestion = function () {       
        if (!$scope.QuestionDetailData.Question) { swal("","Please type Question", "error"); return false;}
        if (!$scope.QuestionDetailData.Answer1) { swal("","Please type answer-a", "error"); return false; }
        if (!$scope.QuestionDetailData.Answer2) { swal("","Please type answer-b", "error"); return false;}
        if (!$scope.QuestionDetailData.Answer3) { swal("","Please type answer-c", "error"); return false; }
        if (!$scope.QuestionDetailData.Answer4) { swal("","Please type answer-d", "error"); return false;}
        if (!$scope.QuestionDetailData.AnswerKey) { swal("","Please type answer-key", "error"); return false;}
        swal({
            title: 'Are you sure?',
            text: "You are going to add question!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {

                $scope.QuestionDetailData.ProgramTestCalenderId = ProgramTestCalenderService.id;

                console.log("Question Detail Data", $scope.QuestionDetailData);

                console.log($scope.Image);
                var fd = new FormData();

                fd.append('file', $scope.Image);
                fd.append('data', angular.toJson($scope.QuestionDetailData));

                console.log(fd);


                QuestionBankService.UploadData(fd).then(function (success) {
                    swal("Save!", "Questions have been added sucessfully.", "success");
                    $scope.init();
                },
                    function (error) {
                        console.log(error.data);
                        swal("error", error.data, "error");
                    });          
      });

    };

    $scope.AddLanguage = function () {
        $scope.showAddBulkButton = false;

        $scope.showQuestionGrid = false;
        $scope.AddGrid = false;
        $scope.AddLanguageGrid = true;
        $scope.BulkGrid = false;

        $scope.UploadImageShow = false;

    };

    $scope.UploadLanguageExcelfunction = function (id)
    {
        console.log(id);

        console.log("UploadExcel ex");


        if (!id)  {
            swal("Error", "Please Select language", "error");
            return false;
        }
        else if (!$scope.File2)
        {
            swal("Error", "Please upload the excel", "error");
            return false;
        }


        console.log($scope.File2);
        var fd = new FormData();
        var Obj = {            
            LanguageMasterId: id,
            CreatedBy: $rootScope.session.User_Id,            //$rootScope.session.data.User_Id,
            ModifiedBy: $rootScope.session.User_Id
        };
        console.log("Obj", Obj);
        fd.append('file', $scope.File2);
        fd.append('data', angular.toJson(Obj));



        console.log("upload");
        

        console.log(fd);

        QuestionBankService.UploadLanguage(fd).then(function (success) {
            swal("Save!", "Your file has been Uploaded.", "success");
            $scope.init();
        },
            function (error) {
                console.log(error.data);
                swal("error", error.data, "error");
            });
        
    };

    $scope.UploadExcelfunction = function () {

        if (!$scope.File1) {
            swal("Error", "Please upload the excel", "error");
            return false;
        }
        if (ProgramTestCalenderService.QuestionPaperType === "Set" && !$scope.Set_Id) {
            swal("", "Please select Set Sequence", "warning");
            return false;
        }
        console.log($scope.files);
        var fd = new FormData();
        var Obj = {
            ProgramTestCalenderId: ProgramTestCalenderService.id,
            CreatedBy: $rootScope.session.User_Id,
            ModifiedBy: $rootScope.session.User_Id,
            Set_Id: $scope.Set_Id
        };
        console.log("Obj", Obj);
        fd.append('file', $scope.File1);
        fd.append('data', angular.toJson(Obj));

        QuestionBankService.UploadExcel(fd).then(function (success) {
            if (success.data.indexOf('Success') != -1) {
                swal("Save!", "Your file has been Uploaded.", "success");
                $scope.init();
            }
            else {
                swal("", success.data,"error");
            }
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
       
    $scope.updateImage = function () {

        console.log($scope.QuestionDetailData);

        var fd = new FormData();

        fd.append('file', $scope.Image);
        fd.append('data', angular.toJson($scope.QuestionDetailData));

        console.log(fd);

        QuestionBankService.UploadData(fd).then(function (success) {
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
        QuestionBankService.DeleteQuestion(dt.DetailId).then(function success(data) {
           
            console.log("Sucessfully Updated", data);
            $scope.init();
        }, function error(data) {
            console.log("Error in loading data from EDB");
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

        var QuestionVariable = {
            id: ProgramTestCalenderService.id,
            LangId: pt,
            Set_Id: $scope.Set_Id
        };
        console.log(ProgramTestCalenderService.id);
        console.log(QuestionVariable);

        QuestionBankService.GetQuestionFormatedList(QuestionVariable).then(function success(data) {
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
});



















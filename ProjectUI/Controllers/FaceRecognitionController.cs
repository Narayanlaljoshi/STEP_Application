using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectUI.Controllers
{
    public class FaceRecognitionController : ApiController
    {
        [HttpGet]
        public FaceRecognitionDataBLL GetFaceRecognitionData(string MSPIN)
        {
            return FaceRecognitionData.GetFaceRecognitionData(MSPIN);
        }

        [HttpPost]
        public string UpdateFaceRecognitionData(FaceRecognitionDataBLL Obj)
        {
            return FaceRecognitionData.UpdateFaceRecognitionData(Obj);
        }
    }
}

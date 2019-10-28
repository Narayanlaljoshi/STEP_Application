
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectBLL.CustomModel;
using STEPDAL.CustomDAL;

namespace Project.Controllers
{
    public class LoginController : ApiController
    {

        [HttpGet]
        public UserDetailsBLL Login(string UserName, string Password)
        {
            return UserDAL.Login(UserName, Password);
        }
        [HttpGet]
        public int AuthUser(string UserName)
        {
            return UserDAL.AuthUser(UserName);
        }
        [HttpPost]
        public string ChangePassword(string UserName, string Password)
        {
            return UserDAL.ChangePassword(UserName, Password);
        }
        [HttpGet]
        public dynamic LoginStudent(string MSPIN, string Password, string Key)
        {
            return UserDAL.LoginStudent(MSPIN, Password,Key);
        }
        [HttpGet]
        public UserDetailsBLL RTMLogin(string UserName, string Password)
        {
            return UserDAL.RTMLogin(UserName, Password);
        }
        [HttpGet]
        public UserDetailsBLL_SSTC SSTCLogin(string UserName, string Password,string Token)
        {
            return UserDAL.SSTCLogin(UserName, Password, Token);
        }
        [HttpGet]
        public bool UpdateToken(int User_Id, string Token)
        {
            return UserDAL.UpdateToken(User_Id, Token);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SPCoreText.App;
using SPCoreText.Common;
using SPCoreText.Interface;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SPCoreText.Controllers
{
    public class LoginController : Controller
    {
        private string _appKey = "HkoPortal";

        private IAuth _authUtil;
        public IRepository<User> _userapp;
        User_VerificationApp _userVerificationApp;
        private AppInfoService _appInfoService;
        private ICacheContext _cacheContext;


        public LoginController(IAuth authUtil, IRepository<User> userApp, User_VerificationApp userVerificationApp, AppInfoService appInfoService, ICacheContext cacheContext)
        {
            _authUtil = authUtil;
            _userVerificationApp = userVerificationApp;
            _appInfoService = appInfoService;
            _cacheContext = cacheContext;
            _userapp = userApp;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //public string Login(string username, string password, bool RememberMe)
        //{
        //    var resp = new Response();
        //    try
        //    {
        //        var result = _authUtil.Login(_appKey, username, password);
        //        if (result.Code == 200)
        //        {
        //            resp.Message = result.Message;
        //            //Response.Cookies.Append("Token", result.Token);
        //            if (RememberMe)
        //            {
        //                Response.Cookies.Append("username", username);
        //            }
        //            else
        //            {
        //                Response.Cookies.Delete("username");
        //            }
        //        }
        //        else
        //        {
        //            resp.Code = 500;
        //            resp.Message = result.Message;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resp.Code = 500;
        //        resp.Message = e.Message;
        //    }

        //    return JsonHelper.Instance.Serialize(resp);
        //}

        //[HttpPost]
        //public string sentVerificationCode(string useremail, string user)
        //{
        //    var resp = new Response();
        //    try
        //    {
        //        resp = _userVerificationApp.SentVerificationCode(useremail, user);
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Code = 1;
        //        resp.Message = ex.Message.ToString();
        //    }
        //    return JsonHelper.Instance.Serialize(resp);
        //}

        //[HttpPost]
        //public string checkVerificationCode(string user, string password, string useremail, string verificationcode, bool rememberMe)
        //{
        //    var resp = new Response();
        //    //先判断验证码是否正确及有效
        //    //var v_result = _userVerificationApp.CheckVerificationCode(user, verificationcode, useremail);
        //    var v_result = new Response();
        //    if (v_result.Code != 200)
        //    {
        //        resp.Code = v_result.Code;
        //        resp.Message = v_result.Message;
        //    }
        //    else
        //    {
        //        var userInfo = _userapp.FindSingle(u => u.Account == user);
        //        var currentSession = new UserAuthSession
        //        {
        //            Account = user,
        //            Name = userInfo.Name,
        //            Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
        //            AppKey = _appKey,
        //            CreateTime = DateTime.Now
        //        };
        //        //创建Session
        //        _cacheContext.Set(currentSession.Token, currentSession, DateTime.Now.AddDays(10));
        //        Response.Cookies.Append("Token", currentSession.Token);
        //        //没有这句就登录不了
        //        _appInfoService.Get(_appKey);
        //    }
        //    return JsonHelper.Instance.Serialize(resp);
        //}

        ////[HttpPost]
        ////public string sentVerifyToPhoneSMS(string user, string userphone)
        ////{
        ////    var resp = new Response();
        ////    try
        ////    {
        ////        resp = _userVerificationApp.sentVerifyToSMS(user, userphone);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        resp.Code = 1;
        ////        resp.Message = ex.Message.ToString();
        ////    }
        ////    return JsonHelper.Instance.Serialize(resp);
        ////}
        //public ActionResult Logout()
        //{
        //    _authUtil.Logout();
        //    return RedirectToAction("Index", "Login");
        //}

        //[HttpPost]
        //public string HKSMSPROsentVerifyToSMS(string user, string userphone)
        //{
        //    var resp = new Response();
        //    try
        //    {
        //        string Verifycode = _userVerificationApp.GenerateVerifyCode(user, userphone);
        //        string username = "hkoptlens";
        //        string password = "hkoptlens100";
        //        string hex = "";
        //        string message = "[HKO]one time passcode:" + Verifycode;
        //        string telephone = userphone;
        //        string userDefineNo = "HKOPortal";
        //        string sender = "HKIT";
        //        string parm = "Username=" + username + "&Password=" + password + "&Hex=" + hex + "&Message=" + message + "&Telephone=" + telephone + "&UserDefineNo=" + userDefineNo + "&Sender=" + sender;
        //        string str = Post("https://api.hksmspro.com/service/smsapi.asmx/SendSMS", parm);
        //        if (str.Contains("<State>1</State>"))
        //        {
        //            resp.Code = 200;
        //            resp.Message = "The verification code has been sent";
        //        }
        //        else
        //        {
        //            string s1 = "<State>";
        //            string s2 = "</State>";
        //            int stateBegin = str.IndexOf(s1, 0) + s1.Length;
        //            int stateEnd = str.IndexOf(s2, stateBegin);
        //            string resultmess = str.Substring(stateBegin, stateEnd - stateBegin);
        //            resp.Code = 1;
        //            resp.Message = "The verify code sending failed!" + resultmess;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Code = 1;
        //        resp.Message = ex.Message.ToString();
        //    }
        //    return JsonHelper.Instance.Serialize(resp);
        //}

        //public string Post(string url, string content)
        //{
        //    string result = "";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "post";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    #region 添加Post 参数
        //    byte[] data = Encoding.UTF8.GetBytes(content);
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Close();
        //    }
        //    #endregion

        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取响应内容
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }

        //    return result;
        //}


        //public string LoginNotVerification(string username, string password, bool RememberMe)
        //{
        //    var resp = new Response();
        //    try
        //    {
        //        var result = _authUtil.Login(_appKey, username, password);
        //        if (result.Code == 200)
        //        {
        //            Response.Cookies.Append("Token", result.Token);
        //            if (RememberMe)
        //            {
        //                Response.Cookies.Append("username", username);
        //            }
        //            else
        //            {
        //                Response.Cookies.Delete("username");
        //            }


        //            //var userInfo = _userapp.FindSingle(u => u.Account == username);
        //            //var currentSession = new UserAuthSession
        //            //{
        //            //    Account = username,
        //            //    Name = userInfo.Name,
        //            //    Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
        //            //    AppKey = _appKey,
        //            //    CreateTime = DateTime.Now
        //            //};
        //            ////创建Session
        //            //_cacheContext.Set(currentSession.Token, currentSession, DateTime.Now.AddDays(10));
        //            //Response.Cookies.Append("Token", currentSession.Token);
        //            //没有这句就登录不了
        //            _appInfoService.Get(_appKey);
        //        }
        //        else
        //        {
        //            resp.Code = 500;
        //            resp.Message = result.Message;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        resp.Code = 500;
        //        resp.Message = e.Message;
        //    }

        //    return JsonHelper.Instance.Serialize(resp);
        //}
    }

    /// <summary>
    /// 应用信息服务
    /// </summary>
    public class AppInfoService
    {
        public AppInfo Get(string appKey)
        {
            //可以从数据库读取
            return _applist.SingleOrDefault(u => u.AppKey == appKey);
        }

        private AppInfo[] _applist = new[]
        {
            new AppInfo
            {
                AppKey = "HkoPortal",
                Icon = "/Areas/SSO/Content/images/logo.png",
                IsEnable = true,
                Remark = "明达内部管理系统",
                ReturnUrl = "http://localhost:56813",
                Title = "HkoPortal",
                CreateTime = DateTime.Now,
            },
            new AppInfo
            {
                AppKey = "HkoPortalTest",
                Icon = "/Areas/SSO/Content/images/logo.png",
                IsEnable = true,
                Remark = "这只是个模拟的测试站点",
                ReturnUrl = "http://localhost:53050",
                Title = "HkoPortal测试站点",
                CreateTime = DateTime.Now,
            }
        };
    }
}

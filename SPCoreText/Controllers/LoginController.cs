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
        public ICacheContext _cacheContext;
        public IRepository<User> _app;


        public LoginController(ICacheContext cacheContext, IRepository<User> userApp)
        {
            _cacheContext = cacheContext;
            _app = userApp;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public string LoginNotVerification(string username, string password, bool RememberMe)
        {
            var resp = new RetResponse();
            try
            {
                PassportLoginRequest passportLoginRequest = new PassportLoginRequest() { AppKey = "HkoPortal", Account = username, Password = password };
                var result = Login(passportLoginRequest);
                if (result.Code == 200)
                {
                    Response.Cookies.Append("Token", result.Token);
                    if (RememberMe)
                    {
                        Response.Cookies.Append("username", username);
                    }
                    else
                    {
                        Response.Cookies.Delete("username");
                    }
                }
                else
                {
                    resp.Code = 500;
                    resp.Message = result.Message;
                }

            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }

            return JsonHelper.Instance.Serialize(resp);
        }


        public LoginResult Login(PassportLoginRequest model)
        {
            var result = new LoginResult();

            try
            {
                model.Trim();

                //获取用户信息
                User userInfo = null;
                if (model.Account == "System")
                {
                    userInfo = new User
                    {
                        Id = Guid.Empty.ToString(),
                        Account = "System",
                        Name = "超级管理员",
                        Password = "System"
                    };
                }
                else
                {

                    userInfo = _app.FindSingle(u => u.Account == model.Account);
                }

                if (userInfo == null)
                {
                    throw new Exception("The user does not exist");
                }

                if (userInfo.Password != model.Password)
                {
                    throw new Exception("Password is wrong");
                }

                var currentSession = new UserAuthSession
                {
                    Account = model.Account,
                    Name = userInfo.Name,
                    Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
                    AppKey = model.AppKey,
                    CreateTime = DateTime.Now
                };

                //创建Session
                _cacheContext.Set(currentSession.Token, currentSession, DateTime.Now.AddDays(10));


                result.Code = 200;
                result.ReturnUrl = "http://localhost:56813";
                result.Token = currentSession.Token;
                result.Message = userInfo.BizCode;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }
    }

}

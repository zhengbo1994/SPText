using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SPCoreText.Models;
using SPCoreText.Unlity;
using SPCoreText.Unlity.WebHelper;


namespace SPCoreText.Controllers
{
    //[TextExecptionFilterAttrbute] //控制器注册
    //[Authorize]鉴权配置
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //[TextActionFilterAttribute(Order = 10)]  //方法注册
        //[TextExecptionFilterAttrbute] //标记特性注册
        //TextResultFilterAttrbute  //return注册
        //[TextResourceFilterAttrbute]  //控制器注册

        //[ServiceFilter(typeof(TextExecptionFilterAttrbute))] //在Startup注册
        //[TypeFilter(typeof(TextExecptionFilterAttrbute))]
        public IActionResult Index()
        {
            
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ApiIndex()
        {
            return View();
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region  调用Api协议
        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

        #endregion

        [AllowAnonymousAttribute]
        public ActionResult Login(string name, string password, string verify)
        {
            string verifyCode = base.HttpContext.Session.GetString("CheckCode");
            if (verifyCode != null && verifyCode.Equals(verify, StringComparison.CurrentCultureIgnoreCase))
            {
                if ("Eleven".Equals(name) && "123456".Equals(password))
                {
                    CurrentUser currentUser = new CurrentUser()
                    {
                        Id = 123,
                        Name = "Eleven",
                        Account = "Administrator",
                        Email = "57265177",
                        Password = "123456",
                        LoginTime = DateTime.Now
                    };
                    #region Cookie/Session 自己写
                    base.HttpContext.SetCookies("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(currentUser), 30);
                    base.HttpContext.Session.SetString("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(currentUser));
                    #endregion
                    //过期时间全局设置

                    #region MyRegion
                    //var claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimTypes.Name,name),
                    //    new Claim("password",password),//可以写入任意数据
                    //    new Claim("Account","Administrator")
                    //};
                    //var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                    //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                    //{
                    //    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                    //}).Wait();//没用await
                    //cookie策略--用户信息---过期时间
                    #endregion

                    return base.Redirect("/Home/Index");
                }
                else
                {
                    base.ViewBag.Msg = "账号密码错误";
                }
            }
            else
            {
                base.ViewBag.Msg = "验证码错误";
            }
            return View();
        }

        public ActionResult VerifyCode()
        {
            string code = "";
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out code);
            base.HttpContext.Session.SetString("CheckCode", code);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            return File(stream.ToArray(), "image/gif");
        }

        [HttpPost]
        //[CustomAllowAnonymous]
        public ActionResult Logout()
        {
            #region Cookie
            base.HttpContext.Response.Cookies.Delete("CurrentUser");
            #endregion Cookie

            #region Session
            CurrentUser sessionUser = base.HttpContext.GetCurrentUserBySession();
            if (sessionUser != null)
            {
                this._logger.LogDebug(string.Format("用户id={0} Name={1}退出系统", sessionUser.Id, sessionUser.Name));
            }
            base.HttpContext.Session.Remove("CurrentUser");
            base.HttpContext.Session.Clear();
            #endregion Session

            #region MyRegion
            //HttpContext.User.Claims//其他信息
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            #endregion
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login(string name, string password)
        {
            if ("Eleven".Equals(name, StringComparison.CurrentCultureIgnoreCase)
                 && password.Equals("123456"))
            {
                #region Filter
                base.HttpContext.Response.Cookies.Append("CurrentUser", "Eleven", new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });
                #endregion

                return new JsonResult(new
                {
                    Result = true,
                    Message = "登录成功"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = "登录失败"
                });
            }
        }
    }
}

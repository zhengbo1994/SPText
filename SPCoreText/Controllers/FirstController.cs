using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_CodeDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SPCoreText.Models;
using SPTextLK.Interface;

namespace SPCoreText.Controllers
{
    public class FirstController : Controller
    {
        //https://localhost:44317/first/index

        private readonly ILogger<FirstController> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _iConfiguration;
        private readonly ICategoryService _iCategoryService;
        public FirstController(ILogger<FirstController> logger
                       , IConfiguration configuration
             , ICategoryService iCategoryService
            , ILoggerFactory iLoggerFactory
            )
        {
            this._iConfiguration = configuration;
            this._iCategoryService = iCategoryService;
            this._logger = logger;
            this._loggerFactory = iLoggerFactory;
        }

        public IActionResult Index()
        {
            try
            {
            
            this._logger.LogInformation("12345667789");
            this._loggerFactory.CreateLogger<FirstController>().LogInformation("12345667789");

            #region ViewData
            base.ViewData["User1"] = new CurrentUser()
            {
                Id = 7,
                Name = "Y",
                Account = " ╰つ Ｈ ♥. 花心胡萝卜",
                Email = "莲花未开时",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };
            base.ViewData["Something"] = 12345;
            #endregion

            #region ViewBag
            base.ViewBag.Name = "Eleven";
            base.ViewBag.Description = "Teacher";
            base.ViewBag.User = new CurrentUser()
            {
                Id = 7,
                Name = "IOC",
                Account = "限量版",
                Email = "莲花未开时",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };
            #endregion

            #region TempData
            base.TempData["User"] = new CurrentUser()
            {
                Id = 7,
                Name = "CSS",
                Account = "季雨林",
                Email = "KOKE",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };//3.0preview默认不能复杂类型
            #endregion

            #region Session：服务器内存的一段内容，在HttpContext
            if (string.IsNullOrWhiteSpace(this.HttpContext.Session.GetString("CurrentUserSession")))
            {
                //SessionExtensions.SetString(this.HttpContext.Session,)
                //好像给这个实例增加了一个实例方法，就像扩展了这个类---所以我们能在抽象设计时，最小化--后续的便捷功能可以通过扩展方法来提供
                base.HttpContext.Session.SetString("CurrentUserSession", Newtonsoft.Json.JsonConvert.SerializeObject(new CurrentUser()
                {
                    Id = 7,
                    Name = "CSS",
                    Account = "季雨林",
                    Email = "KOKE",
                    Password = "落单的候鸟",
                    LoginTime = DateTime.Now
                }));
            }
            #endregion

            #region  EF
            var categoryList = this._iCategoryService.QueryPage<Category, int>(u => 1 == 1, 5, 1, u => u.Id);
            base.ViewBag.CategoryList = categoryList.DataList.ToList();
            #endregion

            #region Model
            return View(new CurrentUser()
            {
                Id = 7,
                Name = "一点半",
                Account = "季雨林",
                Email = "KOKE",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            });
            #endregion


            ViewBag.User1 = "测试1";
            ViewData["User2"] = "测试2";
            TempData["User3"] = "测试3";
            return View(new Text() { User4 = "测试4" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class Text
        {

            public string User4 { get; set; }
        }
    }
}
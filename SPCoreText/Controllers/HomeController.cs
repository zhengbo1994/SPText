using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SPCoreText.Models;
using SPCoreText.Unlity;

namespace SPCoreText.Controllers
{
    //[TextExecptionFilterAttrbute] //控制器注册
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

        [ServiceFilter(typeof(TextExecptionFilterAttrbute))] //在Startup注册
        [TypeFilter(typeof(TextExecptionFilterAttrbute))]
        public IActionResult Index()
        {
            
            return View();

        }

        public IActionResult Privacy()
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
    }
}

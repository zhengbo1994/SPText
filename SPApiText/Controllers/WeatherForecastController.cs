using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web;
using System.Web.Http;
using SPCoreTextLK.Service;
using SPCoreTextLK.Interface;
using SPCoreApiText.Utiltiy;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace SPCoreApiText.Controllers
{
    //启动：start nginx.exe
    //重启：nginx   -s reload


    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //http://localhost:5177/api/WeatherForecast/Get
        /// <summary>
        /// 测试api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }



        //http://localhost:8888/api/WeatherForecast/Get
        [HttpGet]
        [Route("GetString")]
        public string GetString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                应用程序监听端口号 = $"this is 03服务器",
                访问次数 = DateTime.Now
            });
        }
    }


}

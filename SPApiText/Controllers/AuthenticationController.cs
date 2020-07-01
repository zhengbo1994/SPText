using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SPCoreApiText.Utiltiy;

namespace SPCoreApiText.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private IJWTService _iJWTService = null;
        private readonly IConfiguration _iConfiguration;

        public AuthenticationController(ILoggerFactory factory,ILogger<AuthenticationController> logger, IConfiguration configuration, IJWTService service)
        {
            this._logger = logger;
            this._iConfiguration = configuration;
            this._iJWTService = service;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            return new List<int>() { 1, 2, 3, 4, 6, 7 };
        }
        //http://localhost:5177/api/Authentication/Login?name=1&password=1
        [HttpGet]
        public string Login(string name, string password)
        {
            //这里肯定是需要去连接数据库做数据校验
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))//应该数据库
            {
                {
                    //这里可以去数据库做验证
                }
                string token = this._iJWTService.GetToken(name);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }


    }
}

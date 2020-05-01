using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SPApiText.Utiltiy;

namespace SPApiText.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;


        private IJWTService _JWTService = null;

        public AuthenticationController(ILogger<AuthenticationController> logger, IJWTService jWTService)
        {
            _JWTService = jWTService;
            _logger = logger;
        }

        [Route("Login")]
        [HttpGet]
        public string Login(string name, string password)
        {
            ///这里肯定是需要去连接数据库做数据校验
            if ("Richard".Equals(name) && "123456".Equals(password))//应该数据库
            {
                {
                    //这里可以去数据库做验证
                }
                string token = this._JWTService.GetToken(name);
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

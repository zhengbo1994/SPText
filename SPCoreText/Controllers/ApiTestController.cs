using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPCoreText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTestController : Controller
    {
        [HttpGet]
        [Route("All")]
        //[Authorize]//鉴权中心
        public CurrentUserTerxt All()
        {
            CurrentUserTerxt currentUser = new CurrentUserTerxt()
            {
                Id = 123,
                Name = "Eleven",
                Account = "Administrator",
                Email = "57265177",
                Password = "123456",
                LoginTime = DateTime.Now
            };
            return currentUser;
        }
        [HttpGet]
        [Route("Get")]
        public CurrentUserTerxt Get(int id)
        {
            CurrentUserTerxt currentUser = new CurrentUserTerxt()
            {
                Id = 123,
                Name = "Eleven",
                Account = "Administrator",
                Email = "57265177",
                Password = "123456",
                LoginTime = DateTime.Now
            };
            return currentUser;
        }
    }


    public class CurrentUserTerxt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
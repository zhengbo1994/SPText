using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SPCoreText.Areas.First.Controllers
{
    public class HomeController : Controller
    {
        [Area("First")]
        [Route("First/[controller]/[action]")]//可以搞个父类
        public IActionResult Index()
        {
            return View();
        }
    }
}
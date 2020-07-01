using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Areas.System.Controllers
{
    public class HomeController : Controller
    {
        //[Area("Areas")]
        [Route("Areas/[controller]/[action]")]//可以搞个父类
        // GET: System/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    public class ThirdController : Controller
    {
        // GET: Third
        public ActionResult Index()
        {
            return View();
            //return Redirect("~/01.html");//注意：把你的html文件放在Views文件夹外的任何文件夹中，就可以直接访问了，
        }
    }
}
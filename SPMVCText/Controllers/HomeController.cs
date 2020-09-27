using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Response.Cookies["Cookies1"].Value = "1";

            HttpCookie hcCookie = new HttpCookie("Cookies2", "2");
            Response.Cookies.Add(hcCookie);


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult WebSocket()
        {
            return View();
        }

        /// <summary>
        /// WebSocket建立链接的方法
        /// </summary>
        /// <param name="name"></param>
        public void MyWebSocket(string name)
        {
            //HttpContext.Session.Add("userName", HttpContext);
            //WebSocketHelper webSocket = new WebSocketHelper();
            //if (HttpContext.IsWebSocketRequest)
            //{
            //    HttpContext.AcceptWebSocketRequest(webSocket.ProcessChat);
            //}
            //else
            //{
            //    HttpContext.Response.Write("我不处理");
            //}
        }
    }
}
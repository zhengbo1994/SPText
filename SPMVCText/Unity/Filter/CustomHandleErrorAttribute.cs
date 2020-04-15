using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Unity.Filter
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        //private Logger logger = new Logger(typeof(CustomHandleErrorAttribute));

        /// <summary>
        /// 会在异常发生后，跳转到这个方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            var httpContext = filterContext.HttpContext;//"为所欲为"
            if (!filterContext.ExceptionHandled)//没有被别的HandleErrorAttribute处理
            {
                //this.logger.Error($"在响应 {httpContext.Request.Url.AbsoluteUri} 时出现异常，信息：{filterContext.Exception.Message}");//
                if (httpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message),
                        ViewName = "发生错误，请联系管理员",
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult()//短路器
                    {
                        ViewName = "~/Views/Shared/Error.cshtml",
                        ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message)
                    };
                }
                filterContext.ExceptionHandled = true;//已经被我处理了
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Unity.Filter
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        //private Logger logger = new Logger(typeof(CustomActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //this.logger.Info($"CustomActionFilterAttribute  OnActionExecuting");
            filterContext.HttpContext.Response.Write($"CustomActionFilterAttribute  OnActionExecuting");
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //this.logger.Info($"CustomActionFilterAttribute  OnActionExecuted");
            filterContext.HttpContext.Response.Write($"CustomActionFilterAttribute  OnActionExecuted");

            //filterContext.Result = new JsonResult()
            //{
            //    Data = new { id = 123, name = "Eleven" }
            //};
        }
        /// <summary>
        /// Result执行前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //this.logger.Info($"CustomActionFilterAttribute  OnResultExecuting");
            filterContext.HttpContext.Response.Write($"CustomActionFilterAttribute  OnResultExecuting");
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //this.logger.Info($"CustomActionFilterAttribute  OnResultExecuted");
            filterContext.HttpContext.Response.Write($"CustomActionFilterAttribute  OnResultExecuted");
        }
    }
}
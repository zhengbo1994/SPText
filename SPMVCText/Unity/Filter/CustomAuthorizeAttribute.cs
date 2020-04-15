using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Unity.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //private Logger logger = new Logger(typeof(CustomAuthorizeAttribute));
        private string _LoginUrl = null;
        public CustomAuthorizeAttribute(string loginUrl = "~/Home/Login")
        {
            this._LoginUrl = loginUrl;
        }
        //public CustomAuthorizeAttribute(ICompanyUserService service)
        //{
        //}
        //不行


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;//能拿到httpcontext 就可以为所欲为

            //if (filterContext.ActionDescriptor.IsDefined(typeof(CustomAllowAnonymousAttribute), true))
            //{
            //    return;
            //}
            //else if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(CustomAllowAnonymousAttribute), true))
            //{
            //    return;
            //}
            //else if (httpContext.Session["CurrentUser"] == null
            //    || !(httpContext.Session["CurrentUser"] is CurrentUser))//为空了，
            //{
            //    //这里有用户，有地址 其实可以检查权限
            //    if (httpContext.Request.IsAjaxRequest())
            //    //httpContext.Request.Headers["xxx"].Equals("XMLHttpRequst")
            //    {
            //        filterContext.Result = new ActionResult() { 
                    
            //        }
                        
            //                //Result = DoResult.OverTime,
            //                //DebugMessage = "登陆过期",
            //                //RetValue = ""
            //            );
            //    }
            //    else
            //    {
            //        httpContext.Session["CurrentUrl"] = httpContext.Request.Url.AbsoluteUri;
            //        filterContext.Result = new RedirectResult(this._LoginUrl);
            //        //短路器：指定了Result，那么请求就截止了，不会执行action
            //    }
            //}
            //else
            //{
            //    return;//继续
            //}
            //base.OnAuthorization(filterContext);
        }
    }


    public class CustomTestErrorAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
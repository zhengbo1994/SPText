using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Public
{
    public class Common: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Console.Write(filterContext.HttpContext.Request["11"]);
            if (filterContext.ActionDescriptor.IsDefined(typeof(Attribute),true))//类
            {

            }
            else if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Attribute), true))//控制器
            {

            }
            else
            {

            }


        }
    }
}
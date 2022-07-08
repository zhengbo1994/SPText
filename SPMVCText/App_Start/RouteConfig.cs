using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SPMVCText
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            #region Swashbuckle路由拦截
            //routes.MapHttpRoute(name: "swagger_root", routeTemplate: "",
            //      defaults: null,
            //      constraints: null,
            //      handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger/ui/index")
            //);
            #endregion

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}_{Year}_{Month}_{Day}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    constraints:new {Year=@"'\d{4}", Month= @"'\d{2}", Day = @"'\d{2}" }
            //);
        }
    }
}

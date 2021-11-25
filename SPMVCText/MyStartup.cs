using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SPMVCText.MyStartup))]
namespace SPMVCText
{

    public class MyStartup
    {
        public static IContainer DIContainer;
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.EnableCors(new CustomCorsPolicyProvider());
        }
    }




    public class CustomCorsPolicyProvider : ICorsPolicyProvider
    {
        public Task<CorsPolicy> GetPolicyAsync(Microsoft.AspNetCore.Http.HttpContext context, string policyName)
        {
            return Task.Factory.StartNew(() =>
            {
                var corsPolicy = new CorsPolicy
                {
                    //AllowAnyHeader = true,
                    //AllowAnyMethod = true
                };

                //corsPolicy.Origins.Add(origin);
                return corsPolicy;
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SPCoreText.Common
{
    /// <summary>
    /// 静态HttpContext扩展
    /// </summary>
    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessorExt(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            SPCoreText.App.HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}

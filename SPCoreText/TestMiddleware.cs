using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SPCoreText
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // 在这里写中间件的业务代码！！
            // HTTP请求部分的处理
            await _next(httpContext);
            // HTTP响应部分的处理
        }
    }
}

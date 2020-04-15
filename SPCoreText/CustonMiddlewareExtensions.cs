﻿using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText
{
    public static class CustonMiddlewareExtensions
    {
        public static IApplicationBuilder UseTest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TestMiddleware>();
        }
    }
}

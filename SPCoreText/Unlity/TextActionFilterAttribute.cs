﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Unlity
{
    /// <summary>
    /// 方法处理前后
    /// </summary>
    public class TextActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");//浏览器跨域
            context.HttpContext.Response.Headers.Add("Cache-Control", "public,max-age=6000");//缓存在客户端，不进入后台代码
            Console.WriteLine("方法执行前");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("方法执行后");
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("方法执行后");
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("方法执行前");
        }
    }
}

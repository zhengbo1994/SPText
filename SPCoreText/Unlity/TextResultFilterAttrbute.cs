using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Unlity
{
    /// <summary>
    /// 方法执行后
    /// </summary>
    public class TextResultFilterAttrbute : ResultFilterAttribute
    {
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

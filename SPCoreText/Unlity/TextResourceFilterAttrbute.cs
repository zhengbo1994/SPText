using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Unlity
{
    //最主要做缓存（不进入Action和Result）
    public class TextResourceFilterAttrbute : Attribute, IResourceFilter
    {
        private static Dictionary<string, IActionResult> dictionary = new Dictionary<string, IActionResult>();

        private string key = null;
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            string key = context.HttpContext.Request.Path;
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, context.Result);
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string key = context.HttpContext.Request.Path;
            if (dictionary.ContainsKey(key))
            {
                context.Result = dictionary[key];//断路器--到Result生成了，但是Result还需要转换成Html
            }
        }
    }
}

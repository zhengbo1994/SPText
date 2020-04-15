using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Unity.Filter
{
    public class CompressActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //foreach (var item in filterContext.ActionParameters)
            //{
            //    //参数检测  敏感词过滤
            //}  
            var request = filterContext.HttpContext.Request;
            var respose = filterContext.HttpContext.Response;
            string acceptEncoding = request.Headers["Accept-Encoding"];//检测支持格式
            if (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.ToUpper().Contains("GZIP"))
            {
                respose.AddHeader("Content-Encoding", "gzip");//响应头指定类型
                respose.Filter = new GZipStream(respose.Filter, CompressionMode.Compress);//压缩类型指定
            }
        }
    }

    public class LimitActionFilterAttribute : ActionFilterAttribute
    {
        private int _Max = 0;
        public LimitActionFilterAttribute(int max = 1000)
        {
            this._Max = max;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string key = $"{filterContext.RouteData.Values["Controller"]}_{filterContext.RouteData.Values["Action"]}";
            //CacheManager.Add(key,) 存到缓存key 集合 时间  
            filterContext.Result = new JsonResult()
            {
                Data = new { Msg = "超出频率" }
            };
        }
    }
}
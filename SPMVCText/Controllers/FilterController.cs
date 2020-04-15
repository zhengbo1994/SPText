using SPMVCText.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    public class FilterController : Controller
    {
        //[ActionFilterAttribute]
        // GET: Filter
        public ActionResult Index()
        {

            return View();
        }

        [AuthorizeAttributeText]
        [HandleErrorAttributeText]//异常处理特性
        public ActionResult Info()
        {
            return View();
        }
    }

    #region  异常处理
    public class HandleErrorAttributeText : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new ViewResult()
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message)
                };

                filterContext.ExceptionHandled = true;
            }

            //base.OnException(filterContext);
        }
    }
    #endregion

    #region  特性标记
    public class AuthorizeAttributeText : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Console.Write(filterContext.HttpContext.Request["11"]);
            if (filterContext.ActionDescriptor.IsDefined(typeof(Attribute), true))//类
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
    #endregion

    #region  action（方法特性）
    public class ActionFilterAttributeText : ActionFilterAttribute
    {
        //方法执行前
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            //压缩
            string EncodingString = filterContext.HttpContext.Request.Headers["Accept-Encoding"].ToString(); ;
            if (string.IsNullOrWhiteSpace(EncodingString))
            {
                return;
            }
            else
            {
                //if (EncodingString.ToLower().Contains("gzip"))
                //{
                //    var responses = filterContext.HttpContext.Response;
                //    responses.AddHeader("context-encoding", "gzip");
                //    responses.Filter = new GZipStream(responses.Filter, CompressionMode.Compress);
                //}


                #region  压缩有关（百度方式，页面压缩）
                var context = HttpContext.Current;
                var request = context.Request;
                var response = context.Response;
                ResponseCompressionType compressionType = this.GetCompressionMode(request);

                if (compressionType != ResponseCompressionType.None)
                {
                    response.AppendHeader("Content-Encoding", compressionType.ToString().ToLower());
                    if (compressionType == ResponseCompressionType.GZip)
                    {
                        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    }
                    else
                    {
                        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    }
                }
                #endregion
            }


            base.OnActionExecuted(filterContext);
        }
        #region  压缩有关（方法，枚举）
        private ResponseCompressionType GetCompressionMode(HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return ResponseCompressionType.None;
            }
            acceptEncoding = acceptEncoding.ToUpperInvariant();
            if (acceptEncoding.Contains("GZIP"))
            {
                return ResponseCompressionType.GZip;
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                return ResponseCompressionType.Deflate;
            }
            else
            {
                return ResponseCompressionType.None;
            }
        }
        private enum ResponseCompressionType { None, GZip, Deflate }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
    #endregion

    #region  自动以特性
    public class CustomAttribute : Attribute
    {



        public static void Mangge<T>(T type) where T : Text
        {
            Type text = type.GetType();
            if (text.IsDefined(typeof(CustomAttribute), true))
            {
                //var attribute = type.Nmae;
            }
        }
    }

    #endregion
}
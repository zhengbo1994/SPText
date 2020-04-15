﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Unlity
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class TextExecptionFilterAttrbute : ExceptionFilterAttribute
    {
        //public TextExecptionFilterAttrbute()
        //{
        //}
        //public override void OnException(ExceptionContext context) {
        //    if (!context.ExceptionHandled)
        //    {
        //        context.Result = new JsonResult(new
        //        {
        //            Result=false,
        //            Msg="请求错误",
        //            Error=context.HttpContext.Request.Path
        //        });
        //        context.ExceptionHandled = true;
        //    }
        //}



        #region Identity
        private readonly ILogger<TextExecptionFilterAttrbute> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public TextExecptionFilterAttrbute(ILogger<TextExecptionFilterAttrbute> logger
            , IModelMetadataProvider modelMetadataProvider)
        {
            this._modelMetadataProvider = modelMetadataProvider;
            this._logger = logger;
        }
        #endregion
        /// <summary>
        /// 异常发生，但是没有处理时
        /// 异常之后得写日志
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                this._logger.LogError($"{context.HttpContext.Request.RouteValues["controller"]} is Error");
                if (this.IsAjaxRequest(context.HttpContext.Request))//header看看是不是XMLHttpRequest
                {
                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        Msg = context.Exception.Message,
                        Error = context.HttpContext.Request.Path
                    });//中断式---请求到这里结束了，不再继续Action
                }
                else
                {
                    var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                    result.ViewData.Add("Exception", context.Exception);
                    context.Result = result;
                }
                context.ExceptionHandled = true;
            }
        }
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        bool verifiedFlag = false;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return verifiedFlag;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //verifiedFlag = false;
            //IUserCtrl userCtrl = new UserCtrl();
            ////0.检查 被请求 的 类 或者 方法 是否有 MyAttr.SkipLogin 标记，如果有，则不需要验证
            //Type skipLoginType = typeof(IgnoreLoginAttribute);
            ////0.1检查 被请求的【方法-ActionDescriptor】是否有 贴 标记
            //if (!filterContext.ActionDescriptor.IsDefined(skipLoginType, false) //ActionDescriptor 是被请求的方法描述器
            //                                                                    //0.2检查 被请求的 方法所在的【控制器类-ControllerDescriptor】是否 有标记
            //     && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(skipLoginType, false))
            //{
            //    BaseController.UserInfo userInfo = HttpContext.Current.Session["UserInfo"] as BaseController.UserInfo;

            //    if (userInfo != null)
            //    {
            //        Sys_User loginUser = userCtrl.GetUserById(userInfo.UserId);
            //        if (loginUser != null)
            //        {
            //            List<Sys_Page> pageList = new List<Sys_Page>();
            //            {
            //                List<Sys_Role> roleList = new List<Sys_Role>();
            //                if (loginUser.RoleList.Count > 0)
            //                {
            //                    roleList = loginUser.RoleList;
            //                }

            //                List<Sys_Permission> permissionList = new List<Sys_Permission>();
            //                foreach (Sys_Role role in roleList)
            //                {
            //                    permissionList.AddRange(role.PermissionList);
            //                }

            //                foreach (Sys_Permission permission in permissionList)
            //                {
            //                    pageList.AddRange(permission.PageList);
            //                }
            //            }

            //            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //            string strUrl = "/" + controllerName;

            //            //如果是Home页面则验证通过
            //            if (controllerName.ToUpper() == "HOME" && pageList.Any())
            //            {
            //                verifiedFlag = true;
            //            }
            //            else if (pageList.Any(p => (p.Url == strUrl)))
            //            {
            //                verifiedFlag = true;
            //            }
            //            else
            //            {
            //                IPageCtrl pageCtrl = new PageCtrl();
            //                List<Sys_Page> allPageList = pageCtrl.getPageList();
            //                if (!allPageList.Select(p => p.Url).Contains(strUrl))
            //                {
            //                    verifiedFlag = true;
            //                }
            //            }
            //        }
            //    }

            //}
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest()) //ajax调用方式 
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        IsSuccess = false,
                        ErrorMessage = "无权使用此功能"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                string actionName = (filterContext.ActionDescriptor).ActionName;
                if (actionName.ToLower() == "Index".ToLower())
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary() { { "ErrorMessage", "无权使用此功能" } }
                    };
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Login");
            }
        }
    }
}
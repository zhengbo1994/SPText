using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SPTextCommon
{
    /// <summary>
    /// 会话帮助类
    /// </summary>
    public class SessionHelper
    {
        public const string LOGININFO_NAME = "LOGININFO_NAME";

        ///// <summary>
        ///// 生成验证码
        ///// </summary>
        ///// <returns></returns>
        //[OutputCache(Duration = 10, VaryByParam = "rand")]//缓存10秒钟
        //[AllowAnonymous]//跳过登陆验证
        //public ActionResult VCode()
        //{
        //    VerificationCodeHelper vcode = new VerificationCodeHelper();
        //    string codeStr = vcode.GetRandomCode();
        //    if (!string.IsNullOrEmpty(codeStr))
        //    {
        //        byte[] arrImg = vcode.GetVCode(codeStr);
        //        HttpContext.Current.Session["code"] = codeStr;
        //        return File(arrImg, "image/gif");
        //    }
        //    else
        //    {
        //        return RedirectToAction("/控制器/VCode?rand=" + Guid.NewGuid().ToString().Substring(1, 10), "image/jpeg");
        //    }
        //}




        public static T Get<T>(string name)
        {
            var value = HttpContext.Current.Session[name];

            if (value == null)
            {
                return default(T);
            }
            else
            {
                return (T)value;
            }
        }


        public static void Set(string name, object value)
        {
            var s = HttpContext.Current.Session[name];

            if (s != null)
            {
                HttpContext.Current.Session[name] = value;
            }
            else
            {
                HttpContext.Current.Session.Add(name, value);
            }
        }

        public static void Delete(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public static object GetLoginInfo()
        {
            var value = HttpContext.Current.Session[LOGININFO_NAME];
            return value;
        }

        public static void SetLoginInfo(object value)
        {

            var s = HttpContext.Current.Session[LOGININFO_NAME];

            if (s != null)
            {
                HttpContext.Current.Session[LOGININFO_NAME] = value;
            }
            else
            {
                HttpContext.Current.Session.Add(LOGININFO_NAME, value);
            }
        }

        public static void RemoveLoginInfo()
        {
            HttpContext.Current.Session.Remove(LOGININFO_NAME);
        }
    }
}

using Microsoft.AspNetCore.Http;
using SPCoreText.App;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Interface
{
    /// <summary>
    /// 使用本地登录。这个注入IAuth时，只需要OpenAuth.Mvc一个项目即可，无需webapi的支持
    /// </summary>
    public class LocalAuth : IAuth
    {
        private IHttpContextAccessor _httpContextAccessor;

        private AuthContextFactory _app;
        private LoginParse _loginParse;
        private ICacheContext _cacheContext;

        public LocalAuth(IHttpContextAccessor httpContextAccessor
            , AuthContextFactory app
            , LoginParse loginParse
            , ICacheContext cacheContext)
        {
            _httpContextAccessor = new HttpContextAccessor();
            _app = app;
            _loginParse = loginParse;
            _cacheContext = cacheContext;
            //_httpContextAccessor = httpContextAccessor;
            //_app = app;
            //_loginParse = loginParse;
            //_cacheContext = cacheContext;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            string token = _httpContextAccessor.HttpContext.Request.Query["Token"];
            if (!String.IsNullOrEmpty(token)) return token;

            token = _httpContextAccessor.HttpContext.Request.Headers["X-Token"];
            if (!String.IsNullOrEmpty(token)) return token;

            //一般令牌是cookie存放
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            return cookie ?? String.Empty;
        }


        public bool CheckLogin(string token = "", string otherInfo = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                token = GetToken();
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var result = _cacheContext.Get<UserAuthSession>(token) != null;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前登录的用户信息
        /// <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="otherInfo">The otherInfo.</param>
        /// <returns>LoginUserVM.</returns>
        public AuthStrategyContext GetCurrentUser(string otherInfo = "")
        {
            AuthStrategyContext context = null;
            var user = _cacheContext.Get<UserAuthSession>(GetToken());
            if (user != null)
            {
                context = _app.GetAuthStrategyContext(user.Account);
            }
            return context;
        }

        /// <summary>
        /// 获取当前登录的用户名
        /// <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="otherInfo">是否是获取用户Name</param>
        /// <returns>System.String.</returns>
        //public string GetUserName(bool isGetName = false)
        //{
        //    var user = _cacheContext.Get<UserAuthSession>(GetToken());

        //    if (user != null)
        //    {
        //        if (isGetName)//当需要获取用户姓名时
        //        {
        //            return user.Name;
        //        }
        //        return user.Account;//默认返回用户账户
        //    }

        //    return "";
        //}

        /// <summary>
        /// 获取当前登录的用户名
        /// <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="otherInfo">是否是获取用户Name</param>
        /// <returns>System.String.</returns>
        public string GetUserName(bool isGetName = false)
        {
            string LoginMode = _httpContextAccessor.HttpContext.Session.GetString("LoginMode");
            string userName1 = null;

            if (LoginMode == null || LoginMode == "yes")
            {
                userName1 = _httpContextAccessor.HttpContext.Session.GetString("userName1");

                var user = _cacheContext.Get<UserAuthSession>(GetToken());
                if (user != null)
                {
                    if (isGetName)//当需要获取用户姓名时
                    {
                        return user.Name;
                    }
                    return user.Account;//默认返回用户账户
                }
                else if (userName1 != null)
                {
                    return userName1;
                }
            }
            return "";
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appKey">应用程序key.</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>System.String.</returns>
        public LoginResult Login(string appKey, string username, string pwd)
        {
            return _loginParse.Do(new PassportLoginRequest
            {
                AppKey = appKey,
                Account = username,
                Password = pwd
            });
        }

        /// <summary>
        /// 注销
        /// </summary>
        public bool Logout()
        {
            var token = GetToken();
            if (String.IsNullOrEmpty(token)) return true;

            try
            {
                _cacheContext.Remove(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

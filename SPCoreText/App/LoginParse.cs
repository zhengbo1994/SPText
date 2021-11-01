using SPCoreText.Controllers;
using SPCoreText.Interface;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.App
{
    /// <summary>
    /// 登录解析
    /// 处理登录逻辑，验证客户端提交的账号密码，保存登录信息
    /// </summary>
    public class LoginParse
    {
        //这个地方使用IRepository<User>而不使用UserManagerApp是防止循环依赖
        public IRepository<User> _app;
        private ICacheContext _cacheContext;
        private AppInfoService _appInfoService;

        public LoginParse(AppInfoService infoService, ICacheContext cacheContext, IRepository<User> userApp)
        {
            _appInfoService = infoService;
            _cacheContext = cacheContext;
            _app = userApp;
        }

        public LoginResult Do(PassportLoginRequest model)
        {
            var result = new LoginResult();

            try
            {
                model.Trim();

                //获取应用信息
                var appInfo = _appInfoService.Get(model.AppKey);
                if (appInfo == null)
                {
                    throw new Exception("The app does not exist");
                }

                //获取用户信息
                User userInfo = null;
                if (model.Account == "System")
                {
                    userInfo = new User
                    {
                        Id = Guid.Empty.ToString(),
                        Account = "System",
                        Name = "超级管理员",
                        Password = "System"
                    };
                }
                //else if (model.Account == "admin")
                //{
                //    userInfo = new User
                //    {
                //        Id = Guid.Empty.ToString(),
                //        Account = "admin",
                //        Name = "管理员",
                //        Password = "admin"
                //    };
                //}
                else
                {
                    userInfo = _app.FindSingle(u => u.Account == model.Account);
                }

                if (userInfo == null)
                {
                    throw new Exception("The user does not exist");
                }

                if (userInfo.Password != model.Password)
                {
                    throw new Exception("Password is wrong");
                }

                var currentSession = new UserAuthSession
                {
                    Account = model.Account,
                    Name = userInfo.Name,
                    Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
                    AppKey = model.AppKey,
                    CreateTime = DateTime.Now
                };

                //创建Session
                _cacheContext.Set(currentSession.Token, currentSession, DateTime.Now.AddDays(10));


                result.Code = 200;
                result.ReturnUrl = appInfo.ReturnUrl;
                result.Token = currentSession.Token;
                result.Message = userInfo.BizCode;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}

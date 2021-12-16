using SPCoreText.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPCoreText.Model;

namespace SPCoreText.App
{
    /// <summary>
    ///  加载用户所有可访问的资源/机构/模块
    /// </summary>
    public class AuthContextFactory
    {
        //System验证策略
        private SystemAuthStrategy _systemAuth;
        //普通用户验证策略
        private NormalAuthStrategy _normalAuthStrategy;
        private readonly IUnitWork _unitWork;

        public AuthContextFactory(SystemAuthStrategy sysStrategy
            , NormalAuthStrategy normalAuthStrategy
            , IUnitWork unitWork)
        {
            _systemAuth = sysStrategy;
            _normalAuthStrategy = normalAuthStrategy;
            _unitWork = unitWork;
        }

        public AuthStrategyContext GetAuthStrategyContext(string username)
        {
            IAuthStrategy service = null;
            if (username == "System")
            {
                service = _systemAuth;
            }
            else
            {
                //不是system时就查找
                service = _normalAuthStrategy;
                service.User = _unitWork.FindSingle<User>(u => u.Account == username);
            }

            return new AuthStrategyContext(service);
        }
    }
}

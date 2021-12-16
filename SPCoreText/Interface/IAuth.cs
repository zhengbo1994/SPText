using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Interface
{
    public interface IAuth
    {
        /// <summary>
        /// 检验token是否有效
        /// </summary>
        /// <param name="token">token值</param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        bool CheckLogin(string token = "", string otherInfo = "");

        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        AuthStrategyContext GetCurrentUser(string otherInfo = "");

        /// <summary>
        /// 获取用户Account或用户用户Name
        /// </summary>
        /// <param name="isGetName">是否是获取Name</param>
        /// <returns></returns>
        string GetUserName(bool isGetName = false);

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appKey">登录的应用appkey</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        LoginResult Login(string appKey, string username, string pwd);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        bool Logout();
    }

    /// <summary>
    /// 接口验证策略
    /// </summary>
    public interface IAuthStrategy
    {
        List<ModuleView> Modules { get; }

        List<ModuleElement> ModuleElements { get; }

        List<Role> Roles { get; }

        //List<Resource> Resources { get; }

        List<Org> Orgs { get; }

        User User
        {
            get; set;
        }

        /// <summary>
        /// 根据模块id获取可访问的模块字段
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        List<KeyDescription> GetProperties(string moduleCode);

    }

    /// <summary>
    ///  授权策略上下文，一个典型的策略模式
    /// </summary>
    public class AuthStrategyContext
    {
        private readonly IAuthStrategy _strategy;
        public AuthStrategyContext(IAuthStrategy strategy)
        {
            this._strategy = strategy;
        }

        public User User
        {
            get { return _strategy.User; }
        }

        public List<ModuleView> Modules
        {
            get { return _strategy.Modules; }
        }

        public List<ModuleElement> ModuleElements
        {
            get { return _strategy.ModuleElements; }
        }

        public List<Role> Roles
        {
            get { return _strategy.Roles; }
        }

        //public List<Resource> Resources
        //{
        //    get { return _strategy.Resources; }
        //}

        public List<Org> Orgs
        {
            get { return _strategy.Orgs; }
        }

        public List<KeyDescription> GetProperties(string moduleCode)
        {
            return _strategy.GetProperties(moduleCode);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreApiText.ConsulClientExtend
{
    public interface IConsulDispatcher
    {
        /// <summary>
        /// 负载均衡获取地址
        /// </summary>
        /// <param name="serviceName">朝夕的老师</param>
        /// <returns></returns>
        string ChooseAddress(string serviceName);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SPTextProject.IBLL
{
    public interface ITransferLogService
    {
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="host">主机IP</param>
        /// <param name="factoryCode">工厂代号</param>
        /// <param name="condition">条件</param>
        /// <param name="count">数量</param>
        void Log(string host, string factoryCode, string condition, int count);
    }
}

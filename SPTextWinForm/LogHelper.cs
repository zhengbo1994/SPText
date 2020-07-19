using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextWinForm
{
    public static class LogHelper
    {
        private static ILog log;
        static LogHelper()
        {
            log4net.Config.XmlConfigurator.Configure();//加载配置文件
            log = log4net.LogManager.GetLogger(typeof(LogHelper));//通过反射获取日志对象实例Log4netProject.LogHelper
        }
        /// <summary>
        /// BUG记录
        /// </summary>
        /// <param name="info"></param>
        public static void DeBug(string info)
        {
            log.Debug(info);
        }
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            log.Info(info);
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="info"></param>
        public static void Error(string info)
        {
            log.Error(info);
        }

        public static void Error(string info, Exception ex)
        {
            log.Error(info, ex);
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="info"></param>
        public static void Warn(string info)
        {
            log.Warn(info);
        }
    }
}

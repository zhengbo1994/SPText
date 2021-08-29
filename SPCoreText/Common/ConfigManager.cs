using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Common
{
    public class ConfigManager
    {
        //.NET Core 读取配置文件：appsettings
        //1. Microsoft.Extensions.Configuration;
        //2. Microsoft.Extensions.Configuration.Json;
        public static string GetConfig(string key)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");//默认读取  当前运行目录
            IConfigurationRoot configuration = builder.Build();
            //string configValue = configuration.GetSection(key).Value;
            string configValue = configuration[key];
            return configValue;
        }

        /// <summary>
        /// 在作为WorkerService项目访问帮助类时无法使用此方法取到数据
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        [Obsolete("在作为WorkerService项目访问帮助类时无法使用此方法取到数据")]
        private static string GetConnection(string connName)
        {
            var Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true)
             .Build();
            return Configuration.GetConnectionString(connName);
        }
    }
}

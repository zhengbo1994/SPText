using Microsoft.Extensions.Logging;
using NLog;
using SPTextProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTextProject.Models
{
    /// <summary>
    /// 对工厂操作的静态缓存
    /// </summary>
    public static class FactoryCache
    {
        public static string CurrentFactory = string.Empty;

        public readonly static List<FactorySettingItemInfo> FactoriesSettingInfo = new List<FactorySettingItemInfo>();

        static FactoryCache()
        {
            try
            {
                var factoriyConfigs = ConfigManager.GetChildren("DbContext:Factories");
                foreach (var item in factoriyConfigs)
                {
                    FactoriesSettingInfo.Add(new FactorySettingItemInfo(
                        item.Key,
                        ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:ConnStr"),
                        ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:Condition"),
                        ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:EmailRecipientList").Split(";", StringSplitOptions.RemoveEmptyEntries),
                        ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:EmailCCRecipientList").Split(";", StringSplitOptions.RemoveEmptyEntries),
                        Convert.ToInt32(ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:Order")),
                        ConfigManager.GetConfig($"DbContext:Factories:{item.Key}:CustomerCode")
                        ));
                }

                FactoriesSettingInfo = FactoriesSettingInfo.OrderBy(f => f.Order).ToList();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, $"配置文件appsettings.json出错，没有得到正确的配置信息，请检查！异常消息：{ex.ToString()}");
                FactoriesSettingInfo.Clear();
            }
        }
    }
}

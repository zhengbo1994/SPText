using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SPText.Common
{
    public class SettingConfiguration
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用WinAPI将指定名称的打印机设置为默认打印机

        private static PrintDocument fPrintDocument = new PrintDocument();
        /// <summary>
        /// 获取本机默认打印机名称
        /// </summary>
        public static string DefaultPrinter
        {
            get
            {
                return fPrintDocument.PrinterSettings.PrinterName;
            }
        }

        private static string printerName = DefaultPrinter;
        /// <summary>
        /// 获取或设置打印机,默认获取本机默认打印机
        /// </summary>
        public static string PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }

        /// <summary>
        /// 获取本机的打印机列表。列表中的第一项就是默认打印机。
        /// </summary>
        public static List<String> GetLocalPrinters()
        {
            try
            {
                List<String> fPrinters = new List<string>();
                fPrinters.Add(DefaultPrinter); // 默认打印机始终出现在列表的第一项
                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    if (!fPrinters.Contains(fPrinterName))
                        fPrinters.Add(fPrinterName);
                }
                return fPrinters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定节点的配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConfiguration(string configName)
        {
            string configVaule = ConfigurationManager.AppSettings[configName];
            if (configVaule != null && configVaule != "")
            {
                return configVaule.ToString();
            }
            return "";
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static bool ConfigExists(string configName)
        {
            string configVaule = ConfigurationManager.AppSettings[configName];
            if (configVaule == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 移除指定的 app.config 节点
        /// </summary>
        /// <param name="configName"></param>
        public static void RemoveConfigurationNode(string configName)
        {
            try
            {
                ConfigurationManager.AppSettings.Remove(configName);
                ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SetModel">Add/set</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetConfiguration(string key, string value)
        {
            try
            {
                //更新配置文件： 
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                bool setModel = ConfigExists(key);
                if (setModel)
                {
                    //config.AppSettings.Settings.Remove(key);
                    ////添加
                    //config.AppSettings.Settings.Add(key, value);
                    //修改
                    config.AppSettings.Settings[key].Value = value;
                }
                else
                {
                    //添加
                    config.AppSettings.Settings.Add(key, value);
                }
                //最后调用 
                config.Save(ConfigurationSaveMode.Modified);
                //当前的配置文件更新成功。
                ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetAppPath()
        {
            try
            {
                string appPath = System.Windows.Forms.Application.StartupPath.ToLower();
                appPath = appPath.Replace("\\Debug", "");
                appPath = appPath.Replace("\\Release", "");
                appPath = appPath.Replace("bin", "");
                return appPath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 实现byte[]转换成十六进制String
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] arrByte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte by in arrByte)
            {
                sb.Append(by > 15 ? Convert.ToString(by, 16) : '0' + Convert.ToString(by, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertString16ToNumber(string value)
        {
            return Int32.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strOld"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string[] SplitValue(string strOld, char splitChar)
        {
            try
            {
                string[] strNew = strOld.Split(splitChar);
                return strNew;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

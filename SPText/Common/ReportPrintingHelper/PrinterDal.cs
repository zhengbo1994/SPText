using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing.Printing;

namespace ReportPrinting.Model
{
    public class PrinterDal
    {
        public static WcfPrinterSettings GetWcfPrinterSettings()
        {
            WcfPrinterSettings settings = new WcfPrinterSettings();

            //默认打印机
            settings.DefaultPrinterName = GetDefaultPrinterName();

            settings.Printers = GetPrinters();

            settings.CustomersSelectedPrinterAndBox = GetCustomersSelectedPrinterAndBox();

            return settings;
        }

        public static List<PrinterAndBoxSingleIndex> GetCustomersSelectedPrinterAndBox()
        {
            List<PrinterAndBoxSingleIndex> list = new List<PrinterAndBoxSingleIndex>();
            var AppSettings = System.Configuration.ConfigurationManager.AppSettings;
            for (int i = 0; i < AppSettings.Count; i++)
            {
                if (AppSettings.AllKeys[i].Contains("PrinterName"))
                {
                    PrinterAndBoxSingleIndex index = new PrinterAndBoxSingleIndex();
                    index.CustomerName = AppSettings.AllKeys[i].Replace("PrinterName", "");
                    var value = AppSettings.GetValues(i)[0];
                    if (Regex.IsMatch(value, "\\S+\\[[\\s\\S]*\\]"))
                    {
                        index.PrinterName = value.Substring(0, value.LastIndexOf('['));

                        index.BoxName = value.Substring(value.LastIndexOf('[') + 1, value.LastIndexOf(']') - value.LastIndexOf('[') - 1);
                    }
                    else
                    {
                        index.PrinterName = value;
                        index.BoxName = null;
                    }
                    list.Add(index);
                }
            }
            return list;
        }

        public WcfPrinterSettings GetPrinterSettingInfo()
        {
            WcfPrinterSettings settings = new WcfPrinterSettings();

            //默认打印机
            settings.DefaultPrinterName = GetDefaultPrinterName(); //RdlcPrint.GetDefaultPrinterName();

            settings.Printers = GetPrinters();

            settings.CustomersSelectedPrinterAndBox = GetCustomersSelectedPrinterAndBox();

            return settings;
        }

        /// <summary>
        /// 获取当前的打印机list
        /// </summary>
        /// <returns></returns>
        public static List<Printer> GetPrinters()
        {
            List<Printer> Printers = new List<Printer>();

            List<string> printerNames = GetPrinterNames();

            for (int i = 0; i < printerNames.Count; i++)
            {
                string printerName = printerNames[i];
                List<string> boxs = GetPrinterBoxs(printerName); //RdlcPrint.GetPrinterBoxs(printerName);
                Printer printer = new Printer()
                {
                    PrinterName = printerName,
                    BoxItems = boxs
                };
                Printers.Add(printer);
            }
            return Printers;
        }

        /// <summary>
        /// 获取当前所有的打印机名称
        /// </summary>
        /// <returns>当前打印机名称List</returns>
        public static List<string> GetPrinterNames()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                list.Add(PrinterSettings.InstalledPrinters[i]);
            }
            return list;
        }

        /// <summary>
        /// 修改打印机设置
        /// </summary>
        /// <param name="wcfPrinterSettings"></param>
        /// <returns></returns>
        public string EditPrinterSettingInfo(WcfPrinterSettings wcfPrinterSettings)
        {
            var AppSettings = System.Configuration.ConfigurationManager.AppSettings;
            for (int i = 0; i < AppSettings.Count; i++)
            {
                if (AppSettings.AllKeys[i].Contains("PrinterName"))
                {
                    PrinterAndBoxSingleIndex index = new PrinterAndBoxSingleIndex();
                    index.CustomerName = AppSettings.AllKeys[i].Replace("PrinterName", "");
                    for (int j = 0; j < wcfPrinterSettings.CustomersSelectedPrinterAndBox.Count; j++)
                    {
                        if (wcfPrinterSettings.CustomersSelectedPrinterAndBox[j].CustomerName == index.CustomerName)
                        {
                            string value = wcfPrinterSettings.CustomersSelectedPrinterAndBox[j].PrinterName + "[" + wcfPrinterSettings.CustomersSelectedPrinterAndBox[j].BoxName + "]";

                            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            if (config.AppSettings.Settings[AppSettings.AllKeys[i]] != null)
                                config.AppSettings.Settings[AppSettings.AllKeys[i]].Value = value;
                            else
                                config.AppSettings.Settings.Add(AppSettings.AllKeys[i], value);
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                        }
                    }
                }
            }
            return "OK";
        }


        #region 关于系统中的打印机的操作
        /// <summary>
        /// 获取系统默认打印机
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            PrintDocument printDoc = new PrintDocument();
            string defaultPrinterName = printDoc.PrinterSettings.PrinterName;
            printDoc.Dispose();
            return defaultPrinterName;
        }

        /// <summary>
        /// 获取当前系统配置的所有的打印机名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPrinterName()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                list.Add(PrinterSettings.InstalledPrinters[i]);
            }
            return list;
        }

        /// <summary>
        /// 获取指定打印机的所有盒子（纸张来源）可选项
        /// </summary>
        /// <param name="PrinterName">打印机名称</param>
        /// <returns>盒子可选项列表</returns>
        public static List<string> GetPrinterBoxs(string PrinterName)
        {
            PrintDocument printDocument = new PrintDocument();
            string defaultPrinterName = printDocument.PrinterSettings.PrinterName;
            printDocument.PrinterSettings.PrinterName = PrinterName;

            List<string> list = new List<string>();
            PaperSource paperSource;
            for (int i = 0; i < printDocument.PrinterSettings.PaperSources.Count; i++)
            {
                paperSource = printDocument.PrinterSettings.PaperSources[i];
                list.Add(paperSource.SourceName);
            }
            printDocument.PrinterSettings.PrinterName = defaultPrinterName;

            printDocument.Dispose();

            return list;
        }

        /// <summary>
        /// 通过传递来的当前操作打印机及盒子字符串得到实际操作的打印机名称和盒子名称
        /// </summary>
        /// <param name="printerAndBox">当前操作打印机及盒子字符串</param>
        /// <returns>[打印机名称，盒子名称]</returns>
        public static string[] GetCurrentPrinterNameAndBoxName(string printerAndBox)
        {
            PrintDocument printDoc = new PrintDocument();
            string[] strs = new string[2];
            if (printerAndBox.IsNullOrEmpty())
            {
                strs[0] = printDoc.PrinterSettings.PrinterName;
                strs[1] = null;
            }
            else
            {
                if (Regex.IsMatch(printerAndBox, "[\\S\\s]+\\[[\\s\\S]*\\]"))
                {
                    strs[0] = printerAndBox.Substring(0, printerAndBox.LastIndexOf('['));
                    strs[1] = Convert.ToString(printerAndBox.Substring(printerAndBox.LastIndexOf('[') + 1, printerAndBox.LastIndexOf(']') - printerAndBox.LastIndexOf('[') - 1));
                }
                else
                {
                    strs[0] = printerAndBox;
                    strs[1] = null;
                }
            }
            printDoc.Dispose();
            return strs;
        }
        #endregion
    }
}

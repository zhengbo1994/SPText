using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ReportPrinting.Model
{
    public class WcfPrinterSettings
    {
        public string DefaultPrinterName { get; set; }

        //该计算机上所有连接的打印机的明细
        public List<Printer> Printers { get; set; }

        public List<PrinterAndBoxSingleIndex> CustomersSelectedPrinterAndBox { get; set; }
        public static T Clone<T>(T obj)
        {
            T ret = default(T);
            if (obj != null)
            {
                XmlSerializer cloner = new XmlSerializer(typeof(T));
                MemoryStream stream = new MemoryStream();
                cloner.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                ret = (T)cloner.Deserialize(stream);
            }
            return ret;
        }
    }

    public class Printer
    {
        public string PrinterName { get; set; }
        public List<string> BoxItems { get; set; }
    }

    public class PrinterAndBoxSingleIndex
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// 盒子名称
        /// </summary>
        public string BoxName { get; set; }
    }
}

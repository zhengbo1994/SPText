using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace ReportPrinting.Model
{
    /// <summary>
    /// 通过RDLC报表文件进行打印
    /// </summary>
    public class RdlcPrint : IDisposable
    {
        public RdlcPrint()
        {
            this._currentPageIndex = 0;
            this._streams = new List<Stream>();
        }

        /// <summary>
        /// 当前打印页号
        /// </summary>
        private int _currentPageIndex;

        /// <summary>
        /// 当前对象中的所有流，一个报表对象对应一页对应一个stream
        /// </summary>
        private List<Stream> _streams;

        private static RdlcPrint _rdlcPrintl;

        public int CurrentPageIndex { get => _currentPageIndex; set => _currentPageIndex = value; }
        public List<Stream> Streams { get => _streams; set => _streams = value; }
        public static RdlcPrint Instance
        {
            get
            {
                _rdlcPrintl = new RdlcPrint();
                return _rdlcPrintl;
            }
        }

        /// <summary>
        /// 将报表们输出成stream并给streams
        /// </summary>
        /// <param name="reports"></param>
        private void Export(List<LocalReport> reports, Model.PrinterAssembleInfo info)
        {
            string deviceInfo = @"
<DeviceInfo>
<OutputFormat>EMF</OutputFormat>
<MarginTop>0in</MarginTop>
<MarginLeft>0in</MarginLeft>
<MarginRight>0in</MarginRight>
<MarginBottom>0in</MarginBottom>
</DeviceInfo>
";
            Warning[] warnings;

            for (int i = 0; i < reports.Count; i++)
            {
                var report = reports[i];
                report.Render("Image", deviceInfo, CreateStream, out warnings);

                //使 LocalReport 对象立即释放其对沙盒应用程序域的引用
                report.ReleaseSandboxAppDomain();

                //记录
                info.AddNote("已生成报表 " + (i + 1) + "/" + reports.Count);
            }

            foreach (Stream stream in Streams)
                stream.Position = 0;//在派生类中重写时，设置当前流中的位置。
        }

        private Stream CreateStream(string name, string extension, Encoding encoding, string mimeType, bool willSeek)
        {
            var path= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EMF");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };

            //EMF为增强型图元文件
            string emfPath = @"EMF\" + name + Guid.NewGuid() + "." + extension;
            Stream stream = new FileStream(emfPath, FileMode.Create);
            Streams.Add(stream);
            return stream;
        }

        /// <summary>
        /// 打印的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //图形图元文件
            Metafile pageImage = new Metafile(Streams[CurrentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            CurrentPageIndex++;

            //是否还有下一页
            ev.HasMorePages = CurrentPageIndex < Streams.Count;
        }

        public void Print(string printerName, string[] PrinterSets)
        {
            PrintDocument printDoc = new PrintDocument();
            if (Streams == null || Streams.Count == 0)
                return;

            printDoc.PrinterSettings.PrinterName = PrinterSets[0].Trim();
            if (!PrinterSets[1].IsNullOrEmpty() && !PrinterSets[1].Contains("自动"))
            {
                //匹配到了盒子设置
                for (int i = 0; i < printDoc.PrinterSettings.PaperSources.Count; i++)
                {
                    if (printDoc.PrinterSettings.PaperSources[i].SourceName == PrinterSets[1])
                    {
                        printDoc.DefaultPageSettings.PaperSource = printDoc.PrinterSettings.PaperSources[i];
                        break;
                    }
                }
            }

            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".", printerName);
                throw new Exception(msg);
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            StandardPrintController spc = new StandardPrintController();
            printDoc.PrintController = spc;
            printDoc.Print();
            printDoc.Dispose();
        }


        public void Dispose()
        {
            if (Streams != null)
            {
                foreach (Stream stream in Streams)
                {
                    stream.Close();
                }
                Streams = null;
            }
        }

        /// <summary>
        /// 对外接口,启动打印
        /// </summary>
        /// <param name="dtSource">打印报表对应的数据源</param>
        /// <param name="sReport">打印报表名称</param>
        public static void Run(List<LocalReport> reports, string printerName = "", Model.PrinterAssembleInfo info = null)
        {
            RdlcPrint billPrint = new RdlcPrint();
            try
            {
                billPrint.CurrentPageIndex = 0;
                billPrint.Export(reports, info);

                string[] printerSets = PrinterDal.GetCurrentPrinterNameAndBoxName(printerName);

                //打印
                info.AddNote("打印...");
                billPrint.Print(printerName, printerSets);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                billPrint.Dispose();
            }
        }
    }
}

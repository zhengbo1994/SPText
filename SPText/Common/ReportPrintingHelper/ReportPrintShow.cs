using IOSerialize.Serialize;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using PdfiumViewer;
using ReportPrinting.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPText.Common.ReportPrintingHelper
{
    public class ReportPrintShow
    {
        public PageInfo labelInfo = new PageInfo();
        private int currentPageIndex = 0;
        public void Show1()
        {
            string num = "1";
            string strText = num;
            if (string.IsNullOrEmpty(strText))
            {
                //MessageBox.Show("请输入正确的流水号");
                return;
            }
            //string sql = @"SELECT TOP 1 * FROM [order].[dbo].[hkows_order]   where shop_code='AS928'";
            //DBHelper dBHelper = new DBHelper();
            //DataSet dataSet = dBHelper.DataSet(sql, CommandType.StoredProcedure, new SqlParameter[] { });
            DataSet dataSet = new DataSet();

            DataTable dataTable = new DataTable("table");
            dataTable.Columns.Add("Columns");
            dataTable.Rows.Add("Rows");
            dataSet.Tables.Add(dataTable);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                //将datatable值复制到类上
                DataTable dt = dataSet.Tables[0];
                ReportDataSource reportDataSource = new ReportDataSource() { Name = "CustomersDataSet", Value = dt };

                //赋值报表参数
                List<ReportParameter> p = new List<ReportParameter>();
                //p.Add(new ReportParameter("A1", "1"));

                RdlcPrintSettings rdlcPrintSettings = new RdlcPrintSettings();
                bool result = false;
                rdlcPrintSettings.AddReport(reportDataSource, p, result);
            }
        }


        public void Show2()
        {
            string num = "1";
            if (string.IsNullOrEmpty(num))
            {

            }
            //string sql = @"SELECT TOP 1 * FROM [order].[dbo].[hkows_order]   where shop_code='AS928'";
            //DBHelper dBHelper = new DBHelper();
            //DataSet dataSet = dBHelper.DataSet(sql, CommandType.StoredProcedure, new SqlParameter[] { });

            DataSet dataSet = new DataSet();

            DataTable dataTable = new DataTable("table");
            dataTable.Columns.Add("Columns");
            dataTable.Rows.Add("Rows");
            dataSet.Tables.Add(dataTable);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                //将dataSet转化为类数据
                labelInfo = GetDataAssembleResult(labelInfo);
                PrintLable();
            }
        }


        public void Show3()
        {
            string reportPath = string.Empty;//路径
            List<ReportParameter> parameter = new List<ReportParameter>();//参数
            var report = new Microsoft.Reporting.WinForms.LocalReport();
            report.ReportPath = reportPath;
            if (parameter.Count() > 0)
            {
                report.SetParameters(parameter);
            }
            BillPrint.Run(report);
        }
        public bool PrintLable()//打印
        {
            PrintDocument pd = new PrintDocument();
            StandardPrintController controler = new StandardPrintController();

            pd.DefaultPageSettings.PaperSize = new PaperSize("LMS", 850, 550);   //1/100英寸為1單位   1毫米=3.93   52mm 33mm

            try
            {
                pd.PrintPage += new PrintPageEventHandler(this.DrawLabelPic);
                pd.PrinterSettings.Copies = (short)1;
                //选择默认的打印机
                //pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName; //System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
                pd.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
                pd.PrintController = controler;
                PaperSize paperSize = new PaperSize("Custom Size 1", 294, 294);
                pd.DefaultPageSettings.PaperSize = paperSize;
                //pd.OriginAtMargins = false;
                //pd.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
                pd.Print();
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
            finally
            {
                pd.Dispose();
            }

        }


        private void DrawLabelPic(Object Sender, PrintPageEventArgs av)
        {
            Margins margins = new Margins(0, 0, 0, 0);
            av.PageSettings.Margins = margins;
            Brush brush = Brushes.Black;
            int emSizeMont = 15;
            if (currentPageIndex == 0)   //当为第一页时
            {
                #region page2
                //av.Graphics.TranslateTransform(330, 245);
                av.Graphics.TranslateTransform(280, 140);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString(labelInfo.page2Info.row1, new Font("Arial", 10, FontStyle.Bold), brush, 0, 0);

                av.Graphics.DrawString(labelInfo.page2Info.row2, new Font("Arial", 8, FontStyle.Regular), brush, 12, 15);
                av.Graphics.DrawString(labelInfo.page2Info.row3, new Font("Arial", 8, FontStyle.Regular), brush, 12, 30);
                av.Graphics.DrawString(labelInfo.page2Info.row4, new Font("Arial", 10, FontStyle.Bold), brush, 240, 45);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 140, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 180, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 220, 70);

                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_1, new Font("Arial", 8, FontStyle.Bold), brush, 10, 85);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_2, new Font("Arial", 8, FontStyle.Bold), brush, 60, 85);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_3, new Font("Arial", 8, FontStyle.Bold), brush, 100, 85);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_4, new Font("Arial", 8, FontStyle.Bold), brush, 140, 85);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_5, new Font("Arial", 8, FontStyle.Bold), brush, 180, 85);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_6, new Font("Arial", 8, FontStyle.Bold), brush, 220, 85);

                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 100);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 100);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 100);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 140, 100);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 180, 100);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 220, 100);

                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 115);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 115);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 115);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 140, 115);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 180, 115);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 220, 115);
                av.Graphics.ResetTransform();
                #endregion

                #region  page3
                int height = 120;
                //av.Graphics.TranslateTransform(30, 110);
                av.Graphics.TranslateTransform(-20, 15);
                av.Graphics.DrawString(labelInfo.page3Info.row1, new Font("Arial", 8, FontStyle.Bold), brush, 30, 40 + height);
                av.Graphics.DrawString(labelInfo.page3Info.row2, new Font("Arial", 8, FontStyle.Regular), brush, 110, 70 + height);
                av.Graphics.DrawString(labelInfo.page3Info.row3, new Font("Arial", 8, FontStyle.Regular), brush, 130, 85 + height);
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo.page3Info.row4), 30, 100 + height, 260, 30);
                av.Graphics.DrawString(labelInfo.page3Info.row4, new Font("Arial", 7, FontStyle.Bold), brush, 140, 100 + height + 30);
                av.Graphics.ResetTransform();
                #endregion
            }
            av.HasMorePages = false; //停止增加新的页数
        }

        #region  将数据组装
        #region  数据帮助类
        public class PageInfo
        {
            public Page2Info page2Info { get; set; }
            public Page3Info page3Info { get; set; }
        }
        public class Page2Info
        {
            public string row1 { get; set; }
            public string row2 { get; set; }
            public string row3 { get; set; }
            public string row4 { get; set; }
            public List<Row5> row5 { get; set; }
        }
        public class Row5
        {
            public string row_1 { get; set; }
            public string row_2 { get; set; }
            public string row_3 { get; set; }
            public string row_4 { get; set; }
            public string row_5 { get; set; }
            public string row_6 { get; set; }
        }
        public class Page3Info
        {
            public string row1 { get; set; }
            public string row2 { get; set; }
            public string row3 { get; set; }
            public string row4 { get; set; }
            public string row5 { get; set; }
        }
        #endregion
        public PageInfo GetDataAssembleResult(PageInfo pageInfo)
        {

            #region  page2
            List<Row5> row5List = new List<Row5>();
            Row5 row5_0 = new Row5()
            {
                row_1 = "",
                row_2 = "Sph",
                row_3 = "Cyl",
                row_4 = "Axe",
                row_5 = "Add",
                row_6 = "",
            };
            row5List.Add(row5_0);
            Row5 row5_1 = new Row5()
            {
                row_1 = "Nominal",
                row_2 = "+2.00",
                row_3 = "+2.00",
                row_4 = "160",
                row_5 = "3.00",
                row_6 = "A",
            };
            row5List.Add(row5_1);
            Row5 row5_2 = new Row5()
            {
                row_1 = "",
                row_2 = "+4.00",
                row_3 = "-2.00",
                row_4 = "70",
                row_5 = "3.00",
                row_6 = "",
            };
            row5List.Add(row5_2);
            Row5 row5_3 = new Row5()
            {
                row_1 = "Real",
                row_2 = "+4.03",
                row_3 = "-1.87",
                row_4 = "094",
                row_5 = "2.74",
                row_6 = "A1.81/269",
            };
            row5List.Add(row5_3);


            Page2Info page2Info = new Page2Info()
            {
                row1 = "L",
                row2 = "Product,Treatment,Tint",
                row3 = "Infinity XT",
                row4 = "55",
                row5 = row5List
            };
            #endregion

            #region  page3
            Page3Info page3Info = new Page3Info()
            {
                row1 = "Jai Kudo Group Limited",
                row2 = "2070465-187 bacon",
                row3 = "56037839",
                row4 = "40440110",
                row5 = "40440110",
            };

            pageInfo.page2Info = page2Info;
            pageInfo.page3Info = page3Info;
            #endregion

            return pageInfo;
        }
        #endregion
    }

    class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
    }


    #region  通过RDLC向默认打印机输出打印报表
    /// <summary>

    /// 通过RDLC向默认打印机输出打印报表

    /// </summary>

    public class BillPrint : IDisposable
    {

        /// <summary>

        /// 当前打印页号

        /// </summary>

        static int m_currentPageIndex;



        /// <summary>

        /// RDCL转换stream一页对应一个stream

        /// </summary>

        static List<Stream> m_streams;



        /// <summary>

        /// 把report输出成stream

        /// </summary>

        /// <param name="report">传入需要Export的report</param>

        private void Export(LocalReport report)
        {

            string deviceInfo =

              "<DeviceInfo>" +

              "  <OutputFormat>EMF</OutputFormat>" +

                //"  <PageWidth>2in</PageWidth>" +

                //"  <PageHeight>20in</PageHeight>" +

                "  <MarginTop>0in</MarginTop>" +

                "  <MarginLeft>0in</MarginLeft>" +

                "  <MarginRight>0in</MarginRight>" +

                "  <MarginBottom>0in</MarginBottom>" +

              "</DeviceInfo>";

            Warning[] warnings;

            m_streams = new List<Stream>();

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)

                stream.Position = 0;

        }



        /// <summary>

        /// 创建具有指定的名称和格式的流。

        /// </summary>

        private Stream CreateStream(string name, string fileNameExtension,

      Encoding encoding, string mimeType, bool willSeek)
        {

            Stream stream = new FileStream(name + "." + fileNameExtension,

              FileMode.Create);

            m_streams.Add(stream);

            return stream;

        }



        /// <summary>

        /// 打印输出

        /// </summary>

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {

            Metafile pageImage =

              new Metafile(m_streams[m_currentPageIndex]);

            ev.Graphics.DrawImage(pageImage, ev.PageBounds);

            m_currentPageIndex++;

            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);

        }



        /// <summary>

        /// 打印预处理

        /// </summary>

        private void Print()
        {

            PrintDocument printDoc = new PrintDocument();

            string printerName = printDoc.PrinterSettings.PrinterName;

            if (m_streams == null || m_streams.Count == 0)

                return;

            printDoc.PrinterSettings.PrinterName = printerName;

            if (!printDoc.PrinterSettings.IsValid)
            {

                string msg = String.Format("Can't find printer \"{0}\".", printerName);

                throw new Exception(msg);

            }

            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

            StandardPrintController spc = new StandardPrintController();

            printDoc.PrintController = spc;

            printDoc.Print();

        }



        public void Dispose()
        {

            if (m_streams != null)
            {

                foreach (Stream stream in m_streams)

                    stream.Close();

                m_streams = null;

            }

        }



        /// <summary>

        /// 对外接口,启动打印

        /// </summary>

        /// <param name="dtSource">打印报表对应的数据源</param>

        /// <param name="sReport">打印报表名称</param>

        public static void Run(LocalReport report)
        {

            //LocalReport report = new LocalReport();

            //report.ReportPath = @"..\..\" + sReport;

            //ReportDataSource dataset = new ReportDataSource("DataSet1", dtSource);

            //report.DataSources.Add(dataset);

            m_currentPageIndex = 0;

            BillPrint billPrint = new BillPrint();

            billPrint.Export(report);

            billPrint.Print();

            billPrint.Dispose();

        }



        /// <summary>

        /// 获取打印机状态

        /// </summary>

        /// <param name="printerName">打印机名称</param>

        /// <param name="status">输出打印机状态</param>

        private static void GetPrinterStatus2(string printerName, ref uint status)
        {

            try
            {



                string lcPrinterName = printerName;

                IntPtr liHandle = IntPtr.Zero;

                if (!Win32.OpenPrinter(lcPrinterName, out liHandle, IntPtr.Zero))
                {

                    Console.WriteLine("print  is close");

                    return;

                }

                UInt32 level = 2;

                UInt32 sizeNeeded = 0;

                IntPtr buffer = IntPtr.Zero;

                Win32.GetPrinter(liHandle, level, buffer, 0, out sizeNeeded);

                buffer = Marshal.AllocHGlobal((int)sizeNeeded);

                if (!Win32.GetPrinter(liHandle, level, buffer, sizeNeeded, out sizeNeeded))
                {

                    Console.WriteLine(Environment.NewLine + "Fail GetPrinter:" + Marshal.GetLastWin32Error());

                    return;

                }



                Win32.PRINTER_INFO_2 info = (Win32.PRINTER_INFO_2)Marshal.PtrToStructure(buffer, typeof(Win32.PRINTER_INFO_2));

                status = info.Status;

                Marshal.FreeHGlobal(buffer);

                Win32.ClosePrinter(liHandle);

            }

            catch (Exception ex)
            {

                throw ex;

            }

        }



        /// <summary>

        /// 对外接口,调去打印机信息

        /// </summary>

        /// <param name="printerName">打印机名称</param>

        /// <returns>返回打印机当前状态</returns>

        public static string GetPrinterStatus(string printerName)
        {

            uint intValue = 0;

            PrintDocument pd = new PrintDocument();

            printerName = printerName == "" ? pd.PrinterSettings.PrinterName : printerName;

            GetPrinterStatus2(printerName, ref intValue);

            string strRet = string.Empty;

            switch (intValue)
            {

                case 0:

                    strRet = "准备就绪（Ready）";

                    break;

                case 4194432:

                    strRet = "被打开（Lid Open）";

                    break;

                case 144:

                    strRet = "打印纸用完（Out of Paper）";

                    break;

                case 4194448:

                    strRet = "被打开并且打印纸用完（Out of Paper && Lid Open）";

                    break;

                case 1024:

                    strRet = "打印中（Printing）";

                    break;

                case 32768:

                    strRet = "初始化（Initializing）";

                    break;

                case 160:

                    strRet = "手工送纸(Manual Feed in Progress)";

                    break;

                case 4096:

                    strRet = "脱机(Offline)";

                    break;

                default:

                    strRet = "未知状态（unknown state）";

                    break;

            }

            return strRet;

        }

    }

    public class Win32
    {

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]

        public static extern bool OpenPrinter(string printer, out IntPtr handle, IntPtr printerDefaults);

        [DllImport("winspool.drv")]

        public static extern bool ClosePrinter(IntPtr handle);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]

        public static extern bool GetPrinter(IntPtr handle, UInt32 level, IntPtr buffer, UInt32 size, out UInt32 sizeNeeded);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        public struct PRINTER_INFO_2
        {

            public string pServerName;

            public string pPrinterName;

            public string pShareName;

            public string pPortName;

            public string pDriverName;

            public string pComment;

            public string pLocation;

            public IntPtr pDevMode;

            public string pSepFile;

            public string pPrintProcessor;

            public string pDatatype;

            public string pParameters;

            public IntPtr pSecurityDescriptor;

            public UInt32 Attributes;

            public UInt32 Priority;

            public UInt32 DefaultPriority;

            public UInt32 StartTime;

            public UInt32 UntilTime;

            public UInt32 Status;

            public UInt32 cJobs;

            public UInt32 AveragePPM;

        }

    }
    #endregion
}

using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace ReportPrinting.Model
{
    public class RdlcPrintSettings
    {
        public void AddReport(ReportDataSource dtSource, List<ReportParameter> parameter, bool isSuccess)
        {
            var report = new LocalReport();
            report.DataSources.Add(dtSource);//1.数据源

            string path = @"OrderReport\Report1.rdlc";
            report.ReportPath = path;//2.报表文件路径赋值
            if (parameter.Count() > 0)
            {
                report.SetParameters(parameter);//3.报表参数赋值
            }

            List<LocalReport> reports = new List<LocalReport>();
            reports.Add(report);//正常


            Model.PrinterAssembleInfo info = new Model.PrinterAssembleInfo()
            {
                ControlView = "Run",
                Id = "18",
                IsRun = false,
                IsRunView = "未开启",
                Note = new Dictionary<DateTime, string> { },
                PrinterAlias = "NA319",
                PrinterBoxName = "",
                PrinterName = "Microsoft Print to PDF",
                ReportName = null,
                Status = "空",
                isNoteFormShow = false
            };


            if (reports.Count > 0)
            {
                RdlcPrint.Run(reports, "Microsoft Print to PDF", info);
                reports = new List<LocalReport>();
            }
        }

        public bool Print()
        {
            string printerName = "Microsoft Print to PDF";//打印机名称设置


            PrintDocument pd = new PrintDocument();
            StandardPrintController spc = new StandardPrintController();
            try
            {
                pd.PrintPage += new PrintPageEventHandler(this.DrawLabelPic);
                //pd.PrinterSettings.Copies = (short)1;
                //选择默认的打印机
                //pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName; //System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
                pd.PrinterSettings.PrinterName = printerName;
                pd.PrintController = spc;
                pd.Print();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                pd.Dispose();
            }
        }


        public void DrawLabelPic(Object Sender, PrintPageEventArgs av)
        {

        }
    }
}

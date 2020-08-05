using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common
{
    public class PrinterSettingHelper
    {
        public string printerName = System.Configuration.ConfigurationSettings.AppSettings["PrinterName"];
        public int printCount;
        public string printContent; //打印內容
        public List<string> strList;

        public PrinterSettingHelper()
        {

        }

        public PrinterSettingHelper(int printCount, string printContent)
        {
            this.printContent = printContent;
            this.printCount = printCount;

        }

        /// <param name="printCount">文档打印页数</param>
        /// <param name="list">值</param>
        public PrinterSettingHelper(int printCount, List<string> list)
        {
            // TODO: Complete member initialization
            this.printCount = printCount;
            this.strList = list;
        }

        private string Get128CodeString(string inputData)//转换CODE128码
        {
            string result;
            int checksum = 104;
            for (int ii = 0; ii < inputData.Length; ii++)
            {
                if (inputData[ii] >= 32)
                {
                    checksum += (inputData[ii] - 32) * (ii + 1);
                }
                else
                {
                    checksum += (inputData[ii] + 64) * (ii + 1);
                }
            }
            checksum = checksum % 103;
            if (checksum < 95)
            {
                checksum += 32;
            }
            else
            {
                checksum += 100;
            }
            result = Convert.ToChar(204) + inputData.ToString() + Convert.ToChar(checksum) + Convert.ToChar(206);
            return result;
        }

        public void PrintLable()//打印
        {
            PrintDocument pd = new PrintDocument();
            StandardPrintController controler = new StandardPrintController();

            pd.DefaultPageSettings.PaperSize = new PaperSize("LMS", 204, 130);   //1/100英寸為1單位   1毫米=3.93   52mm 33mm

            try
            {
                pd.PrintPage += new PrintPageEventHandler(this.PrintCustomLable);
                pd.PrinterSettings.Copies = (short)printCount;
                pd.PrinterSettings.PrinterName = printerName;

                pd.PrintController = controler;
                pd.Print();
                return;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }
            finally
            {
                pd.Dispose();
            }

        }

        public void PrintCustomLable(Object Sender, PrintPageEventArgs av)//打印的内容
        {


            Brush br = new SolidBrush(Color.Black);
            Margins margins = new Margins(0, 0, 0, 0);
            av.PageSettings.Margins = margins;


            Font ft = new System.Drawing.Font("Times New Roman", 15, FontStyle.Regular, GraphicsUnit.World);

            if (this.strList != null)
            {
                //条码
                for (int i = 0; i < this.strList.Count; i++)
                {
                    av.Graphics.DrawString(this.strList[i], ft, br, 10, 5 + i * 18);
                }
            }

            av.Graphics.DrawString(Get128CodeString(printContent), ft, br, 80, 10);
            av.Graphics.DrawString(DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), ft, br, 90, 50);
            av.HasMorePages = false;



        }

        public List<string> GetPrinters()
        {
            List<string> printers = new List<string>();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                printers.Add(PrinterSettings.InstalledPrinters[i]);
            }
            return printers;
        }

        public string GetDefaultPrinterName()
        {
            using (PrintDocument pd = new PrintDocument())
            {
                return pd.PrinterSettings.PrinterName;
            }
        }
    }
}

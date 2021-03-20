using Microsoft.Reporting.WebForms;
using ReportPrinting.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool PrintLable()//打印
        {
            PrintDocument pd = new PrintDocument();
            StandardPrintController controler = new StandardPrintController();

            try
            {
                pd.PrintPage += new PrintPageEventHandler(this.DrawLabelPic);
                pd.PrinterSettings.Copies = (short)1;
                //选择默认的打印机
                //pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName; //System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
                pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                pd.PrintController = controler;

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
                av.Graphics.TranslateTransform(320, 110);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString(labelInfo.page2Info.row1, new Font("Arial", 10, FontStyle.Bold), brush, 0, 0);

                av.Graphics.DrawString(labelInfo.page2Info.row2, new Font("Arial", 8, FontStyle.Regular), brush, 12, 10);
                av.Graphics.DrawString(labelInfo.page2Info.row3, new Font("Arial", 8, FontStyle.Regular), brush, 12, 20);
                av.Graphics.DrawString(labelInfo.page2Info.row4, new Font("Arial", 10, FontStyle.Bold), brush, 300, 40);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 60);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 60);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 60);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 150, 60);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 200, 60);
                av.Graphics.DrawString(labelInfo.page2Info.row5[0].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 250, 60);

                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_1, new Font("Arial", 8, FontStyle.Bold), brush, 10, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_2, new Font("Arial", 8, FontStyle.Bold), brush, 60, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_3, new Font("Arial", 8, FontStyle.Bold), brush, 100, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_4, new Font("Arial", 8, FontStyle.Bold), brush, 150, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_5, new Font("Arial", 8, FontStyle.Bold), brush, 200, 70);
                av.Graphics.DrawString(labelInfo.page2Info.row5[1].row_6, new Font("Arial", 8, FontStyle.Bold), brush, 250, 70);

                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 80);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 80);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 80);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 150, 80);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 200, 80);
                av.Graphics.DrawString(labelInfo.page2Info.row5[2].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 250, 80);

                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_1, new Font("Arial", 8, FontStyle.Regular), brush, 10, 90);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_2, new Font("Arial", 8, FontStyle.Regular), brush, 60, 90);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_3, new Font("Arial", 8, FontStyle.Regular), brush, 100, 90);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_4, new Font("Arial", 8, FontStyle.Regular), brush, 150, 90);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_5, new Font("Arial", 8, FontStyle.Regular), brush, 200, 90);
                av.Graphics.DrawString(labelInfo.page2Info.row5[3].row_6, new Font("Arial", 8, FontStyle.Regular), brush, 250, 90);
                av.Graphics.ResetTransform();
                #endregion

                int height = 120;
                #region  page3
                av.Graphics.DrawString(labelInfo.page3Info.row1, new Font("Arial", 8, FontStyle.Bold), brush, 30, 40 + height);
                av.Graphics.DrawString(labelInfo.page3Info.row2, new Font("Arial", 8, FontStyle.Regular), brush, 160, 70 + height);
                av.Graphics.DrawString(labelInfo.page3Info.row3, new Font("Arial", 8, FontStyle.Regular), brush, 180, 80 + height);
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo.page3Info.row4), 80, 90 + height, 200, 30);
                av.Graphics.DrawString(labelInfo.page3Info.row4, new Font("Arial", 7, FontStyle.Bold), brush, 170, 90 + height + 30);
                #endregion
            }
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
}

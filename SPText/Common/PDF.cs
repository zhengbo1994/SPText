using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common
{
    /// <summary>
    /// PDF 的摘要说明
    /// </summary>
    public class PDF
    {
        private static System.Web.HttpServerUtility Server = System.Web.HttpContext.Current.Server;
        public static string PrintPdf(System.Data.DataTable datatable, string txtname)
        {

            DataView dataView = datatable.DefaultView;
            System.Data.DataTable dataTableDistinct = dataView.ToTable(true, "BatchNumber");
            System.Data.DataTable dtDistinctByLens = dataView.ToTable(true, "LensCode");
            try
            {   //定义PDF
                Document document = new Document();
                PdfWriter writer = null;
                string uuulll = "~/TemporaryFile/" + txtname;
                writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
                document.Open();
                BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
                int[] TableWidths = { 15, 10, 15, 10, 45 };//按百分比分配单元格宽带
                int number = 0;
                if (dtDistinctByLens.Rows.Count > 1 && dataTableDistinct.Rows.Count == 1)
                {
                    number = dtDistinctByLens.Rows.Count;
                }
                else
                {
                    number = dataTableDistinct.Rows.Count;
                }
                for (int i = 0; i < number; i++)
                {
                    if (i > 0)
                    { document.NewPage(); }

                    string bacth = null;
                    System.Data.DataTable dtsx = datatable.Select().CopyToDataTable();

                    if (dataTableDistinct.Rows.Count <= 1)
                    {
                        bacth = dataTableDistinct.Rows[0]["BatchNumber"].ToString();
                        dtsx = datatable.Select("BatchNumber='" + bacth + "'  and  LensCode='" + dtDistinctByLens.Rows[i]["LensCode"].ToString() + "'").CopyToDataTable();
                    }
                    else
                    {
                        bacth = dataTableDistinct.Rows[i]["BatchNumber"].ToString();
                        dtsx = datatable.Select("BatchNumber='" + bacth + "'").CopyToDataTable();

                    }

                    string getCussql = string.Format(@"select top 1  main.CusCode,base.Name as  ShortName from [dbo].[ConsigmentOrderSub]   sub 
                    left join  dbo.ConsigmentOrderMain   main on sub.MainId=main.ID 
                    left join  dbo.Base_Customer base  on base.Code=main.CusCode 
                    where BatchNumber='{0}'   ", bacth);
                    System.Data.DataTable dtgetCus = DBHelper.GetDataTable(getCussql);
                    string cusname = dtgetCus.Rows[0]["CusCode"].ToString() + dtgetCus.Rows[0]["ShortName"].ToString();

                    if (dtsx.Rows.Count <= 0)
                    { return ""; }
                    //每个批次编号生成1个条码（图片）
                    //SaveBaecode(bacth);
                    PdfPTable table1 = new PdfPTable(5);
                    table1.TotalWidth = 560;//设置绝对宽度
                    table1.LockedWidth = true;//使绝对宽度模式生效
                    table1.SetWidths(TableWidths);
                    HeardPdf(document, bacth, fontChinese, cusname, table1, writer);
                    #region 数据（40行为1页，超出的另起新页）
                    int dtcount = dtsx.Rows.Count;
                    int c = (int)Math.Ceiling((double)dtcount / 20);//几个40
                    for (int k = 0; k < c; k++)
                    {
                        int countnum = 0;
                        int xhcs = 20;
                        if (c == 1)
                        {
                            xhcs = dtcount;
                        }
                        if (k == (c - 1))
                        {
                            xhcs = dtcount - (k * 20);
                        }

                        if (k > 0)//大于0则代表有第二页，需要另起一页
                        {
                            countnum = 0;
                            document.NewPage();
                            table1 = new PdfPTable(5);//清空table的内容
                            table1.TotalWidth = 560;//设置绝对宽度
                            table1.LockedWidth = true;//使绝对宽度模式生效
                            table1.SetWidths(TableWidths);
                            HeardPdf(document, bacth, fontChinese, cusname, table1, writer);

                        }
                        string lens = dtsx.Rows[0]["LensCode"].ToString();
                        PdfPCell cell2 = new PdfPCell(new Paragraph(lens, fontChinese));
                        cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell2.Rowspan = xhcs;
                        table1.AddCell(cell2);
                        for (int o = 0; o < xhcs; o++)
                        {
                            string value1 = dtsx.Rows[20 * k + o]["C"].ToString();
                            PdfPCell cc1 = new PdfPCell(new Paragraph(value1, fontChinese));
                            cc1.HorizontalAlignment = Element.ALIGN_CENTER;
                            table1.AddCell(cc1);

                            string value2 = dtsx.Rows[20 * k + o]["RangeCode"].ToString();
                            PdfPCell cc2 = new PdfPCell(new Paragraph(value2, fontChinese));
                            cc2.HorizontalAlignment = Element.ALIGN_CENTER;
                            table1.AddCell(cc2);

                            string value3 = dtsx.Rows[20 * k + o]["SumQty"].ToString();
                            PdfPCell cc3 = new PdfPCell(new Paragraph(value3, fontChinese));
                            cc3.HorizontalAlignment = Element.ALIGN_CENTER;
                            table1.AddCell(cc3);

                            string value4 = lens + value2 + "#" + value3;
                            PdfContentByte cd = writer.DirectContent;
                            Barcode128 code128 = new Barcode128();
                            code128.Code = value4;
                            //code128.BarHeight = 15F;//条码的高度
                            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cd, null, null);
                            PdfPCell cc4 = new PdfPCell(image128);
                            cc4.HorizontalAlignment = Element.ALIGN_CENTER;
                            table1.AddCell(cc4);
                            countnum += Convert.ToInt32(value3);
                        }
                        //某尾加总
                        PdfPCell cfoot1 = new PdfPCell(new Paragraph("Total", fontChinese));
                        cfoot1.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell cfoot2 = new PdfPCell(new Paragraph("", fontChinese));
                        cfoot2.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell cfoot3 = new PdfPCell(new Paragraph("", fontChinese));
                        cfoot3.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell cfoot4 = new PdfPCell(new Paragraph("", fontChinese));
                        cfoot4.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell cfoot5 = new PdfPCell(new Paragraph(countnum.ToString(), fontChinese));
                        cfoot5.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cfoot1);
                        table1.AddCell(cfoot2);
                        table1.AddCell(cfoot3);
                        table1.AddCell(cfoot4);
                        table1.AddCell(cfoot5);
                        document.Add(table1);
                    }
                    #endregion

                }
                document.Close();
                //导出指定位置
                string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
                return Path;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static void HeardPdf(Document document, string bacth, iTextSharp.text.Font fontChinese, string cusname, PdfPTable table1, PdfWriter writer)
        {
            PdfContentByte cd = writer.DirectContent;
            Barcode128 code1 = new Barcode128();
            code1.Code = bacth;
            //code1.BarHeight = 20F;
            iTextSharp.text.Image jpeg01 = code1.CreateImageWithBarcode(cd, null, null);
            //第一列，装Code图片
            PdfPCell cell = new PdfPCell(jpeg01);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 15 * 3;
            cell.Colspan = 5;
            table1.AddCell(cell);
            //第二列，店家名称
            //string cusname = dtsx.Rows[0]["CusCode"].ToString() + dtsx.Rows[0]["CusNameCHT"].ToString();
            PdfPCell cell1 = new PdfPCell(new Paragraph(cusname, fontChinese));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.Colspan = 5;
            table1.AddCell(cell1);
            //第三列
            PdfPCell c1 = new PdfPCell(new Paragraph("LensCode", fontChinese));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(c1);
            c1 = new PdfPCell(new Phrase("CYL", fontChinese));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(c1);
            c1 = new PdfPCell(new Phrase("RangeCode", fontChinese));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(c1);
            c1 = new PdfPCell(new Phrase("Quantity", fontChinese));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(c1);
            c1 = new PdfPCell(new Phrase("Barcard", fontChinese));
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(c1);
        }

        public static string SaleOrderPrintPdfA4(System.Data.DataSet ds, string txtname, string type)
        {

            DataTable dtmain = ds.Tables["Main"];
            DataTable dtsub = ds.Tables["Sub"];
            DataTable dtprocess = ds.Tables["Process"];
            DataTable dtcus = ds.Tables["Cus"];
            DataTable dtfg = ds.Tables["FG"];
            DataTable dtdate = ds.Tables["Date"];
            //Server_Sale_Stock_Main serverSaleStockMain = new Server_Sale_Stock_Main();
            string title;
            string name;
            string name2;
            Document document = new Document(PageSize.A4);
            document.NewPage();
            PdfWriter writer = null;
            string uuulll = "~/TemporaryFile/" + txtname;
            writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();
            //2020-6-11
            double totalSum = 0;
            double finalTotalSum = 0;
            double balanceDueSum = 0;
            //微软雅黑
            //String fontPath = "C:\\Windows\\Fonts\\msyh.ttf";
            //BaseFont bfChinese = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //Dosis-Bold.ttf
            //BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\Dosis-Bold.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //COOLVETI.ttf
            //BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\COOLVETI.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //宋体
            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //幼圆
            //BaseFont bfChinese = BaseFont.CreateFont("c:\\windows\\fonts\\SIMYOU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //黑体
            //BaseFont bfHei = BaseFont.CreateFont(@"c:\WINDOWS\fonts\SIMHEI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 13, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese1 = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese2 = new iTextSharp.text.Font(bfChinese, 14, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese3 = new iTextSharp.text.Font(bfChinese, 19, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese4 = new iTextSharp.text.Font(bfChinese, 17, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese11 = new iTextSharp.text.Font(bfChinese, 10, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinesebold = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));

            string url = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Fonts\Dosis-Bold.ttf";
            BaseFont bfChineseTS = BaseFont.CreateFont(url, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontChineseTS = new iTextSharp.text.Font(bfChineseTS, 14, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));

            string ul = "../../Images/0002.jpg";
            iTextSharp.text.Image jpeg01 = iTextSharp.text.Image.GetInstance(Server.MapPath(ul));
            jpeg01.ScalePercent(5f);//A4纸张是5f
                                    //jpeg01.SetAbsolutePosition(document.PageSize.Width - 0f - 580f,
                                    //    document.PageSize.Height - 0f - 70f);
                                    //document.Add(jpeg01);
            int allwidth = 560;

            if (type == "SalesP" || type == "Bulk2")
            {
                //水印
                BaseFont bfChinese2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                BaseColor bc = new BaseColor(220, 220, 220, 0);
                iTextSharp.text.Font times = new iTextSharp.text.Font(bfChinese2, 135.5F, iTextSharp.text.Font.NORMAL, bc);
                //20200909 summer增加-jimi要求取消单可PREVIEW，但水印为Cancelled
                Phrase phrase = new Phrase("PREVIEW", times);
                if (type == "SalesP")
                {
                    if (Convert.ToInt32(dtmain.Rows[0]["Status"]) == 1)
                    {
                        phrase = new Phrase("Cancelled", times);
                    }
                }
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, phrase, 320.5F, 435.0F, 40);

            }
            #region 表1  抬头
            PdfPTable table1 = new PdfPTable(4);
            int[] tbWidths1 = { 25, 45, 13, 17 };
            table1.TotalWidth = allwidth;//设置绝对宽度
            table1.LockedWidth = true;
            table1.SetWidths(tbWidths1);
            PdfPCell cell1 = new PdfPCell(jpeg01);//jpeg01
            cell1.Rowspan = 5;
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("APOLLO OPTICAL PTE LTD.", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("1093 LOWER DELTA ROAD #05-04/05", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("UEN", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("201923712W", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);


            cell1 = new PdfPCell(new Paragraph("SINGAPORE 169204", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("Fax No:", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("+65 62569107", fontChinese));
            //dtcus.Rows[0]["FAX"].ToString()
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("", fontChinese));
            //Email:cs@dinfung.com.sg
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell1.VerticalAlignment = Element.ALIGN_TOP;//垂直
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("Office No.", fontChinese));
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.VerticalAlignment = Element.ALIGN_TOP;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("+65 63166109", fontChinese));
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.VerticalAlignment = Element.ALIGN_TOP;
            table1.AddCell(cell1);

            #endregion

            #region 表2  发货信息
            //-----------------又一行
            PdfPTable table2 = new PdfPTable(4);
            int[] tbWidths2 = { 30, 30, 23, 17 };
            table2.TotalWidth = allwidth;//设置绝对宽度
            table2.LockedWidth = true;
            table2.SetWidths(tbWidths2);

            if (type.Equals("SalesP2") || type.Equals("Bulk3"))
            {
                title = "Delivery Note";
                name = "Bill To:";
                name2 = "Ship To:";
            }
            else
            {
                title = "Tax Invoice";
                name = "Bill To:";
                name2 = "Ship To:";
            }

            PdfPCell cell2 = new PdfPCell(new Paragraph(name, fontChinese));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.MinimumHeight = 20;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);

            //Delivery Note按钮不显示Ship To

            cell2 = new PdfPCell(new Paragraph(name2, fontChinese));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(title, fontChinese4));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.Rowspan = 3;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);

            PdfContentByte cd = writer.DirectContent;
            Barcode128 code1 = new Barcode128();
            code1.Code = dtmain.Rows[0]["Number"].ToString();
            iTextSharp.text.Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
            jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - 125f,
                document.PageSize.Height - 0f - 145f);
            document.Add(jpeg02);
            //jpeg02
            cell2 = new PdfPCell(new Paragraph("", fontChinese));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.Rowspan = 3;

            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_CENTER;
            table2.AddCell(cell2);
            // 判断是否有oldNumber
            DataTable table = new DataTable();
            if (dtmain.Columns.Contains("OldNumber"))
            {
                if (dtmain.Rows[0]["OldNumber"].ToString() != "")
                {
                    string where = string.Format("Number = '{0}'", dtmain.Rows[0]["OldNumber"].ToString());
                    //table = serverSaleStockMain.GetShipByRX(where);
                }
                else if (dtmain.Rows[0]["Number"].ToString() != "")
                {
                    string where = string.Format("Number = '{0}'", dtmain.Rows[0]["Number"].ToString());
                    //table = serverSaleStockMain.GetShipByStock(where);
                }
            }

            //总部信息
            //DataTable customerHead = serverSaleStockMain.GetCustomerHead();
            DataTable customerHead = new DataTable();

            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Name"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;

            // 红
            if (type.Equals("SalesP2"))
            {

                if (table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    //客户名称
                    table2.AddCell(cell2);
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["Name"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    //客户名称
                    table2.AddCell(cell2);
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Name"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }
            else
            {
                if (table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    table2.AddCell(cell2);
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["Name"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    table2.AddCell(cell2);
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Name"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }
            //又一行

            cell2 = new PdfPCell(new Paragraph("", fontChinese));
            cell2.BorderWidth = 0;
            //cell2.MinimumHeight = 5;

            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph("", fontChinese));
            cell2.BorderWidth = 0;
            //cell2.MinimumHeight = 5;
            table2.AddCell(cell2);

            //地址赋值开始  2020-4-6 Address改取BillingAddress
            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["BillingAddress"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            if (type.Equals("SalesP2"))
            {

                if (type.Equals("SalesP2") && table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["Address"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Address"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }
            else
            {
                if (table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["Address"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Address"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }
            cell2 = new PdfPCell(new Paragraph(@"GST Reg No:", fontChinese));
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 40;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.Colspan = 2;
            table2.AddCell(cell2);
            //又一行 PostCode
            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["PostCode"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            if (type.Equals("SalesP2"))
            {

                if (type.Equals("SalesP2") && table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["PostCode"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["PostCode"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }
            else
            {
                if (table.Rows.Count != 0 && table.Rows[0]["Ship"].Equals("HQ"))
                {
                    cell2 = new PdfPCell(new Paragraph(customerHead.Rows[0]["PostCode"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
                else
                {
                    cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["PostCode"].ToString(), fontChinese1));
                    cell2.BorderWidth = 0;
                    table2.AddCell(cell2);
                }
            }


            //2020-09-11修改
            var paragraph = new Paragraph(string.Empty, fontChinese1);
            paragraph.Add("Invoice No:");
            if (type == "SalesP")
            {
                if (Convert.ToInt32(dtmain.Rows[0]["Status"]) == 1)
                {
                    paragraph.Add("\nReference No:");
                }
            }
            cell2 = new PdfPCell(paragraph);
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            cell2.Rowspan = 2;
            table2.AddCell(cell2);

            //2020-09-11 增加如果是取消单则显示Reference No单号
            var paragraphvalue = new Paragraph();
            paragraphvalue.Add(dtmain.Rows[0]["Number"].ToString());
            if (type == "SalesP")
            {
                if (Convert.ToInt32(dtmain.Rows[0]["Status"]) == 1)
                {
                    paragraphvalue.Add("\n" + dtmain.Rows[0]["Number"].ToString() + "_C");
                }
            }
            cell2 = new PdfPCell(paragraphvalue);
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            cell2.Rowspan = 2;
            table2.AddCell(cell2);

            //cell2 = new PdfPCell(new Paragraph(dtmain.Rows[0]["Number"].ToString(), fontChinesebold));
            //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell2.MinimumHeight = 25;
            //cell2.Rowspan = 2;
            //table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Singapore", fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Singapore", fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);
            //又一行
            cell2 = new PdfPCell(new Paragraph(@"", fontChinese1));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.Colspan = 2;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(@"OrderDate:", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);
            DateTime dt;
            if (!dtmain.Columns.Contains("OrderDate"))
            {
                dt = Convert.ToDateTime(dtmain.Rows[0]["OrderMakeDate"]);
            }
            else
            {
                dt = Convert.ToDateTime(dtmain.Rows[0]["OrderDate"]);
            }
            string date = dt.ToShortDateString();
            //string time = dtmain.Rows[0]["OrderDate"].ToString();
            cell2 = new PdfPCell(new Paragraph(date, fontChinese1));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);

            //又一行 电话
            cell2 = new PdfPCell(new Paragraph(@"Phone:" + dtcus.Rows[0]["Phone"].ToString(), fontChinese1));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);


            cell2 = new PdfPCell(new Paragraph(@"Fax:" + dtcus.Rows[0]["FAX"].ToString(), fontChinese1));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(@"Invoice Date:", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);
            string printtime = "";
            if (type == "Bulk")
            {
                if (!string.IsNullOrEmpty(dtdate.Rows[0]["PrintDate"].ToString()))
                {
                    printtime = dtdate.Rows[0]["PrintDate"].ToString();
                }
                else
                {
                    printtime = DateTime.Now.ToShortDateString();
                }
            }
            else if (type == "Bulk2" || type == "Bulk3")
            {
                string time = "1900-01-01";
                DateTime printDate = Convert.ToDateTime(dtdate.Rows[0]["PrintDate"].ToString());
                if (printDate == null || printDate == (Convert.ToDateTime(time)))
                {
                    printtime = "";
                }
                else
                {
                    printtime = dtdate.Rows[0]["PrintDate"].ToString();
                }
            }
            else
            {
                printtime = dtmain.Rows[0]["InvoicePrintDate"].ToString();
            }

            if (string.IsNullOrEmpty(printtime))
            {
                if (type.Contains("SalesP") || type.Contains("SalesP2"))
                {
                    printtime = "";
                }
                else if (type.Contains("Bulk2") || type.Contains("Bulk3"))
                {
                    printtime = "";
                }
                else
                {
                    printtime = DateTime.Now.ToShortDateString();
                }
            }
            else
            {
                printtime = Convert.ToDateTime(printtime).ToShortDateString();
            }
            cell2 = new PdfPCell(new Paragraph(printtime, fontChinese1));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);

            //又一行
            cell2 = new PdfPCell(new Paragraph(@"Attention", fontChinese1));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.Colspan = 2;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(@"Pages:", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(@"1", fontChinese1));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 25;
            table2.AddCell(cell2);


            #endregion

            #region 表3  订单信息
            PdfPTable table3 = new PdfPTable(5);
            int[] tbWidths3 = { 20, 15, 25, 20, 20 };//按百分比分配单元格宽带
            table3.TotalWidth = allwidth;//设置绝对宽度
            table3.LockedWidth = true;
            table3.SetWidths(tbWidths3);


            PdfPCell cell3 = new PdfPCell(new Paragraph(@"OrderNumber", fontChinese));
            //P/O Number
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);


            cell3 = new PdfPCell(new Paragraph(@"Area", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"CustomerCode", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);
            //Ship Date
            cell3 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"Terms", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            //赋值处 又一行
            cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["CustomerNumber"].ToString(), fontChinesebold));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            //Area 2021-01-18 增加（jimi要求修改）
            string areaZone = dtcus.Rows[0]["AreaZone"].ToString();
            cell3 = new PdfPCell(new Paragraph(areaZone, fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            if (type != "Bulk" && type != "Bulk2" && type != "Bulk3")
            {
                cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["CustomerCode"].ToString(), fontChinese1));
            }
            else
            {
                cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["CusCode"].ToString(), fontChinese1));
            }

            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);
            //2020-09-25 kyle要求删掉
            //cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["DeliverDate"].ToString().Contains("9999") == true ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(dtmain.Rows[0]["DeliverDate"]).ToShortDateString(), fontChinese1));
            cell3 = new PdfPCell(new Paragraph(""));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(dtcus.Rows[0]["TradeCode"].ToString() == "0D" ? "C.O.D" : dtcus.Rows[0]["TradeCode"].ToString(), fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);
            #endregion

            #region 表4  镜种及工序信息
            PdfPTable table4 = new PdfPTable(6);
            int[] tbWidths4 = { 7, 24, 37, 9, 10, 13 };//按百分比分配单元格宽带
            table4.TotalWidth = allwidth;//设置绝对宽度
            table4.LockedWidth = true;
            table4.SetWidths(tbWidths4);

            PdfPCell cell4 = new PdfPCell(new Paragraph(@"Qty", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Lens Item", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Description", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Price", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Discount", fontChinese1));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Amount(S$)", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table4.AddCell(cell4);

            #region 数据循环赋值
            for (int i = 0; i < dtsub.Rows.Count; i++)
            {
                int subid = Convert.ToInt32(dtsub.Rows[i]["SubIndex"]);
                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Quantity"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                cell4.MinimumHeight = 15;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Code"].ToString(), fontChineseTS));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Name"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                if (type.Contains("SalesP2") || type.Contains("Bulk3"))
                {
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                }
                else
                {
                    cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Price"].ToString(), fontChinese1));
                }
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);


                double lendis = (100 - Convert.ToDouble(dtsub.Rows[i]["Discount"]));
                string lensdis = lendis == 0 ? "" : Math.Round(lendis, 2) + "%";
                if (type.Contains("SalesP2") || type.Contains("Bulk3"))
                {
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                }
                else
                {
                    cell4 = new PdfPCell(new Paragraph(lensdis, fontChinese1));
                }
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                if (type.Contains("SalesP2") || type.Contains("Bulk3"))
                {
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                }
                else
                {
                    cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Total"].ToString(), fontChinese1));
                }
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                #region 非Bulk、非Bulk2、非Bulk3
                //SPHCYL等信息-----------换行
                if (type != "Bulk" && type != "Bulk2" && type != "Bulk3")
                {
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.MinimumHeight = 15;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    string sphcyl = "";
                    if (dtsub.Rows[i]["SubIndex"].ToString() == "10")
                    {
                        sphcyl += "R:";
                    }
                    else if (dtsub.Rows[i]["SubIndex"].ToString() == "20")
                    {
                        sphcyl += "L:";
                    }
                    sphcyl += "SPH:" + dtsub.Rows[i]["SPH"].ToString() + ";";
                    sphcyl += "CYL:" + dtsub.Rows[i]["CYL"].ToString() + ";";
                    sphcyl += "AXIS:" + dtsub.Rows[i]["WarehouseCode"].ToString();
                    cell4 = new PdfPCell(new Paragraph(sphcyl, fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    //又一行ADD
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.MinimumHeight = 15;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    string sphcy2 = "";
                    sphcy2 += "ADD:" + dtsub.Rows[i]["ADD"].ToString() + "; ";
                    sphcy2 += "PD:" + dtsub.Rows[i]["ThreeNumber"].ToString() + "; ";
                    sphcy2 += "H:" + dtsub.Rows[i]["ActivityNumber"].ToString();
                    cell4 = new PdfPCell(new Paragraph(sphcy2, fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);



                    DataRow[] dtselect = dtprocess.Select("SubIndex=" + subid);
                    //此处为工序
                    for (int p = 0; p < dtselect.Count(); p++)
                    {
                        if (dtselect[p]["Quantity"].ToString() == "0" && dtselect[p]["Name"].ToString() == "Coating")
                        {
                            continue;
                        }
                        string pqty = dtselect[p]["Quantity"].ToString() == "0" ? "" : dtselect[p]["Quantity"].ToString();
                        cell4 = new PdfPCell(new Paragraph(pqty, fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));//dtselect[p]["Name"].ToString()
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph(dtselect[p]["Content"].ToString(), fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        string Pprice1 = pqty == "" ? "" : dtselect[p]["UnitPrice"].ToString();
                        if (type.Contains("SalesP2"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(Pprice1, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);
                        string disprocess = "";
                        if (pqty != "")
                        {
                            double disp = 100 - Convert.ToDouble(dtselect[p]["Discount"].ToString());
                            if (disp != 0)
                            {
                                disprocess = Math.Round(disp, 2) + "%";
                            }
                        }
                        if (type.Contains("SalesP2"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(disprocess, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);

                        string Pprice2 = pqty == "" ? "" : dtselect[p]["Price"].ToString();
                        if (type.Contains("SalesP2"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(Pprice2, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);
                    }
                    //又一行-------AIXI等信息
                    string axixmess = "";
                    dtsub.Rows[i]["WarehouseCode"] = 0;
                    dtsub.Rows[i]["ThreeNumber"] = 0;
                    dtsub.Rows[i]["ActivityNumber"] = 0;
                    if (Convert.ToDouble(dtsub.Rows[i]["WarehouseCode"]) != 0)
                    {
                        axixmess += "AXIS:" + dtsub.Rows[i]["WarehouseCode"].ToString() + ";";
                    }
                    if (Convert.ToDouble(dtsub.Rows[i]["ThreeNumber"]) != 0)
                    {
                        axixmess += "PD:" + dtsub.Rows[i]["ThreeNumber"].ToString() + ";";
                    }
                    if (Convert.ToDouble(dtsub.Rows[i]["ActivityNumber"]) != 0)
                    {
                        axixmess += "Height:" + dtsub.Rows[i]["ActivityNumber"].ToString();
                    }
                    if (axixmess != "")
                    {
                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.MinimumHeight = 15;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);


                        cell4 = new PdfPCell(new Paragraph(axixmess, fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);
                    }
                }
                #endregion
                #region Bulk、Bulk2、Bulk3
                else
                {
                    //Bulk订单
                    //工序
                    DataRow[] dtselect = dtprocess.Select();
                    for (int p = 0; p < dtselect.Count(); p++)
                    {
                        string pqty = dtselect[p]["Quantity"].ToString() == "0" ? "" : dtselect[p]["Quantity"].ToString();
                        cell4 = new PdfPCell(new Paragraph(pqty, fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        cell4 = new PdfPCell(new Paragraph(dtselect[p]["Content"].ToString(), fontChinese1));
                        cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        table4.AddCell(cell4);

                        string Pprice1 = pqty == "" ? "" : dtselect[p]["UnitPrice"].ToString();
                        if (type.Contains("Bulk3"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(Pprice1, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);


                        string disprocess = "";
                        if (pqty != "")
                        {
                            double disp = 100 - Convert.ToDouble(dtselect[p]["Discount"].ToString());
                            if (disp != 0)
                            {
                                disprocess = Math.Round(disp, 2) + "%";
                            }
                        }
                        if (type.Contains("Bulk3"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(disprocess, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthRight = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);

                        string Pprice2 = pqty == "" ? "" : dtselect[p]["Price"].ToString();
                        if (type.Contains("Bulk3"))
                        {
                            cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                        }
                        else
                        {
                            cell4 = new PdfPCell(new Paragraph(Pprice2, fontChinese1));
                        }
                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.BorderWidthTop = 0;
                        cell4.BorderWidthBottom = 0;
                        cell4.MinimumHeight = 15;
                        table4.AddCell(cell4);
                    }
                    //有一行Remark
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.MinimumHeight = 50;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("Remark:" + dtmain.Rows[0]["Remark"].ToString(), fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_BOTTOM;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    // Bulk要显示其他的信息
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("Please refer attached", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_BOTTOM;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    //另一行
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.MinimumHeight = 15;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("Price agreed by Andy", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                }
                #endregion
                #region 每次循环镜种后加一个换行
                if (i != dtsub.Rows.Count - 1)
                {
                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.MinimumHeight = 10;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);


                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);
                }
                #endregion
            }
            if (type != "Bulk" && type != "Bulk2" && type != "Bulk3")
            {
                //又一行 remark
                cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.MinimumHeight = 15;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);


                cell4 = new PdfPCell(new Paragraph("Remark:" + dtmain.Rows[0]["Remark"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph("", fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);
            }
            #endregion

            #endregion

            #region 表5  加总
            PdfPTable table5 = new PdfPTable(4);
            int[] tbWidths5 = { 45, 15, 20, 20 };//按百分比分配单元格宽带
            table5.TotalWidth = allwidth;//设置绝对宽度
            table5.LockedWidth = true;
            table5.SetWidths(tbWidths5);

            PdfPCell cell5 = new PdfPCell(new Paragraph(@"Memo:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidth = 0;
            cell5.BorderWidthTop = 0.5f;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Total:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidth = 0;
            cell5.BorderWidthTop = 0.5f;
            table5.AddCell(cell5);

            double subtotal = Convert.ToDouble(dtsub.Compute("Sum(Total)", "true"));
            double total1 = subtotal;
            if (dtprocess.Rows.Count > 0)
            {
                double processtotal = Convert.ToDouble(dtprocess.Compute("Sum(Price)", "true"));
                total1 = subtotal + processtotal;
            }

            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChinese));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph("$" + total1.ToString("f2"), fontChinese));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0.5f;
            table5.AddCell(cell5);

            //2020-6-11
            string mess = "                                             ";
            mess += @" Please make payment to: Apollo Optical Pte Ltd";
            mess += "                                             ";
            mess += @"  Bank account : DBS current 012-902668-9       ";
            cell5 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Destination"].ToString() + mess, fontChinese1));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Freight:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);
            double freight = 0;
            DataRow[] rowf = dtfg.Select(" Type='Freight'");
            if (rowf.Count() > 0)
            {
                freight = Convert.ToDouble(rowf[0]["Num"]);
            }
            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChinese));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph(@"$" + freight.ToString("f2"), fontChinese));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            string gst = "7";
            DataRow[] rowg = dtfg.Select(" Type='GST'");
            if (rowg.Count() > 0)
            {
                gst = rowg[0]["Num"].ToString();
            }
            //2020-5-22 指定客户不显示GST
            string sqlNotInGst = @"SELECT  [CustomerCode]
                            FROM [twerp].[dbo].[Base_GST_Exception]";
            DataTable dtRuleOutCustomer = DBHelper.GetDataTable(sqlNotInGst);
            List<string> listRuleOutCustomer = new List<string>();
            //dt转换为list
            if (dtRuleOutCustomer != null && dtRuleOutCustomer.Rows.Count > 0)
            {
                listRuleOutCustomer = (from p in dtRuleOutCustomer.AsEnumerable() select p.Field<string>("CustomerCode")).ToList(); //将这个集合转换成list  
            }
            //判断是saleOrder订单还是批量订单
            if (type != "Bulk" && type != "Bulk2" && type != "Bulk3")
            {
                if (listRuleOutCustomer.Contains(dtmain.Rows[0]["CustomerCode"].ToString()) || DateTime.Compare(Convert.ToDateTime(dtmain.Rows[0]["OrderDate"]), Convert.ToDateTime("2020-05-18 00:00:00")) < 0) //2020-5-22
                {
                    gst = "0";
                }
            }
            else
            {
                if (listRuleOutCustomer.Contains(dtmain.Rows[0]["CusCode"].ToString()) || DateTime.Compare(Convert.ToDateTime(dtmain.Rows[0]["OrderMakeDate"]), Convert.ToDateTime("2020-05-18 00:00:00")) < 0) //2020-5-22
                {
                    gst = "0";
                }
            }

            cell5 = new PdfPCell(new Paragraph(@"GST " + gst + "%", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidth = 0;
            cell5.Colspan = 3;
            table5.AddCell(cell5);

            //（total+freight）*gst后的价格
            double t1 = (total1 + Convert.ToDouble(freight)) * (Convert.ToDouble(gst) / 100);
            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChinese));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph("$" + t1.ToString("f2"), fontChinese));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"Final Total:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.Colspan = 3;
            table5.AddCell(cell5);


            double ttotal = (total1 + Convert.ToDouble(freight)) * (1 + (Convert.ToDouble(gst) / 100));
            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChinese));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph("$" + ttotal.ToString("f2"), fontChinese));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"", fontChinese1));
            //Sale:"+dtcus.Rows[0]["Name"].ToString()
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Less: Deposit", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);

            double less = 0;
            DataRow[] rowl = dtfg.Select(" Type='Less'");
            if (rowl.Count() > 0)
            {
                less = Convert.ToDouble(rowl[0]["Num"]);
            }
            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChinese));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph(@"$" + less.ToString("f2"), fontChinese));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"", fontChinese1));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Balance Due", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);

            if (type.Contains("SalesP2") || type.Contains("Bulk3"))
            {
                cell5 = new PdfPCell(new Paragraph("", fontChineseTS));
            }
            else
            {
                cell5 = new PdfPCell(new Paragraph("$" + ttotal.ToString("f2"), fontChineseTS));
            }
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 25;
            table5.AddCell(cell5);

            #region 最尾部
            cell5 = new PdfPCell(new Paragraph(@"RECEIVED IN GOOD CONDITION BY", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"DATE", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"APOLLO OPTICAL P/L", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.Colspan = 2;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 60;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 60;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;

            cell5.Colspan = 2;
            cell5.MinimumHeight = 60;
            table5.AddCell(cell5);

            #endregion

            #endregion

            document.Add(table1);
            document.Add(table2);
            document.Add(table3);
            document.Add(table4);
            document.Add(table5);
            document.Close();
            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;

            return Path;
        }




        public static string SaleOrderPrintPdfA5(System.Data.DataSet ds, string txtname)
        {

            DataTable dtmain = ds.Tables["Main"];
            DataTable dtsub = ds.Tables["Sub"];
            DataTable dtprocess = ds.Tables["Process"];
            DataTable dtcus = ds.Tables["Cus"];
            DataTable dtfg = ds.Tables["FG"];

            Document document = new Document(PageSize.A5, 10, 10, 10, 10);
            document.NewPage();
            PdfWriter writer = null;
            string uuulll = "~/TemporaryFile/" + txtname;
            writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();

            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 9, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese1 = new iTextSharp.text.Font(bfChinese, 8, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese2 = new iTextSharp.text.Font(bfChinese, 10, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            iTextSharp.text.Font fontChinese3 = new iTextSharp.text.Font(bfChinese, 14, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            //公司图片
            string ul = "../../Images/0002.jpg";
            iTextSharp.text.Image jpeg01 = iTextSharp.text.Image.GetInstance(Server.MapPath(ul));
            jpeg01.ScalePercent(3.5f);
            jpeg01.SetAbsolutePosition(document.PageSize.Width - 0f - 410f,
              document.PageSize.Height - 0f - 55f);
            document.Add(jpeg01);
            //Barcode128
            PdfContentByte cd = writer.DirectContent;
            Barcode128 code1 = new Barcode128();
            code1.Code = dtmain.Rows[0]["Number"].ToString();
            iTextSharp.text.Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
            jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - 80f,
                document.PageSize.Height - 0f - 96f);
            jpeg02.ScalePercent(80f);
            document.Add(jpeg02);


            int allwidth = 400;
            #region 表1  抬头
            PdfPTable table1 = new PdfPTable(4);
            int[] tbWidths1 = { 24, 41, 15, 20 };
            table1.TotalWidth = allwidth;//设置绝对宽度
            table1.LockedWidth = true;
            table1.SetWidths(tbWidths1);
            PdfPCell cell1 = new PdfPCell(new Paragraph("", fontChinese2));//jpeg01
            cell1.Rowspan = 5;
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("APOLLO OPTICAL PTE LTD.", fontChinese2));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("1093 LOWER DELTA ROAD #05-04/05", fontChinese2));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("UEN", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("201511627E", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);


            cell1 = new PdfPCell(new Paragraph("SINGAPORE 169204", fontChinese2));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("Fax No:", fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("+65 " + dtcus.Rows[0]["FAX"].ToString(), fontChinese));
            cell1.BorderWidth = 0;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("Email:cs@dinfung.com.sg", fontChinese));
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell1.VerticalAlignment = Element.ALIGN_TOP;//垂直
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("Office No.", fontChinese));
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.VerticalAlignment = Element.ALIGN_TOP;
            table1.AddCell(cell1);
            cell1 = new PdfPCell(new Paragraph("+65 63166109", fontChinese));
            cell1.BorderWidth = 0;
            cell1.MinimumHeight = 10;
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.VerticalAlignment = Element.ALIGN_TOP;
            table1.AddCell(cell1);

            #endregion

            #region 表2  发货信息
            PdfPTable table2 = new PdfPTable(4);
            int[] tbWidths2 = { 30, 30, 20, 20 };
            table2.TotalWidth = allwidth;//设置绝对宽度
            table2.LockedWidth = true;
            table2.SetWidths(tbWidths2);
            PdfPCell cell2 = new PdfPCell(new Paragraph("Bill To:", fontChinese));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.MinimumHeight = 15;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Ship To:", fontChinese));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;//水平
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Tax Invoice", fontChinese3));
            cell2.BorderWidth = 0;
            cell2.BorderWidthTop = 2;
            cell2.Rowspan = 2;
            cell2.Colspan = 2;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            table2.AddCell(cell2);


            cell2 = new PdfPCell(new Paragraph("", fontChinese));
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 5;

            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph("", fontChinese));
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 5;
            table2.AddCell(cell2);

            //地址赋值开始
            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Address"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Address"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(@"GST Reg No: 201511627E", fontChinese));
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 25;
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.Colspan = 2;
            table2.AddCell(cell2);
            //又一行
            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["PostCode"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(dtcus.Rows[0]["PostCode"].ToString(), fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(@"Invoice No:", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.MinimumHeight = 20;
            cell2.Rowspan = 2;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(dtmain.Rows[0]["Number"].ToString(), fontChinese));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.Rowspan = 2;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Singapore", fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph("Singapore", fontChinese1));
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            //又一行
            cell2 = new PdfPCell(new Paragraph(@"Phone:" + dtcus.Rows[0]["Phone"].ToString(), fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.MinimumHeight = 20;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(@"Fax:" + dtcus.Rows[0]["FAX"].ToString(), fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(@"Date:", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);

            cell2 = new PdfPCell(new Paragraph(Convert.ToDateTime(dtmain.Rows[0]["OrderMakeDate"]).ToShortDateString(), fontChinese));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);

            //又一行
            cell2 = new PdfPCell(new Paragraph(@"Attention", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.BorderWidth = 0;
            cell2.Colspan = 2;
            cell2.MinimumHeight = 20;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(@"Pages", fontChinese));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            cell2 = new PdfPCell(new Paragraph(@"1", fontChinese));
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table2.AddCell(cell2);
            #endregion

            #region 表3  订单信息
            PdfPTable table3 = new PdfPTable(5);
            int[] tbWidths3 = { 20, 25, 15, 20, 20 };//按百分比分配单元格宽带
            table3.TotalWidth = allwidth;//设置绝对宽度
            table3.LockedWidth = true;
            table3.SetWidths(tbWidths3);


            PdfPCell cell3 = new PdfPCell(new Paragraph(@"P/O Number", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 15;
            table3.AddCell(cell3);


            cell3 = new PdfPCell(new Paragraph(@"SalesPerson", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"Ship Via", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"Ship Date", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"Terms", fontChinese));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            //赋值处 又一行
            cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["OldNumber"].ToString(), fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.MinimumHeight = 20;
            table3.AddCell(cell3);


            cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["Seller"].ToString(), fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(@"", fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(dtmain.Rows[0]["DeliverDate"].ToString().Contains("9999") == true ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(dtmain.Rows[0]["DeliverDate"]).ToShortDateString(), fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);

            cell3 = new PdfPCell(new Paragraph(dtcus.Rows[0]["TradeCode"].ToString() == "0" ? "Net EOM after EOM" : dtcus.Rows[0]["TradeCode"].ToString(), fontChinese1));
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table3.AddCell(cell3);
            #endregion

            #region 表4  镜种及工序信息
            PdfPTable table4 = new PdfPTable(5);
            int[] tbWidths4 = { 7, 25, 35, 13, 20 };//按百分比分配单元格宽带
            table4.TotalWidth = allwidth;//设置绝对宽度
            table4.LockedWidth = true;
            table4.SetWidths(tbWidths4);

            PdfPCell cell4 = new PdfPCell(new Paragraph(@"Qty", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Lens Item", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Description", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Price", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            cell4 = new PdfPCell(new Paragraph(@"Amount(S$)", fontChinese));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell4.MinimumHeight = 15;
            table4.AddCell(cell4);

            #region 数据循环赋值
            for (int i = 0; i < dtsub.Rows.Count; i++)
            {
                int subid = Convert.ToInt32(dtsub.Rows[i]["SubIndex"]);
                DataRow[] dtselect = dtprocess.Select("SubIndex=" + subid);
                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Quantity"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Code"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Name"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Price"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthRight = 0;
                cell4.BorderWidthBottom = 0;
                cell4.MinimumHeight = 15;
                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Paragraph(dtsub.Rows[i]["Total"].ToString(), fontChinese1));
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.BorderWidthTop = 0;
                cell4.BorderWidthBottom = 0;
                cell4.MinimumHeight = 15;
                table4.AddCell(cell4);
                //此处为工序
                for (int p = 0; p < dtselect.Count(); p++)
                {
                    if (dtselect[p]["Quantity"].ToString() == "0" && dtselect[p]["Name"].ToString() == "Coating")
                    {
                        continue;
                    }
                    string pqty = dtselect[p]["Quantity"].ToString() == "0" ? "" : dtselect[p]["Quantity"].ToString();
                    cell4 = new PdfPCell(new Paragraph(pqty, fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph("", fontChinese1));//dtselect[p]["Name"].ToString()
                    cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    cell4 = new PdfPCell(new Paragraph(dtselect[p]["Content"].ToString(), fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    table4.AddCell(cell4);

                    string Pprice1 = pqty == "" ? "" : dtselect[p]["UnitPrice"].ToString();
                    cell4 = new PdfPCell(new Paragraph(Pprice1, fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthRight = 0;
                    cell4.BorderWidthBottom = 0;
                    cell4.MinimumHeight = 15;
                    table4.AddCell(cell4);

                    string Pprice2 = pqty == "" ? "" : dtselect[p]["Price"].ToString();
                    cell4 = new PdfPCell(new Paragraph(Pprice2, fontChinese1));
                    cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell4.BorderWidthTop = 0;
                    cell4.BorderWidthBottom = 0;
                    cell4.MinimumHeight = 15;
                    table4.AddCell(cell4);
                }
            }
            #endregion

            #endregion

            #region 表5  加总
            PdfPTable table5 = new PdfPTable(4);
            int[] tbWidths5 = { 45, 15, 20, 20 };//按百分比分配单元格宽带
            table5.TotalWidth = allwidth;//设置绝对宽度
            table5.LockedWidth = true;
            table5.SetWidths(tbWidths5);

            PdfPCell cell5 = new PdfPCell(new Paragraph(@"Memo:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.BorderWidthTop = 0.5f;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Total:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.BorderWidth = 0;
            cell5.BorderWidthTop = 0.5f;
            table5.AddCell(cell5);

            double subtotal = Convert.ToDouble(dtsub.Compute("Sum(Total)", "true"));
            double total1 = subtotal;
            if (dtprocess.Rows.Count > 0)
            {
                double processtotal = Convert.ToDouble(dtprocess.Compute("Sum(Price)", "true"));
                total1 = subtotal + processtotal;
            }

            cell5 = new PdfPCell(new Paragraph("$" + total1.ToString(), fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0.5f;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(dtcus.Rows[0]["Destination"].ToString(), fontChinese1));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Freight:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);
            string freight = "0.00";
            DataRow[] rowf = dtfg.Select(" Type='Freight'");
            if (rowf.Count() > 0)
            {
                freight = rowf[0]["Num"].ToString();
            }
            cell5 = new PdfPCell(new Paragraph(@"$" + freight, fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            string gst = "7";
            DataRow[] rowg = dtfg.Select(" Type='GST'");
            if (rowg.Count() > 0)
            {
                gst = rowg[0]["Num"].ToString();
            }
            cell5 = new PdfPCell(new Paragraph(@"GST " + gst + "%", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidth = 0;
            cell5.Colspan = 3;
            table5.AddCell(cell5);
            //（total+freight）*gst后的价格
            double t1 = (total1 + Convert.ToDouble(freight)) * (Convert.ToDouble(gst) / 100);
            cell5 = new PdfPCell(new Paragraph("$" + t1.ToString(), fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell5.MinimumHeight = 20;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"Final Total:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.Colspan = 3;
            table5.AddCell(cell5);

            double ttotal = (total1 + Convert.ToDouble(freight)) * (1 + (Convert.ToDouble(gst) / 100));
            cell5 = new PdfPCell(new Paragraph("$" + ttotal.ToString(), fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_TOP;
            cell5.MinimumHeight = 15;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            //又一行
            cell5 = new PdfPCell(new Paragraph(@"Sale:" + dtcus.Rows[0]["Name"].ToString(), fontChinese1));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Less: Deposit", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);

            string less = "0";
            DataRow[] rowl = dtfg.Select(" Type='Less'");
            if (rowl.Count() > 0)
            {
                less = rowl[0]["Num"].ToString();
            }
            cell5 = new PdfPCell(new Paragraph(@"$" + less, fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.BorderWidthBottom = 0;
            cell5.BorderWidthTop = 0;
            table5.AddCell(cell5);

            //又一行
            cell5 = new PdfPCell(new Paragraph(@"", fontChinese1));
            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 20;
            cell5.BorderWidth = 0;
            cell5.Colspan = 2;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"Balance Due:", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.BorderWidth = 0;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph("$" + ttotal, fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table5.AddCell(cell5);

            #region 最尾部
            cell5 = new PdfPCell(new Paragraph(@"RECEIVED IN GOOD CONDITION BY", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"DATE", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"DIN FUNG LENS SUPPLY PTE LTD", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.Colspan = 2;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);


            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            cell5 = new PdfPCell(new Paragraph(@"", fontChinese));
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;

            cell5.Colspan = 2;
            cell5.MinimumHeight = 15;
            table5.AddCell(cell5);

            #endregion

            #endregion

            document.Add(table1);
            document.Add(table2);
            document.Add(table3);
            document.Add(table4);
            document.Add(table5);
            document.Close();
            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
            return Path;
        }

        public static string SaleOrderIssueSlipPdf(System.Data.DataSet ds, string txtname)
        {
            DataTable dtmain = ds.Tables["Main"];
            DataTable dtsub = ds.Tables["Sub"];
            Document document = new Document(PageSize.A5);
            PdfWriter writer = null;
            string uuulll = "~/TemporaryFile/" + txtname;
            writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();
            document.NewPage();

            int[] tbWidths = { 10, 50, 20, 20 };
            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 11, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            PdfPTable table1 = new PdfPTable(4);
            table1.TotalWidth = 400;//设置绝对宽度
            table1.LockedWidth = true;
            table1.SetWidths(tbWidths);//设置每行宽度、

            //条码
            PdfContentByte cd = writer.DirectContent;
            Barcode128 code1 = new Barcode128();
            code1.Code = dtmain.Rows[0]["Number"].ToString();
            iTextSharp.text.Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
            jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - 260f,
                document.PageSize.Height - 0f - 75f);
            jpeg02.Width = 100;
            document.Add(jpeg02);

            PdfPCell cell1 = new PdfPCell(new Paragraph("", fontChinese));
            cell1.Colspan = 4;
            cell1.MinimumHeight = 40;
            table1.AddCell(cell1);
            //第一列
            cell1 = new PdfPCell(new Paragraph("R/L", fontChinese));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.MinimumHeight = 20;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("ItemCode", fontChinese));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("SPH", fontChinese));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);

            cell1 = new PdfPCell(new Paragraph("CYL", fontChinese));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table1.AddCell(cell1);
            //第二列
            for (int i = 0; i < dtsub.Rows.Count; i++)
            {
                string rl = dtsub.Rows[i]["SubIndex"].ToString() == "10" ? "R" : "L";
                cell1 = new PdfPCell(new Paragraph(rl, fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["BrandCode"].ToString(), fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["SPH"].ToString(), fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["CYL"].ToString(), fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);
            }
            document.Add(table1);
            document.Close();
            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
            return Path;
        }

        public static string SaleOrderIssueSlipPdfA5(System.Data.DataSet ds, string txtname)
        {
            DataTable dtmainall = ds.Tables["Main"];
            DataTable dtsuball = ds.Tables["Sub"];
            Document document = new Document(PageSize.A5);
            PdfWriter writer = null;
            string uuulll = "~/TemporaryFile/" + txtname;
            writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();
            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 11, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            int hh = -1;
            for (int m = 0; m < dtmainall.Rows.Count; m++)
            {
                int mainid = Convert.ToInt32(dtmainall.Rows[m]["Id"]);
                DataRow[] rowsub = dtsuball.Select(" MainId=" + mainid);
                if (rowsub.Count() <= 0) { continue; }
                DataTable dtmain = dtmainall.Select("Id=" + mainid).CopyToDataTable();
                DataTable dtsub = rowsub.CopyToDataTable();
                //开始
                if (m % 4 == 0)
                {
                    document.NewPage();
                    hh = -1;
                }
                hh = hh + 1;
                int[] tbWidths = { 10, 30, 30, 15, 15 };
                PdfPTable table1 = new PdfPTable(5);
                table1.TotalWidth = 400;//设置绝对宽度
                table1.LockedWidth = true;
                table1.SetWidths(tbWidths);//设置每行宽度、
                                           //条码
                PdfContentByte cd = writer.DirectContent;
                Barcode128 code1 = new Barcode128();
                code1.Code = dtmain.Rows[0]["Number"].ToString();

                float w = 260f;
                float h = 75f + (hh * 140f);
                iTextSharp.text.Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
                jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - w,
                    document.PageSize.Height - 0f - h);

                document.Add(jpeg02);

                PdfPCell cell1 = new PdfPCell(new Paragraph("TrayNumber:" + dtmain.Rows[0]["TrayNumber"].ToString(), fontChinese));
                if (hh == 0)
                {
                    cell1.Colspan = 5;
                    cell1.MinimumHeight = 40;
                }
                else
                {
                    cell1.Colspan = 5;
                    cell1.MinimumHeight = 80;
                    //cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                }

                table1.AddCell(cell1);
                //第一列
                cell1 = new PdfPCell(new Paragraph("R/L", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("ItemCode", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("WarehouseCode", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("SPH", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("CYL", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);
                //第二列
                for (int i = 0; i < dtsub.Rows.Count; i++)
                {
                    string rl = dtsub.Rows[i]["SubIndex"].ToString() == "10" ? "R" : "L";
                    cell1 = new PdfPCell(new Paragraph(rl, fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.MinimumHeight = 20;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["BrandCode"].ToString(), fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["WareCode"].ToString(), fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["SPH"].ToString(), fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(dtsub.Rows[i]["CYL"].ToString(), fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table1.AddCell(cell1);
                }
                //20201204增加
                if (dtsub.Rows.Count == 1)
                {
                    cell1 = new PdfPCell(new Paragraph(" ", fontChinese));
                    cell1.MinimumHeight = 20;
                    table1.AddCell(cell1);
                    cell1 = new PdfPCell(new Paragraph(" ", fontChinese));
                    table1.AddCell(cell1);
                    cell1 = new PdfPCell(new Paragraph(" ", fontChinese));
                    table1.AddCell(cell1);
                    cell1 = new PdfPCell(new Paragraph(" ", fontChinese));
                    table1.AddCell(cell1);
                    cell1 = new PdfPCell(new Paragraph(" ", fontChinese));
                    table1.AddCell(cell1);
                }
                document.Add(table1);

            }

            document.Close();
            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
            return Path;
        }
        public static string SaleOrderDeliveryPrint(DataSet ds, string txtname)
        {
            DataTable dtmain = ds.Tables["Main"];
            Document document = new Document(PageSize.A4, 40f, 40f, 10f, 20f);
            PdfWriter writer = null;
            string uuulll = "~/TemporaryFile/" + txtname;
            writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();
            PdfContentByte cd = writer.DirectContent;
            // 页码的 横轴 坐标 居中
            float xx = (document.Left + document.Right) / 2;
            // 页码的 纵轴 坐标
            float yy = document.Bottom - 10;
            int[] tbWidths = { 9, 32, 10, 9, 20, 20 };
            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font fontChinese = new Font(bfChinese, 11, Font.NORMAL, new BaseColor(0, 0, 0));
            //根据BatchNumber分组
            DataView dataView = dtmain.DefaultView;
            DataTable dtmainDistinct = dataView.ToTable(true, "BatchNumber");//注：其中ToTable（）的第一个参数为是否DISTINCT
            for (int i = 0; i < dtmainDistinct.Rows.Count; i++)
            {
                document.NewPage();
                //Phrase phrase = new Phrase(writer.PageNumber.ToString(), fontChinese);
                //// 添加文本内容，进行展示页码
                //ColumnText.ShowTextAligned(cd, Element.ALIGN_CENTER, phrase, xx, yy, 0);
                PdfPTable table1 = new PdfPTable(6);
                table1.TotalWidth = 560;//设置绝对宽度
                table1.LockedWidth = true;
                table1.SetWidths(tbWidths);//设置每行宽度、
                string batchNumber = dtmainDistinct.Rows[i]["BatchNumber"].ToString();
                DataRow[] drorder = dtmain.Select(" BatchNumber='" + batchNumber + "'");
                string area = drorder[0]["AreaZone"].ToString();

                //Delivery List
                Font deliveryList_font = new Font(bfChinese, 20, Font.NORMAL, new BaseColor(0, 0, 0));
                PdfPCell cell1_0 = new PdfPCell(new Paragraph("Delivery List", deliveryList_font));
                cell1_0.Colspan = 6;
                //cell1_0.MinimumHeight = 20;
                cell1_0.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1_0.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1_0.BorderWidth = 0;
                table1.AddCell(cell1_0);

                //DeliveryDate
                Font deliveryList_date = new Font(bfChinese, 14, Font.NORMAL, new BaseColor(0, 0, 0));
                PdfPCell cell1_1 = new PdfPCell(new Paragraph(drorder[0]["DeliveryDate"].ToString() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : drorder[0]["DeliveryDate"].ToString(), deliveryList_date));
                cell1_1.Colspan = 6;
                cell1_1.BorderWidth = 0;
                cell1_1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1_1);

                //Area
                PdfPCell cell1_2 = new PdfPCell(new Paragraph(area, deliveryList_date));
                cell1_2.Colspan = 6;
                cell1_2.BorderWidth = 0;
                cell1_2.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1_2);


                //条码
                Barcode128 code1 = new Barcode128();
                code1.Code = batchNumber;
                //code128.BarHeight = 20f;
                Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
                //jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - 360f,
                //    document.PageSize.Height - 0f - 75f);
                //document.Add(jpeg02);
                PdfPCell cell1 = new PdfPCell(jpeg02);
                cell1.Colspan = 6;
                cell1.MinimumHeight = 40;
                cell1.BorderWidth = 0;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                //DeliveryMan
                PdfPCell cell1_3 = new PdfPCell(new Paragraph(drorder[0]["DeliveryManName"].ToString(), deliveryList_date));
                cell1_3.Colspan = 6;
                cell1_3.BorderWidth = 0;
                cell1_3.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1_3.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1_3);

                //total qty
                string totalqty = drorder.Sum(x => x.Field<Int32>("SumQty")).ToString();
                PdfPCell cell1_4 = new PdfPCell(new Paragraph("Total Qty:" + totalqty, deliveryList_date));
                cell1_4.Colspan = 6;
                cell1_4.BorderWidth = 0;
                cell1_4.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1_4.VerticalAlignment = Element.ALIGN_MIDDLE;
                table1.AddCell(cell1_4);

                //列头
                cell1 = new PdfPCell(new Paragraph("CustomerCode", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);
                cell1 = new PdfPCell(new Paragraph("CustomerName", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);
                cell1 = new PdfPCell(new Paragraph("Payment", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);
                cell1 = new PdfPCell(new Paragraph("Units", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);
                cell1 = new PdfPCell(new Paragraph("OrderNumber", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);
                cell1 = new PdfPCell(new Paragraph("BarCode", fontChinese));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.MinimumHeight = 20;
                table1.AddCell(cell1);

                //
                DataView dv_cus = drorder.CopyToDataTable().DefaultView;
                DataTable dtcus_Distinct = dv_cus.ToTable(true, "CusCode");
                for (int c = 0; c < dtcus_Distinct.Rows.Count; c++)
                {
                    string cusCode = dtcus_Distinct.Rows[c]["CusCode"].ToString();
                    DataRow[] dr_cuslist = dtmain.Select(" BatchNumber='" + batchNumber + "' and  CusCode='" + cusCode + "'");
                    int totalordernum = dr_cuslist.Count();
                    //CusCode
                    cell1 = new PdfPCell(new Paragraph(cusCode, fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.MinimumHeight = 30;
                    cell1.Rowspan = totalordernum;
                    table1.AddCell(cell1);
                    //CustomerName
                    string cusName = dr_cuslist[0]["CustomerName"].ToString();
                    cell1 = new PdfPCell(new Paragraph(cusName, fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.MinimumHeight = 30;
                    cell1.Rowspan = totalordernum;
                    table1.AddCell(cell1);
                    //Payment
                    string payment = dr_cuslist[0]["PayCode"].ToString();
                    cell1 = new PdfPCell(new Paragraph(payment, fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.MinimumHeight = 30;
                    cell1.Rowspan = totalordernum;
                    table1.AddCell(cell1);
                    //Units
                    string units = dr_cuslist.Sum(x => x.Field<Int32>("SumQty")).ToString();
                    cell1 = new PdfPCell(new Paragraph(units, fontChinese));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell1.MinimumHeight = 30;
                    cell1.Rowspan = totalordernum;
                    table1.AddCell(cell1);

                    for (int s = 0; s < dr_cuslist.Count(); s++)
                    {
                        //OrderNumber
                        string orderNumber = dr_cuslist[s]["OrderNumber"].ToString();
                        cell1 = new PdfPCell(new Paragraph(orderNumber, fontChinese));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell1.MinimumHeight = 30;
                        table1.AddCell(cell1);
                        //Barcode
                        PdfContentByte cdorder = writer.DirectContent;
                        Barcode128 ordercode1 = new Barcode128();
                        ordercode1.Code = orderNumber;
                        Image order_jpeg = ordercode1.CreateImageWithBarcode(cdorder, null, null);
                        cell1 = new PdfPCell(order_jpeg);
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell1.MinimumHeight = 30;
                        table1.AddCell(cell1);
                    }

                }


                ////内容
                //for (int j = 0; j < drorder.Count(); j++)
                //{

                //    //CusCode
                //    string cusCode = drorder[j]["CusCode"].ToString();
                //    cell1 = new PdfPCell(new Paragraph(cusCode, fontChinese));
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1);
                //    //CustomerName
                //    string cusName = drorder[j]["CustomerName"].ToString();
                //    cell1 = new PdfPCell(new Paragraph(cusName, fontChinese));
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1);
                //    //Payment
                //    string payment = drorder[j]["PayCode"].ToString();
                //    cell1 = new PdfPCell(new Paragraph(payment, fontChinese));
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1);
                //    //Units
                //    string units = drorder[j]["SumQty"].ToString();
                //    cell1 = new PdfPCell(new Paragraph(units, fontChinese));
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1);
                //    //OrderNumber
                //    string orderNumber = drorder[j]["OrderNumber"].ToString();
                //    cell1 = new PdfPCell(new Paragraph(orderNumber, fontChinese));
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1); 
                //    //Barcode
                //    PdfContentByte cdorder = writer.DirectContent;
                //    Barcode128 ordercode1 = new Barcode128();
                //    ordercode1.Code = orderNumber;
                //    Image order_jpeg = ordercode1.CreateImageWithBarcode(cdorder, null, null);
                //    cell1 = new PdfPCell(order_jpeg);
                //    cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
                //    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    cell1.MinimumHeight = 30;
                //    table1.AddCell(cell1);

                //}
                document.Add(table1);
            }



            document.Close();
            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
            return Path;
        }
        //public static string SaleOrderDeliveryPrintOld(System.Data.DataSet ds, string txtname)
        //{
        //    DataTable dtmain = ds.Tables["Main"];
        //    Document document = new Document(PageSize.A4);
        //    PdfWriter writer = null;
        //    string uuulll = "~/TemporaryFile/" + txtname;
        //    writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
        //    document.Open();
        //    int[] tbWidths = { 30, 40, 30 };
        //    BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //    iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 11, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));

        //    //根据BatchNumber分组
        //    DataView dataView = dtmain.DefaultView;
        //    DataTable dtmainDistinct = dataView.ToTable(true, "BatchNumber");//注：其中ToTable（）的第一个参数为是否DISTINCT
        //    for (int i = 0; i < dtmainDistinct.Rows.Count; i++)
        //    {
        //        document.NewPage();
        //        PdfPTable table1 = new PdfPTable(3);
        //        table1.TotalWidth = 560;//设置绝对宽度
        //        table1.LockedWidth = true;
        //        table1.SetWidths(tbWidths);//设置每行宽度、
        //        string batchNumber = dtmainDistinct.Rows[i]["BatchNumber"].ToString();
        //        DataRow[] drorder = dtmain.Select(" BatchNumber='" + batchNumber + "'");
        //        string cusCode = drorder[0]["CusCode"].ToString();
        //        string cusName = drorder[0]["CustomerName"].ToString();
        //        string area = drorder[0]["AreaZone"].ToString();
        //        //条码
        //        PdfContentByte cd = writer.DirectContent;
        //        Barcode128 code1 = new Barcode128();
        //        code1.Code = batchNumber;
        //        //code128.BarHeight = 20f;
        //        iTextSharp.text.Image jpeg02 = code1.CreateImageWithBarcode(cd, null, null);
        //        //jpeg02.SetAbsolutePosition(document.PageSize.Width - 0f - 360f,
        //        //    document.PageSize.Height - 0f - 75f);
        //        //document.Add(jpeg02);
        //        PdfPCell cell1 = new PdfPCell(jpeg02);
        //        cell1.Colspan = 3;
        //        cell1.MinimumHeight = 40;
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table1.AddCell(cell1);
        //        //列头
        //        cell1 = new PdfPCell(new Paragraph("CustomerCode", fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Paragraph("CustomerName", fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);
        //        cell1 = new PdfPCell(new Paragraph("Area", fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Paragraph(cusCode, fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Paragraph(cusName, fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);

        //        cell1 = new PdfPCell(new Paragraph(area, fontChinese));
        //        cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell1.MinimumHeight = 20;
        //        table1.AddCell(cell1);
        //       //内容
        //       for (int j = 0; j < drorder.Count(); j++)
        //        {
        //            string orderNumber = drorder[j]["OrderNumber"].ToString();
        //            PdfContentByte cdnumber = writer.DirectContent;
        //            Barcode128 code128 = new Barcode128();
        //            code128.Code = orderNumber;
        //            code128.BarHeight = 20f;
        //            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cdnumber, null, null);
        //            cell1 = new PdfPCell(image128);
        //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;//水平
        //            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            cell1.Colspan = 3;
        //            cell1.MinimumHeight = 40;
        //            table1.AddCell(cell1);
        //        }
        //        document.Add(table1);
        //    }
        //    document.Close();
        //    //导出指定位置
        //    string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
        //    return Path;   
        //}

        public static string SaleOrderPrintPdfA4_VN(System.Data.DataSet ds, string txtname)
        {
            DataTable dtmain = ds.Tables["Main"];
            DataTable dtsub = ds.Tables["Sub"];
            DataTable dtprocess = ds.Tables["Process"];
            DataTable dtcus = ds.Tables["Cus"];
            //DataTable dtfg = ds.Tables["FG"];
            //DataTable dtdate = ds.Tables["Date"];
            Document document = new Document(new RectangleReadOnly(505.0F, 397.0F), 10, 10, 10, 10);
            document.NewPage();
            string uuulll = "~/TemporaryFile/" + txtname;
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(uuulll), FileMode.Create));
            document.Open();
            string ul = "../../Images/0002.jpg";
            Image jpeg01 = Image.GetInstance(Server.MapPath(ul));
            jpeg01.ScalePercent(4f);//A4纸张是5f
            jpeg01.SetAbsolutePosition(document.PageSize.Width - 300f, document.PageSize.Height - 40f);
            document.Add(jpeg01);
            //宋体
            //BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //Font fontChinese = new Font(bfChinese, 13, Font.NORMAL, new BaseColor(0, 0, 0));

            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            string cusnume = dtcus.Rows[0]["Name"].ToString();
            //document.PageSize.Width-390f→数字越小越靠右边，数字越大越靠左边
            //document.PageSize.Height-100f→数字越小越靠上面，数字越大越靠下面
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, cusnume, document.PageSize.Width - 450f, document.PageSize.Height - 100f, 0);
            string cusaddress = dtcus.Rows[0]["Address"].ToString();
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, cusaddress, document.PageSize.Width - 450f, document.PageSize.Height - 120f, 0);

            string orderdate = Convert.ToDateTime(dtmain.Rows[0]["OrderDate"]).ToString("yyyy-MM-dd");
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, orderdate, document.PageSize.Width - 120f, document.PageSize.Height - 90f, 0);

            string ponumber = dtmain.Rows[0]["OldNumber"].ToString();
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ponumber, document.PageSize.Width - 120f, document.PageSize.Height - 105f, 0);

            string trems = dtcus.Rows[0]["TradeCode"].ToString() == "0D" ? "C.O.D" : dtcus.Rows[0]["TradeCode"].ToString();
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, trems, document.PageSize.Width - 120f, document.PageSize.Height - 120f, 0);

            string trayNumber = dtmain.Rows[0]["TrayNumber"].ToString();
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, trayNumber, document.PageSize.Width - 120f, document.PageSize.Height - 135f, 0);


            for (int i = 0; i < dtsub.Rows.Count; i++)
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, dtsub.Rows[i]["Quantity"].ToString(), document.PageSize.Width - (460f), document.PageSize.Height - (170f + (i * 15)), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PC", document.PageSize.Width - 430f, document.PageSize.Height - (170f + (i * 15)), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, dtsub.Rows[i]["Code"].ToString(), document.PageSize.Width - 400f, document.PageSize.Height - (170f + (i * 15)), 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, dtsub.Rows[i]["Price"].ToString(), document.PageSize.Width - 70f, document.PageSize.Height - (170f + (i * 15)), 0);
            }

            //总计
            double subtotalQty = Convert.ToDouble(dtsub.Compute("Sum(Quantity)", "true"));
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, subtotalQty.ToString(), document.PageSize.Width - (460f), document.PageSize.Height - 320f, 0);
            double subtotalPrice = Convert.ToDouble(dtsub.Compute("Sum(Total)", "true"));
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, subtotalPrice.ToString(), document.PageSize.Width - 70f, document.PageSize.Height - 320f, 0);
            cb.EndText();




            document.Close();

            //导出指定位置
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"TemporaryFile\" + txtname;
            return Path;
        }
    }
}

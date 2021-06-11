using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPText;
using System.Drawing.Printing;
using System.IO;
using ReportPrinting.Model;
using System.Drawing.Drawing2D;

namespace ReportPrinting
{
    public partial class AS928PrintPictures : Form
    {
        public AS928PrintPictures()
        {
            InitializeComponent();
        }

        public LabelInfo labelInfo;
        public int printCount = 2;

        private int currentPageIndex = 0;

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            string sqn = this.txtNum.Text.Trim();

            string sql = $@"
                SELECT o._REFERENCE,
                       o._NUMCLI,
                       ISNULL(o._MATRIX,'') _MATRIX,
                       o._COMMENT,
                       o._SUPPL,
                       o.SPHR,
                       o.CYLR,
                       o.AXISR,
                       o.ADDR,
                       o.SPHL,
                       o.CYLL,
                       o.AXISL,
                       o.ADDL,
                       ISNULL(o._LOTNUMBER,'') _LOTNUMBER,
                       o._RUCHER,
                       ISNULL(o._MONT,'') _MONT
                FROM dbo.novacel_orders o,
                     MDDB.dbo.YJJDK y
                WHERE o.客户单号 = y.客戶單號
                AND y.流水號 = '{sqn}'";

            //DataSet dataSet = SqlHelper.GetDataSet(sql);
            DataSet dataSet = new DataSet();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                labelInfo = new LabelInfo();
                labelInfo._REFERENCE = row["_REFERENCE"].ToString();
                labelInfo._NUMCLI = row["_NUMCLI"].ToString();
                labelInfo._MATRIX = row["_MATRIX"].ToString();
                labelInfo._COMMENT = row["_COMMENT"].ToString();
                labelInfo._SUPPL = row["_SUPPL"].ToString();
                labelInfo.SPHR = Convert.ToInt32(row["SPHR"].ToString());
                labelInfo.CYLR = Convert.ToInt32(row["CYLR"].ToString());
                labelInfo.AXISR = Convert.ToInt32(row["AXISR"].ToString());
                labelInfo.ADDR = Convert.ToInt32(row["ADDR"].ToString());
                labelInfo.SPHL = Convert.ToInt32(row["SPHL"].ToString());
                labelInfo.CYLL = Convert.ToInt32(row["CYLL"].ToString());
                labelInfo.AXISL = Convert.ToInt32(row["AXISL"].ToString());
                labelInfo.ADDL = Convert.ToInt32(row["ADDL"].ToString());
                labelInfo._LOTNUMBER = row["_LOTNUMBER"].ToString();
                labelInfo._RUCHER = row["_RUCHER"].ToString();
                labelInfo._MONT = row["_MONT"].ToString();
                PrintLable();
            }
            else
            {
                MessageBox.Show("没有该订单的信息");
            }
            this.txtNum.Text = "";
            //}
        }
        /// <summary>
        /// 扫描枪扫入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            string sqn = this.txtNum.Text.Trim();

            string sql = $@"
                SELECT o._REFERENCE,
                       o._NUMCLI,
                       ISNULL(o._MATRIX,'') _MATRIX,
                       o._COMMENT,
                       o._SUPPL,
                       o.SPHR,
                       o.CYLR,
                       o.AXISR,
                       o.ADDR,
                       o.SPHL,
                       o.CYLL,
                       o.AXISL,
                       o.ADDL,
                       ISNULL(o._LOTNUMBER,'') _LOTNUMBER,
                       o._RUCHER,
                       ISNULL(o._MONT,'') _MONT
                FROM dbo.novacel_orders o,
                     MDDB.dbo.YJJDK y
                WHERE o.客户单号 = y.客戶單號
                AND y.流水號 = '{sqn}'";

            DataSet dataSet = new DataSet();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                labelInfo = new LabelInfo();
                labelInfo._REFERENCE = row["_REFERENCE"].ToString();
                labelInfo._NUMCLI = row["_NUMCLI"].ToString();
                labelInfo._MATRIX = row["_MATRIX"].ToString();
                labelInfo._COMMENT = row["_COMMENT"].ToString();
                labelInfo._SUPPL = row["_SUPPL"].ToString();
                labelInfo.SPHR = Convert.ToInt32(row["SPHR"].ToString());
                labelInfo.CYLR = Convert.ToInt32(row["CYLR"].ToString());
                labelInfo.AXISR = Convert.ToInt32(row["AXISR"].ToString());
                labelInfo.ADDR = Convert.ToInt32(row["ADDR"].ToString());
                labelInfo.SPHL = Convert.ToInt32(row["SPHL"].ToString());
                labelInfo.CYLL = Convert.ToInt32(row["CYLL"].ToString());
                labelInfo.AXISL = Convert.ToInt32(row["AXISL"].ToString());
                labelInfo.ADDL = Convert.ToInt32(row["ADDL"].ToString());
                labelInfo._LOTNUMBER = row["_LOTNUMBER"].ToString();
                labelInfo._RUCHER = row["_RUCHER"].ToString();
                labelInfo._MONT = row["_MONT"].ToString();
                PrintLable();
            }
            else
            {
                MessageBox.Show("没有该订单的信息");
            }
            this.txtNum.Text = "";
            //}
        }


        private void DrawLabelPic(Object Sender, PrintPageEventArgs av)
        {
            Margins margins = new Margins(0, 0, 0, 0);
            av.PageSettings.Margins = margins;
            Brush brush = Brushes.Black;

            //关于_MONT的字号
            int emSizeMont = 15;
            if (labelInfo._MONT.Length >= 8)
            {
                if (labelInfo._MONT.Length >= 12)
                {
                    emSizeMont = 10;
                }
                else
                {
                    emSizeMont = 12;
                }
            }

            if (currentPageIndex == 0)   //当为第一页时
            {
                //1. _REFERENCE
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo._REFERENCE.Split(';')[0]), 160, 160, 120, 25);
                av.Graphics.DrawString(labelInfo._REFERENCE.Split(';')[0], new Font("Arial", 7, FontStyle.Regular), brush, 195, 185);

                //2. _NUMCLI
                av.Graphics.DrawString(labelInfo._NUMCLI, new Font("Arial", 9, FontStyle.Regular), brush, 10, 155);

                ////3. _MATRIX
                //DmtxImageEncoderOptions opt = new DmtxImageEncoderOptions();
                ////opt.ModuleSize = moduleSize;
                ////opt.MarginSize = margin;
                //DmtxImageEncoder encoder = new DmtxImageEncoder();
                //Bitmap qrCodeImage = encoder.EncodeImage(labelInfo._MATRIX, opt);

                //QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(labelInfo._MATRIX, QRCodeGenerator.ECCLevel.Q);
                //QRCode qrcode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);

                //av.Graphics.DrawImage(qrCodeImage, 220, 225, 60, 60);

                //4. R/L 先右眼
                av.Graphics.DrawString("D", new Font("Arial", 11, FontStyle.Regular), brush, 10, 225);

                //5. Lens + Diameter + Options
                av.Graphics.DrawString(labelInfo._COMMENT.Split(';')[0], new Font("Arial", 11, FontStyle.Bold), brush, 30, 225);
                av.Graphics.DrawString(labelInfo._SUPPL.Split(';')[0], new Font("Arial", 7, FontStyle.Regular), brush, 30, 240);

                //6. Power cyl +
                double cylr = (((double)labelInfo.CYLR) / 100);
                string powerR1 = (((double)labelInfo.SPHR) / 100).ToString("f2");

                if (cylr != 0)
                {
                    powerR1 += "  " + (cylr > 0 ? ("+" + cylr.ToString("f2")) : "0") + "  " + labelInfo.AXISR + "°";
                }

                if (labelInfo.ADDR != 0)
                {
                    powerR1 += " Add " + (((double)labelInfo.ADDR) / 100).ToString("f2");
                }

                av.Graphics.DrawString(powerR1, new Font("Arial", 11, FontStyle.Bold), brush, 30, 255);

                //7. Power cyl -
                string powerR2;
                if (cylr > 0)
                {
                    powerR2 = (((double)labelInfo.SPHR + (double)labelInfo.CYLR) / 100).ToString("f2") + "  -" + cylr.ToString("f2") + "  " + (labelInfo.AXISR <= 90 ? (labelInfo.AXISR + 90) : (labelInfo.AXISR - 90)) + "° ";
                    if (labelInfo.ADDR != 0)
                    {
                        powerR2 += " Add " + (((double)labelInfo.ADDR) / 100).ToString("f2");
                    }
                }
                else
                {
                    powerR2 = powerR1;
                }

                av.Graphics.DrawString(powerR2, new Font("Arial", 7, FontStyle.Bold), brush, 30, 270);

                //8. Barcode Lot Number _LOTNUMBER
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo._LOTNUMBER), 20, 93, 150, 30);

                //9.Lot Number
                av.Graphics.TranslateTransform(172, 88);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("  LOT   " + labelInfo._LOTNUMBER, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                // 要实现 圆角化的 矩形
                Rectangle rect = new Rectangle(125, 73, 47, 16);
                int cRadius = 4;  // 圆角半径

                // 指定图形路径， 有一系列 直线/曲线 组成
                GraphicsPath myPath = new GraphicsPath();
                myPath.StartFigure();
                myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 180, 90);
                myPath.AddLine(new Point(rect.X + cRadius, rect.Y), new Point(rect.Right - cRadius, rect.Y));
                myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 270, 90);
                myPath.AddLine(new Point(rect.Right, rect.Y + cRadius), new Point(rect.Right, rect.Bottom - cRadius));
                myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 0, 90);
                myPath.AddLine(new Point(rect.Right - cRadius, rect.Bottom), new Point(rect.X + cRadius, rect.Bottom));
                myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 90, 90);
                myPath.AddLine(new Point(rect.X, rect.Bottom - cRadius), new Point(rect.X, rect.Y + cRadius));
                myPath.CloseFigure();
                av.Graphics.DrawPath(new Pen(Color.Black, (float)1.2), myPath);

                //10. Manufacture
                //SizeF sf = av.Graphics.MeasureString("HONK KONG OPTICAL LENS", new Font("Arial", 7, FontStyle.Regular));
                av.Graphics.TranslateTransform(260, 50);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("Pioneer Optical (Shenzhen) Co., Ltd", new Font("Arial", (float)7, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.TranslateTransform(260, 40);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("4~6/F, ASA Plant No. 1, East of E. Honghu Road, SongGang Street,", new Font("Arial", (float)6, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.TranslateTransform(260, 30);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("Bao’an District, Shenzhen, Guangdong, PRC", new Font("Arial", (float)6, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.DrawImage(DrawManufacture(), 260, 20, 34, 28);

                //11. Dispatcher _RUCHER
                av.Graphics.DrawString(labelInfo._RUCHER == "" ? "" : labelInfo._RUCHER, new Font("Arial", 15, FontStyle.Regular), brush, 225, 195);

                //12. Frame _MONT

                av.Graphics.DrawString(labelInfo._MONT == "" ? "" : labelInfo._MONT, new Font("Arial", emSizeMont, FontStyle.Regular), brush, 40, 180);
            }
            else if (currentPageIndex == 1)   //当为第二页时
            {
                //1. _REFERENCE
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo._REFERENCE.Split(';')[1]), 160, 160, 120, 25);
                av.Graphics.DrawString(labelInfo._REFERENCE.Split(';')[1], new Font("Arial", 7, FontStyle.Regular), brush, 195, 185);

                //2. _NUMCLI
                av.Graphics.DrawString(labelInfo._NUMCLI, new Font("Arial", 9, FontStyle.Regular), brush, 10, 155);

                ////3. _MATRIX
                //DmtxImageEncoderOptions opt = new DmtxImageEncoderOptions();
                //DmtxImageEncoder encoder = new DmtxImageEncoder();
                //Bitmap qrCodeImage = encoder.EncodeImage(labelInfo._MATRIX, opt);

                //av.Graphics.DrawImage(qrCodeImage, 220, 225, 60, 60);

                //4. R/L 先右眼
                av.Graphics.DrawString("G", new Font("Arial", 11, FontStyle.Regular), brush, 10, 225);

                //5. Lens + Diameter + Options
                av.Graphics.DrawString(labelInfo._COMMENT.Split(';')[1], new Font("Arial", 11, FontStyle.Bold), brush, 30, 225);
                av.Graphics.DrawString(labelInfo._SUPPL.Split(';')[1], new Font("Arial", 7, FontStyle.Regular), brush, 30, 240);

                //6. Power cyl +
                double cyll = (((double)labelInfo.CYLL) / 100);
                string powerL1 = (((double)labelInfo.SPHL) / 100).ToString("f2");
                if (cyll != 0)
                {
                    powerL1 += "  " + (cyll > 0 ? ("+" + cyll.ToString("f2")) : "0") + "  " + labelInfo.AXISL + "°";
                }
                if (labelInfo.ADDL != 0)
                {
                    powerL1 += " Add " + (((double)labelInfo.ADDL) / 100).ToString("f2");
                }

                av.Graphics.DrawString(powerL1, new Font("Arial", 11, FontStyle.Bold), brush, 30, 255);

                //7. Power cyl -
                string powerL2;
                if (cyll > 0)
                {
                    powerL2 = (((double)labelInfo.SPHL + (double)labelInfo.CYLL) / 100).ToString("f2") + "  -" + cyll.ToString("f2") + "  " + (labelInfo.AXISL <= 90 ? (labelInfo.AXISL + 90) : (labelInfo.AXISL - 90)) + "°";
                    if (labelInfo.ADDL != 0)
                    {
                        powerL2 += " Add " + (((double)labelInfo.ADDL) / 100).ToString("f2");
                    }
                }
                else
                {
                    powerL2 = powerL1;
                }

                av.Graphics.DrawString(powerL2, new Font("Arial", 7, FontStyle.Bold), brush, 30, 270);

                //8. Barcode Lot Number _LOTNUMBER
                av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage(labelInfo._LOTNUMBER), 20, 93, 150, 30);

                //9.Lot Number
                av.Graphics.TranslateTransform(172, 88);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("  LOT   " + labelInfo._LOTNUMBER, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                // 要实现 圆角化的 矩形
                Rectangle rect = new Rectangle(125, 73, 47, 16);
                int cRadius = 4;  // 圆角半径

                // 指定图形路径， 有一系列 直线/曲线 组成
                GraphicsPath myPath = new GraphicsPath();
                myPath.StartFigure();
                myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 180, 90);
                myPath.AddLine(new Point(rect.X + cRadius, rect.Y), new Point(rect.Right - cRadius, rect.Y));
                myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 270, 90);
                myPath.AddLine(new Point(rect.Right, rect.Y + cRadius), new Point(rect.Right, rect.Bottom - cRadius));
                myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 0, 90);
                myPath.AddLine(new Point(rect.Right - cRadius, rect.Bottom), new Point(rect.X + cRadius, rect.Bottom));
                myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 90, 90);
                myPath.AddLine(new Point(rect.X, rect.Bottom - cRadius), new Point(rect.X, rect.Y + cRadius));
                myPath.CloseFigure();
                av.Graphics.DrawPath(new Pen(Color.Black, (float)1.2), myPath);

                //10. Manufacture
                av.Graphics.TranslateTransform(260, 50);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("Pioneer Optical (Shenzhen) Co., Ltd", new Font("Arial", (float)7, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.TranslateTransform(260, 40);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("4~6/F, ASA Plant No. 1, East of E. Honghu Road, SongGang Street,", new Font("Arial", (float)6, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.TranslateTransform(260, 30);
                av.Graphics.RotateTransform(180F);
                av.Graphics.DrawString("Bao’an District, Shenzhen, Guangdong, PRC", new Font("Arial", (float)6, FontStyle.Regular), Brushes.Black, 0, 0);
                av.Graphics.ResetTransform();

                av.Graphics.DrawImage(DrawManufacture(), 260, 20, 34, 28);

                //11. Dispatcher
                av.Graphics.DrawString(labelInfo._RUCHER == "" ? "" : labelInfo._RUCHER, new Font("Arial", 15, FontStyle.Regular), brush, 225, 195);

                //12. Frame _MONT
                av.Graphics.DrawString(labelInfo._MONT == "" ? "" : labelInfo._MONT, new Font("Arial", emSizeMont, FontStyle.Regular), brush, 40, 180);
            }

            currentPageIndex++;

            if (currentPageIndex < printCount)
            {
                av.HasMorePages = true;  //如果小于定义页 那么增加新的页数
            }
            else
            {
                av.HasMorePages = false; //停止增加新的页数
                currentPageIndex = 0;
            }
        }


        /// <summary>
        /// 测试时
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="av"></param>
        private void DrawLabelPicTest(Object Sender, PrintPageEventArgs av)
        {
            Margins margins = new Margins(0, 0, 0, 0);
            av.PageSettings.Margins = margins;
            Brush brush = Brushes.Black;

            //1. _REFERENCE
            av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage("5813701"), 160, 160, 120, 25);
            av.Graphics.DrawString("5813701", new Font("Arial", 7, FontStyle.Regular), brush, 195, 185);

            //2. _NUMCLI
            av.Graphics.DrawString("88889", new Font("Arial", 9, FontStyle.Regular), brush, 10, 155);

            //3. _MATRIX
            //DmtxImageEncoderOptions opt = new DmtxImageEncoderOptions();
            //DmtxImageEncoder encoder = new DmtxImageEncoder();
            //Bitmap qrCodeImage = encoder.EncodeImage("1234", opt);

            //av.Graphics.DrawImage(qrCodeImage, 220, 220, 60, 60);

            //4. R/L 先右眼
            av.Graphics.DrawString("D", new Font("Arial", 11, FontStyle.Regular), brush, 10, 225);

            //5. Lens + Diameter + Options
            av.Graphics.DrawString("ESPRIX PROG 1,5 o75", new Font("Arial", 11, FontStyle.Bold), brush, 30, 225);
            av.Graphics.DrawString("CALIBRE AR", new Font("Arial", 7, FontStyle.Regular), brush, 30, 240);

            //6. Power cyl +
            av.Graphics.DrawString("-1.75  +0.175  90° Add 1.75", new Font("Arial", 11, FontStyle.Bold), brush, 30, 255);

            //7. Power cyl -
            av.Graphics.DrawString("-1.75  -0.75  180° Add 1.75", new Font("Arial", 7, FontStyle.Bold), brush, 30, 270);

            //8. Barcode Lot Number _LOTNUMBER
            av.Graphics.DrawImage(BarcodeHelper.GetBarcodeImage("5813758564"), 20, 93, 150, 30);

            //9.Lot Number
            //Bitmap bitmap = RotateText("0,0", 50, "[LOT] " + "5813758564", 180);
            //av.Graphics.DrawImage(bitmap, 40, 80, 150, 20); 
            //SizeF sf = av.Graphics.MeasureString(" LOT  " + "5813758564", new Font("Arial", 7, FontStyle.Regular));
            //av.Graphics.TranslateTransform(200, 80);
            av.Graphics.TranslateTransform(172, 88);
            av.Graphics.RotateTransform(180F);
            av.Graphics.DrawString("  LOT   " + "5813758564", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, 0);
            av.Graphics.ResetTransform();

            // 要实现 圆角化的 矩形
            Rectangle rect = new Rectangle(125, 73, 47, 16);
            int cRadius = 4;  // 圆角半径

            // 指定图形路径， 有一系列 直线/曲线 组成
            GraphicsPath myPath = new GraphicsPath();
            myPath.StartFigure();
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 180, 90);
            myPath.AddLine(new Point(rect.X + cRadius, rect.Y), new Point(rect.Right - cRadius, rect.Y));
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 270, 90);
            myPath.AddLine(new Point(rect.Right, rect.Y + cRadius), new Point(rect.Right, rect.Bottom - cRadius));
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 0, 90);
            myPath.AddLine(new Point(rect.Right - cRadius, rect.Bottom), new Point(rect.X + cRadius, rect.Bottom));
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 90, 90);
            myPath.AddLine(new Point(rect.X, rect.Bottom - cRadius), new Point(rect.X, rect.Y + cRadius));
            myPath.CloseFigure();
            av.Graphics.DrawPath(new Pen(Color.Black, (float)1.2), myPath);

            //10. Manufacture
            //SizeF sf = av.Graphics.MeasureString("HONK KONG OPTICAL LENS", new Font("Arial", 7, FontStyle.Regular));
            av.Graphics.TranslateTransform(260, 50);
            av.Graphics.RotateTransform(180F);
            av.Graphics.DrawString("HONK KONG OPTICAL LENS", new Font("Arial", 7, FontStyle.Regular), Brushes.Black, 0, 0);
            av.Graphics.ResetTransform();

            av.Graphics.TranslateTransform(260, 40);
            av.Graphics.RotateTransform(180F);
            av.Graphics.DrawString("4~6/F, ASA Plant No. 1, East of E. Honghu Road, SongGang Street,", new Font("Arial", 7, FontStyle.Regular), Brushes.Black, 0, 0);
            av.Graphics.ResetTransform();

            av.Graphics.TranslateTransform(260, 30);
            av.Graphics.RotateTransform(180F);
            av.Graphics.DrawString("Bao’an District, Shenzhen, Guangdong, PRC", new Font("Arial", 7, FontStyle.Regular), Brushes.Black, 0, 0);
            av.Graphics.ResetTransform();

            av.Graphics.DrawImage(DrawManufacture(), 260, 20, 34, 28);

            //11. Dispatcher
            av.Graphics.DrawString("H12", new Font("Arial", 15, FontStyle.Regular), brush, 225, 195);

            //12. Frame
            av.Graphics.DrawString("MONTAGE", new Font("Arial", 15, FontStyle.Regular), brush, 40, 180);

            //TEST
            //FileStream files = new FileStream(@"D:\document\Olly's\Desktop\3.png", FileMode.Open);
            //byte[] imgByte = new byte[files.Length];
            //files.Read(imgByte, 0, imgByte.Length);
            //string base64String = Convert.ToBase64String(imgByte);

            //av.Graphics.DrawString("HONG KONG OPTICAL LENS", new Font("Arial", (float)6.5, FontStyle.Regular), brush, 0, 0);
            //av.Graphics.DrawString("FACTORY ADDRESS", new Font("Arial", (float)6.5, FontStyle.Regular), brush, 0, 10);

            av.HasMorePages = false;
        }

        /// <summary>
        /// 生产商 logo
        /// </summary>
        /// <returns></returns>
        public Image DrawManufacture()
        {
            string imgString = "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";
            byte[] bt = Convert.FromBase64String(imgString);
            MemoryStream ms = new MemoryStream(bt);
            return Bitmap.FromStream(ms, true);
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
                pd.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
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
    }




    public class LabelInfo
    {
        public string _REFERENCE { get; set; }
        public string _NUMCLI { get; set; }
        public string _MATRIX { get; set; }
        public string _COMMENT { get; set; }
        public string _SUPPL { get; set; }
        public int SPHR { get; set; }
        public int CYLR { get; set; }
        public int AXISR { get; set; }
        public int ADDR { get; set; }
        public int SPHL { get; set; }
        public int CYLL { get; set; }
        public int AXISL { get; set; }
        public int ADDL { get; set; }
        public string _LOTNUMBER { get; set; }
        public string _RUCHER { get; set; }
        public string _MONT { get; set; }
    }
}

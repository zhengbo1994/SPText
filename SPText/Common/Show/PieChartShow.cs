using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.Show
{
    public class PieChartShow
    {
        public void Show()
        {
            List<TempDictionary> tempDictionaryList = new List<TempDictionary>();
            tempDictionaryList.Add(new TempDictionary() { Key = "GOL", Value = 10496 });
            tempDictionaryList.Add(new TempDictionary() { Key = "POI", Value = 1427 });
            tempDictionaryList.Add(new TempDictionary() { Key = "SOL", Value = 2715 });
            tempDictionaryList.Add(new TempDictionary() { Key = "Error", Value = 1000 });

            Bitmap image = GetBitmap(500, 230, "宋体", tempDictionaryList, 70);
            DateTime dateTime = DateTime.Now;
            string relativePath = @"MonthlyReport-Image_" + dateTime.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string jpgPath = Path.Combine(startupPath, @"Result\JPG\");
            CreateDirectory(jpgPath);
            string filePath = Path.Combine(jpgPath, relativePath);
            image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
        public Bitmap GetBitmap(int width, int heigh, string familyName, List<TempDictionary> data, int r)
        {
            Bitmap bitmap = new Bitmap(width, heigh);
            Graphics graphics = Graphics.FromImage(bitmap);
            //用白色填充整个图片，因为默认是黑色           
            graphics.Clear(Color.White);
            //抗锯齿           
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //高质量的文字           
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //像素均偏移0.5个单位，以消除锯齿           
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            //第一个色块的原点位置           
            PointF basePoint = new PointF(10, 20);
            //色块的大小           
            SizeF theSize = new SizeF(45, 16);
            //第一个色块的说明文字的位置           
            PointF textPoint = new PointF(basePoint.X + 50, basePoint.Y);

            DateTime dateTime = DateTime.Now;
            graphics.DrawString(string.Format("{0}年{1}月 OMS月报表", dateTime.Year, dateTime.Month), new Font(familyName, 14, FontStyle.Bold), Brushes.Black, 250, 10);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];

                RectangleF baseRectangle = new RectangleF(basePoint, theSize);
                //画代表色块               
                graphics.FillRectangle(new SolidBrush(getColor(i)), baseRectangle);
                //填充文字
                graphics.DrawString(string.Format("{0}({1})", item.Key.ToString(), item.Value), new Font(familyName, 11), Brushes.Black, textPoint);
                basePoint.Y += 30;
                textPoint.Y += 30;
            }

            //扇形区所在边框的原点位置           
            Point circlePoint = new Point(Convert.ToInt32(textPoint.X + 190), 55);
            //总比 初始值           
            float totalRate = data.Sum(p => p.Value);
            //起始角度 Y周正方向           
            float startAngle = 0;
            //当前比 初始值           
            float currentRate = 0;
            //圆所在边框的大小           
            Size cicleSize = new Size(r * 2, r * 2);
            //圆所在边框的位置           
            Rectangle circleRectangle = new Rectangle(circlePoint, cicleSize);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                //计算在圆中所占有的比例
                currentRate = float.Parse(item.Value.ToString()) / totalRate * 360;
                //填充扇形区域
                graphics.DrawPie(Pens.White, circleRectangle, startAngle, currentRate);
                graphics.FillPie(new SolidBrush(getColor(i)), circleRectangle, startAngle, currentRate);


                //至此 扇形图已经画完，下面是在扇形图上写上说明文字
                #region  至此 扇形图已经画完，下面是在扇形图上写上说明文字
                //当前圆的圆心 相对图片边框原点的坐标               
                PointF cPoint = new PointF(circlePoint.X + r, circlePoint.Y + r);
                //当前圆弧上的点               
                //cos(弧度)=X轴坐标/r               
                //弧度=角度*π/180               
                double relativeCurrentX = r * Math.Cos((360 - startAngle - currentRate / 2) * Math.PI / 180);
                double relativecurrentY = r * Math.Sin((360 - startAngle - currentRate / 2) * Math.PI / 180);
                double currentX = relativeCurrentX + cPoint.X;
                double currentY = cPoint.Y - relativecurrentY;
                //内圆上弧上的 浮点型坐标               
                PointF currentPoint = new PointF(float.Parse(currentX.ToString()), float.Parse(currentY.ToString()));
                //外圆弧上的点          
                double largerR = r + 15;
                double relativeLargerX = largerR * Math.Cos((360 - startAngle - currentRate / 2) * Math.PI / 180);
                double relativeLargerY = largerR * Math.Sin((360 - startAngle - currentRate / 2) * Math.PI / 180);
                double largerX = relativeLargerX + cPoint.X;
                double largerY = cPoint.Y - relativeLargerY;
                //外圆上弧上的 浮点型坐标                
                PointF largerPoint = new PointF(float.Parse(largerX.ToString()), float.Parse(largerY.ToString()));
                //将两个点连起来                
                graphics.DrawLine(Pens.Black, currentPoint, largerPoint);
                //外圆上 说明文字的位置                
                PointF circleTextPoint = new PointF(float.Parse(largerX.ToString()), float.Parse(largerY.ToString()));

                float xOriginal = cPoint.X;
                float yOriginal = cPoint.Y;
                if (largerX > xOriginal && largerY > yOriginal)
                {
                    circleTextPoint.Y -= 10;
                    circleTextPoint.X -= 30;
                }
                else if (largerX < xOriginal && largerY > yOriginal)
                {
                    circleTextPoint.X -= 40;
                }
                else if (largerX < xOriginal && largerY < yOriginal)
                {
                    circleTextPoint.X -= 40;
                    circleTextPoint.Y -= 15;
                }
                if (largerX > xOriginal && largerY < yOriginal)
                {
                    circleTextPoint.Y -= 8;
                }





                ////在外圆上的点的附近合适的位置 写上说明                
                //if (largerX >= 0 && largerY >= 0)//第1象限  实际第二象限                
                //{
                //    //circleTextPoint.Y -= 15;                    
                //    circleTextPoint.X -= 35;
                //}
                //if (largerX <= 0 && largerY >= 0)//第2象限  实际第三象限                
                //{
                //    //circleTextPoint.Y -= 15;                    
                //    //circleTextPoint.X -= 65;                
                //}
                //if (largerX <= 0 && largerY <= 0)//第3象限  实际第四象限                
                //{
                //    //circleTextPoint.X -= 45;               
                //    circleTextPoint.Y += 30;
                //}

                //if (largerX >= 0 && largerY <= 0)//第4象限  实际第一象限                
                //{
                //    circleTextPoint.X -= 15;
                //    //circleTextPoint.Y += 5;                
                //}
                //象限差异解释：在数学中 二维坐标轴中 右上方 全为正，在计算机处理图像时，右下方全为正。相当于顺时针移了一个象限序号                              
                graphics.DrawString(item.Key.ToString() + " " + (currentRate / 360).ToString("p2"), new Font(familyName, 11), Brushes.Black, circleTextPoint);

                #endregion
                startAngle += currentRate;
            }
            return bitmap;

        }

        public Color getColor(int i)
        {
            Color c = Color.White;
            switch (i)
            {
                case 0: c = Color.Crimson; break;
                case 1: c = Color.LightSalmon; break;
                case 2: c = Color.LimeGreen; break;
                case 3: c = Color.MediumOrchid; break;
                case 4: c = Color.Moccasin; break;
                case 5: c = Color.Khaki; break;
                case 6: c = Color.GreenYellow; break;
                case 7: c = Color.RoyalBlue; break;
                case 8: c = Color.Goldenrod; break;
                default: break;
            }
            return c;
        }

        public class TempDictionary
        {
            public string Key { get; set; }
            public int Value { get; set; }
        }

        public void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}

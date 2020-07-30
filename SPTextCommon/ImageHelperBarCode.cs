using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.IO;

namespace SPTextCommon
{
    /// <summary>
    /// 生成条形码
    /// </summary>
    public class ImageHelperBarCode
    {
        /// <summary>
        /// 使代码以64为基数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string GetCodeToBase64(string code, CodeTypeEnum codeType, DrawParameter para)
        {

            Image bitMap;
            byte[] bytemap;
            try
            {
                ICode codeinstantiation = CodeFactory.CreateCode(codeType);
                if (para != null)
                {
                    if (para.Height != 0)
                        codeinstantiation.Height = para.Height;
                    if (para.Magnify != 0)
                        codeinstantiation.Magnify = para.Magnify;
                    if (para.ViewFont != null)
                        codeinstantiation.ViewFont = para.ViewFont;
                }
                //codeinstantiation.ViewFont = new Font("宋体", 20);
                bitMap = codeinstantiation.GetCodeImage(code);  // 根据传入的参数，生成对应的“位图”；
                bytemap = ByteHelper.ImageToBytes(bitMap);
            }
            catch (Exception ex)
            {
                //code为null
                Console.WriteLine(ex.ToString());

                string appPath = Environment.CurrentDirectory;
                string path = string.Concat(appPath, "\\Image\\white.jpg");

                bytemap = ByteHelper.SaveImage(path);
            }

            string strB64 = Convert.ToBase64String(bytemap);
            return strB64;
        }
        /// <summary>
        /// 生成文字图片流，转换为其等效的字符串表示形式，使用base-64数字进行编码，并返回
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isBold"></param>
        /// <param name="fontSize"></param>
        public static string GetTextToImageReturnBase64(string text, bool isBold, int fontSize)
        {
            int wid = 400;
            int high = 200;
            Font font;
            if (isBold)
            {
                font = new Font("Arial", fontSize, FontStyle.Bold);

            }
            else
            {
                font = new Font("Arial", fontSize, FontStyle.Regular);

            }
            //绘笔颜色
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高
            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 1);
            image.Dispose();
            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明

            RectangleF rect = new RectangleF(0, 0, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();

            //旋转180度
            image = Rotate(image, 180);

            return Convert.ToBase64String(ByteHelper.ImageToBytes(image));
            //return image;
        }

        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="b">位图流</param>
        /// <param name="angle">旋转角度[0,360]</param>
        /// <returns></returns>
        public static Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;
        }

        /// <summary>
        /// 获取顶级中间数据库64
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="lensIndex"></param>
        /// <param name="br"></param>
        /// <param name="rINSSPH"></param>
        /// <param name="rINSCYL"></param>
        /// <param name="rINSAX"></param>
        /// <param name="rINSADD"></param>
        /// <param name="pM1R"></param>
        /// <param name="pM2R"></param>
        /// <param name="lINSSPH"></param>
        /// <param name="lINSCYL"></param>
        /// <param name="lINSAX"></param>
        /// <param name="lINSADD"></param>
        /// <param name="pM1L"></param>
        /// <param name="pM2L"></param>
        /// <returns></returns>
        public static string GetTopMiddleDataBase64(string edge, string lensIndex, string br, string rINSSPH, string rINSCYL, string rINSAX, string rINSADD, string pM1R, string pM2R, string lINSSPH, string lINSCYL, string lINSAX, string lINSADD, string pM1L, string pM2L)
        {
            int wid = 200;
            int high = 61;

            //字体
            Font font = new Font("Arial", 8, FontStyle.Regular);

            //笔刷
            SolidBrush brush = new SolidBrush(Color.Black);

            //位图
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //清空一下画布
            g.Clear(Color.White);

            //直接绘制
            g.DrawString(edge, new Font("Arial", 10, FontStyle.Regular), brush, 0, 0);
            g.DrawString(lensIndex, new Font("Arial", 10, FontStyle.Regular), brush, 50, 0);
            g.DrawString(br, font, brush, 150, 0);

            g.DrawString(rINSSPH, font, brush, 0, 12);
            g.DrawString(rINSCYL, font, brush, 40, 12);
            g.DrawString(rINSAX, font, brush, 80, 12);
            g.DrawString(rINSADD, font, brush, 120, 12);

            g.DrawString(pM1R, font, brush, 120, 24);
            g.DrawString(pM2R, font, brush, 160, 24);

            g.DrawString(lINSSPH, font, brush, 0, 36);
            g.DrawString(lINSCYL, font, brush, 40, 36);
            g.DrawString(lINSAX, font, brush, 80, 36);
            g.DrawString(lINSADD, font, brush, 120, 36);

            g.DrawString(pM1L, font, brush, 120, 48);
            g.DrawString(pM2L, font, brush, 160, 48);

            //释放对象
            g.Dispose();

            //旋转180度
            image = Rotate(image, 180);

            return Convert.ToBase64String(ByteHelper.ImageToBytes(image));
        }

    }

    #region  辅助信息
    public enum CodeTypeEnum
    {
        Code39 = 0,
        Code128 = 1,
        EAN13 = 2

    }

    public interface ICode
    {
        Image GetCodeImage(string code);

        int Height { get; set; }

        byte Magnify { get; set; }

        Font ViewFont { get; set; }
    }

    public class CodeFactory
    {
        public static ICode CreateCode(CodeTypeEnum codeType)
        {
            string codeName = System.Configuration.ConfigurationManager.AppSettings[codeType.ToString()];
            string assemblyCodeName = System.Configuration.ConfigurationManager.AppSettings["assemblyCodeName"];//<add key="assemblyCodeName" value="SPTextCommon" />
            return Assembly.Load(assemblyCodeName).CreateInstance(codeName) as ICode;
        }
    }

    public class DrawParameter
    {
        public int Height { get; set; }

        public byte Magnify { get; set; }

        public Font ViewFont { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height">高度,0：则是默认</param>
        /// <param name="magnify">倍数，0：则是默认</param>
        /// <param name="viewFont">画笔，null:则是默认</param>
        public DrawParameter(int height, byte magnify, Font viewFont)
        {
            this.Height = height;
            this.Magnify = magnify;
            this.ViewFont = viewFont;

        }

    }

    public class Code128 : ICode
    {
        private DataTable m_Code128 = new DataTable();
        private int m_Height = 40;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get { return m_Height; } set { m_Height = value; } }
        private Font m_ValueFont = null;
        /// <summary>
        /// 是否显示可见号码 如果为NULL不显示号码
        /// </summary>
        public Font ViewFont { get { return m_ValueFont; } set { m_ValueFont = value; } }
        private byte m_Magnify = 0;
        /// <summary>
        /// 放大倍数
        /// </summary>
        public byte Magnify { get { return m_Magnify; } set { m_Magnify = value; } }
        /// <summary>
        /// 条码类别
        /// </summary>
        public enum Encode
        {
            Code128A,
            Code128B,
            Code128C,
            EAN128
        }
        public Code128()
        {
            m_Code128.Columns.Add("ID");
            m_Code128.Columns.Add("Code128A");
            m_Code128.Columns.Add("Code128B");
            m_Code128.Columns.Add("Code128C");
            m_Code128.Columns.Add("BandCode");
            m_Code128.CaseSensitive = true;
            #region 数据表
            m_Code128.Rows.Add("0", " ", " ", "00", "212222");
            m_Code128.Rows.Add("1", "!", "!", "01", "222122");
            m_Code128.Rows.Add("2", "\"", "\"", "02", "222221");
            m_Code128.Rows.Add("3", "#", "#", "03", "121223");
            m_Code128.Rows.Add("4", "$", "$", "04", "121322");
            m_Code128.Rows.Add("5", "%", "%", "05", "131222");
            m_Code128.Rows.Add("6", "&", "&", "06", "122213");
            m_Code128.Rows.Add("7", "'", "'", "07", "122312");
            m_Code128.Rows.Add("8", "(", "(", "08", "132212");
            m_Code128.Rows.Add("9", ")", ")", "09", "221213");
            m_Code128.Rows.Add("10", "*", "*", "10", "221312");
            m_Code128.Rows.Add("11", "+", "+", "11", "231212");
            m_Code128.Rows.Add("12", ",", ",", "12", "112232");
            m_Code128.Rows.Add("13", "-", "-", "13", "122132");
            m_Code128.Rows.Add("14", ".", ".", "14", "122231");
            m_Code128.Rows.Add("15", "/", "/", "15", "113222");
            m_Code128.Rows.Add("16", "0", "0", "16", "123122");
            m_Code128.Rows.Add("17", "1", "1", "17", "123221");
            m_Code128.Rows.Add("18", "2", "2", "18", "223211");
            m_Code128.Rows.Add("19", "3", "3", "19", "221132");
            m_Code128.Rows.Add("20", "4", "4", "20", "221231");
            m_Code128.Rows.Add("21", "5", "5", "21", "213212");
            m_Code128.Rows.Add("22", "6", "6", "22", "223112");
            m_Code128.Rows.Add("23", "7", "7", "23", "312131");
            m_Code128.Rows.Add("24", "8", "8", "24", "311222");
            m_Code128.Rows.Add("25", "9", "9", "25", "321122");
            m_Code128.Rows.Add("26", ":", ":", "26", "321221");
            m_Code128.Rows.Add("27", ";", ";", "27", "312212");
            m_Code128.Rows.Add("28", "<", "<", "28", "322112");
            m_Code128.Rows.Add("29", "=", "=", "29", "322211");
            m_Code128.Rows.Add("30", ">", ">", "30", "212123");
            m_Code128.Rows.Add("31", "?", "?", "31", "212321");
            m_Code128.Rows.Add("32", "@", "@", "32", "232121");
            m_Code128.Rows.Add("33", "A", "A", "33", "111323");
            m_Code128.Rows.Add("34", "B", "B", "34", "131123");
            m_Code128.Rows.Add("35", "C", "C", "35", "131321");
            m_Code128.Rows.Add("36", "D", "D", "36", "112313");
            m_Code128.Rows.Add("37", "E", "E", "37", "132113");
            m_Code128.Rows.Add("38", "F", "F", "38", "132311");
            m_Code128.Rows.Add("39", "G", "G", "39", "211313");
            m_Code128.Rows.Add("40", "H", "H", "40", "231113");
            m_Code128.Rows.Add("41", "I", "I", "41", "231311");
            m_Code128.Rows.Add("42", "J", "J", "42", "112133");
            m_Code128.Rows.Add("43", "K", "K", "43", "112331");
            m_Code128.Rows.Add("44", "L", "L", "44", "132131");
            m_Code128.Rows.Add("45", "M", "M", "45", "113123");
            m_Code128.Rows.Add("46", "N", "N", "46", "113321");
            m_Code128.Rows.Add("47", "O", "O", "47", "133121");
            m_Code128.Rows.Add("48", "P", "P", "48", "313121");
            m_Code128.Rows.Add("49", "Q", "Q", "49", "211331");
            m_Code128.Rows.Add("50", "R", "R", "50", "231131");
            m_Code128.Rows.Add("51", "S", "S", "51", "213113");
            m_Code128.Rows.Add("52", "T", "T", "52", "213311");
            m_Code128.Rows.Add("53", "U", "U", "53", "213131");
            m_Code128.Rows.Add("54", "V", "V", "54", "311123");
            m_Code128.Rows.Add("55", "W", "W", "55", "311321");
            m_Code128.Rows.Add("56", "X", "X", "56", "331121");
            m_Code128.Rows.Add("57", "Y", "Y", "57", "312113");
            m_Code128.Rows.Add("58", "Z", "Z", "58", "312311");
            m_Code128.Rows.Add("59", "[", "[", "59", "332111");
            m_Code128.Rows.Add("60", "\\", "\\", "60", "314111");
            m_Code128.Rows.Add("61", "]", "]", "61", "221411");
            m_Code128.Rows.Add("62", "^", "^", "62", "431111");
            m_Code128.Rows.Add("63", "_", "_", "63", "111224");
            m_Code128.Rows.Add("64", "NUL", "`", "64", "111422");
            m_Code128.Rows.Add("65", "SOH", "a", "65", "121124");
            m_Code128.Rows.Add("66", "STX", "b", "66", "121421");
            m_Code128.Rows.Add("67", "ETX", "c", "67", "141122");
            m_Code128.Rows.Add("68", "EOT", "d", "68", "141221");
            m_Code128.Rows.Add("69", "ENQ", "e", "69", "112214");
            m_Code128.Rows.Add("70", "ACK", "f", "70", "112412");
            m_Code128.Rows.Add("71", "BEL", "g", "71", "122114");
            m_Code128.Rows.Add("72", "BS", "h", "72", "122411");
            m_Code128.Rows.Add("73", "HT", "i", "73", "142112");
            m_Code128.Rows.Add("74", "LF", "j", "74", "142211");
            m_Code128.Rows.Add("75", "VT", "k", "75", "241211");
            m_Code128.Rows.Add("76", "FF", "I", "76", "221114");
            m_Code128.Rows.Add("77", "CR", "m", "77", "413111");
            m_Code128.Rows.Add("78", "SO", "n", "78", "241112");
            m_Code128.Rows.Add("79", "SI", "o", "79", "134111");
            m_Code128.Rows.Add("80", "DLE", "p", "80", "111242");
            m_Code128.Rows.Add("81", "DC1", "q", "81", "121142");
            m_Code128.Rows.Add("82", "DC2", "r", "82", "121241");
            m_Code128.Rows.Add("83", "DC3", "s", "83", "114212");
            m_Code128.Rows.Add("84", "DC4", "t", "84", "124112");
            m_Code128.Rows.Add("85", "NAK", "u", "85", "124211");
            m_Code128.Rows.Add("86", "SYN", "v", "86", "411212");
            m_Code128.Rows.Add("87", "ETB", "w", "87", "421112");
            m_Code128.Rows.Add("88", "CAN", "x", "88", "421211");
            m_Code128.Rows.Add("89", "EM", "y", "89", "212141");
            m_Code128.Rows.Add("90", "SUB", "z", "90", "214121");
            m_Code128.Rows.Add("91", "ESC", "{", "91", "412121");
            m_Code128.Rows.Add("92", "FS", "|", "92", "111143");
            m_Code128.Rows.Add("93", "GS", "}", "93", "111341");
            m_Code128.Rows.Add("94", "RS", "~", "94", "131141");
            m_Code128.Rows.Add("95", "US", "DEL", "95", "114113");
            m_Code128.Rows.Add("96", "FNC3", "FNC3", "96", "114311");
            m_Code128.Rows.Add("97", "FNC2", "FNC2", "97", "411113");
            m_Code128.Rows.Add("98", "SHIFT", "SHIFT", "98", "411311");
            m_Code128.Rows.Add("99", "CODEC", "CODEC", "99", "113141");
            m_Code128.Rows.Add("100", "CODEB", "FNC4", "CODEB", "114131");
            m_Code128.Rows.Add("101", "FNC4", "CODEA", "CODEA", "311141");
            m_Code128.Rows.Add("102", "FNC1", "FNC1", "FNC1", "411131");
            m_Code128.Rows.Add("103", "StartA", "StartA", "StartA", "211412");
            m_Code128.Rows.Add("104", "StartB", "StartB", "StartB", "211214");
            m_Code128.Rows.Add("105", "StartC", "StartC", "StartC", "211232");
            m_Code128.Rows.Add("106", "Stop", "Stop", "Stop", "2331112");
            #endregion
        }
        /// <summary>
        /// 获取128图形
        /// </summary>
        /// <param name="p_Text">文字</param>
        /// <param name="p_Code">编码</param>   
        /// <returns>图形</returns>
        public Bitmap GetCodeImage(string p_Text, Encode p_Code)
        {
            string _ViewText = p_Text;
            string _Text = "";
            IList<int> _TextNumb = new List<int>();
            int _Examine = 0; //首位
            switch (p_Code)
            {
                case Encode.Code128C:
                    _Examine = 105;
                    if (!((p_Text.Length & 1) == 0)) throw new Exception("128C长度必须是偶数");
                    while (p_Text.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(p_Text.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("128C必须是数字！");
                        }
                        _Text += GetValue(p_Code, p_Text.Substring(0, 2), ref _Temp);
                        _TextNumb.Add(_Temp);
                        p_Text = p_Text.Remove(0, 2);
                    }
                    break;
                case Encode.EAN128:
                    _Examine = 105;
                    if (!((p_Text.Length & 1) == 0)) throw new Exception("EAN128长度必须是偶数");
                    _TextNumb.Add(102);
                    _Text += "411131";
                    while (p_Text.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(p_Text.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("128C必须是数字！");
                        }
                        _Text += GetValue(Encode.Code128C, p_Text.Substring(0, 2), ref _Temp);
                        _TextNumb.Add(_Temp);
                        p_Text = p_Text.Remove(0, 2);
                    }
                    break;
                default:
                    if (p_Code == Encode.Code128A)
                    {
                        _Examine = 103;
                    }
                    else
                    {
                        _Examine = 104;
                    }

                    while (p_Text.Length != 0)
                    {
                        int _Temp = 0;
                        string _ValueCode = GetValue(p_Code, p_Text.Substring(0, 1), ref _Temp);
                        if (_ValueCode.Length == 0) throw new Exception("无效的字符集!" + p_Text.Substring(0, 1).ToString());
                        _Text += _ValueCode;
                        _TextNumb.Add(_Temp);
                        p_Text = p_Text.Remove(0, 1);
                    }
                    break;
            }
            if (_TextNumb.Count == 0) throw new Exception("错误的编码,无数据");
            _Text = _Text.Insert(0, GetValue(_Examine)); //获取开始位

            for (int i = 0; i != _TextNumb.Count; i++)
            {
                _Examine += _TextNumb[i] * (i + 1);
            }
            _Examine = _Examine % 103;      //获得严效位
            _Text += GetValue(_Examine); //获取严效位
            _Text += "2331112"; //结束位
            Bitmap _CodeImage = GetImage(_Text);
            GetViewText(_CodeImage, _ViewText);
            return _CodeImage;
        }
        /// <summary>
        /// 获取目标对应的数据
        /// </summary>
        /// <param name="p_Code">编码</param>
        /// <param name="p_Value">数值 A b 30</param>
        /// <param name="p_SetID">返回编号</param>
        /// <returns>编码</returns>
        private string GetValue(Encode p_Code, string p_Value, ref int p_SetID)
        {
            if (m_Code128 == null) return "";
            DataRow[] _Row = m_Code128.Select(p_Code.ToString() + "='" + p_Value + "'");
            if (_Row.Length != 1) throw new Exception("错误的编码" + p_Value.ToString());
            p_SetID = Int32.Parse(_Row[0]["ID"].ToString());
            return _Row[0]["BandCode"].ToString();
        }
        /// <summary>
        /// 根据编号获得条纹
        /// </summary>
        /// <param name="p_CodeId"></param>
        /// <returns></returns>
        private string GetValue(int p_CodeId)
        {
            DataRow[] _Row = m_Code128.Select("ID='" + p_CodeId.ToString() + "'");
            if (_Row.Length != 1) throw new Exception("验效位的编码错误" + p_CodeId.ToString());
            return _Row[0]["BandCode"].ToString();
        }
        /// <summary>
        /// 获得条码图形
        /// </summary>
        /// <param name="p_Text">文字</param>
        /// <returns>图形</returns>
        private Bitmap GetImage(string p_Text)
        {
            char[] _Value = p_Text.ToCharArray();
            int _Width = 0;
            for (int i = 0; i != _Value.Length; i++)
            {
                _Width += Int32.Parse(_Value[i].ToString()) * (m_Magnify + 1);
            }
            Bitmap _CodeImage = new Bitmap(_Width, (int)m_Height);
            Graphics _Garphics = Graphics.FromImage(_CodeImage);
            //Pen _Pen;
            int _LenEx = 0;
            for (int i = 0; i != _Value.Length; i++)
            {
                int _ValueNumb = Int32.Parse(_Value[i].ToString()) * (m_Magnify + 1); //获取宽和放大系数
                if (!((i & 1) == 0))
                {
                    //_Pen = new Pen(Brushes.White, _ValueNumb);
                    _Garphics.FillRectangle(Brushes.White, new Rectangle(_LenEx, 0, _ValueNumb, (int)m_Height));
                }
                else
                {
                    //_Pen = new Pen(Brushes.Black, _ValueNumb);
                    _Garphics.FillRectangle(Brushes.Black, new Rectangle(_LenEx, 0, _ValueNumb, (int)m_Height));
                }
                //_Garphics.(_Pen, new Point(_LenEx, 0), new Point(_LenEx, m_Height));
                _LenEx += _ValueNumb;
            }
            _Garphics.Dispose();
            return _CodeImage;
        }
        /// <summary>
        /// 显示可见条码文字 如果小于40 不显示文字
        /// </summary>
        /// <param name="p_Bitmap">图形</param>      
        private void GetViewText(Bitmap p_Bitmap, string p_ViewText)
        {
            if (m_ValueFont == null) return;

            Graphics _Graphics = Graphics.FromImage(p_Bitmap);
            SizeF _DrawSize = _Graphics.MeasureString(p_ViewText, m_ValueFont);
            if (_DrawSize.Height > p_Bitmap.Height - 10 || _DrawSize.Width > p_Bitmap.Width)
            {
                _Graphics.Dispose();
                return;
            }

            int _StarY = p_Bitmap.Height - (int)_DrawSize.Height;
            _Graphics.FillRectangle(Brushes.White, new Rectangle(0, _StarY, p_Bitmap.Width, (int)_DrawSize.Height));
            _Graphics.DrawString(p_ViewText, m_ValueFont, Brushes.Black, 0, _StarY);
        }

        //12345678
        //(105 + (1 * 12 + 2 * 34 + 3 * 56 + 4 *78)) % 103 = 47
        //结果为starc +12 +34 +56 +78 +47 +end

        public Image GetCodeImage(string code)
        {
            return this.GetCodeImage(code, Encode.Code128A);
        }
    }
    public class Code39 : ICode
    {
        private Hashtable m_Code39 = new Hashtable();

        private byte m_Magnify = 0;
        /// <summary> 
        /// 放大倍数 
        /// </summary> 
        public byte Magnify { get { return m_Magnify; } set { m_Magnify = value; } }

        private int m_Height = 40;
        /// <summary> 
        /// 图形高 
        /// </summary> 
        public int Height { get { return m_Height; } set { m_Height = value; } }

        private Font m_ViewFont = null;
        /// <summary> 
        /// 字体大小 
        /// </summary> 
        public Font ViewFont { get { return m_ViewFont; } set { m_ViewFont = value; } }



        public Code39()
        {

            m_Code39.Add("A", "1101010010110");
            m_Code39.Add("B", "1011010010110");
            m_Code39.Add("C", "1101101001010");
            m_Code39.Add("D", "1010110010110");
            m_Code39.Add("E", "1101011001010");
            m_Code39.Add("F", "1011011001010");
            m_Code39.Add("G", "1010100110110");
            m_Code39.Add("H", "1101010011010");
            m_Code39.Add("I", "1011010011010");
            m_Code39.Add("J", "1010110011010");
            m_Code39.Add("K", "1101010100110");
            m_Code39.Add("L", "1011010100110");
            m_Code39.Add("M", "1101101010010");
            m_Code39.Add("N", "1010110100110");
            m_Code39.Add("O", "1101011010010");
            m_Code39.Add("P", "1011011010010");
            m_Code39.Add("Q", "1010101100110");
            m_Code39.Add("R", "1101010110010");
            m_Code39.Add("S", "1011010110010");
            m_Code39.Add("T", "1010110110010");
            m_Code39.Add("U", "1100101010110");
            m_Code39.Add("V", "1001101010110");
            m_Code39.Add("W", "1100110101010");
            m_Code39.Add("X", "1001011010110");
            m_Code39.Add("Y", "1100101101010");
            m_Code39.Add("Z", "1001101101010");
            m_Code39.Add("0", "1010011011010");
            m_Code39.Add("1", "1101001010110");
            m_Code39.Add("2", "1011001010110");
            m_Code39.Add("3", "1101100101010");
            m_Code39.Add("4", "1010011010110");
            m_Code39.Add("5", "1101001101010");
            m_Code39.Add("6", "1011001101010");
            m_Code39.Add("7", "1010010110110");
            m_Code39.Add("8", "1101001011010");
            m_Code39.Add("9", "1011001011010");
            m_Code39.Add("+", "1001010010010");
            m_Code39.Add("-", "1001010110110");
            m_Code39.Add("*", "1001011011010");
            m_Code39.Add("/", "1001001010010");
            m_Code39.Add("%", "1010010010010");
            m_Code39.Add(";", "1001001001010");
            m_Code39.Add(".", "1100101011010");
            m_Code39.Add(" ", "1001101011010");

        }

        public enum Code39Model
        {
            /// <summary> 
            /// 基本类别 1234567890ABC 
            /// </summary> 
            Code39Normal,
            /// <summary> 
            /// 全ASCII方式 +A+B 来表示小写 
            /// </summary> 
            Code39FullAscII
        }
        /// <summary> 
        /// 获得条码图形 
        /// </summary> 
        /// <param name="p_Text">文字信息</param> 
        /// <param name="p_Model">类别</param> 
        /// <param name="p_StarChar">是否增加前后*号</param> 
        /// <returns>图形</returns> 
        public Bitmap GetCodeImage(string p_Text, Code39Model p_Model, bool p_StarChar)
        {
            string _ValueText = "";
            string _CodeText = "";
            char[] _ValueChar = null;
            switch (p_Model)
            {
                case Code39Model.Code39Normal:
                    _ValueText = p_Text.ToUpper();
                    break;
                default:
                    _ValueChar = p_Text.ToCharArray();
                    for (int i = 0; i != _ValueChar.Length; i++)
                    {
                        if ((int)_ValueChar[i] >= 97 && (int)_ValueChar[i] <= 122)
                        {
                            _ValueText += "+" + _ValueChar[i].ToString().ToUpper();

                        }
                        else
                        {
                            _ValueText += _ValueChar[i].ToString();
                        }
                    }
                    break;
            }


            _ValueChar = _ValueText.ToCharArray();

            if (p_StarChar == true) _CodeText += m_Code39["*"];

            for (int i = 0; i != _ValueChar.Length; i++)
            {
                if (p_StarChar == true && _ValueChar[i] == '*') throw new Exception("带有起始符号不能出现*");

                object _CharCode = m_Code39[_ValueChar[i].ToString()];
                if (_CharCode == null) throw new Exception("不可用的字符" + _ValueChar[i].ToString());
                _CodeText += _CharCode.ToString();
            }


            if (p_StarChar == true) _CodeText += m_Code39["*"];

            if (p_StarChar == true)
            {
                p_Text = string.Format("*{0}*", p_Text);
            }
            Bitmap _CodeBmp = GetImage(_CodeText);
            GetViewImage(_CodeBmp, p_Text);
            return _CodeBmp;
        }



        /// <summary> 
        /// 绘制编码图形 
        /// </summary> 
        /// <param name="p_Text">编码</param> 
        /// <returns>图形</returns> 
        private Bitmap GetImage(string p_Text)
        {
            char[] _Value = p_Text.ToCharArray();


            //宽 == 需要绘制的数量*放大倍数 + 两个字的宽    
            Bitmap _CodeImage = new Bitmap(_Value.Length * ((int)m_Magnify + 1), (int)m_Height);
            Graphics _Garphics = Graphics.FromImage(_CodeImage);

            _Garphics.FillRectangle(Brushes.White, new Rectangle(0, 0, _CodeImage.Width, _CodeImage.Height));

            int _LenEx = 0;
            for (int i = 0; i != _Value.Length; i++)
            {
                int _DrawWidth = m_Magnify + 1;
                if (_Value[i] == '1')
                {
                    _Garphics.FillRectangle(Brushes.Black, new Rectangle(_LenEx, 0, _DrawWidth, m_Height));

                }
                else
                {
                    _Garphics.FillRectangle(Brushes.White, new Rectangle(_LenEx, 0, _DrawWidth, m_Height));
                }
                _LenEx += _DrawWidth;
            }



            _Garphics.Dispose();
            return _CodeImage;
        }
        /// <summary> 
        /// 绘制文字 
        /// </summary> 
        /// <param name="p_CodeImage">图形</param> 
        /// <param name="p_Text">文字</param> 
        private void GetViewImage(Bitmap p_CodeImage, string p_Text)
        {
            if (m_ViewFont == null) return;
            Graphics _Graphics = Graphics.FromImage(p_CodeImage);
            SizeF _FontSize = _Graphics.MeasureString(p_Text, m_ViewFont);

            if (_FontSize.Width > p_CodeImage.Width || _FontSize.Height > p_CodeImage.Height - 20)
            {
                _Graphics.Dispose();
                return;
            }
            int _StarHeight = p_CodeImage.Height - (int)_FontSize.Height;

            _Graphics.FillRectangle(Brushes.White, new Rectangle(0, _StarHeight, p_CodeImage.Width, (int)_FontSize.Height));

            int _StarWidth = (p_CodeImage.Width - (int)_FontSize.Width) / 2;

            _Graphics.DrawString(p_Text, m_ViewFont, Brushes.Black, _StarWidth, _StarHeight);

            _Graphics.Dispose();

        }


        public Image GetCodeImage(string code)
        {
            return this.GetCodeImage(code, Code39Model.Code39Normal, true);
        }
    }

    public class EAN13 : ICode
    {
        private DataTable m_EAN13 = new DataTable();

        private Font m_ViewFont;
        public Font ViewFont { get { return m_ViewFont; } set { m_ViewFont = value; m_FontSize = (byte)value.Size; } }

        public EAN13()
        {
            m_EAN13.Columns.Add("ID");
            m_EAN13.Columns.Add("Type");
            m_EAN13.Columns.Add("A");
            m_EAN13.Columns.Add("B");
            m_EAN13.Columns.Add("C");
            m_EAN13.Rows.Add("0", "AAAAAA", "0001101", "0100111", "1110010");
            m_EAN13.Rows.Add("1", "AABABB", "0011001", "0110011", "1100110");
            m_EAN13.Rows.Add("2", "AABBAB", "0010011", "0011011", "1101100");
            m_EAN13.Rows.Add("3", "AABBBA", "0111101", "0100001", "1000010");
            m_EAN13.Rows.Add("4", "ABAABB", "0100011", "0011101", "1011100");
            m_EAN13.Rows.Add("5", "ABBAAB", "0110001", "0111001", "1001110");
            m_EAN13.Rows.Add("6", "ABBBAA", "0101111", "0000101", "1010000");
            m_EAN13.Rows.Add("7", "ABABAB", "0111011", "0010001", "1000100");
            m_EAN13.Rows.Add("8", "ABABBA", "0110111", "0001001", "1001000");
            m_EAN13.Rows.Add("9", "ABBABA", "0001011", "0010111", "1110100");
        }
        private int m_Height = 80;
        /// <summary>
        /// 绘制高
        /// </summary>
        public int Height { get { return m_Height; } set { m_Height = value; } }
        private byte m_FontSize = 12;

        ///// <summary>
        ///// 字体大小（宋体）
        ///// </summary>
        //public byte FontSize { get { return m_FontSize; } set { m_FontSize = value; } }

        private byte m_Magnify = 1;
        /// <summary>     
        /// 放大系数    
        /// </summary>
        public byte Magnify { get { return m_Magnify; } set { m_Magnify = value; } }
        public Bitmap GetCodebByImage(string p_Text)
        {
            if (p_Text.Length != 13) throw new Exception("数字不是13位!");
            string _CodeText = p_Text.Remove(0, 1);
            string _CodeIndex = "101";
            char[] _LeftType = GetValue(p_Text.Substring(0, 1), "Type").ToCharArray();
            for (int i = 0; i != 6; i++)
            {
                _CodeIndex += GetValue(_CodeText.Substring(0, 1), _LeftType[i].ToString());
                _CodeText = _CodeText.Remove(0, 1);
            }

            _CodeIndex += "01010";

            for (int i = 0; i != 6; i++)
            {
                _CodeIndex += GetValue(_CodeText.Substring(0, 1), "C");
                _CodeText = _CodeText.Remove(0, 1);
            }
            _CodeIndex += "101";
            return GetImage(_CodeIndex, p_Text);
        }
        /// <summary>
        /// 获取目标对应的数据
        /// </summary>
        /// <param name="p_Code">编码</param>
        /// <param name="p_Value">类型</param>        
        /// <returns>编码</returns>
        private string GetValue(string p_Value, string p_Type)
        {
            if (m_EAN13 == null) return "";
            DataRow[] _Row = m_EAN13.Select("ID='" + p_Value + "'");
            if (_Row.Length != 1) throw new Exception("错误的编码" + p_Value.ToString());
            return _Row[0][p_Type].ToString();
        }


        /// <summary>
        /// 绘制编码图形
        /// </summary>
        /// <param name="p_Text">编码</param>
        /// <returns>图形</returns>
        private Bitmap GetImage(string p_Text, string p_ViewText)
        {
            char[] _Value = p_Text.ToCharArray();
            int _FontWidth = 0;
            Font _MyFont = m_ViewFont;
            //m_FontSize = (byte)_MyFont.Size;
            if (m_FontSize != 0)
            {
                #region 获取符合大小的字体
                _MyFont = new Font("宋体", m_FontSize);
                Bitmap _MyFontBmp = new Bitmap(m_FontSize, m_FontSize);
                Graphics _FontGraphics = Graphics.FromImage(_MyFontBmp);

                for (byte i = m_FontSize; i != 0; i--)
                {
                    SizeF _DrawSize = _FontGraphics.MeasureString(p_ViewText.Substring(0, 1), _MyFont);
                    if (_DrawSize.Height > m_FontSize)
                    {
                        _MyFont = new Font("宋体", i);
                    }
                    else
                    {
                        _FontWidth = (int)_DrawSize.Width;
                        break;
                    }
                }
                #endregion
            }
            if (ScanDrawText(_MyFont, p_Text, _FontWidth) == false)
            {
                _FontWidth = 0;
                m_FontSize = 0;
            }
            //宽 == 需要绘制的数量*放大倍数 + 两个字的宽   
            Bitmap _CodeImage = new Bitmap(_Value.Length * ((int)m_Magnify + 1) + (_FontWidth * 2), (int)m_Height);
            Graphics _Garphics = Graphics.FromImage(_CodeImage);
            _Garphics.FillRectangle(Brushes.White, new Rectangle(0, 0, _CodeImage.Width, _CodeImage.Height));

            int _Height = 0;
            int _LenEx = _FontWidth;
            for (int i = 0; i != _Value.Length; i++)
            {
                int _DrawWidth = m_Magnify + 1;
                if (i == 0 || i == 2 || i == 46 || i == 48 || i == 92 || i == 94)
                {
                    _Height = (int)m_Height;
                }
                else
                {
                    _Height = (int)m_Height - m_FontSize;
                }
                if (_Value[i] == '1')
                {
                    _Garphics.FillRectangle(Brushes.Black, new Rectangle(_LenEx, 0, _DrawWidth, _Height));

                }
                else
                {
                    _Garphics.FillRectangle(Brushes.White, new Rectangle(_LenEx, 0, _DrawWidth, _Height));
                }
                _LenEx += _DrawWidth;
            }
            //绘制文字
            if (_FontWidth != 0 && m_FontSize != 0)
            {
                int _StarX = 0;
                int _StarY = (int)m_Height - _MyFont.Height;
                _Garphics.DrawString(p_ViewText.Substring(0, 1), _MyFont, Brushes.Black, 0, _StarY);
                _StarX = _FontWidth + (3 * (m_Magnify + 1));
                _Garphics.DrawString(p_ViewText.Substring(1, 6), _MyFont, Brushes.Black, _StarX, _StarY);
                _StarX = _FontWidth + (50 * (m_Magnify + 1));
                _Garphics.DrawString(p_ViewText.Substring(7, 6), _MyFont, Brushes.Black, _StarX, _StarY);
            }
            _Garphics.Dispose();
            return _CodeImage;
        }
        /// <summary>

        /// <summary>
        /// 判断字体是否大与绘制图形
        /// </summary>
        /// <param name="_MyFont">字体</param>
        /// <param name="p_Text">文字</param>
        /// <param name="p_Width">字体的宽</param>
        /// <returns>true可以绘制 False不可以绘制</returns>
        private bool ScanDrawText(Font _MyFont, string p_Text, int p_Width)
        {
            if (_MyFont == null) return false;
            int _Width = (p_Text.Length - 6 - 5) * ((int)m_Magnify + 1);
            if ((p_Width * 12) > _Width) return false;
            return true;
        }
        /// <summary>
        /// 获得条码的最后一位（验证位）
        /// </summary>
        /// <param name="ANumbers">条码</param>
        /// <returns></returns>
        public static char EAN13ISBN(string _Numb)
        {
            int _Sum = 0;
            int _i = 1;   //权值
            foreach (char _Char in _Numb)
            {
                if ("0123456789".IndexOf(_Char) < 0) continue; // 非数字
                _Sum += (_Char - '0') * _i;
                _i = _i == 1 ? 3 : 1;
            }
            return "01234567890"[10 - _Sum % 10];
        }


        public Image GetCodeImage(string code)
        {

            char _ISBN = EAN13.EAN13ISBN(code);

            return this.GetCodebByImage(code + _ISBN);
        }
    }

    public class ByteHelper
    {
        /// <summary>
        /// 转换为byte[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }

        public static byte[] SaveImage(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中
            return imgBytesIn;
        }
    }
    #endregion
}

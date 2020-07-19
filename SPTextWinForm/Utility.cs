using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPTextWinForm
{
    public static class Utility
    {
        #region 时间处理
        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到天，如：2008/01/01
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToDayString(this DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd");
        }

        /// <summary>
        /// 格式化object类型为字符串类型，精确到天，如：2008/01/01
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToDayString(this object dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return ConvertToDayString((DateTime)dateTime);
        }

        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到小时，如：2008/01/01 18
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToHourString(this DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd HH");
        }

        /// <summary>
        /// 格式化object类型为字符串类型，精确到小时，如：2008/01/01 18
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToHourString(this object dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return ConvertToHourString((DateTime)dateTime);
        }

        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到分钟，如：2008/01/01 18:09
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToMiniuteString(this DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd HH:mm");
        }

        /// <summary>
        /// 格式化object类型为字符串类型，精确到分钟，如：2008/01/01 18:09
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToMiniuteString(this object dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return ConvertToMiniuteString((DateTime)dateTime);
        }

        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到秒，如：2008/01/01 18:09:20
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToSecondString(this DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到秒，如：18:09:20
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToTimeString(this DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"HH:mm:ss");
        }

        /// <summary>
        /// 格式化object类型为字符串类型，精确到秒，如：2008/01/01 18:09:20
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToSecondString(this object dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return ConvertToSecondString((DateTime)dateTime);
        }
        /// <summary>
        /// 指定日期加上天、月、年，然后格式化成字符串，如，2013-08-08 08:08:08
        /// </summary>
        /// <param name="dateTime">指定日期</param>
        /// <param name="tp">3年、2月、1日、4小时</param>
        /// <param name="days">追加天数，可正数或负数</param>
        /// <returns></returns>
        public static string FormatDateTimeByDays(DateTime dateTime, int tp, int num)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            if (tp == 1)
                return dateTime.AddDays(num).ToString(@"yyyy-MM-dd HH:mm:ss");
            else if (tp == 2)
                return dateTime.AddMonths(num).ToString(@"yyyy-MM-dd HH:mm:ss");
            else if (tp == 3)
                return dateTime.AddYears(num).ToString(@"yyyy-MM-dd HH:mm:ss");
            else if (tp == 4)
                return dateTime.AddHours(num).ToString(@"yyyy-MM-dd HH:mm:ss");
            else
                return "";
        }

        /// <summary>
        /// 指定日期加上指定年月日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="tp"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string FormatDateTimeByType(DateTime dateTime, int tp, int num, string endTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            if (tp == 1)
                dateTime.AddDays(num);
            else if (tp == 2)
            {
                dateTime = dateTime.AddMonths(num);
                return String.Format("{0}-{1}-01 {2}", dateTime.Year, dateTime.Month < 10 ? ("0" + dateTime.Month.ToString()) : dateTime.Month.ToString(), endTime);
            }
            else if (tp == 3)
            {
                dateTime = dateTime.AddYears(num);
                return String.Format("{0}-01-01 {1}", dateTime.Year, endTime);
            }
            return "";
        }
        /// <summary>
        /// 格式化指定日期
        /// </summary>
        /// <param name="dateTime">需格式化的日期</param>
        /// <returns></returns>
        public static string FormatDateTimes(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region 对象格式化

        /// <summary>
        /// 返回当前object的字符串类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ConvertToString(this object b)
        {
            return Convert.ToString(b);
        }

        /// <summary>
        /// 返回当前object的Int类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ConvertToInt(this object b)//扩展方法
        {
            int val = 0;
            if (Int32.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// 返回当前object的Log类型(长整型)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long ConvertToLog(this object b)
        {
            long val = 0;
            if (Int64.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// 返回当前object的Float类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float ConvertToFloat(this object b)
        {
            float val = 0f;
            if (float.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0f;
        }

        /// <summary>
        /// 返回当前object的Double类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double ConvertToDouble(this object b)
        {
            double val = 0d;
            if (double.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0d;
        }

        /// <summary>
        /// 返回当前object的Decimal类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Decimal ConvertToDecimal(this object b)
        {
            Decimal val = 0;
            if (Decimal.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// 返回当前object的DateTime类型
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this object b)
        {
            DateTime val = new DateTime();
            if (DateTime.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return Convert.ToDateTime("1900/01/01");
        }

        /// <summary>
        /// 返回格式为yyyy/MM/dd的日期字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ConvertToFromatDateTime(this object b)
        {
            DateTime val = new DateTime();
            if (DateTime.TryParse(Convert.ToString(b), out val))
            {
                return val.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
            }
            return "--/--/--";
        }

        #endregion

        #region 文本处理函数

        /// <summary>
        /// 截取文本的长度，过长则用“...”替换symbol之前的部分文本
        /// </summary>
        /// <param name="length">要调整到的长度</param>
        /// <param name="symbol">确保不被替换的文本的起始字符串，如“(”“[”等</param>
        /// <param name="content">要处理的内容</param>
        /// <returns>要处理的内容</returns>
        public static string FixLength(int length, string symbol, string content)
        {
            content = content.Trim();
            if (content.Length <= length)
                return content;
            else
            {
                int pos = content.LastIndexOf(symbol);
                int distance = content.Length - pos;
                if (length < distance + 3)
                    content = content.Substring(0, length - 2) + "...";
                else
                    content = content.Substring(0, length - distance - 2) + "..." + content.Substring(pos);
                return content;
            }
        }

        /// <summary>
        /// 按字节，取左边的Ｎ位
        /// </summary>
        /// <param name="content">要处理的内容</param>
        /// <param name="fixLength">截取的位数</param>
        /// <returns>要处理的内容</returns>
        public static string LeftB(string content, int fixLength)
        {
            if (fixLength > content.Length * 2)
                return content;

            int length = 0;
            char[] chars = content.ToCharArray();
            int i = 0;
            for (; i < chars.Length; i++)
            {
                if ((int)chars[i] < 256)
                    length++;
                else
                    length = length + 2;
                if (length > fixLength)
                    break;
            }
            if (length <= fixLength)
                return content;

            return content.Substring(0, i - 3);
        }

        /// <summary>
        /// 按字节，截取文本的长度，过长则用“...”结束
        /// </summary>
        /// <param name="fixLength">截取的位数</param>
        /// <param name="content">要处理的内容</param>
        /// <returns>要处理的内容</returns>
        public static string FixLengthB(int fixLength, string content)
        {
            if (fixLength >= content.Length * 2)
                return content;

            if (fixLength < 3)
                return LeftB(content, fixLength);

            int length = 0;
            char[] chars = content.ToCharArray();
            int i = 0;
            for (; i < chars.Length; i++)
            {
                if ((int)chars[i] < 256)
                    length++;
                else
                    length = length + 2;
                if (length > fixLength)
                    break;
            }
            if (length <= fixLength)
                return content;

            for (; i >= 0; i--)
            {
                if ((int)chars[i] < 256)
                    length--;
                else
                    length = length - 2;
                if (length <= fixLength - 3)
                    break;
            }
            return content.Substring(0, i) + "...";
        }

        /// <summary>
        /// 截取文本的长度，过长则用“...”替换symbol之前的部分文本
        /// </summary>
        /// <param name="length">要截取到的长度</param>
        /// <param name="symbol">确保不被替换的文本的起始字符串，如“(”“[”等</param>
        /// <param name="content">要处理的内容</param>
        /// <returns>要处理的内容</returns>
        public static string FixLengthB(int length, string symbol, string content)
        {
            if (length >= content.Length * 2)
                return content;

            int pos = content.LastIndexOf(symbol);
            string leftPart = content.Substring(0, pos);
            string rightPart = content.Substring(pos);
            int leftLength = LengthB(leftPart);
            int rightLength = LengthB(rightPart);

            if (length >= leftLength + rightLength)
                return content;

            if (length > rightLength + 3)
                return FixLengthB(length - rightLength, leftPart) + rightPart;

            if (length == leftLength + 3)
                return content + "...";

            if (length < leftLength + 3)
                return FixLengthB(length, leftPart);

            return leftPart + FixLengthB(length - leftLength, content);
        }

        public static int LengthB(string content)
        {
            int length = 0;
            char[] chars = content.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if ((int)chars[i] < 256)
                    length++;
                else
                    length = length + 2;
            }
            return length;
        }

        #endregion

        #region 字符串过滤处理
        /// <summary>
        /// 过滤非法字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FiltRiskChar(string str) //过滤非法字符
        {
            string s = "";
            s = str.ToLower().Replace("'", "");
            s = s.Replace(";", "");
            s = s.Replace("1=1", "");
            s = s.Replace("1 = 1", "");
            s = s.Replace("|", "");
            s = s.Replace("<", "");
            s = s.Replace(">", "");
            s = s.Replace("insert", "");
            s = s.Replace("update", "");
            s = s.Replace("drop", "");
            s = s.Replace("cmd", "");
            s = s.Replace("delete", "");
            s = s.Replace("exec", "");
            s = s.Replace("%", "");
            return s;
        }

        /// <summary>
        /// 过滤危险字符串
        /// </summary>
        /// <param name="chr">要过滤的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string ReplaceStr(string chr)
        {
            if (chr == null)
                return "";
            chr = chr.Replace("<", "");
            chr = chr.Replace(">", "");
            chr = chr.Replace("\n", "");
            chr = chr.Replace("\"", "");
            chr = chr.Replace("'", "");
            chr = chr.Replace(" ", "");
            chr = chr.Replace("\r", "");
            chr = chr.Replace("--", "");
            return (chr);

        }

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }

        #endregion

        #region 字符串类型检测

        /// <summary>
        /// 检查一个字符串是否可以转化为日期，一般用于验证用户输入日期的合法性。
        /// </summary>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否可以转化为日期的bool值。</returns>
        public static bool IsStringDate(string _value)
        {
            DateTime dt;
            try
            {
                dt = DateTime.Parse(_value);
            }
            catch (FormatException)
            {
                //日期格式不正确时
                //Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return Utility.QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        /// <summary>
        /// 只能输入由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsLetterOrNumber(string _value)
        {
            return Utility.QuickValidate(@"^\w+$", _value);
        }

        ///<summary>
        ///判断是否是数字，包括小数和整数。
        ///</summary>
        ///<param name="_value">需验证的字符串。</param>
        ///<returns>是否合法的bool值。</returns>
        public static bool IsNumber(string _value)
        {
            return Utility.QuickValidate("^(0|([1-9]+[0-9]*))(.[0-9]+)?$", _value);
        }

        ///<summary>
        ///快速验证一个字符串是否符合指定的正则表达式。
        ///</summary>
        ///<param name="_express">正则表达式的内容。</param>
        ///<param name="_value">需验证的字符串。</param>
        ///<returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            System.Text.RegularExpressions.Regex myRegex = new System.Text.RegularExpressions.Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }

        ///<summary>
        ///检测是否有中文字符
        ///</summary>
        ///<param name="inputData"></param>
        ///<returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        ///<summary>
        ///邮件地址
        ///</summary>
        ///<param name="inputData">输入字符串</param>
        ///<returns></returns>
        public static bool IsEmail(string inputData)
        {
            //Regex RegEmail = new Regex("^[\\w-.]+@[\\w-.]+\\.(com|net|org|edu|mil|tv|biz|info|cn|com.cn)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 

            //邮件地址不限制后缀
            //@Author：wind.kong
            //@Date：2013-06-24
            Regex RegEmail = new Regex("^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 

            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 检查时否是英文字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsEnglish(string inputData)
        {
            //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
            Regex RegEng = new Regex("^[a-zA-Z][a-zA-Z0-9_]*$");

            Match m = RegEng.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 只能输入中文字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsCn(string inputData)
        {
            //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
            Regex RegCn = new Regex("^[\u4e00-\u9fa5]{0,}$");

            Match m = RegCn.Match(inputData);
            return m.Success;
        }



        #endregion

        #region 图片处理
        ///<summary>
        ///生成缩略图
        ///</summary>
        ///<param name="originalImagePath">源图路径（物理路径）</param>
        ///<param name="thumbnailPath">缩略图路径（物理路径）</param>
        ///<param name="width">缩略图宽度</param>
        ///<param name="height">缩略图高度</param>
        ///<param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        /// <param name="addText">生成的文字</param>
        public static void AddWater(string Path, string Path_sy, string addText)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }

        #endregion

        #region 其他功能函数

        /// <summary>
        /// 获取本机IP
        /// 仅限ipv4
        /// </summary>
        /// <returns></returns>
        public static string InitIp()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in localIPs)
            {
                //根据AddressFamily判断是否为ipv4,如果是InterNetWork则为ipv6 
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "";
        }

        /// <summary>
        /// 繁简互转
        /// </summary>
        /// <param name="content">要处理的内容</param>
        /// <param name="isToSimple">true:转为简体 false:转为繁体</param>
        /// <returns>要处理的内容</returns>
        public static string Trans(string content, bool isToSimple)
        {
            //注意：简体有些字没有，仍用繁体字。如下：“琺”
            //新加的字都放在前面
            string complex = "槓穌靨嘆繽軼隻製嚀幺皚藹礙愛翺襖奧壩罷擺敗頒辦絆幫綁鎊謗剝飽寶報鮑輩貝鋇狽備憊繃筆畢斃幣閉邊編貶變辯辮標鼈別癟瀕濱賓擯餅並撥缽鉑駁蔔補財參蠶殘慚慘燦蒼艙倉滄廁側冊測層詫攙摻蟬饞讒纏鏟産闡顫場嘗長償腸廠暢鈔車徹塵沈陳襯撐稱懲誠騁癡遲馳恥齒熾沖蟲寵疇躊籌綢醜櫥廚鋤雛礎儲觸處傳瘡闖創錘純綽辭詞賜聰蔥囪從叢湊躥竄錯達帶貸擔單鄲撣膽憚誕彈當擋黨蕩檔搗島禱導盜燈鄧敵滌遞締顛點墊電澱釣調叠諜疊釘頂錠訂丟東動棟凍鬥犢獨讀賭鍍鍛斷緞兌隊對噸頓鈍奪墮鵝額訛惡餓兒爾餌貳發罰閥琺礬釩煩範販飯訪紡飛誹廢費紛墳奮憤糞豐楓鋒風瘋馮縫諷鳳膚輻撫輔賦複負訃婦縛該鈣蓋幹趕稈贛岡剛鋼綱崗臯鎬擱鴿閣鉻個給龔宮鞏貢鈎溝構購夠蠱顧剮關觀館慣貫廣規矽歸龜閨軌詭櫃貴劊輥滾鍋國過駭韓漢號閡鶴賀橫轟鴻紅後壺護滬戶嘩華畫劃話懷壞歡環還緩換喚瘓煥渙黃謊揮輝毀賄穢會燴彙諱誨繪葷渾夥獲貨禍擊機積饑譏雞績緝極輯級擠幾薊劑濟計記際繼紀夾莢頰賈鉀價駕殲監堅箋間艱緘繭檢堿鹼揀撿簡儉減薦檻鑒踐賤見鍵艦劍餞漸濺澗將漿蔣槳獎講醬膠澆驕嬌攪鉸矯僥腳餃繳絞轎較稭階節莖鯨驚經頸靜鏡徑痙競淨糾廄舊駒舉據鋸懼劇鵑絹傑潔結誡屆緊錦僅謹進晉燼盡勁荊覺決訣絕鈞軍駿開凱顆殼課墾懇摳庫褲誇塊儈寬礦曠況虧巋窺饋潰擴闊蠟臘萊來賴藍欄攔籃闌蘭瀾讕攬覽懶纜爛濫撈勞澇樂鐳壘類淚籬離裏鯉禮麗厲勵礫曆瀝隸倆聯蓮連鐮憐漣簾斂臉鏈戀煉練糧涼兩輛諒療遼鐐獵臨鄰鱗凜賃齡鈴淩靈嶺領餾劉龍聾嚨籠壟攏隴樓婁摟簍蘆盧顱廬爐擄鹵虜魯賂祿錄陸驢呂鋁侶屢縷慮濾綠巒攣孿灤亂掄輪倫侖淪綸論蘿羅邏鑼籮騾駱絡媽瑪碼螞馬罵嗎買麥賣邁脈瞞饅蠻滿謾貓錨鉚貿麽黴沒鎂門悶們錳夢謎彌覓冪綿緬廟滅憫閩鳴銘謬謀畝鈉納難撓腦惱鬧餒內擬膩攆撚釀鳥聶齧鑷鎳檸獰甯擰濘鈕紐膿濃農瘧諾歐鷗毆嘔漚盤龐賠噴鵬騙飄頻貧蘋憑評潑頗撲鋪樸譜棲淒臍齊騎豈啓氣棄訖牽扡釺鉛遷簽謙錢鉗潛淺譴塹槍嗆牆薔強搶鍬橋喬僑翹竅竊欽親寢輕氫傾頃請慶瓊窮趨區軀驅齲顴權勸卻鵲確讓饒擾繞熱韌認紉榮絨軟銳閏潤灑薩鰓賽叁傘喪騷掃澀殺紗篩曬刪閃陝贍繕傷賞燒紹賒攝懾設紳審嬸腎滲聲繩勝聖師獅濕詩屍時蝕實識駛勢適釋飾視試壽獸樞輸書贖屬術樹豎數帥雙誰稅順說碩爍絲飼聳慫頌訟誦擻蘇訴肅雖隨綏歲孫損筍縮瑣鎖獺撻擡態攤貪癱灘壇譚談歎湯燙濤縧討騰謄銻題體屜條貼鐵廳聽烴銅統頭禿圖塗團頹蛻脫鴕馱駝橢窪襪彎灣頑萬網韋違圍爲濰維葦偉僞緯謂衛溫聞紋穩問甕撾蝸渦窩臥嗚鎢烏汙誣無蕪吳塢霧務誤錫犧襲習銑戲細蝦轄峽俠狹廈嚇鍁鮮纖鹹賢銜閑顯險現獻縣餡羨憲線廂鑲鄉詳響項蕭囂銷曉嘯蠍協挾攜脅諧寫瀉謝鋅釁興洶鏽繡虛噓須許敘緒續軒懸選癬絢學勳詢尋馴訓訊遜壓鴉鴨啞亞訝閹煙鹽嚴顔閻豔厭硯彥諺驗鴦楊揚瘍陽癢養樣瑤搖堯遙窯謠藥爺頁業葉醫銥頤遺儀彜蟻藝億憶義詣議誼譯異繹蔭陰銀飲隱櫻嬰鷹應纓瑩螢營熒蠅贏穎喲擁傭癰踴詠湧優憂郵鈾猶遊誘輿魚漁娛與嶼語籲禦獄譽預馭鴛淵轅園員圓緣遠願約躍鑰嶽粵悅閱雲鄖勻隕運蘊醞暈韻雜災載攢暫贊贓髒鑿棗竈責擇則澤賊贈紮劄軋鍘閘柵詐齋債氈盞斬輾嶄棧戰綻張漲帳賬脹趙蟄轍鍺這貞針偵診鎮陣掙睜猙爭幀鄭證織職執紙摯擲幟質滯鍾終種腫衆謅軸皺晝驟豬諸誅燭矚囑貯鑄築駐專磚轉賺樁莊裝妝壯狀錐贅墜綴諄著濁茲資漬蹤綜總縱鄒詛組鑽為麼於產崙眾餘衝準兇佔歷釐髮臺嚮啟週譁薑寧傢尷鉅乾倖徵逕誌愴恆託摺掛闆樺慾洩瀏薰箏籤蹧係紓燿骼臟捨甦盪穫讚輒蹟跡採裡鐘鏢閒闕僱靂獃騃佈牀脣閧鬨崑崐綑蔴阩昇牠蓆巖灾剳紥註";
            string simple = "杠稣厣叹缤轶只制咛么皑蔼碍爱翱袄奥坝罢摆败颁办绊帮绑镑谤剥饱宝报鲍辈贝钡狈备惫绷笔毕毙币闭边编贬变辩辫标鳖别瘪濒滨宾摈饼并拨钵铂驳卜补财参蚕残惭惨灿苍舱仓沧厕侧册测层诧搀掺蝉馋谗缠铲产阐颤场尝长偿肠厂畅钞车彻尘沉陈衬撑称惩诚骋痴迟驰耻齿炽冲虫宠畴踌筹绸丑橱厨锄雏础储触处传疮闯创锤纯绰辞词赐聪葱囱从丛凑蹿窜错达带贷担单郸掸胆惮诞弹当挡党荡档捣岛祷导盗灯邓敌涤递缔颠点垫电淀钓调迭谍叠钉顶锭订丢东动栋冻斗犊独读赌镀锻断缎兑队对吨顿钝夺堕鹅额讹恶饿儿尔饵贰发罚阀琺矾钒烦范贩饭访纺飞诽废费纷坟奋愤粪丰枫锋风疯冯缝讽凤肤辐抚辅赋复负讣妇缚该钙盖干赶秆赣冈刚钢纲岗皋镐搁鸽阁铬个给龚宫巩贡钩沟构购够蛊顾剐关观馆惯贯广规硅归龟闺轨诡柜贵刽辊滚锅国过骇韩汉号阂鹤贺横轰鸿红后壶护沪户哗华画划话怀坏欢环还缓换唤痪焕涣黄谎挥辉毁贿秽会烩汇讳诲绘荤浑伙获货祸击机积饥讥鸡绩缉极辑级挤几蓟剂济计记际继纪夹荚颊贾钾价驾歼监坚笺间艰缄茧检碱硷拣捡简俭减荐槛鉴践贱见键舰剑饯渐溅涧将浆蒋桨奖讲酱胶浇骄娇搅铰矫侥脚饺缴绞轿较秸阶节茎鲸惊经颈静镜径痉竞净纠厩旧驹举据锯惧剧鹃绢杰洁结诫届紧锦仅谨进晋烬尽劲荆觉决诀绝钧军骏开凯颗壳课垦恳抠库裤夸块侩宽矿旷况亏岿窥馈溃扩阔蜡腊莱来赖蓝栏拦篮阑兰澜谰揽览懒缆烂滥捞劳涝乐镭垒类泪篱离里鲤礼丽厉励砾历沥隶俩联莲连镰怜涟帘敛脸链恋炼练粮凉两辆谅疗辽镣猎临邻鳞凛赁龄铃凌灵岭领馏刘龙聋咙笼垄拢陇楼娄搂篓芦卢颅庐炉掳卤虏鲁赂禄录陆驴吕铝侣屡缕虑滤绿峦挛孪滦乱抡轮伦仑沦纶论萝罗逻锣箩骡骆络妈玛码蚂马骂吗买麦卖迈脉瞒馒蛮满谩猫锚铆贸么霉没镁门闷们锰梦谜弥觅幂绵缅庙灭悯闽鸣铭谬谋亩钠纳难挠脑恼闹馁内拟腻撵捻酿鸟聂啮镊镍柠狞宁拧泞钮纽脓浓农疟诺欧鸥殴呕沤盘庞赔喷鹏骗飘频贫苹凭评泼颇扑铺朴谱栖凄脐齐骑岂启气弃讫牵扦钎铅迁签谦钱钳潜浅谴堑枪呛墙蔷强抢锹桥乔侨翘窍窃钦亲寝轻氢倾顷请庆琼穷趋区躯驱龋颧权劝却鹊确让饶扰绕热韧认纫荣绒软锐闰润洒萨鳃赛三伞丧骚扫涩杀纱筛晒删闪陕赡缮伤赏烧绍赊摄慑设绅审婶肾渗声绳胜圣师狮湿诗尸时蚀实识驶势适释饰视试寿兽枢输书赎属术树竖数帅双谁税顺说硕烁丝饲耸怂颂讼诵擞苏诉肃虽随绥岁孙损笋缩琐锁獭挞抬态摊贪瘫滩坛谭谈叹汤烫涛绦讨腾誊锑题体屉条贴铁厅听烃铜统头秃图涂团颓蜕脱鸵驮驼椭洼袜弯湾顽万网韦违围为潍维苇伟伪纬谓卫温闻纹稳问瓮挝蜗涡窝卧呜钨乌污诬无芜吴坞雾务误锡牺袭习铣戏细虾辖峡侠狭厦吓锨鲜纤咸贤衔闲显险现献县馅羡宪线厢镶乡详响项萧嚣销晓啸蝎协挟携胁谐写泻谢锌衅兴汹锈绣虚嘘须许叙绪续轩悬选癣绚学勋询寻驯训讯逊压鸦鸭哑亚讶阉烟盐严颜阎艳厌砚彦谚验鸯杨扬疡阳痒养样瑶摇尧遥窑谣药爷页业叶医铱颐遗仪彝蚁艺亿忆义诣议谊译异绎荫阴银饮隐樱婴鹰应缨莹萤营荧蝇赢颖哟拥佣痈踊咏涌优忧邮铀犹游诱舆鱼渔娱与屿语吁御狱誉预驭鸳渊辕园员圆缘远愿约跃钥岳粤悦阅云郧匀陨运蕴酝晕韵杂灾载攒暂赞赃脏凿枣灶责择则泽贼赠扎札轧铡闸栅诈斋债毡盏斩辗崭栈战绽张涨帐账胀赵蛰辙锗这贞针侦诊镇阵挣睁狰争帧郑证织职执纸挚掷帜质滞钟终种肿众诌轴皱昼骤猪诸诛烛瞩嘱贮铸筑驻专砖转赚桩庄装妆壮状锥赘坠缀谆着浊兹资渍踪综总纵邹诅组钻为么于产仑众余冲准凶占历厘发台向启周哗姜宁家尴巨干幸征径志怆恒托折挂板桦欲泄浏熏筝签糟系纾耀胳脏舍苏荡获赞辄迹迹采里钟镖闲阙雇雳呆呆布床唇哄哄昆昆捆麻升升它席岩灾札扎注";
            string str = "";
            if (isToSimple)
            {
                for (int i = 0; i < content.Length; i++)
                {
                    string word = content.Substring(i, 1);
                    //忽略字母
                    if (string.CompareOrdinal(word, "~") <= 0)
                    {
                        str += word;
                        continue;
                    }
                    int pos = complex.IndexOf(word);
                    if (pos != -1)
                        str += simple.Substring(pos, 1);
                    else
                        str += word;
                }
            }
            else
            {
                for (int i = 0; i < content.Length; i++)
                {
                    string word = content.Substring(i, 1);
                    //忽略字母
                    if (string.CompareOrdinal(word, "~") <= 0)
                    {
                        str += word;
                        continue;
                    }
                    int pos = simple.IndexOf(word);
                    if (pos != -1)
                        str += complex.Substring(pos, 1);
                    else
                        str += word;
                }
            }
            return str;
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的表格通用方法
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPageTable(DataTable dt, int pageSize, int pageIndex)
        {
            DataTable dt1 = new DataTable();
            if (dt != null)
            {
                int pageEnd = pageIndex * pageSize;
                int pagebegin = (pageIndex - 1) * pageSize;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt1.Columns.Add(new DataColumn(dt.Columns[i].ColumnName));
                }
                for (int i = pagebegin; i < pageEnd; i++)
                {
                    if (i < dt.Rows.Count)
                    {
                        dt1.ImportRow(dt.Rows[i]);
                    }
                }

            }
            return dt1;
        }

        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int getStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        /// <summary>
        /// 将邮件正文写入文件
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="_Msg"></param>
        public static void initLocalFile(string localFile, string _Msg)
        {
            if (!File.Exists(localFile))
            {
                FileStream stream = new FileStream(localFile, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
                sw.Write(_Msg);
                sw.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// 乱码加密key
        /// </summary>
        public static readonly string uncodeKey = "益百立睿方";

        /// <summary>
        /// 读取文件内容将乱码后的内容反编译程非乱码
        /// </summary>
        /// <param name="fileName">文件</param>
        /// <returns>反编译后的内容</returns>
        public static string ReadFile(string fileName, int isUncode)
        {
            StreamReader reader = new StreamReader(fileName, Encoding.UTF8);
            string res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            if (isUncode == 1)
            {
                return StringXor(res, uncodeKey);
            }
            else
            {
                return res;
            }
        }

        /// <summary>
        /// 乱码解密
        /// </summary>
        /// <param name="Str">乱码内容</param>
        /// <param name="Key">乱码加密解密key</param>
        /// <returns>解密后的内容</returns>
        public static string StringXor(string Str, string Key)
        {
            int vKeyLen = Key.Length;
            char[] StrChars = Str.ToCharArray();
            char[] KeyChars = Key.ToCharArray();
            int j = 0; for (int i = 0; i < Str.Length; i++)
                StrChars[i] ^= KeyChars[j++ % Key.Length];
            return new string(StrChars);
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="obj">需转换值</param>
        /// <param name="point">保留小数</param>
        /// <returns></returns>
        public static double ConvertNumber(double obj, int point)
        {
            return Math.Round(obj, point, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换字节大小
        /// </summary>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string convertFileSize(int size, string unit)
        {
            return convertFileSize(size, unit, false);
        }

        /// <summary>
        /// 转换字节大小
        /// </summary>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <param name="showUnit"></param>
        /// <returns></returns>
        public static string convertFileSize(int size, string unit, bool showUnit)
        {
            if (size <= 0)
                return "0";
            double dSize = Utility.ConvertNumber(size, 0);
            if (unit == "K")
            {
                dSize = dSize / 1024;
                return Utility.ConvertNumber(dSize, 0) + (showUnit == true ? " K" : "");
            }
            return "1";
        }

        /// <summary>
        /// 文件大小转换
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string convertFileSize(int size)
        {
            string Files = "";
            double dSize = Utility.ConvertNumber(size, 1);
            if (size > 0)
            {
                if (size < 1024)
                {
                    Files = Utility.ConvertNumber(dSize, 1) + "B";
                }
                else
                {
                    dSize = dSize / 1024;
                    if (dSize < 1024)
                    {
                        Files = Utility.ConvertNumber(dSize, 1) + "K";
                    }
                    else
                    {
                        dSize = dSize / 1024;
                        if (dSize < 1024)
                        {
                            Files = Utility.ConvertNumber(dSize, 1) + "M";
                        }
                        else
                        {
                            dSize = dSize / 1024;
                            if (dSize < 1024)
                            {
                                Files = Utility.ConvertNumber(dSize, 1) + "G";
                            }
                            else
                            {
                                dSize = dSize / 1024;
                                Files = Utility.ConvertNumber(dSize, 1) + "T";
                            }
                        }
                    }
                }
            }
            else
            {
                Files = "0B";
            }
            return Files;
        }

        /// <summary>
        /// 附件图标
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int MatchImgIndex(string type)
        {
            int index = 0;
            type = type.ToLower();
            switch (type)
            {
                case "doc":
                case "docx":
                    index = 0;
                    break;
                case "xls":
                case "xlsx":
                    index = 2;
                    break;
                case "pdf":
                    index = 4;
                    break;
                case "ppt":
                case "pptx":
                    index = 5;
                    break;
                case "zip":
                case "rar":
                    index = 7;
                    break;
                case "htm":
                case "html":
                    index = 8;
                    break;
                case "chm":
                    index = 9;
                    break;
                case "txt":
                    index = 10;
                    break;
                case "jpg":
                case "png":
                case "bmp":
                    index = 11;
                    break;
                /*下面的图标皆是未验证出来，可以测试后修改*/
                //case "fla":
                //    index = 7;
                //    break;
                //case "php":
                //    index = 9;
                //    break;
                default:
                    index = 15;
                    break;
            }
            return index;
        }

        /// <summary>
        /// 二进制流还原成文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool ByteToObject(byte[] bytes, string FilePath)
        {
            bool result = false;
            if (bytes != null)
            {
                try
                {
                    using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    result = true;
                }
                catch (Exception)
                {

                }
            }
            return result;
        }

        /// <summary>
        /// 将文件转换成二进制
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static byte[] ConvertToBinary(string Path)
        {
            FileStream stream = new FileInfo(Path).OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            stream.Close();
            return buffer;
        }

        /// <summary>
        /// 杀掉程序
        /// </summary>
        public static void ProcessKill(string projectName)
        {
            //判断进程并且杀掉
            try
            {
                Process[] p1 = Process.GetProcessesByName(projectName + ".vshost");
                foreach (Process _p in p1)
                    _p.Kill();
                Process[] p = Process.GetProcessesByName(projectName);
                foreach (Process _p in p)
                    _p.Kill();
            }
            catch (System.Exception)
            { }
        }
        #endregion

        #region 后缀名获取对应图标
        /// <summary> 
        /// 通过扩展名得到图标和描述 
        /// </summary> 
        /// <param name="ext">扩展名</param> 
        /// <param name="LargeIcon">得到大图标</param> 
        public static void GetExtsIcon(string ext, out Icon largeIcon)
        {
            largeIcon = null;
            //当没有后缀名时直接返回null
            if (ext.Trim() == "." || ext.Trim() == "") return;
            try
            {
                var extsubkey = Registry.ClassesRoot.OpenSubKey(ext); //从注册表中读取扩展名相应的子键 
                if (extsubkey != null)
                {
                    var extdefaultvalue = (string)extsubkey.GetValue(null); //取出扩展名对应的文件类型名称 

                    //未取到值，返回预设图标 
                    if (extdefaultvalue == null)
                    {
                        GetDefaultIcon(out largeIcon);
                        return;
                    }

                    var typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue); //从注册表中读取文件类型名称的相应子键 
                    if (typesubkey != null)
                    {
                        var defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon"); //取默认图标子键 
                        if (defaulticonsubkey != null)
                        {
                            //得到图标来源字符串 
                            var defaulticon = (string)defaulticonsubkey.GetValue(null); //取出默认图标来源字符串 
                            var iconstringArray = defaulticon.Split(',');
                            int nIconIndex = 0;
                            if (iconstringArray.Length > 1) int.TryParse(iconstringArray[1], out nIconIndex);
                            //得到图标 
                            System.IntPtr phiconLarge = new IntPtr();
                            System.IntPtr phiconSmall = new IntPtr();
                            ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall, 1);
                            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
                            //if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary> 
        /// 获取缺省图标 
        /// </summary> 
        /// <param name="largeIcon"></param> 
        /// <param name="smallIcon"></param> 
        public static void GetDefaultIcon(out Icon largeIcon)
        {
            largeIcon = null;
            System.IntPtr phiconLarge = new IntPtr();
            System.IntPtr phiconSmall = new IntPtr();
            ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            //if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }

        /// Return Type: UINT->unsigned int 
        ///lpszFile: LPCWSTR->WCHAR* 
        ///nIconIndex: int 
        ///phiconLarge: HICON* 
        ///phiconSmall: HICON* 
        ///nIcons: UINT->unsigned int 
        [DllImportAttribute("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([System.Runtime.InteropServices.InAttribute()][System.Runtime.InteropServices.MarshalAsAttribute(UnmanagedType.LPWStr)] string lpszFile, int nIconIndex, ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIcons);

        /// <summary>
        /// 创建文件图标
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="large"></param>
        public static void CreateFileIcon(string fileType, out Icon large)
        {
            if (fileType.Trim() == "") //预设图标
            {
                GetDefaultIcon(out large);
            }
            else if (fileType.ToUpper() == ".EXE") //应用程序图标单独获取
            {
                IntPtr l = IntPtr.Zero;
                IntPtr s = IntPtr.Zero;

                ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2, ref l, ref s, 1);

                large = Icon.FromHandle(l);
            }
            else //其它类型的图标
            {
                GetExtsIcon(fileType, out large);
            }

            if (large == null) //无法获取图标,预设图标
                GetDefaultIcon(out large);
        }

        public static byte[] ImageToByte(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            byte[] bs = ms.ToArray();
            ms.Close();
            return bs;
        }

        public static Image ByteToImage(byte[] bs)
        {
            MemoryStream ms = new MemoryStream(bs);
            Bitmap bmp = new Bitmap(ms);
            ms.Close();
            return bmp;
        }

        [StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HICON__
        {
            public int unused;
        }
        #endregion

        #region 数据压缩
        /// <summary>   
        /// 获取数据压缩后的字节码   
        /// </summary>   
        public static byte[] Compress(DataSet dt)
        {
            try
            {
                // 声明MemoryStream   
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                // 写入DataSet中的数据到ms中   
                dt.WriteXml(ms, XmlWriteMode.WriteSchema);
                // ms转换为数组序列   
                byte[] bsrc = ms.ToArray();
                //关闭ms并释放资源   
                ms.Close();
                ms.Dispose();

                ms = new System.IO.MemoryStream();
                ms.Position = 0;
                // 压缩数组序列中的数据   
                System.IO.Compression.DeflateStream zipStream = new System.IO.Compression.DeflateStream(ms,
                                                                                                        System.IO
                                                                                                              .Compression
                                                                                                              .CompressionMode
                                                                                                              .Compress);
                zipStream.Write(bsrc, 0, bsrc.Length);
                zipStream.Close();
                zipStream.Dispose();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>   
        /// 获取解压缩后的数据集   
        /// </summary>   
        public static DataSet DeCompress(byte[] arrbts)
        {
            try
            {
                //    
                MemoryStream ms = new MemoryStream();
                ms.Write(arrbts, 0, arrbts.Length);
                ms.Position = 0;
                //   
                System.IO.Compression.DeflateStream ZipStream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
                MemoryStream UnzipStream = new MemoryStream();
                byte[] sDecompressed = new byte[128];
                while (true)
                {
                    int bytesRead = ZipStream.Read(sDecompressed, 0, 128);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    UnzipStream.Write(sDecompressed, 0, bytesRead);
                }
                ZipStream.Close();
                ms.Close();
                UnzipStream.Position = 0;
                DataSet ds = new DataSet();
                // 读取解压后xml数据   
                ds.ReadXml(UnzipStream);
                ds.AcceptChanges(); //更新所有行的状态为初始状态   
                return ds;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>   
        /// 获取数据压缩后的字节码   
        /// </summary>   
        public static byte[] Compress(Object obj)
        {
            try
            {
                // 声明MemoryStream   
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                // 写入Object对象到ms中   
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                // ms转换为数组序列   
                byte[] bsrc = ms.ToArray();
                //关闭ms并释放资源   
                ms.Close();
                ms.Dispose();

                ms = new System.IO.MemoryStream();
                ms.Position = 0;
                // 压缩数组序列中的数据   
                System.IO.Compression.DeflateStream zipStream = new System.IO.Compression.DeflateStream(ms,
                                                                                                        System.IO
                                                                                                              .Compression
                                                                                                              .CompressionMode
                                                                                                              .Compress);
                zipStream.Write(bsrc, 0, bsrc.Length);
                zipStream.Close();
                zipStream.Dispose();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>   
        /// 获取解压缩后的数据集   
        /// </summary>   
        public static object DeCompressObj(byte[] arrbts)
        {
            try
            {
                //    
                MemoryStream ms = new MemoryStream();
                ms.Write(arrbts, 0, arrbts.Length);
                ms.Position = 0;
                //   
                System.IO.Compression.DeflateStream ZipStream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
                MemoryStream UnzipStream = new MemoryStream();
                byte[] sDecompressed = new byte[128];
                while (true)
                {
                    int bytesRead = ZipStream.Read(sDecompressed, 0, 128);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    UnzipStream.Write(sDecompressed, 0, bytesRead);
                }
                ZipStream.Close();
                ms.Close();
                UnzipStream.Position = 0;

                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(UnzipStream);
                return obj;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}

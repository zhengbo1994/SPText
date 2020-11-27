using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SPTextCommon
{
    public class GeneralHelper
    {
        //private static string[] _extensionList = { ".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff" };
        private static string[] optionABC = { "A", "B", "C" };
        private static string numStr = "123456789";
        private static string lowEnStr = "abcdefghijkmnpqrstuvwxyz";
        private static string uperEnStr = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
        public GeneralHelper() { }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMixPwd(int num)//生成混合随机数
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成随机数不包含0跟O
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMT4pwd(int num)
        {
            string a = "23456789ABCDEFGHIJKMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";


            StringBuilder sb = new StringBuilder();
            Random randObj = new Random();
            if (num > 4)
            {
                int ri = randObj.Next(1, num);
                int rli = (num - ri) / 2;
                int rui = num - ri - rli;
                for (int i = 0; i < rli; i++)
                {
                    sb.Append(lowEnStr[new Random(Guid.NewGuid().GetHashCode()).Next(0, lowEnStr.Length - 1)]);
                }
                for (int i = 0; i < ri; i++)
                {
                    sb.Append(numStr[new Random(Guid.NewGuid().GetHashCode()).Next(0, numStr.Length - 1)]);
                }
                for (int i = 0; i < rui; i++)
                {
                    sb.Append(uperEnStr[new Random(Guid.NewGuid().GetHashCode()).Next(0, uperEnStr.Length - 1)]);
                }
            }
            else
                for (int i = 0; i < num; i++)
                {
                    sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
                }

            string str = sb.ToString();
            Regex re = new Regex("(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]");//必须包含大小写字母和数字。
            while (!re.IsMatch(str))
            {
                str = GetMT4pwd(num);
            }
            return str;
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMixPWD(int num)//生成混合随机数
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_!@#$%&";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }
        //随机字符串生成器的主要功能如下： 
        //1、支持自定义字符串长度
        //2、支持自定义是否包含数字
        //3、支持自定义是否包含小写字母
        //4、支持自定义是否包含大写字母
        //5、支持自定义是否包含特殊符号
        //6、支持自定义字符集
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，1=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string GetRndstr(int length, string custom = "", bool useNum = true, bool useLow = true, bool useUpp = false, bool useSpe = false)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "23456789"; }
            if (useLow == true) { str += "abcdefghijkmnpqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        /// <summary>
        /// 检查上传文件的后缀名
        /// 允许上传的有：".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff"
        /// </summary>
        /// <param name="fileName">待检查的文件完整名称</param>
        /// <returns></returns>
        public static bool CheckFileExtension(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                fileName = fileName.ToLower();
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                    return false;
                string[] _extensionList = { ".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff" };
                if (!_extensionList.Contains(extension))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查上传文件的后缀名
        /// 允许上传的有：“.jpg|.gif|.bmp|.jpeg|.png”
        /// </summary>
        /// <param name="fileName">待检查的文件完整名称</param>
        /// <returns></returns>
        public static bool CheckIsImgExtension(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                fileName = fileName.ToLower();
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                    return false;
                string[] _extensionList = { ".jpg", ".gif", ".bmp", ".jpeg", ".png" };
                if (!_extensionList.Contains(extension))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GeneraOpionAnswer(int mmss)
        {
            int CurrentMMss = DateTime.Now.Millisecond;
            if (mmss == CurrentMMss)
                CurrentMMss = Convert.ToInt32(Convert.ToInt32(CurrentMMss / 1245) * 131337933);
            else
                CurrentMMss = mmss;
            Random ran = new Random(CurrentMMss);
            int j = ran.Next(0, optionABC.Length);//因为数组长度是7，所以这里范围就用0到6
            return optionABC[j];
        }
        public static string CorrectAnswerList()
        {
            string[] CorrectAnswer = new string[2];
            Random rdr = new Random(DateTime.Now.Millisecond * 12734);
            CorrectAnswer[0] = "A,C,B,B";
            CorrectAnswer[1] = "B,C,B,B";
            int rj = rdr.Next(0, CorrectAnswer.Length);
            return CorrectAnswer[rj];
        }
        public static string ErrorAnswerList()
        {
            string[] ErrorAnswer = new string[7];
            Random rdd = new Random(DateTime.Now.Millisecond);
            // RandomHelper r = new RandomHelper();
            ErrorAnswer[0] = "C," + GeneraOpionAnswer(DateTime.Now.Millisecond * 123) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 760) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 123);
            ErrorAnswer[1] = GeneraOpionAnswer(DateTime.Now.Millisecond * 123) + ",A," + GeneraOpionAnswer(DateTime.Now.Millisecond * 321) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 231);
            ErrorAnswer[2] = GeneraOpionAnswer(DateTime.Now.Millisecond * 456) + ",B," + GeneraOpionAnswer(DateTime.Now.Millisecond * 654) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 564);
            ErrorAnswer[3] = GeneraOpionAnswer(DateTime.Now.Millisecond * 674) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 789) + ",A," + GeneraOpionAnswer(DateTime.Now.Millisecond * 761);
            ErrorAnswer[4] = GeneraOpionAnswer(DateTime.Now.Millisecond * 954) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 785) + ",C," + GeneraOpionAnswer(DateTime.Now.Millisecond * 234);
            ErrorAnswer[5] = GeneraOpionAnswer(DateTime.Now.Millisecond * 243) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 879) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 563) + ",A";
            ErrorAnswer[6] = GeneraOpionAnswer(DateTime.Now.Millisecond * 908) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 547) + "," + GeneraOpionAnswer(DateTime.Now.Millisecond * 748) + ",C";
            return ErrorAnswer[rdd.Next(0, ErrorAnswer.Length)];
        }




        /// <summary>
        /// 检查上传文件的后缀名
        /// 允许上传的有：".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff"
        /// </summary>
        /// <param name="fileName">待检查的文件完整名称</param>
        /// <returns></returns>
        public static bool CheckFileExtension(string fileName, HttpPostedFileBase postedFile = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                {
                    if (postedFile == null)
                        return false;
                    else
                    {
                        int iEx = GetIExtension(postedFile);
                        if (iEx == (int)FileExtension.BMP ||
                            iEx == (int)FileExtension.JPG ||
                            iEx == (int)FileExtension.GIF ||
                            iEx == (int)FileExtension.PNG)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                    extension = extension.ToLower();
                string[] _extensionList = { ".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff" };
                if (!_extensionList.Contains(extension))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public enum FileExtension
        {
            GIF = 7173,
            JPG = 255216,
            BMP = 6677,
            PNG = 13780,
            DOC = 208207,
            DOCX = 8075,
            XLSX = 8075,
            JS = 239187,
            XLS = 208207,
            SWF = 6787,
            MID = 7784,
            RAR = 8297,
            ZIP = 8075,
            XML = 6063,
            TXT = 7067,
            MP3 = 7368,
            WMA = 4838

            // 239187 aspx
            // 117115 cs
            // 119105 js
            // 210187 txt
            //255254 sql         
            // 7790 exe dll,
            // 8297 rar
            // 6063 xml
            // 6033 html
        }
        /// <summary>
        /// 根据数据流，获取文件格式
        /// </summary>
        /// <param name="fu"></param>
        /// <returns></returns>
        private static int GetIExtension(HttpPostedFileBase fu)
        {
            try
            {
                int fileLen = fu.ContentLength;
                byte[] imgArray = new byte[fileLen];
                fu.InputStream.Read(imgArray, 0, fileLen);
                MemoryStream ms = new MemoryStream(imgArray);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
                string fileclass = "";
                byte buffer;
                try
                {
                    buffer = br.ReadByte();
                    fileclass = buffer.ToString();
                    buffer = br.ReadByte();
                    fileclass += buffer.ToString();
                }
                catch
                {
                }
                br.Close();
                ms.Close();
                //注意将文件流指针还原
                fu.InputStream.Position = 0;
                int iExt = Int32.Parse(fileclass);
                return iExt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

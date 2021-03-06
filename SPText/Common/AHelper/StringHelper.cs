﻿using System.Text.RegularExpressions;

namespace System
{
    public static class StringHelper
    {
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AcronymFirstLetter(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 去除字符串前后空格，并且如果中间有多个相邻空格，只保留一个
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetRemoveExcessSpaceStr(this string str)
        {
            return Regex.Replace(str.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns> 
        public static string GetRandomString(int length)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(GetNewSeed());
            while (reValue.Length < length)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }

        /// <summary>
        /// 获取Guid值
        /// </summary>
        /// <returns></returns>
        public static string GetGuidStr()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 种子
        /// </summary>
        /// <returns></returns>
        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        public static string GetTab(int num)
        {
            string str = "";
            for (int i = 0; i < num; i++)
            {
                str += "	";
            }
            return str;
        }

        /// <summary>
        /// 字符串长度处理
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="outputlength">长度</param>
        /// <returns>把输入字符串长度进行处理，如输入字符串长度超过10时，则后面字符用"..."表示</returns>
        public static string StringLengthProcesse(this string str, int outputlength = 10)
        {
            return str.Length > outputlength ? str.Substring(0, outputlength) + "..." : str;
        }

        #region  对字符串特定格式进行拆分
        #region 拆分字符串
        /// <summary>
        /// 根据字符串拆分字符串
        /// </summary>
        /// <param name="strSource">要拆分的字符串</param>
        /// <param name="strSplit">拆分符</param>
        /// <returns></returns>
        public static string[] StringSplit(string strSource, string strSplit)
        {
            string[] strtmp = new string[1];
            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[0] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        #endregion

        #region 采用递归将字符串分割成数组
        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        private static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);


            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        #endregion
        #endregion
    }
}

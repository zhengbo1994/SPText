using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReportPrinting.Model
{
    public static class Ext
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string StrToInt(this string str)
        {
            double tmp;
            if (double.TryParse(str, out tmp))
            {
                return Convert.ToInt32(tmp).ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// string转float，不能转、“”、NULL全部都转为0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float StrToFloat(this string str)
        {
            float tmp;
            if (str == null || str == "")
            {
                return 0;
            }

            if (float.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 延迟
        /// </summary>
        /// <param name="delayTime">毫秒数</param>
        /// <returns></returns>
        public static bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int f = 0;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                //f = spand.Milliseconds;
                f = Convert.ToInt32(spand.Ticks / 10000);
                Application.DoEvents();
            }
            while (f < delayTime);
            return true;
        }
    }
}

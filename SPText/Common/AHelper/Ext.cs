using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPText.Common.AHelper
{
    public static class Ext
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool TryParseDatetimieWithNull(this string s, out DateTime? result)
        {
            if (s.IsNullOrEmpty())
            {
                result = null;
                return true;
            }
            else
            {
                DateTime resultTemp;
                if (DateTime.TryParse(s, out resultTemp))
                {
                    result = resultTemp;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }

        public static void AppendMsg(this RichTextBox rtbMsg, MsgType msgType, string msg)
        {
            rtbMsg.Invoke(new MethodInvoker(() =>
            {
                rtbMsg.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msgType.ToString() + ": " + msg + "\n");
                Application.DoEvents();
            }));
            Delay(300);
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
    public enum MsgType
    {
        INFO = 0,
        NOTE = 1,
        WARNING = 2
    }
}

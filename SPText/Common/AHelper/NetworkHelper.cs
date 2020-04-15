using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public class NetworkHelper
    {
        private static string url = "http://1212.ip138.com/ic.asp";
        /// <summary>
        /// 获取外网Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetOuterNetIP()
        {
            //重复获取n次
            int index = 1;
            string ipStr = "";
            while (index <= 10)
            {
                try
                {
                    Uri uri = new Uri(url);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                    req.Method = "get";
                    using (Stream s = req.GetResponse().GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(s))
                        {
                            string str = reader.ReadToEnd();
                            Match m = Regex.Match(str, @"\[(?<IP>[0-9\.]*)\]");
                            ipStr = m.Groups[1].Value;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    index += 1;
                }
            }
            return ipStr;
        }

        /// <summary>
        /// 根据ip地址获取城市信息
        /// </summary>
        /// <param name="ipAddress"></param>
        public static NetworkCityInfo GetCityInfo(string ipAddress)
        {
            //string result = HttpHelper.GetContent("http://int.dpool.sina.com.cn/iplookup/iplookup.php"
            //    , $"format=json&ip={ipAddress}", Encoding.GetEncoding("gbk"));
            string result = HttpHelper.GetContent("http://int.dpool.sina.com.cn/iplookup/iplookup.php"
    , "format=json&ip={ipAddress}", Encoding.GetEncoding("gbk"));
            var strArr = result.Split(new String[] { "-", "1", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            NetworkCityInfo networkCityInfo = new NetworkCityInfo
            {
                country = strArr[0],
                province = strArr[1],
                city = strArr[2],
            };
            return networkCityInfo;
        }

        /// <summary>
        /// 网络城市信息
        /// </summary>
        public class NetworkCityInfo
        {
            /// <summary>
            /// 国家
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 省
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 市
            /// </summary>
            public string city { get; set; }
        }
    }
}

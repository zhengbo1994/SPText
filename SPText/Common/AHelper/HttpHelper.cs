using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Linq;
using System.Web;


namespace System
{
	public class HttpHelper
	{
		#region post请求
		public static string GetContent(string url, string paramStrs, Encoding encoding)
		{
			Uri uri = new Uri(url);
			try
			{
				HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
				request.Method = "post";
				request.ContentType = "application/x-www-form-urlencoded";
				request.Headers.Add("Access-Control-Allow-Origin:*");
				string reqdata = paramStrs;
				byte[] buf = encoding.GetBytes(reqdata);
				Stream s = request.GetRequestStream();
				s.Write(buf, 0, buf.Length);
				s.Close();
				HttpWebResponse res = request.GetResponse() as HttpWebResponse;
				StreamReader sr = new StreamReader(res.GetResponseStream(), encoding);
				string html = sr.ReadToEnd();
				return html;
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}
		#endregion

		public static string GetContent(string url, string postDataStr = "", string formParamStrs = "", CookieContainer cookie = null)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
			if (cookie == null || cookie.Count == 0)
			{
				request.CookieContainer = new CookieContainer();
				cookie = request.CookieContainer;
			}
			else
			{
				request.CookieContainer = cookie;
			}
			request.ContentType = "text/html; charset=utf-8";
			request.Method = "GET";
			request.Host = "hao.360.cn";
			request.KeepAlive = true;
			request.AllowAutoRedirect = true;
			//request.Referer = $"https://pan.baidu.com/share/init";
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 QIHU 360EE";
			request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
			request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6");
			request.Headers.Add("Cache-Control", "no-cache");
			//request.Headers.Add("X-Requested-With", "XMLHttpRequest");
			//request.Headers.Add("Origin", "https://pan.baidu.com");
			request.Headers.Add("Pragma", "no-cache");

			if (!formParamStrs.IsNullOrWhiteSpace())
			{
				byte[] buf = Encoding.UTF8.GetBytes(formParamStrs);
				Stream s = request.GetRequestStream();
				s.Write(buf, 0, buf.Length);
				s.Close();
			}
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			string encd = response.ContentEncoding; //如果是gzip，要解压

			string htmlCode = "";
			using (Stream streamReceive = response.GetResponseStream())
			{
				using (var zipStream = new GZipStream(streamReceive, CompressionMode.Decompress))
				{
					using (StreamReader sr = new StreamReader(zipStream, Encoding.UTF8))
					{
						htmlCode = sr.ReadToEnd();
					}
				}
			}

			return htmlCode;
		}


        public static string GetAppSettingsValue(string appSettingKey)
        {
            string value = null;
            if (System.Web.Configuration.WebConfigurationManager.AppSettings.AllKeys.Contains(appSettingKey))
            {
                value = System.Web.Configuration.WebConfigurationManager.AppSettings[appSettingKey];
            }
            return value;
        }
	}
}

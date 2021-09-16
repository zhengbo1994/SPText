using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

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


		#region  Http请求
		/// <summary>
		/// 使用GET请求访问某个url并得到文本文档
		/// </summary>
		/// <param name="url">访问的地址</param>
		/// <returns>该文本文档的全部内容</returns>
		public static string GetCallReturnText(string url)
		{
			var stream = Call(RequestMethod.get, url);
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);
			string result = reader.ReadToEnd().Replace("\n", "");

			return result;
		}

		/// <summary>
		/// 简单的请求访问（暂无请求流数据）
		/// </summary>
		/// <param name="method">请求方式：POST或GET</param>
		/// <param name="url">请求的HOST地址</param>
		/// <returns>相应的流</returns>
		public static Stream Call(RequestMethod method, string url)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
											| SecurityProtocolType.Tls
											| (SecurityProtocolType)0x300 //Tls11
											| (SecurityProtocolType)0xC00; //Tls12
			Encoding encoding = Encoding.Default;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = method.ToString();
			request.KeepAlive = true;

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();

			return stream;
		}

		/// <summary>
		/// HTTP请求方式
		/// </summary>
		public enum RequestMethod
		{
			get,
			post
		}
		#endregion

		public static string HttpGet(string url, Hashtable headht = null)
		{
			HttpWebRequest request;

			//如果是发送HTTPS请求  
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
				request = WebRequest.Create(url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(url) as HttpWebRequest;
			}
			request.Method = "GET";
			//request.ContentType = "application/x-www-form-urlencoded";
			request.Accept = "*/*";
			request.Timeout = 15000;
			request.AllowAutoRedirect = false;
			WebResponse response = null;
			string responseStr = null;
			if (headht != null)
			{
				foreach (DictionaryEntry item in headht)
				{
					request.Headers.Add(item.Key.ToString(), item.Value.ToString());
				}
			}

			try
			{
				response = request.GetResponse();

				if (response != null)
				{
					StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
					responseStr = reader.ReadToEnd();
					reader.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return responseStr;
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true; //总是接受  
		}
	}
}

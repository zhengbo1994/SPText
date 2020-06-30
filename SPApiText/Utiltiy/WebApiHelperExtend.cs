using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    public class WebApiHelperExtend
    {
        public static string InvokeApi(string url, HttpMethod httpMethod)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = httpMethod;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
    }
}

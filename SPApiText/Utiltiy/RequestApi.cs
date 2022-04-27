using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    /// <summary>
    /// 模拟Http请求
    /// </summary>
    public class RequestApi
    {
        //[CustomPollyFallbackAttribute]  //我就像要在特性标记的时候，就明确谁先执行;
        //[CustomPollyRetryAttribute]
        public virtual string InvokeApi(string url, string parameter = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(result.RequestMessage));
                }
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
    }
}

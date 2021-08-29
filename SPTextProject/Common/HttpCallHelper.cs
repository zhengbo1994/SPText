using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPTextProject.Common
{
    public class HttpCallHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> Get(string url)
        {
            return await client.GetStringAsync(url);
        }

        public static async Task<string> Post(string url, Dictionary<string, string> data = null)
        {
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}

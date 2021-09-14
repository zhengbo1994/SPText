using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.Cache
{
    public class SessionManager : IDisposable
    {
        IHttpContextAccessor _httpContextAccessor;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SetSession(string key, string data)
        {
            _httpContextAccessor.HttpContext.Session.Set(key, ConvertUtils.StringToBytesByUtf8(data));
        }

        public void SetSession(string key, object data)
        {
            _httpContextAccessor.HttpContext.Session.Set(key, ConvertUtils.StringToBytesByUtf8(JsonConvert.SerializeObject(data)));
        }

        public string GetSession(string key)
        {
            byte[] sessionData;
            string temp = "";
            _httpContextAccessor.HttpContext.Session.TryGetValue(key, out sessionData);
            if (sessionData != null)
            {
                temp = ConvertUtils.BytesToStringByUtf8(sessionData);
            }
            return temp;
        }

        public T GetSession<T>(string key) where T : class, new()
        {

            byte[] sessionObjJson;
            T t = new T();
            _httpContextAccessor.HttpContext.Session.TryGetValue(key, out sessionObjJson);
            if (sessionObjJson != null)
            {
                t = JsonConvert.DeserializeObject<T>(ConvertUtils.BytesToStringByUtf8(sessionObjJson));
            }
            return t;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

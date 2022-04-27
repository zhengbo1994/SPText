using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    public class RequestApiProxy
    {
        public static string InvokeApi(string url, string parameter = null)
        {
            ProxyGenerator generator = new ProxyGenerator();
            CustomInterceptor interceptor = new CustomInterceptor();
            RequestApi requestApi = generator.CreateClassProxy<RequestApi>(interceptor);
            return requestApi.InvokeApi(url, parameter);
        }
    }
}

using FastDFS.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SPCoreApiText.Middleware
{
    /// <summary>
    /// 初始化FastDFS的中间件
    /// </summary>
    public class FastDFSMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FastDFSOptions _options;
        //如果需要其他服务，可以自己加
        public FastDFSMiddleware(RequestDelegate next, IOptions<FastDFSOptions> options)
        {
            _options = options.Value;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            List<IPEndPoint> trackerPoints = new List<IPEndPoint>();
            var addressList = _options.AddressList;
            foreach (var address in addressList)
            {
                string host = address.Host;
                int port = address.Port;
                Console.WriteLine($"Host:{host}====Port:{port}");
                trackerPoints.Add(new IPEndPoint(IPAddress.Parse(host), port));
            }
            // 初始化FastDFS对象
            ConnectionManager.Initialize(trackerPoints);
            await _next(context);
        }
    }


    public static class FastDFSMidlewareExtension
    {
        public static IApplicationBuilder UseFastDFS(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FastDFSMiddleware>();
        }
    }


    public class FastDFSOptions
    {
        public List<FastDFSAddress> AddressList { get; set; }
    }

    public class FastDFSAddress
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}

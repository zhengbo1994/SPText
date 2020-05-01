using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SPCoreText
{
    //dotnet SPCoreText.dll --urls=”http://*:5177” –-ip=”127.0.0.1” --port=5177
    public class Program
    {
        //(1)挂载到iis上加载项目
        //1.进入文件夹输入cmd
        //2.dotnet publish -o:c:\Deploy.IIS

        //(2)
        //1.进入文件夹输入cmd
        //2.dotnet run
        //3.http://localhost:5000

        //(3)
        //dotnet SPCoreText.dll --urls=”http://*:5177” –-ip=”127.0.0.1” --port=5177



        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.ConfigureKestrel((context, options) =>
                    //{
                    //    // Handle requests up to 50 MB
                    //    options.Limits.MaxRequestBodySize = 52428800;
                    //});
                    // 主机的WEB应用启动类
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())//设置工厂来替换实例

                .ConfigureLogging((context, builder) =>
                {
                    builder.AddFilter("System", LogLevel.Warning);//过滤掉命名空间
                    builder.AddFilter("Microsoft", LogLevel.Warning);
                    builder.AddLog4Net();//使用log4net
                });
    }
}

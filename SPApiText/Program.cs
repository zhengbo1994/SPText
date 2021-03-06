using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace SPCoreApiText
{
    //dotnet SPCoreApiText.dll --urls=”http://*:5177” –-ip=”127.0.0.1” --port=5177
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddCommandLine(args)//支持命令行
            //    .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(c =>
                //{
                //    c.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);
                //})
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddLog4Net();//需要配置文件
                    builder.AddFilter("ApiSystem", LogLevel.Warning);//过滤掉命名空间
                    builder.AddFilter("Api", LogLevel.Warning);
                    builder.AddLog4Net();//使用log4net
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

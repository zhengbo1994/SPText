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
    //dotnet SPCoreText.dll --urls=��http://*:5177�� �C-ip=��127.0.0.1�� --port=5177
    public class Program
    {
        //(1)���ص�iis�ϼ�����Ŀ
        //1.�����ļ�������cmd
        //2.dotnet publish -o:c:\Deploy.IIS

        //(2)
        //1.�����ļ�������cmd
        //2.dotnet run
        //3.http://localhost:5000

        //(3)
        //dotnet SPCoreText.dll --urls=��http://*:5177�� �C-ip=��127.0.0.1�� --port=5177



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
                    // ������WEBӦ��������
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())//���ù������滻ʵ��

                .ConfigureLogging((context, builder) =>
                {
                    builder.AddFilter("System", LogLevel.Warning);//���˵������ռ�
                    builder.AddFilter("Microsoft", LogLevel.Warning);
                    builder.AddLog4Net();//ʹ��log4net
                });
    }
}

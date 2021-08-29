using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SPTextProject.BLL;
using SPTextProject.Core;
using SPTextProject.DAL;
using SPTextProject.IBLL;
using SPTextProject.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPTextProject
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            })
            .ConfigureServices((hostContext, services) =>
            {
                //注册用户定义的服务
                //数据库上下文
                services.AddDbContext<InOrderContext>(ServiceLifetime.Singleton, ServiceLifetime.Singleton);
                services.AddDbContext<OutOrderContext>(ServiceLifetime.Singleton, ServiceLifetime.Singleton);

                //数据库操作
                services.AddSingleton(typeof(IUnitWork<>), typeof(UnitWork<>));
                services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));

                //BLL
                services.AddSingleton<IOrderTransferService, OrderTransferService>();
                services.AddSingleton<ITransferLogService, TransferLogService>();
                services.AddSingleton<IOmaShapeDataService, OmaShapeDataService>();
                services.AddSingleton<IRunSqlRepository, RunSqlRepository>();
            });
    }

}

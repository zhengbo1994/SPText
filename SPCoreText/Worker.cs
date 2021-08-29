using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using Autofac;
using System.Reflection;

namespace SPCoreText
{
    public class Worker : BackgroundService
    {
        private readonly Logger _logger;

        public Worker()
        {
            //_logger = log4net.LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// 服务开始后执行的处理
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("StartAsync");

                    ContainerBuilder containetBuilder = new ContainerBuilder();
                    containetBuilder.RegisterModule(new AufacModule());
                    var container = containetBuilder.Build();
                    var orderApp = container.Resolve<OrderApp>();
                    orderApp.OrderExecute();
                });
            }

            return base.StartAsync(cancellationToken);
        }
        /// <summary>
        /// 服务停止时的处理
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("StopAsync");
            });
        }
        /// <summary>
        /// 服务执行中的处理
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("ExecuteAsync");
            });
        }

    }

    public class AufacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly);
        }
    }

    public class OrderApp
    {
        public void OrderExecute()
        {
            Console.WriteLine("OrderExecute");
        }
    }
}

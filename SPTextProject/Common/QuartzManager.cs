using Autofac;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPTextProject.Common
{
    /// <summary>
    /// Quartz定时任务管理类
    /// </summary>
    public class QuartzManager
    {
        /// <summary>
        /// 初始化Quartz计时器，按照设定时间运行对应的逻辑（OrderRun下的Execute方法）
        /// </summary>
        public async static void Init()
        {
            IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();
            IJobDetail job = JobBuilder.Create<OrderRun>().WithIdentity("job", "jobGroup").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger", "triggerGroup").ForJob(job).StartNow().Build();
            //ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger", "triggerGroup").ForJob(job).StartNow().WithCronSchedule(""0/10 * * * * ? *").Build();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }


    /// <summary>
    /// 完整订单处理的类
    /// </summary>
    public class OrderRun : IJob
    {
        /// <summary>
        /// 完整订单处理的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    ContainerBuilder containetBuilder = new ContainerBuilder();
                    containetBuilder.RegisterModule(new AufacModule());
                    var container = containetBuilder.Build();
                    var orderApp = container.Resolve<OrderApp>();
                    orderApp.OrderExecute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
        }
    }
}

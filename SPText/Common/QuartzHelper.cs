using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common
{
    public class QuartzHelper
    {
        public async Task Show()
        {
            IScheduler scheduler = await ScheduleManager.BuildScheduler();
            //2.启动 scheduler
            await scheduler.Start();

            #region JobDetail
            IJobDetail jobDetail = JobBuilder.Create<SendMessage>()
                  .WithIdentity("sendJob", "group1")
                  .WithDescription("This is sendJob")
                  .Build();

            jobDetail.JobDataMap.Add("Student1", "阳光下的微笑");
            jobDetail.JobDataMap.Add("Student2", "明日梦");
            jobDetail.JobDataMap.Add("Student3", "大白");
            jobDetail.JobDataMap.Add("Student4", "池鱼");

            jobDetail.JobDataMap.Add("Year", DateTime.Now.Year);

            #endregion

            #region trigger

            //ITrigger trigger = TriggerBuilder.Create()
            //               .WithIdentity("sendTrigger", "group1")
            //               //.StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(10)))
            //               .StartNow()
            //              .WithCronSchedule("5/3 * * * * ?")//每隔一分钟 
            //              //5，8，11，14
            //              .WithDescription("This is sendJob's sendTrigger")
            //              .Build();

            ITrigger trigger = TriggerBuilder.Create()
                   .WithIdentity("sendTrigger", "group1")
                   .StartNow()
                   .WithSimpleSchedule(x => x
                       .WithIntervalInSeconds(10) //多少秒执行一次 
                       .WithRepeatCount(10) //表示最多执行多少次
                       .RepeatForever())
                       .WithDescription("This is testjob's Trigger")
                   .Build();

            trigger.JobDataMap.Add("Student5", "非常Nice");
            trigger.JobDataMap.Add("Student6", "Jerry");
            trigger.JobDataMap.Add("Student7", "龙");
            trigger.JobDataMap.Add("Student8", "月光寒");
            trigger.JobDataMap.Add("Year", DateTime.Now.Year + 1);

            #endregion


            await scheduler.ScheduleJob(jobDetail, trigger);
        }

        public async Task Show1()
        {
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = await schedFact.GetScheduler();

            //2.启动 scheduler
            await sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            await sched.ScheduleJob(job, trigger);
        }
    }



    [PersistJobDataAfterExecution] //执行后可以保留执行结果
    [DisallowConcurrentExecution] // 保证不去重复执行   可以把任务串行起来  让一个任务执行完毕以后  才去执行下一个任务
    public class SendMessage : IJob
    {

        //private static object obj = new object(); //定义一个静态变量也可以实现 执行后可以保留执行结果

        public SendMessage()
        {
            Console.WriteLine("SendMessage 被构造");
        }

        public async Task Execute(IJobExecutionContext context) //context 很强大  他会包含我们想要的切
        {
            await Task.Run(() =>
            {

                Thread.Sleep(5000);

                //发消息：给谁发，需要传递参数；
                Console.WriteLine();
                Console.WriteLine("**********************************************");
                JobDataMap jobDetailMap = context.JobDetail.JobDataMap;

                Console.WriteLine($"{jobDetailMap.Get("Student1")}同学：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{jobDetailMap.Get("Student2")}同学：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{jobDetailMap.Get("Student3")}同学：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{jobDetailMap.Get("Student4")}同学：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{jobDetailMap.Get("Year")}");
                jobDetailMap.Put("Year", jobDetailMap.GetInt("Year") + 1);

                Console.WriteLine($"{jobDetailMap.Get("Student4")}同学：准备开始上课了！{DateTime.Now}");
                JobDataMap triggerMap = context.Trigger.JobDataMap;
                Console.WriteLine();

                Console.WriteLine($"{triggerMap.Get("Student5")}同学：第二次提示：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{triggerMap.Get("Student6")}同学：第二次提示：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{triggerMap.Get("Student7")}同学：第二次提示：准备开始上课了！{DateTime.Now}");
                Console.WriteLine($"{triggerMap.Get("Student8")}同学：第二次提示：准备开始上课了！{DateTime.Now}");

                Console.WriteLine($"{triggerMap.Get("Year")}");

                Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                Console.WriteLine(context.MergedJobDataMap.Get("Year"));
                Console.WriteLine("**********************************************");
                Console.WriteLine();
            });
        }
    }


    public class ScheduleManager
    {
        public async static Task<IScheduler> BuildScheduler()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "后台作业监控系统";

            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "8008";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            var schedulerFactory = new StdSchedulerFactory(properties);
            IScheduler _scheduler = await schedulerFactory.GetScheduler();
            return _scheduler;
        }
    }

    /// <summary>
    /// 任务
    /// </summary>
    public class SimpleJob : IJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            return Console.Out.WriteLineAsync($"job工作了 在{DateTime.Now}");
        }

    }
}

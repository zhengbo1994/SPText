using Castle.DynamicProxy;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace SPCoreApiText.Utiltiy
{
    public class CustomInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            base.PreProceed(invocation);
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            ////重试 
            ////1.定义策略
            ////异常分为很多种，Handle，只要是继承者Excepiton的异常我都认；匹配异常：先具体，再大概；
            ////可以根据不同场景分别定义不同的异常； 
            //Policy retryPolicy = Policy.Handle<Exception>().Retry(retryCount: 2, (ex, count) =>
            //{
            //            //这里就可以做点自己的事儿，记录一下日志。。。。
            //            Console.WriteLine($"执行失败！重试{count} 次");
            //    Console.WriteLine($"异常信息：{ex.Message}");
            //}); 
            //retryPolicy.Execute(() =>
            //{
            //    base.PerformProceed(invocation);//这里是去执行真正的方法
            //});  
            //1.读取特性，把定在在特性中的规则去出来，然后拼接
            //if (invocation.Method.IsDefined(typeof(CustomPollyRetryAttribute), true))
            //{
            //    CustomPollyRetryAttribute attribute = invocation.Method.GetCustomAttribute<CustomPollyRetryAttribute>();
            //    //现在就可以获取这个规则
            //    ISyncPolicy policy= attribute.Do();
            //} 
            //if (invocation.Method.IsDefined(typeof(CustomPollyRetryAttribute), true))
            //{
            //    CustomPollyRetryAttribute attribute = invocation.Method.GetCustomAttribute<CustomPollyRetryAttribute>();
            //    //现在就可以获取这个规则
            //    ISyncPolicy policy = attribute.Do();
            //} 
            //if (invocation.Method.IsDefined(typeof(PollyRetryAttribute), true))
            //{
            //    IEnumerable<PollyRetryAttribute> attributeList = invocation.Method.GetCustomAttributes<PollyRetryAttribute>();
            //    ///相当于拿到了所有的规则(拿到所有的特性)
            //    //拿到以后就应该去组装策略 

            //    ISyncPolicy policy = null; 
            //    foreach (var attribute in attributeList)
            //    { 
            //        ISyncPolicy policy2 = attribute.Do();

            //        if (policy == null)
            //        {
            //            policy = policy2;
            //        }
            //        else
            //        {
            //            policy = policy.Wrap(policy2);
            //        }
            //    } 
            //    if (policy != null)
            //    {
            //        policy.Execute(() => { base.PerformProceed(invocation); });
            //    }
            //    else
            //    {
            //        base.PerformProceed(invocation);
            //    }
            //}

            ISyncPolicy policy = null; ;
            Action<ISyncPolicy> action = policy =>
            {
                policy.Execute(() =>
                {
                    base.PerformProceed(invocation);
                });
            };
            if (invocation.Method.IsDefined(typeof(PollyRetryAttribute), true))
            {
                var attributeList = invocation.Method.GetCustomAttributes<PollyRetryAttribute>().Reverse().OrderByDescending(t => t.Order);
                foreach (var attribute in attributeList)
                {
                    action = attribute.Do(action);
                }
            }
            action.Invoke(policy);
            //Postman：那怎么去控制这个顺序？


            //今晚觉得有收获的，觉得这波操作挺厉害的 刷个666；

        }

        protected override void PostProceed(IInvocation invocation)
        {
            base.PostProceed(invocation);
        }
    }

    public abstract class PollyRetryAttribute : Attribute
    {
        public int Order = 0;
        public abstract Action<ISyncPolicy> Do(Action<ISyncPolicy> action);
    }


    public class CustomPollyRetryAttribute : PollyRetryAttribute
    {
        //在这里定义策略，对外提供策略

        public override Action<ISyncPolicy> Do(Action<ISyncPolicy> action)
        {
            Policy retryPolicy = Policy.Handle<Exception>().Retry(retryCount: 2, (ex, count) =>
            {
                //这里就可以做点自己的事儿，记录一下日志。。。。
                Console.WriteLine($"执行失败！重试{count} 次");
                Console.WriteLine($"异常信息：{ex.Message}");
            });

            return s =>
            {

                Policy policy = null;
                if (s != null)
                {
                    policy = Policy.Wrap(s, retryPolicy);
                }
                else
                {
                    policy = retryPolicy;
                }

                action.Invoke(policy);
            };

        }
    }

    /// <summary>
    /// 这是一个规则
    /// </summary>
    public class CustomPollyFallbackAttribute : PollyRetryAttribute
    {
        //在这里定义策略，对外提供策略 
        public override Action<ISyncPolicy> Do(Action<ISyncPolicy> action)
        {
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(
                  exceptionsAllowedBeforeBreaking: 4,             // 连续4次异常
                  durationOfBreak: TimeSpan.FromMilliseconds(10000),       // 断开1分钟
                  onBreak: (exception, breakDelay) =>
                  {
                      Console.WriteLine($"{DateTime.Now}:熔断了。。");
                      Console.WriteLine($"熔断: {breakDelay.TotalMilliseconds } ms, 异常: " + exception.Message);
                  },
                  onReset: () => //// 熔断器关闭时
                  {
                      Console.WriteLine($"{DateTime.Now}:熔断器关闭了。。。");
                  },
                  onHalfOpen: () => // 熔断时间结束时，从断开状态进入半开状态
                  {
                      Console.WriteLine($"{DateTime.Now}:熔断时间到，进入半开状态。。。");
                  });

            return s =>
            {

                Policy policy = null;
                if (s != null)
                {
                    policy = Policy.Wrap(s, circuitBreakerPolicy);
                }
                else
                {
                    policy = circuitBreakerPolicy;
                }

                action.Invoke(policy);
            };
        }


    }
}

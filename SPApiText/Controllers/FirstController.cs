using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using SPCoreApiText.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http;

namespace SPCoreApiText.Controllers
{
    [ApiController]
    public class FirstController : ControllerBase
    {

        private readonly ILogger<FirstController> _logger;

        public FirstController(ILogger<FirstController> logger)
        {
            _logger = logger;
        }

        private static int i = 0;

        [HttpGet]
        [Route("api/First/Query")]
        public string Query()
        {
            //RequestApi requestApi = new RequestApi();
            //return requestApi.InvokeApi("http://localhost:8002/api/Order/GetString");

            //重试
            {
                ////1.定义策略
                ////异常分为很多种，Handle，只要是继承者Excepiton的异常我都认；匹配异常：先具体，再大概；
                ////可以根据不同场景分别定义不同的异常； 
                //Policy retryPolicy = Policy.Handle<Exception>().Retry(retryCount: 2, (ex, count) =>
                //{ 
                //    //这里就可以做点自己的事儿，记录一下日志。。。。
                //    Console.WriteLine($"执行失败！重试{count} 次");
                //    Console.WriteLine($"异常信息：{ex.Message}");
                //});
                ////2.根据规则来调用服务，如果发生异常了，就出发规则中定义好的后续处理  
                //return retryPolicy.Execute<string>(() =>
                // {
                //     RequestApi requestApi = new RequestApi();
                //     return requestApi.InvokeApi("http://localhost:8002/api/Order/GetCategoryList");
                // });
            }
            //超时timeout
            {
                //Policy timeoutPolicy = Policy.Timeout(TimeSpan.FromMilliseconds(20), Polly.Timeout.TimeoutStrategy.Optimistic, (context, timespan, task, exception) =>
                //{
                //    Console.WriteLine($"***************{context.Count}**************");
                //    Console.WriteLine($"***************{timespan.TotalSeconds}**************");
                //    //Console.WriteLine($"***************{task.Wait()}**************");
                //    Console.WriteLine($"***************{exception.Message}**************"); 
                //});
                //return timeoutPolicy.Execute<string>(() =>
                // {
                //     RequestApi requestApi = new RequestApi();
                //     return requestApi.InvokeApi("http://localhost:8002/api/Order/GetString");
                // });
            }
            {
                // //
                // ///回退：如果发生异常了，有一个替补的结果返回给调用者；
                // ///
                /////1.我们在定义策略的时候，是不是应该在报异常后，先重试个几遍，万一不行，就返回点指定的消息回去呗；
                /////
                ///// 如果说是在高并发的情况下，最好不要这么搞！
                ///// 
                /////最好的案例：就是发邮件、发短信；
                // string result = string.Empty;
                // var fallbackPolicy = Policy<string>.Handle<Exception>().Fallback("这里是代理的内容...", (exception, context) =>
                //{
                //    //这里就又可以做点自己的事儿呗；
                //    Console.WriteLine("发生异常了。。。。");
                //});

                // result = fallbackPolicy.Execute(() =>
                //  {
                //      Console.WriteLine("开始调用微服务。。。。");
                //      return new RequestApi().InvokeApi("http://localhost:8001/Api/Order/GetCategoryList"); //调用api 
                //   });
                // return result;
            }
            {
                ////熔断：
                ////在高并发的场景下，容易雪崩的时候就，考虑熔断；
                //string result = string.Empty;
                //var circuitBreakerPolicy = Policy
                //    .Handle<Exception>()
                //    .CircuitBreaker(
                //      exceptionsAllowedBeforeBreaking: 4,             // 连续4次异常
                //      durationOfBreak: TimeSpan.FromMilliseconds(10000),       // 断开1分钟
                //      onBreak: (exception, breakDelay) =>
                //      {
                //          Console.WriteLine($"{DateTime.Now}:熔断了。。");
                //          Console.WriteLine($"熔断: {breakDelay.TotalMilliseconds } ms, 异常: " + exception.Message);
                //      },
                //      onReset: () => //// 熔断器关闭时
                //      {
                //          Console.WriteLine($"{DateTime.Now}:熔断器关闭了。。。");
                //      },
                //      onHalfOpen: () => // 熔断时间结束时，从断开状态进入半开状态
                //      {
                //          Console.WriteLine($"{DateTime.Now}:熔断时间到，进入半开状态。。。");
                //      });

                //for (int i = 0; i < 7; i++)  
                //{
                //    Console.WriteLine($"第{i+1}次请求。。。。");
                //    try
                //    {
                //        result = circuitBreakerPolicy.Execute<string>(() =>
                //         {
                //             Console.WriteLine("开始调用微服务。。。。");
                //             return new RequestApi().InvokeApi("http://localhost:8002/api/Order/GetCategoryList");
                //         });
                //    }
                //    catch (Exception ex)
                //    {
                //        result = ex.Message;
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                //return result;
            }

            //那么如果在比较复杂的场景下，如果我需要定义多个策略呢？
            //我需要让多个策略都生效，那么怎么办？---自然是多个策略合并；

            {
                #region 合并使用 
                //重试三次   
                Policy retryTwoTimesPolicy =
                 Policy.Handle<Exception>()
                .Retry(3, (ex, count) =>
                {
                    Console.WriteLine("执行失败! 重试次数 {0}", count);
                    Console.WriteLine("异常来自 {0}", ex.GetType().Name);
                });

                ISyncPolicy fallbackPolicy =
                      Policy.Handle<Exception>()
                     .Fallback(() =>
                     {
                         Console.WriteLine("第三级");
                     }, e =>
                     {
                         Console.WriteLine("降级前操作，，比方说可以记录一下日志。。。");
                         Console.WriteLine($"{e.Message}");
                     });

                Policy policy = Policy.Wrap(retryTwoTimesPolicy, fallbackPolicy);
                return policy.Execute(() =>
                {
                    Console.WriteLine("开始调用微服务。。。。");
                    return new RequestApi().InvokeApi("http://localhost:8001/Api/Order/Getlogistics");
                });

                //策略有很多，在不同的场景下，其实我需要的策略是不一样的；
                //最好能够根据不同的场景来自由组装策略，最好能够做到动态的增加策略进来；肯定不能写成上面的样子；

                #endregion
            }
        }

        [HttpGet]
        [Route("api/First/Query01")]
        public string Query01()
        {
            RequestApiProxy.InvokeApi("http://localhost:8001/Api/Order/Getlogistics");
            return DateTime.Now.ToString();
        }


        [HttpGet]
        [Route("api/First/Query02")]
        public string Query02()
        {
            RequestApi requestApi = new RequestApi();
            return requestApi.InvokeApi("http://localhost:8002/api/Order/GetCategoryList");
        }

        [HttpGet]
        [Route("api/First/CircuitBreaker")]
        public void CircuitBreaker()
        {
            Console.WriteLine("循环调用。。。");
            for (int i = 0; i < 5; i++)
            {
                Query();
            }
        }
    }
}

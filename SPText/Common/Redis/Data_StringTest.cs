using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.Redis
{
    public class Data_StringTest
    {
        public static void Show()
        {
            using (RedisClient client = new RedisClient("127.0.0.1", 6379))
            {
                //删除当前数据库中的所有Key  默认删除的是select 0
                client.FlushDb();
                //删除所有数据库中的key 
                client.FlushAll();
                //统计网站访问数量、当前在线人数、微博数、粉丝数等，全局递增ID等
                #region 设置key的value
                //client.Set<string>("name", "朝夕教育");
                //Console.WriteLine("错误输出");
                //Console.WriteLine(client.GetValue("name"));
                //Console.WriteLine(JsonConvert.DeserializeObject<string>(client.GetValue("name")));
                //Console.WriteLine("正确输出");
                //Console.WriteLine(client.Get<string>("name"));
                //Console.WriteLine(JsonConvert.DeserializeObject<string>(client.GetValue("name")));
                #endregion

                #region 设置多个key的value
                //批量的写入redis key
                //client.SetAll(new Dictionary<string, string> { { "id", "001" }, { "name", "clay" } });
                ////批量读取内存中多个key的结果 如果我们获取的key不存在，程序会返回一个空的字符串
                //// 来判断当前用户是否是老用户 老师建议你们如果经济允许搞一个vpn
                //var getall = client.GetAll<string>(new string[] { "id", "name", "number" });
                //foreach (var item in getall)
                //{
                //	Console.WriteLine(item);
                //}
                #endregion

                #region  设置key的value并设置过期时间
                //client.Set<string>("name", "朝夕教育", TimeSpan.FromSeconds(1));
                //Task.Delay(1 * 1000).Wait();
                //Console.WriteLine(client.Get<string>("name"));
                #endregion

                #region 设置key的value并设置过期时间

                //client.Set<string>("name", "朝夕教育", DateTime.Now.AddSeconds(1));
                ////client.Set<string>("name", "朝夕教育", DateTime.Now.AddMonths(15));
                //Console.WriteLine("刚写进去的结果");
                //Console.WriteLine(client.Get<string>("name"));
                //Task.Delay(1 * 1000).Wait();
                //Console.WriteLine("1秒钟之后的结果");
                //Console.WriteLine(client.Get<string>("name"));

                //client.Set<string>("class", "优秀班级", TimeSpan.FromSeconds(10));
                //Task.Delay(1 * 1000).Wait();
                //Console.WriteLine(client.Get<string>("class"));
                #endregion

                #region 在原有key的value值之后追加value
                //client.AppendToValue("name", "I");
                //client.AppendToValue("name", " ");
                //client.AppendToValue("name", "LOVE YOU");
                //Console.WriteLine(client.Get<string>("name"));
                #endregion

                #region 获取旧值赋上新值
                //client.Set("name", "朝夕教育");
                ////获取当前key的之前的值，然后把新的结果替换进入
                //var value = client.GetAndSetValue("name", "clay");
                //Console.WriteLine("原先的值" + value);
                //Console.WriteLine("新值" + client.GetValue("name"));
                #endregion

                #region 自增1，返回自增后的值
                //给key为sid的键自增1 ，返回了自增之后的结果
                //Console.WriteLine(client.Incr("sid"));
                //Console.WriteLine(client.Incr("sid"));
                //Console.WriteLine(client.Incr("sid"));
                //Console.WriteLine("华丽丽的结束");

                //Console.WriteLine(client.GetValue("sid"));
                //每次通过传递的count累计，count就是累加的值
                //client.IncrBy("sid", 2);
                //Console.WriteLine(client.Get<string>("sid"));
                //client.IncrBy("sid", 100);
                //Console.WriteLine("最后的结果***" + client.GetValue("sid"));
                #endregion

                #region 自减1，返回自减后的值
                //Console.WriteLine(client.Decr("sid"));
                //Console.WriteLine(client.Decr("sid"));
                //Console.WriteLine(client.Decr("sid"));
                //Console.WriteLine("最后的结果"+client.GetValue("sid"));
                ////通过传入的count去做减肥 之前的结果-count
                //client.DecrBy("sid", 2);
                //Console.WriteLine("最终的结果"+client.GetValue("sid"));  
                #endregion

                #region add 和set 的区别？
                // 当使用add 方法去操作redis的时候，如果key存在的话，则不会再次进行操作 返回false 如果操作成功返回true
                //Console.WriteLine(client.Add("name", "clay"));
                ////用add的时候帮你去判断如果有则不进行操作，如果没有则写，它只能写新值，不能修改
                //Console.WriteLine(client.Add("name", "你很好clay"));
                //Console.WriteLine(client.Get<string>("name"));


                //使用set去操作 redis的时候，如果key不存在则写入当前值，并且返回true，通过存在，则对之前的值进行了一个替换 返回操作的结果
                //Console.WriteLine(client.Set("name", "clay"));
                //Console.WriteLine(client.Set("name", "你很好clay"));
                //Console.WriteLine(client.Get<string>("name"));
                #endregion

                #region  分布式锁
                TimeSpan timeSpan = new TimeSpan(100);
                /*
                    分布式锁的面试宝典
                    阻塞锁原理：
                    尝试在redis中创建一个字符串结构缓存，方法传入的key，value为锁的过期时间timeout的时间戳。
                    ***若redis中没有这个key，则创建成功（即抢到锁），然后立即返回。
                    若已经有这个key，则先watch，然后校验value中的时间戳是否已经超过当前时间。
                    若已超过，则尝试使用提交事务的方式覆盖新的时间戳，事务提交成功（即抢到锁），
                    然后立即返回；若未超过当前时间或事务提交失败（即被别人抢到锁），
                    ***则进入  一个内部优化过的微循环，不断重试。
                    
                    传入的timeout还有一个作用，就是控制重试时间，重试超时后则抛异常。
                    using完成方法调用或者显式调用dispose，都会直接清除key。
                    注意：
                    timeout有两个意思，一是如果成功加锁后锁的过期时间， 二是未成功加锁后阻塞等待的时间
                    数据锁服务通过检查value中时间戳来判断是否过期，并不是利用redis在key上设置expire time来通过key的过期实现。
                    
                    100
                    -- 切记，代码非常非常简单，原理非常非常重要，听不懂，不明白，多听听，面试的记住，把=
                 */
                {
                    //using (client.AcquireLock("Lock", timeSpan))
                    //{

                    //};//锁
                }

                /*
                    非阻塞锁原理
                    尝试在redis中创建一个字符串结构缓存项,方法传入的key，value无意义，过期时间为传入的timeout。
                    若redis中没有这个key，则创建成功（即抢到锁），然后立即返回true。若已经有这个key，则立即返回false。
                    以上过程为全局单线程原子操作，整个过程为独占式操作。
                    IsLock可以检测key是否存在。
                    注意：
                    
                    timeout即成功加锁后锁的过期时间
                    利用redis在key上设置expire time来通过key的过期实现。
                    不要先用IsLock判断是否有锁再用Add加锁，因为这两个操作非原子性操作，期间会被其他操作干
                 */
                {
                    //bool block = client.Add("Lock", true, timeSpan);
                    //if (block)
                    //{

                    //}
                }
                #endregion
            }



        }
    }
}

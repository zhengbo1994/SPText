using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.Redis.Framework002
{
    public class TransAction
    {

        public void Show()
        {
            Console.WriteLine("ok");
            #region String
            {
                Console.WriteLine("*****************String****************");
                Data_StringTest.Show();
            }
            #endregion

            #region Hash
            {
                Console.WriteLine("*****************Hash****************");
                Data_HashTest.Show();
            }
            #endregion

            #region Set ZSet
            {
                Console.WriteLine("*****************Set ZSet****************");
                Data_SetAndZsetTest.Show();
            }
            #endregion
            #region List
            {
                Console.WriteLine("*****************List****************");
                Data_ListTest.Show();
            }
            #endregion

            #region 事务
            {
                Console.WriteLine("*****************Lua****************");
                TransActionShow();
            }
            #endregion
            #region Lua
            {
                Console.WriteLine("*****************Lua****************");
                LuaTest.Show();
            }
            #endregion

            #region Subscription
            //SubscriptionShow();
            #endregion



            TransActionShow();
        }

        public void TransActionShow()
        {
            try
            {

                //事务模式 
                using (RedisClient client = new RedisClient("127.0.0.1", 6379))
                {
                    //删除当前数据库中的所有Key  默认删除的是db0
                    client.FlushDb();
                    //删除所有数据库中的key 
                    client.FlushAll();

                    client.Set("a", "1");
                    client.Set("b", "1");
                    client.Set("c", "1");

                    ////获取当前这三个key的版本号 实现事务
                    client.Watch("c");
                    using (var trans = client.CreateTransaction())
                    {
                        trans.QueueCommand(p => p.Set("a", "3"));
                        trans.QueueCommand(p => p.Set("b", "3"));
                        trans.QueueCommand(p => p.Set("c", "3"));

                        var flag = trans.Commit();
                        Console.WriteLine(flag);
                    }
                    //根据key取出值，返回string
                    Console.WriteLine(client.Get<string>("a") + ":" + client.Get<string>
                    ("b") + ":" + client.Get<string>
                    ("c"));
                    Console.ReadLine();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();

        }

        public void SubscriptionShow()
        {
            {
                try
                {
                    using (RedisClient consumer = new RedisClient("127.0.0.1", 6379))
                    {

                        Console.WriteLine($"创建订阅异常信息文本记录");
                        var subscription = consumer.CreateSubscription();
                        //接受到消息时
                        subscription.OnMessage = (channel, msg) =>
                        {
                            if (msg != "CTRL:PULSE")
                            {
                                Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:sss")}");
                                Console.WriteLine("_________________________________记录成功__________________________________");
                            }

                        };
                        //订阅频道时
                        subscription.OnSubscribe = (channel) =>
                        {
                            Console.WriteLine("订阅客户端：开始订阅" + channel);
                        };
                        //取消订阅频道时
                        subscription.OnUnSubscribe = (a) => { Console.WriteLine("订阅客户端：取消订阅"); };
                        //订阅频道
                        string topicname = "Send_Log";
                        subscription.SubscribeToChannels(topicname);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            {
                try
                {
                    using (RedisClient consumer = new RedisClient("127.0.0.1", 6379))
                    {

                        Console.WriteLine($"创建订阅异常信息文本记录");
                        var subscription = consumer.CreateSubscription();
                        //接受到消息时
                        subscription.OnMessage = (channel, msg) =>
                        {
                            if (msg != "CTRL:PULSE")
                            {
                                Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:sss")}");
                                Console.WriteLine("_________________________________记录成功__________________________________");
                            }

                        };
                        //订阅频道时
                        subscription.OnSubscribe = (channel) =>
                        {
                            Console.WriteLine("订阅客户端：开始订阅" + channel);
                        };
                        //取消订阅频道时
                        subscription.OnUnSubscribe = (a) => { Console.WriteLine("订阅客户端：取消订阅"); };
                        //订阅频道
                        string topicname = "Send_Log";
                        subscription.SubscribeToChannels(topicname);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            {
                try
                {
                    //创建一个公众号--创建一个主题
                    Console.WriteLine("发布服务");
                    IRedisClientsManager redisClientManager = new PooledRedisClientManager("127.0.0.1:6379");
                    string topicname = "Send_Log";
                    RedisPubSubServer pubSubServer = new RedisPubSubServer(redisClientManager, topicname)
                    {
                        OnMessage = (channel, msg) =>
                        {
                            Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}");
                            Console.WriteLine("___________________________________________________________________");
                        },
                        OnStart = () =>
                        {
                            Console.WriteLine("发布服务已启动");
                            Console.WriteLine("___________________________________________________________________");
                        },
                        OnStop = () => { Console.WriteLine("发布服务停止"); },
                        OnUnSubscribe = channel => { Console.WriteLine(channel); },
                        OnError = e => { Console.WriteLine(e.Message); },
                        OnFailover = s => { Console.WriteLine(s); },
                    };
                    //接收消息
                    pubSubServer.Start();
                    while (1 == 1)
                    {
                        Console.WriteLine("请输入记录的日志");
                        string message = Console.ReadLine();
                        redisClientManager.GetClient().PublishMessage(topicname, message);
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }



}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common.RabbitMQ.Framework002
{
    internal class RabbitMQProvider
    {
        public void Show()
        {
            RabbitMQConnection.SendMessage();

            #region 测试普通队列模式
            Send.SendMessage();
            #endregion

            #region 测试工作队列模式
            WorkerSend.SendMessage();
            #endregion

            #region 测试扇形队列模式
            FanoutSend.SendMessage();
            #endregion

            #region 测试匹配直接队列模式
            DirectSend.SendMessage();
            #endregion

            #region 测试模糊匹配队列模式
            TopicProvider.SendMessage();
            #endregion

            #region 测试消息确认机制
            // 事务方式
            Transaction.TransactionMode();
            // 确认方式
            ConfirmDemo.ConfirmModel();
            #endregion

            #region 测试持久化操作
            DurableDemo.SendDurableMessage();
            #endregion

            #region 测试优先级队列
            PriorityProvider.SendMessage();
            #endregion

            #region 测试死信队列
            DLXSend.SendMessage();
            #endregion

            #region 测试延迟队列
            DelayProvider.SendMessage();
            #endregion

            Console.WriteLine("====================打发打发=Provider================");

        }

        #region Confirm
        public class ConfirmDemo
        {
            public static void ConfirmModel()
            {
                var conn = Common.RabbitMQHelper.GetConnection();
                {
                    var channel = conn.CreateModel();
                    {
                        channel.ExchangeDeclare("confirm-exchange", ExchangeType.Direct, true);
                        channel.QueueDeclare("confirm-queue", false, false, false, null);
                        // channel.QueueBind("confirm-queue", "confirm-exchange", "", null);
                        var properties = channel.CreateBasicProperties();
                        //properties.DeliveryMode = 2;
                        // properties.Persistent = true;
                        byte[] message = Encoding.UTF8.GetBytes("Gerry: 欢迎来到朝夕架构班VIP");
                        //方式1：普通confirm 
                        // NormalConfirm(channel, null, message);
                        //方式2：批量confirm
                        // BatchConfirm(channel, null, message);
                        //方式3：异步确认Ack
                        ListenerConfirm(channel, properties, message);

                    }
                }
            }

            /// <summary>
            /// 方式1：普通confirm模式
            /// 每发送一条消息后，调用waitForConfirms()方法，等待服务器端confirm。实际上是一种串行confirm了。
            /// </summary>
            /// <param name="channel"></param>
            /// <param name="properties"></param>
            /// <param name="message"></param>
            static void NormalConfirm(IModel channel, IBasicProperties properties, byte[] message)
            {
                channel.ConfirmSelect(); // 开启消息确认模式
                channel.BasicPublish("confirm-exchange", ExchangeType.Fanout, properties, message);
                // 消息到达服务端队列中才返回结果
                if (!channel.WaitForConfirms())
                {
                    Console.WriteLine("消息发送失败了。");
                }
                Console.WriteLine("消息发送成功！");
                channel.Close();
            }

            /// <summary>
            /// 方式2：批量confirm模式
            /// 每发送一批消息后，调用waitForConfirms()方法，等待服务器端confirm。
            /// </summary>
            /// <param name="channel"></param>
            /// <param name="properties"></param>
            /// <param name="message"></param>
            static void BatchConfirm(IModel channel, IBasicProperties properties, byte[] message)
            {
                channel.ConfirmSelect();
                for (int i = 0; i < 10; i++)
                {
                    channel.BasicPublish("confirm-exchange", ExchangeType.Fanout, properties, message);
                }
                if (!channel.WaitForConfirms())
                {
                    Console.WriteLine("消息发送失败了。");
                }
                Console.WriteLine("消息发送成功！");
                channel.Close();
            }

            /// <summary>
            /// 使用异步回调方式监听消息是否正确送达
            /// </summary>
            /// <param name="channel"></param>
            /// <param name="properties"></param>
            /// <param name="message"></param>
            static void ListenerConfirm(IModel channel, IBasicProperties properties, byte[] message)
            {
                // properties.DeliveryMode = 2;
                channel.ConfirmSelect();//开启消息确认模式
                /*-------------Return机制：不可达的消息消息监听--------------*/
                //这个事件就是用来监听我们一些不可达的消息的内容的：比如某些情况下交换机没有绑定到队列的情况下
                EventHandler<BasicReturnEventArgs> evreturn = new EventHandler<BasicReturnEventArgs>((o, basic) =>
                {
                    var rc = basic.ReplyCode; //消息失败的code
                    var rt = basic.ReplyText; //描述返回原因的文本。
                    var msg = Encoding.UTF8.GetString(basic.Body.Span.ToArray()); //失败消息的内容
                                                                                  //在这里我们可能要对这条不可达消息做处理，比如是否重发这条不可达的消息呀，或者这条消息发送到其他的路由中等等
                    System.IO.File.AppendAllText("d:/return.txt", "调用了Return;ReplyCode:" + rc + ";ReplyText:" + rt + ";Body:" + msg);
                    Console.WriteLine("send message failed,不可达的消息消息监听.");
                });
                channel.BasicReturn += evreturn;
                //消息发送成功的时候进入到这个事件：即RabbitMq服务器告诉生产者，我已经成功收到了消息
                EventHandler<BasicAckEventArgs> BasicAcks = new EventHandler<BasicAckEventArgs>((o, basic) =>
                {
                    Console.WriteLine("abbitMq服务器告诉生产者，我已经成功收到了消息");
                });
                /*//消息发送失败的时候进入到这个事件：即RabbitMq服务器告诉生产者，你发送的这条消息我没有成功的投递到Queue中，或者说我没有收到这条消息。
                EventHandler<BasicNackEventArgs> BasicNacks = new ((o, basic) =>
                {
                    //MQ服务器出现了异常，可能会出现Nack的情况
                    Console.WriteLine("send message fail,Nacks.");
                });*/
                channel.BasicAcks += BasicAcks;
                /* channel.BasicNacks += BasicNacks;*/

                channel.BasicPublish("confirm-exchange", ExchangeType.Direct, true, properties, message);
            }
        }

        public class Transaction
        {
            /// <summary>
            /// 使用事务方式确保数据正确到达消息服务端
            /// </summary>
            public static void TransactionMode()
            {
                string exchange = "tx-exchange";
                string queue = "tx-queue";
                string routeKey = "direct_key";

                using (IConnection conn = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = conn.CreateModel())
                    {
                        try
                        {
                            channel.TxSelect(); //用于将当前channel设置成transaction事务模式
                            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                            channel.QueueDeclare(queue, false, false, false, null);
                            channel.QueueBind(queue, exchange, routeKey, null);

                            var properties = channel.CreateBasicProperties();
                            // properties.Persistent = true;
                            // properties.DeliveryMode = 2;

                            Console.Write("输入发送的内容：");
                            var msg = Console.ReadLine();

                            byte[] message = Encoding.UTF8.GetBytes("发送消息:" + msg);
                            channel.BasicPublish(exchange, routeKey, properties, message);
                            // 故意抛异常
                            throw new Exception("出现异常");

                            channel.TxCommit();//txCommit用于提交事务
                        }
                        catch (Exception ex)
                        {
                            if (channel.IsOpen)
                            {
                                Console.WriteLine("触发事务回滚");
                                channel.TxRollback();
                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region Delay
        public class DelayProvider
        {
            public static void SendMessage()
            {
                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("x-expires", 30000); // 30秒后队列自动干掉
                        dic.Add("x-message-ttl", 12000);//队列上消息过期时间，应小于队列过期时间  
                        dic.Add("x-dead-letter-exchange", "exchange-direct");//过期消息转向路由  
                        dic.Add("x-dead-letter-routing-key", "routing-delay");//过期消息转向路由相匹配routingkey  
                        channel.QueueDeclare(queue: "delay_queue",
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: dic);

                        var message = "Hello World!";
                        var body = Encoding.UTF8.GetBytes(message);

                        //向该消息队列发送消息message
                        channel.BasicPublish(exchange: "",
                            routingKey: "delay_queue",
                            basicProperties: null,
                            body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }

            }
        }
        #endregion

        #region DLX
        public class DLXSend
        {

            public static void SendMessage()
            {
                var exchangeA = "exchange";
                var routeA = "routekey";
                var queueA = "queue";

                var exchangeD = "dlx.exchange";
                var routeD = "dlx.route";
                var queueD = "dlx.queue";

                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 创建死信交换机
                        channel.ExchangeDeclare(exchangeD, type: "fanout", durable: true, autoDelete: false);
                        // 创建死信队列
                        channel.QueueDeclare(queueD, durable: true, exclusive: false, autoDelete: false);
                        // 绑定死信交换机和队列
                        channel.QueueBind(queueD, exchangeD, routeD);

                        channel.ExchangeDeclare(exchangeA, type: "fanout", durable: true, autoDelete: false);
                        channel.QueueDeclare(queueA, durable: true, exclusive: false, autoDelete: false, arguments:
                                            new Dictionary<string, object> {
                                             { "x-dead-letter-exchange",exchangeD}, //设置当前队列的DLX
                                             { "x-dead-letter-routing-key",routeD}, //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
                                             { "x-message-ttl",10000} //设置消息的存活时间，即过期时间
                                             });
                        channel.QueueBind(queueA, exchangeA, routeA);


                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        //发布消息
                        channel.BasicPublish(exchange: exchangeA,
                                             routingKey: routeA,
                                             basicProperties: properties,
                                             body: Encoding.UTF8.GetBytes("hello rabbitmq message"));
                    }
                }

            }
        }
        #endregion

        #region Durable
        /// <summary>
        /// RabbitMQ消息持久化验证
        /// 1、发送消息必须有持久化标签
        /// 2、交换机必须是持久化
        /// 3、队列必须持久化
        /// 
        /// </summary>
        public class DurableDemo
        {
            public static void SendDurableMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection("192.168.3.200", 5671);
                var channel = connection.CreateModel();
                //1、 创建持久化队列
                channel.QueueDeclare("durable_queue", durable: true, false, false, null);
                //2、 创建持久化的交换机
                channel.ExchangeDeclare("durable_exchange", "fanout", durable: true, false, null);
                channel.QueueBind("durable_queue", "durable_exchange", "", null);
                //3、消息持久化
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true; // 标记消息持久化
                                              // properties.DeliveryMode = 2; // 1 非持久化 2 持久化

                for (int i = 0; i < 30; i++)
                {
                    string message = $"Druable Message number is {i + 1}";
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    // 发送消息
                    channel.BasicPublish("durable_exchange", "", false, properties, body);
                }
            }
        }
        #endregion

        #region Exchange
        #region Direct
        public class DirectSend
        {

            public static void SendMessage()
            {
                using (var connection = Common.RabbitMQHelper.GetClusterConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明Direct交换机
                        channel.ExchangeDeclare("direct_exchange", "direct");
                        // 创建队列
                        string queueName1 = "direct_queue1";
                        channel.QueueDeclare(queueName1, false, false, false, null);
                        string queueName2 = "direct_queue2";
                        channel.QueueDeclare(queueName2, false, false, false, null);
                        string queueName3 = "direct_queue3";
                        channel.QueueDeclare(queueName3, false, false, false, null);
                        // 绑定到交互机 指定routingKey
                        channel.QueueBind(queue: queueName1, exchange: "direct_exchange", routingKey: "red");
                        channel.QueueBind(queue: queueName2, exchange: "direct_exchange", routingKey: "yellow");
                        channel.QueueBind(queue: queueName3, exchange: "direct_exchange", routingKey: "green");

                        for (int i = 0; i < 10; i++)
                        {
                            string message = $"RabbitMQ Direct {i + 1} Message =>green";
                            var body = Encoding.UTF8.GetBytes(message);
                            // 发送消息的时候需要指定routingKey发送
                            channel.BasicPublish(exchange: "direct_exchange", routingKey: "green", null, body);
                            Console.WriteLine("Send Direct {0} message", i + 1);
                        }
                    }
                }

            }
        }
        #endregion

        #region Fanout
        public class FanoutSend
        {

            public static void SendMessage()
            {
                using (var connection = Common.RabbitMQHelper.GetClusterConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明交换机对象
                        channel.ExchangeDeclare("fanout_exchange", "fanout", true);
                        // 创建队列
                        string queueName1 = "fanout_queue1";
                        channel.QueueDeclare(queueName1, true, false, false, null);
                        string queueName2 = "fanout_queue2";
                        channel.QueueDeclare(queueName2, true, false, false, null);
                        string queueName3 = "fanout_queue3";
                        channel.QueueDeclare(queueName3, true, false, false, null);
                        // 绑定到交互机
                        // fanout_exchange 绑定了 3个队列 
                        channel.QueueBind(queue: queueName1, exchange: "fanout_exchange", routingKey: "");
                        channel.QueueBind(queue: queueName2, exchange: "fanout_exchange", routingKey: "");
                        channel.QueueBind(queue: queueName3, exchange: "fanout_exchange", routingKey: "");

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        for (int i = 0; i < 10; i++)
                        {
                            string message = $"RabbitMQ Fanout {i + 1} Message";
                            var body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish("fanout_exchange", "", properties, body);
                            Console.WriteLine("Send Fanout {0} message", i + 1);
                        }
                    }
                }

            }
        }
        #endregion

        #region Topic
        public class TopicProvider
        {
            public static void SendMessage()
            {
                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare("topic_exchange", "topic");
                        // 创建队列
                        string queueName1 = "topic_queue1";
                        channel.QueueDeclare(queueName1, false, false, false, null);
                        string queueName2 = "topic_queue2";
                        channel.QueueDeclare(queueName2, false, false, false, null);
                        string queueName3 = "topic_queue3";
                        channel.QueueDeclare(queueName3, false, false, false, null);
                        // 绑定到交互机
                        channel.QueueBind(queue: queueName1, exchange: "topic_exchange", routingKey: "user.data.#");
                        channel.QueueBind(queue: queueName2, exchange: "topic_exchange", routingKey: "user.data.delete");
                        channel.QueueBind(queue: queueName3, exchange: "topic_exchange", routingKey: "user.data.update");

                        for (int i = 0; i < 10; i++)
                        {
                            string message = $"RabbitMQ Topic {i + 1} Delete Message";
                            var body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish("topic_exchange", "user.data.abc.dd.d.d", null, body);
                            Console.WriteLine("Send Topic {0} message", i + 1);
                        }
                    }
                }

            }
        }
        #endregion
        #endregion

        #region Normal
        #region RabbitMQConnection
        public class RabbitMQConnection
        {
            public static void SendMessage()
            {
                // 创建工厂对象
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = "192.168.3.215",
                    Port = 5672,
                    UserName = "gerry",
                    Password = "gerry",
                    VirtualHost = "/"
                };
                // 通过工厂对象创建连接对象
                var connection = connectionFactory.CreateConnection();
                // 通过连接对象获取Channel对象
                var channel = connection.CreateModel();
                Console.WriteLine(channel);

            }
        }
        #endregion


        #region Send
        public class Send
        {

            public static void SendMessage()
            {
                string queueName = "normal";

                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    // 创建信道
                    using (var channel = connection.CreateModel())
                    {
                        // 创建队列
                        channel.QueueDeclare(queue: queueName, durable: false, false, false, null);
                        // 没有绑定交换机，怎么找到路由队列的呢？
                        while (true)
                        {
                            string message = "Hello RabbitMQ Message";
                            var body = Encoding.UTF8.GetBytes(message);
                            // 发送消息到rabbitmq,使用rabbitmq中默认提供交换机路由,默认的路由Key和队列名称完全一致
                            channel.BasicPublish(exchange: "", routingKey: queueName, null, body);
                            Thread.Sleep(1000);
                            Console.WriteLine("Send Normal message");
                        }

                    }
                }

            }
        }
        #endregion
        #endregion

        #region Priority
        public class PriorityProvider
        {
            public static void SendMessage()
            {
                string exchange = "pro.exchange";
                string queueName = "pro.queue";
                const string MessagePrefix = "message_";
                const int PublishMessageCount = 10;
                byte messagePriority = 0;

                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false);

                        //x-max-priority属性必须设置，否则消息优先级不生效
                        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object> { { "x-max-priority", 10 } });
                        channel.QueueBind(queueName, exchange, queueName);

                        //向该消息队列发送消息message
                        Random random = new Random();
                        for (int i = 0; i < PublishMessageCount; i++)
                        {
                            var properties = channel.CreateBasicProperties();
                            messagePriority = (byte)random.Next(1, 10);
                            properties.Priority = messagePriority;//设置消息优先级，取值范围在1~9之间。
                            var message = MessagePrefix + i.ToString() + "____" + messagePriority;
                            var body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish(exchange: exchange, routingKey: ExchangeType.Fanout, basicProperties: properties, body: body);
                            Console.WriteLine($"{DateTime.Now.ToString()} Send {message} , Priority {messagePriority}");
                        }
                    }
                }
            }
        }
        #endregion

        #region Worker
        public class WorkerSend
        {

            public static void SendMessage()
            {
                string queueName = "Worker_Queue";

                using (var connection = Common.RabbitMQHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queueName, false, false, false, null);
                        for (int i = 0; i < 30; i++)
                        {
                            string message = $"RabbitMQ Worker {i + 1} Message";
                            var body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish("", queueName, null, body);
                            Console.WriteLine("send Task {0} message", i + 1);
                        }

                    }
                }

            }
        }
        #endregion
    }
}

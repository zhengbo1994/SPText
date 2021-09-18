using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common.RabbitMQ
{
    public class RabbitMQHelper
    {
        public void Show()
        {
            ProductionConsumerShow();           ////生产者消费者
            MultiProductionConsumerShow();      ////多生产者多消费者
            MutualProductionConsumerShow();     ////互为生产者消费者
            SeckillConsumerShow();              ////秒杀
            PriorityQueueShow();                ////优先级 
            PublishSubscribeConsumerShow();     ////发布订阅模式
            DirectExchangeShow();               ////直接交换机模式
            FanoutExchangeShow();               ////扇形交换机模式
            TopicExchangeShow();                ////通配符模式
            HeaderExchangeShow();               ////标签模式
            ProductionMessageTxShow();          ////事务模式
            ProductionMessageConfirmShow();     ////消息确认
            ConsumptionACKConfirmShow();        ////消息ACK自动确认
            ProductionColonyConsumerShow();     ////集群
        }

        #region  生产者消费者
        public void ProductionConsumerShow()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            //factory.VirtualHost = "/Richard";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "OnlyProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);


                    channel.ExchangeDeclare(exchange: "OnlyProducerMessageExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "OnlyProducerMessage", exchange: "OnlyProducerMessageExChange", routingKey: string.Empty, arguments: null);


                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者ProducerDemo已准备就绪~~~");
                    int i = 1;
                    {
                        while (true)
                        {
                            IBasicProperties basicProperties = channel.CreateBasicProperties();
                            basicProperties.Persistent = true;
                            //basicProperties.DeliveryMode = 2;
                            string message = $"消息{i}";
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish(exchange: "OnlyProducerMessageExChange", routingKey: string.Empty, basicProperties: basicProperties, body: body);
                            Console.WriteLine($"消息消息消息消息消息消息消息消息消息消息消息消息消息消息：{message} 已发送~");
                            i++;
                            //Thread.Sleep(10);
                        }
                    }
                }
            }
        }
        #endregion

        #region  多生产者多消费者
        public void MultiProductionConsumerShow()
        {
            string strMinute = "40";  //什么时候开始执行
            string No = "1"; //生产者编号 
            int minute = Convert.ToInt32(strMinute);
            bool flg = true;
            while (flg)
            {
                if (DateTime.Now.Minute == minute)
                {
                    Console.WriteLine($"到{strMinute}分钟，开始写入消息。。。");
                    flg = false;
                    ConnectionFactory factory = new ConnectionFactory();
                    factory.HostName = "localhost";//RabbitMQ服务在本地运行
                    factory.UserName = "guest";//用户名
                    factory.Password = "guest";//密码 
                    using (IConnection connection = factory.CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "MultiProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);
                            channel.ExchangeDeclare(exchange: "MultiProducerMessageExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                            channel.QueueBind(queue: "MultiProducerMessage", exchange: "MultiProducerMessageExChange", routingKey: string.Empty, arguments: null);

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"生产者{No}已准备就绪~~~");
                            int i = 1;
                            {
                                while (true)
                                {
                                    string message = $"生产者{No}：消息{i}";
                                    byte[] body = Encoding.UTF8.GetBytes(message);
                                    channel.BasicPublish(exchange: "MultiProducerMessageExChange", routingKey: string.Empty, basicProperties: null, body: body);
                                    Console.WriteLine($"消息：{message} 已发送~");
                                    i++;
                                    Thread.Sleep(200);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region  互为生产者消费者
        public void MutualProductionConsumerShow()
        {
            Task.Run(() => { this.ShowProductio(); });
            Task.Run(() => { this.ShowConsumer(); });
        }

        public void ShowProductio()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //生产者
                    channel.QueueDeclare(queue: "MutualProductionConsumerMessage01", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "MutualProductionConsumerMessageExChange01", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "MutualProductionConsumerMessage01", exchange: "MutualProductionConsumerMessageExChange01", routingKey: string.Empty, arguments: null);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者01已准备就绪~~~");
                    for (int i = 0; i < 1000; i++)
                    {
                        string message = $"生产者01----消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "MutualProductionConsumerMessageExChange01",
                                        routingKey: string.Empty,
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine($"{message} 已发送~");
                        Thread.Sleep(300);
                    }
                }
            }
        }

        public void ShowConsumer()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "MutualProductionConsumerMessage02", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "MutualProductionConsumerMessageExChange01", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "MutualProductionConsumerMessage02", exchange: "MutualProductionConsumerMessageExChange02", routingKey: string.Empty, arguments: null);

                    //消费者
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"消费者01 接受消息: {message}");
                        };
                        channel.BasicConsume(queue: "MutualProductionConsumerMessage02",
                                     autoAck: true,
                                     consumer: consumer);
                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }
        #endregion

        #region  秒杀
        public void SeckillConsumerShow()
        {
            string strMinute = "40";  //什么时候开始执行 
            int minute = Convert.ToInt32(strMinute);

            bool flg = true;
            while (flg)
            {
                Console.WriteLine($"到{strMinute}分钟，开始写入消息。。。");
                if (DateTime.Now.Minute == minute)
                {
                    flg = false;
                    var factory = new ConnectionFactory();
                    factory.HostName = "localhost";//RabbitMQ服务在本地运行
                    factory.UserName = "guest";//用户名
                    factory.Password = "guest";//密码 
                    using (var connection = factory.CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "SeckillProduct", durable: true, exclusive: false, autoDelete: false, arguments: null);
                            channel.ExchangeDeclare(exchange: "SeckillProductExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                            channel.QueueBind(queue: "SeckillProduct", exchange: "SeckillProductExChange", routingKey: string.Empty, arguments: null);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("开始秒杀~~~~");
                            for (int i = 1; i <= 1000; i++)
                            {
                                string message = $"第{i}用户进入 秒杀商品。。。。。";
                                byte[] body = Encoding.UTF8.GetBytes(message);
                                channel.BasicPublish(exchange: "SeckillProductExChange",
                                                routingKey: string.Empty,
                                                basicProperties: null,
                                                body: body);
                                Console.WriteLine(message);

                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region  优先级
        public void PriorityQueueShow()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "PriorityQueue", durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>() {
                         {"x-max-priority",10 }  //指定队列要支持优先级设置；
                       });

                    //声明交换机exchang
                    channel.ExchangeDeclare(exchange: "PriorityQueueExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    //绑定exchange和queue
                    channel.QueueBind(queue: "PriorityQueue", exchange: "PriorityQueueExchange", routingKey: "PriorityKey");
                    //一些待发送的消息

                    {
                        IBasicProperties props = channel.CreateBasicProperties();
                        props.DeliveryMode = 2;
                        int i = 1;
                        while (true)
                        {
                            props.Priority = 1;
                            string strMessage = $"消息：{i}";
                            channel.BasicPublish(exchange: "PriorityQueueExchange",
                                           routingKey: "PriorityKey",
                                           basicProperties: props,
                                           body: Encoding.UTF8.GetBytes(strMessage));
                            Console.WriteLine($"{strMessage} 已发送。。。。");
                            i++;
                        }
                    }
                    {
                        //string[] questionList = { "vip学员1 来请教", "甲 同学来请教问题", "乙 同学来请教问题", "丙 同学来请教问题", "丁 同学来请教问题", "vip学员2 来请教" };
                        ////设置消息优先级
                        //IBasicProperties props = channel.CreateBasicProperties();
                        //foreach (string questionMsg in questionList)
                        //{
                        //    //有优先级吗？ 
                        //    //channel.BasicPublish(exchange: "PriorityQueueExchange",
                        //    //                  routingKey: "PriorityKey",
                        //    //                  basicProperties: null,
                        //    //                  body: Encoding.UTF8.GetBytes(questionMsg));

                        //    if (questionMsg.StartsWith("vip"))
                        //    {
                        //        props.Priority = 9;
                        //        channel.BasicPublish(exchange: "PriorityQueueExchange",
                        //                       routingKey: "PriorityKey",
                        //                       basicProperties: props,
                        //                       body: Encoding.UTF8.GetBytes(questionMsg));
                        //    }
                        //    else
                        //    {
                        //        props.Priority = 1;
                        //        channel.BasicPublish(exchange: "PriorityQueueExchange",
                        //                       routingKey: "PriorityKey",
                        //                       basicProperties: props,
                        //                       body: Encoding.UTF8.GetBytes(questionMsg));
                        //    }
                        //    Console.WriteLine($"{questionMsg} 已发送~~");
                        //}
                    }
                    Console.Read();
                }
            }
        }
        #endregion

        #region  发布订阅模式
        public void PublishSubscribeConsumerShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "PublishSubscrib01", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "PublishSubscrib02", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "PublishSubscribExChange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "PublishSubscrib01", exchange: "PublishSubscribExChange", routingKey: string.Empty, arguments: null);
                    channel.QueueBind(queue: "PublishSubscrib02", exchange: "PublishSubscribExChange", routingKey: string.Empty, arguments: null);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("开始发布消息~~~~");
                    for (int i = 1; i <= 2000; i++)
                    {
                        string message = $"广播第{i}条消息...";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "PublishSubscribExChange",
                                        routingKey: string.Empty,
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine(message);
                        Thread.Sleep(200);
                    }
                }
            }
        }
        #endregion

        #region DirectExchange
        public void DirectExchangeShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "DirectExchangeLogAllQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "DirectExchangeErrorQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.ExchangeDeclare(exchange: "DirectExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    string[] logtypes = new string[] { "debug", "info", "warn", "error" };

                    foreach (string logtype in logtypes)
                    {
                        channel.QueueBind(queue: "DirectExchangeLogAllQueue", exchange: "DirectExChange", routingKey: logtype);
                    }

                    channel.QueueBind(queue: "DirectExchangeErrorQueue", exchange: "DirectExChange", routingKey: "error");

                    List<LogMsgModel> logList = new List<LogMsgModel>();
                    for (int i = 1; i <= 100; i++)
                    {
                        if (i % 4 == 0)
                        {
                            logList.Add(new LogMsgModel() { LogType = "info", Msg = Encoding.UTF8.GetBytes($"info第{i}条信息") });
                        }
                        if (i % 4 == 1)
                        {
                            logList.Add(new LogMsgModel() { LogType = "debug", Msg = Encoding.UTF8.GetBytes($"debug第{i}条信息") });
                        }
                        if (i % 4 == 2)
                        {
                            logList.Add(new LogMsgModel() { LogType = "warn", Msg = Encoding.UTF8.GetBytes($"warn第{i}条信息") });
                        }
                        if (i % 4 == 3)
                        {
                            logList.Add(new LogMsgModel() { LogType = "error", Msg = Encoding.UTF8.GetBytes($"error第{i}条信息") });
                        }
                    }

                    Console.WriteLine("生产者发送100条日志信息");
                    //发送日志信息
                    foreach (var log in logList)
                    {
                        channel.BasicPublish(exchange: "DirectExChange", routingKey: log.LogType, basicProperties: null, body: log.Msg);
                        Console.WriteLine($"{Encoding.UTF8.GetString(log.Msg)}  已发送~~");
                    }

                }
            }
        }
        public class LogMsgModel
        {
            public string LogType { get; set; }

            public byte[] Msg { get; set; }
        }
        #endregion

        #region FanoutExchange
        public void FanoutExchangeShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "FanoutExchangeZhaoxi001", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "FanoutExchangeZhaoxi002", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "FanoutExchange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "FanoutExchangeZhaoxi001", exchange: "FanoutExchange", routingKey: string.Empty, arguments: null);
                    channel.QueueBind(queue: "FanoutExchangeZhaoxi002", exchange: "FanoutExchange", routingKey: string.Empty, arguments: null);
                    //在控制台输入消息，按enter键发送消息
                    int i = 1;
                    while (true)
                    {
                        //Console.WriteLine("请输入通知~~");
                        //var message = Console.ReadLine();
                        var message = $"通知{i}";
                        var body = Encoding.UTF8.GetBytes(message);
                        //基本发布
                        channel.BasicPublish(exchange: "FanoutExchange", routingKey: string.Empty, basicProperties: null, body: body);
                        Console.WriteLine($"通知【{message}】已发送到队列");
                        Thread.Sleep(2000);
                        i++;
                    }

                }
            }
        }
        #endregion

        #region TopicExchange
        public void TopicExchangeShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "TopicExchange", type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "ChinaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "newsQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "ChinaQueue", exchange: "TopicExchange", routingKey: "China.#", arguments: null);
                    channel.QueueBind(queue: "newsQueue", exchange: "TopicExchange", routingKey: "#.news", arguments: null);
                    {
                        string message = "来自中国的新闻消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "China.news", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }

                    {
                        string message = "来自中国的天气消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "China.weather", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                    {
                        string message = "来自美国的新闻消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "usa.news", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                    {
                        string message = "来自美国的天气消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "usa.weather", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                }
            }
        }
        #endregion

        #region HeaderExchange
        public void HeaderExchangeShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "HeaderExchange", type: ExchangeType.Headers, durable: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "HeaderExchangeAllqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "HeaderExchangeAnyqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine("生产者准备就绪....");
                    channel.QueueBind(queue: "HeaderExchangeAllqueue", exchange: "HeaderExchange", routingKey: string.Empty,
                        arguments: new Dictionary<string, object> {
                                                                    { "x-match","all"},
                                                                    { "teacher","Richard"},
                                                                    { "pass","123"}});
                    {
                        string message = "teacher和pass都相同时发送的消息";
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                                           { "teacher","Richard"},
                                                                           { "pass","123"}
                                                                          };
                        var body = Encoding.UTF8.GetBytes(message);
                        //基本发布
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);
                        Console.WriteLine($"消息【{message}】已发送");
                    }
                    {
                        string message = "teacher和pass有一个不相同时发送的消息";
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                                           { "teacher","Richard"},
                                                                           { "pass","234"}
                                                                          };
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);
                        Console.WriteLine($"消息【{message}】已发送");
                    }
                    Console.WriteLine("**************************************************************");
                    {
                        channel.QueueBind(queue: "HeaderExchangeAnyqueue", exchange: "HeaderExchange", routingKey: string.Empty,
                        arguments: new Dictionary<string, object> {
                                            { "x-match","any"},
                                            { "teacher","Richard"},
                                            { "pass","123"},});

                        string msg = "teacher和pass完全相同时发送的消息";
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                 { "teacher","Richard"},
                                                 { "pass","123"}
                                            };
                        var body = Encoding.UTF8.GetBytes(msg);
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);
                        Console.WriteLine($"消息【{msg}】已发送");
                    }

                    {
                        string msg = "teacher和pass有一个不相同时发送的消息";
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                 { "teacher","Richard"},
                                                 { "pass","234"}
                                            };
                        var body = Encoding.UTF8.GetBytes(msg);
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);
                        Console.WriteLine($"消息【{msg}】已发送");
                    }

                }
            }
            Console.ReadKey();
        }
        #endregion

        #region ProductionMessageTx（事务模式）
        public void ProductionMessageTxShow()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                //创建通道channel
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine("生产者准备就绪....");
                    channel.QueueDeclare(queue: "MessageTxQueue01", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "MessageTxQueue02", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //声明交换机exchang
                    channel.ExchangeDeclare(exchange: "MessageTxQueueExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    //绑定exchange和queue
                    channel.QueueBind(queue: "MessageTxQueue01", exchange: "MessageTxQueueExchange", routingKey: "MessageTxKey01");
                    channel.QueueBind(queue: "MessageTxQueue02", exchange: "MessageTxQueueExchange", routingKey: "MessageTxKey02");
                    string message = "";
                    //发送消息
                    //在控制台输入消息，按enter键发送消息
                    while (!message.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
                    {
                        message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);
                        try
                        {
                            //开启事务机制
                            channel.TxSelect(); //事务是协议支持的
                            //发送消息
                            //同时给多个队列发送消息；要么都成功；要么都失败；
                            channel.BasicPublish(exchange: "MessageTxQueueExchange", routingKey: "MessageTxKey01", basicProperties: null, body: body);


                            channel.BasicPublish(exchange: "MessageTxQueueExchange", routingKey: "MessageTxKey02", basicProperties: null, body: body);

                            //int i = 0;
                            //int j = 1;
                            //int b = j / i;

                            //事务提交
                            channel.TxCommit(); //只有事务提交成功以后，才会真正的写入到队列里面去
                            Console.WriteLine($"【{message}】发送到Broke成功！");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"【{message}】发送到Broker失败！");
                            channel.TxRollback(); //事务回滚
                            //可能在这里还重试一下。。。
                            throw;
                        }
                    }
                    Console.Read();
                }
            }
        }
        #endregion

        #region ProductionMessageConfirm（消息确认）
        public void ProductionMessageConfirmShow()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                //创建通道channel
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine("生产者准备就绪....");
                    channel.QueueDeclare(queue: "ConfirmSelectQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //声明交换机exchang
                    channel.ExchangeDeclare(exchange: "ConfirmSelectQueueExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    //绑定exchange和queue
                    channel.QueueBind(queue: "ConfirmSelectQueue", exchange: "ConfirmSelectQueueExchange", routingKey: "ConfirmSelectKey");
                    string message = "";
                    //发送消息
                    //在控制台输入消息，按enter键发送消息
                    while (!message.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
                    {
                        message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);
                        try
                        {
                            //开启消息确认模式
                            channel.ConfirmSelect();
                            //发送消息
                            channel.BasicPublish(exchange: "ConfirmSelectQueueExchange", routingKey: "ConfirmSelectKey", basicProperties: null, body: body);
                            //事务提交 
                            if (channel.WaitForConfirms()) //如果一条消息或多消息都确认发送
                            {
                                Console.WriteLine($"【{message}】发送到Broke成功！");
                            }
                            else
                            {
                                //可以记录个日志，重试一下；
                            }

                            channel.WaitForConfirmsOrDie();//如果所有消息发送成功 就正常执行；如果有消息发送失败；就抛出异常；
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"【{message}】发送到Broker失败！");
                            //就应该通知管理员
                            // 重新试一下
                        }
                    }
                    Console.Read();
                }
            }
        }
        #endregion

        #region ConsumptionACKConfirm(消费确认)
        public void ConsumptionACKConfirmShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "ConsumptionACKConfirmQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //声明交换机exchang
                    channel.ExchangeDeclare(exchange: "ConsumptionACKConfirmQueueExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    //绑定exchange和queue
                    channel.QueueBind(queue: "ConsumptionACKConfirmQueue", exchange: "ConsumptionACKConfirmQueueExchange", routingKey: "ConsumptionACKConfirmKey");

                    for (int i = 1; i <= 1000; i++)
                    {
                        string message = $"消息{i}";
                        channel.BasicPublish(exchange: "ConsumptionACKConfirmQueueExchange",
                                         routingKey: "ConsumptionACKConfirmKey",
                                         basicProperties: null,
                                         body: Encoding.UTF8.GetBytes(message));

                        Thread.Sleep(300);
                        Console.WriteLine($"【{message}】 已发送~~~");
                    }



                    Console.Read();

                }
            }
        }
        #endregion

        #region ProductionColonyConsumer(集群)
        public void ProductionColonyConsumerShow()
        {
            ConnectionFactory factory = new ConnectionFactory();
            //factory.HostName = "192.168.3.211";//RabbitMQ服务在本地运行 


            factory.Port = 5672;
            factory.UserName = "richard";//用户名
            factory.Password = "richard";//密码 
            //factory.AutomaticRecoveryEnabled = true; //如果connection挂掉是否重新连接 
            //factory.TopologyRecoveryEnabled = true;//连接恢复后，连接的交换机，队列等是否一同恢复 

            factory.VirtualHost = "/";
            using (IConnection connection = factory.CreateConnection(new List<string>() {

                      "192.168.3.211",
                    "192.168.3.212"

            }))
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "ColonyProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.ExchangeDeclare(exchange: "ColonyProducerMessageExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "ColonyProducerMessage", exchange: "ColonyProducerMessageExChange", routingKey: "ColonyProducerMessageExChangeKey", arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者ColonyProducerDemo已准备就绪~~~");
                    int i = 1;
                    {
                        while (true)
                        {
                            IBasicProperties basicProperties = channel.CreateBasicProperties();
                            basicProperties.Persistent = true;
                            string message = $"消息{i}";
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish(exchange: "ColonyProducerMessageExChange",
                                            routingKey: "ColonyProducerMessageExChangeKey",
                                            basicProperties: basicProperties,
                                            body: body);
                            Console.WriteLine($"集群消息：{message} 已发送~");
                            i++;
                            Thread.Sleep(10);
                        }
                    }
                }
            }
        }
        #endregion

        #region  测试
        public void CreateRabbitMQ()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel model = connection.CreateModel())
                {
                    model.QueueDeclare(queue: "queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    model.ExchangeDeclare(exchange: "exchange", type: "type", durable: true, autoDelete: false, arguments: null);
                    model.QueueBind(queue: "queue", exchange: "exchange", routingKey: string.Empty, arguments: null);
                    IBasicProperties basicProperties = model.CreateBasicProperties();
                    int i = 1;
                    while (true)
                    {
                        {
                            basicProperties.Priority = 9;
                            string message = $"生产者，生产消息{i}";
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            model.BasicPublish(exchange: "exchange", routingKey: string.Empty, basicProperties: null, body: body);
                        }
                        {
                            var consumer = new EventingBasicConsumer(model);
                            consumer.Received += (p, q) =>
                            {
                                var str = q.Body;
                                var message = Encoding.UTF8.GetString(str.ToArray());
                                Console.WriteLine($"消费者{i}，消费消息{message}");
                            };
                            model.BasicConsume(queue: "queue", autoAck: false, consumer: consumer);
                        }
                    }

                }
            }
        }
        #endregion
    }
}

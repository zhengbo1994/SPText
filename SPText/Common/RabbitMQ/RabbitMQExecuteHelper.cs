using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common.RabbitMQ
{
    public class RabbitMQExecuteHelper
    {
        public void Show()
        {
            ProductionConsumerShow();
            MultiProductionConsumerShow();
            MutualProductionConsumerShow();
            SeckillConsumerShow();
            PriorityQueueShow();
            PublishSubscribeConsumerShow();
            DirectExchangeShow();
            FanoutExchangeShow();
            TopicExchangeShow();
            ConsumptionACKConfirmShow();
        }

        public void ProductionConsumerShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    {
                        {


                            Console.ForegroundColor = ConsoleColor.Green;
                            try
                            {

                                channel.QueueDeclare(queue: "OnlyProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);


                                channel.ExchangeDeclare(exchange: "OnlyProducerMessageExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                                channel.QueueBind(queue: "OnlyProducerMessage", exchange: "OnlyProducerMessageExChange", routingKey: string.Empty, arguments: null);

                                var consumer = new EventingBasicConsumer(channel);
                                consumer.Received += (model, ea) =>
                                {
                                    var body = ea.Body;
                                    var message = Encoding.UTF8.GetString(body.ToArray());
                                    Console.WriteLine($"消费者01 接受消息: {message}");
                                };
                                channel.BasicConsume(queue: "OnlyProducerMessage",
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
            }
        }

        #region MultiProductionConsumer
        public void MultiProductionConsumerShow()
        {
            Show01();
            Show02();
            Show03();
        }
        public static void Show01()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    {
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            try
                            {
                                var consumer = new EventingBasicConsumer(channel);
                                consumer.Received += (model, ea) =>
                                {
                                    var body = ea.Body;
                                    var message = Encoding.UTF8.GetString(body.ToArray());
                                    Console.WriteLine($"消费者01 接受消息: {message}");
                                };
                                channel.BasicConsume(queue: "MultiProducerMessage",
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
            }
        }

        public static void Show02()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    {
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            try
                            {
                                var consumer = new EventingBasicConsumer(channel);
                                consumer.Received += (model, ea) =>
                                {
                                    var body = ea.Body;
                                    var message = Encoding.UTF8.GetString(body.ToArray());
                                    Console.WriteLine($"消费者02 接受消息: {message}");
                                };
                                channel.BasicConsume(queue: "MultiProducerMessage",
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
            }
        }

        public static void Show03()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    {
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            try
                            {
                                var consumer = new EventingBasicConsumer(channel);
                                consumer.Received += (model, ea) =>
                                {
                                    var body = ea.Body;
                                    var message = Encoding.UTF8.GetString(body.ToArray());
                                    Console.WriteLine($"消费者03 接受消息: {message}");
                                };
                                channel.BasicConsume(queue: "MultiProducerMessage",
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
            }
        }
        #endregion

        #region MutualProductionConsumer
        public void MutualProductionConsumerShow()
        {
            ShowProductio();
            ShowConsumer();
        }
        /// <summary>
        /// 互为生产消费者--生产者
        /// </summary>
        public static void ShowProductio()
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
                    channel.QueueDeclare(queue: "MutualProductionConsumerMessage02", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "MutualProductionConsumerMessageExChange02", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "MutualProductionConsumerMessage02", exchange: "MutualProductionConsumerMessageExChange02", routingKey: string.Empty, arguments: null);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者已准备就绪~~~");
                    for (int i = 0; i < 1000; i++)
                    {
                        string message = $"生产者02------消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "MutualProductionConsumerMessageExChange02",
                                        routingKey: string.Empty,
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine($"{message} 已发送~");
                        Thread.Sleep(300);
                    }
                }
            }
        }

        /// <summary>
        /// 互为生产消费者--消费者
        /// </summary>
        public static void ShowConsumer()
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
                    //消费者
                    try
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"消费者02 接受消息: {message}");
                        };
                        channel.BasicConsume(queue: "MutualProductionConsumerMessage01",
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

        public void SeckillConsumerShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        int count = 0;
                        consumer.Received += (model, ea) =>
                        {
                            if (count >= 10)
                            {
                                Console.WriteLine("商品已经秒杀结束~这里其实没有去数据库操作~~~~");
                            }
                            else
                            {
                                ///这里其实是要到数据库中去操作数据；
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body.ToArray());
                                Console.WriteLine($"{message} 秒杀成功~");
                                count++;
                            }
                        };
                        channel.BasicConsume(queue: "SeckillProduct", autoAck: true, consumer: consumer);
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void PriorityQueueShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    #region EventingBasicConsumer
                    //定义消费者                                      
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        string msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine(msg);
                        if (msg.Equals("消息：1"))
                        {
                            channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);

                            ///设置消息优先级最高 重新写入到队列中去
                            IBasicProperties props = channel.CreateBasicProperties();
                            props.Priority = 10;
                            channel.BasicPublish(exchange: "PriorityQueueExchange",
                                           routingKey: "PriorityKey",
                                           basicProperties: props,
                                           body: Encoding.UTF8.GetBytes(msg));

                        }
                    };
                    Console.WriteLine("消费者准备就绪....");
                    //处理消息
                    channel.BasicConsume(queue: "PriorityQueue", autoAck: false, consumer: consumer);
                    Console.ReadKey();
                    #endregion
                }
            }
        }

        public void PublishSubscribeConsumerShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "Richard02";//用户名
            factory.Password = "123456";//密码 
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    channel.QueueDeclare(queue: "PublishSubscrib01", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: "PublishSubscrib02", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "PublishSubscribExChange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "PublishSubscrib01", exchange: "PublishSubscribExChange", routingKey: string.Empty, arguments: null);
                    channel.QueueBind(queue: "PublishSubscrib02", exchange: "PublishSubscribExChange", routingKey: string.Empty, arguments: null);

                    Console.WriteLine("订阅者01 已经准备就绪~~");
                    try
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"订阅者01收到消息:{message} ~");
                        };
                        channel.BasicConsume(queue: "PublishSubscrib01", autoAck: true, consumer: consumer);
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

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
                    channel.ExchangeDeclare(exchange: "DirectExChange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    string[] logtypes = new string[] { "debug", "info", "warn", "error" };
                    foreach (string logtype in logtypes)
                    {
                        channel.QueueBind(queue: "DirectExchangeLogAllQueue",
                                exchange: "DirectExChange",
                                routingKey: logtype);
                    }

                    //消费队列中的所有消息；                                   
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"【{message}】，写入文本~~");
                    };
                    //处理消息
                    channel.BasicConsume(queue: "DirectExchangeLogAllQueue",
                                         autoAck: true,
                                         consumer: consumer);
                    Console.ReadLine();
                }
            }
        }

        public void FanoutExchangeShow()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.UserName = "guest";//用户名
            factory.Password = "guest";//密码 
            using (var connection = factory.CreateConnection())
            {
                //创建通道channel
                using (var channel = connection.CreateModel())
                {
                    //声明交换机exchang
                    channel.ExchangeDeclare(exchange: "FanoutExchange",
                                            type: ExchangeType.Fanout,
                                            durable: true,
                                            autoDelete: false,
                                            arguments: null);
                    //声明队列queue
                    channel.QueueDeclare(queue: "FanoutExchangeZhaoxi001",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    //绑定exchange和queue
                    channel.QueueBind(queue: "FanoutExchangeZhaoxi001", exchange: "FanoutExchange", routingKey: string.Empty, arguments: null);
                    //定义消费者                                      
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        //只是为了演示，并没有存入文本文件
                        Console.WriteLine($"接收成功！【{message}】，邮件通知");
                    };
                    Console.WriteLine("通知服务准备就绪...");
                    //处理消息
                    channel.BasicConsume(queue: "FanoutExchangeZhaoxi001",
                                         autoAck: true,
                                         consumer: consumer);
                    Console.ReadLine();
                }

            }
        }

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
                    channel.QueueBind(queue: "ChinaQueue", exchange: "TopicExchange", routingKey: "China.#", arguments: null);
                    //定义消费者                                      
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"接收成功！【{message}】");
                    };

                    //处理消息
                    channel.BasicConsume(queue: "ChinaQueue",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine("对来自于中国的消息比较感兴趣的 消费者");
                }
            }
        }

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
                    #region EventingBasicConsumer
                    //定义消费者                                      
                    var consumer = new EventingBasicConsumer(channel);
                    int i = 0;
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        //如果在这里处理消息的手，异常了呢？ 
                        //Console.WriteLine($"接收到消息：{message}"); ; 
                        if (i < 50)
                        {
                            //手动确认  消息正常消费  告诉Broker：你可以把当前这条消息删除掉了
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            Console.WriteLine(message);
                        }
                        else
                        {
                            //否定：告诉Broker，这个消息我没有正常消费；  requeue: true：重新写入到队列里去； false:你还是删除掉；
                            channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                        }
                        i++;
                    };
                    Console.WriteLine("消费者准备就绪....");
                    {
                        ////处理消息 
                        ////autoAck: true 自动确认； 
                        //channel.BasicConsume(queue: "ConsumptionACKConfirmQueue", autoAck: true, consumer: consumer);
                    }
                    {
                        //处理消息 
                        //autoAck: false  显示确认； 
                        channel.BasicConsume(queue: "ConsumptionACKConfirmQueue", autoAck: false, consumer: consumer);
                    }


                    Console.ReadKey();
                    #endregion
                }
            }
        }
    }
}

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
    internal class RabbitMQConsumer
    {
        public void Show()
        {
            #region 测试普通队列模式
            Receive.ReceiveMessage();
            #endregion

            #region 测试工作队列模式
            WorkerReceive.ReceiveMessage();
            #endregion

            #region 扇形队列模式
            FanoutConsumer.ConsumerMessage();
            #endregion

            #region 直接队列模式
            DirectConsumer.ConsumerMessage();
            #endregion

            #region 测试模糊匹配队列模式
            TopicConsumer.ConsumerMessage();
            #endregion

            #region 测试死信交换机
            DeadExchange.TestDemo();
            #endregion

            #region 测试持久化消息消费
            DurableConsumer.ReceiveMessage();
            #endregion

            #region 测试Nack回调消费
            ConfirmConsumer.ReceiveMessage();
            #endregion

            #region 测试优先级队列
            PriorityConsumer.ConsumerMessage();
            #endregion

            #region 测试延时队列
            DelayConsumer.ReceiveMessage();
            #endregion


            #region 其它
            BackupExchange.BackupMethod();
            ReturnModel.Producer();
            TransactionModel.TxProducer();
            TransactionModel.ConfirmProducer();
            #endregion

            Console.WriteLine("消费端已经启动");
        }


        #region Confirm
        public class ConfirmConsumer
        {
            public static void ReceiveMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection("192.168.3.215", 5672);
                {
                    var channel = connection.CreateModel();
                    {
                        channel.ExchangeDeclare("confirm-exchange", ExchangeType.Direct, true);
                        channel.QueueDeclare("confirm-queue", false, false, false, null);
                        channel.QueueBind("confirm-queue", "confirm-exchange", "", null);

                        //回调，当consumer收到消息后会执行该函数
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(ea.RoutingKey);
                            Console.WriteLine(" [x] Received {0}", message);
                        };

                        //Console.WriteLine("name:" + name);
                        //消费队列"hello"中的消息
                        channel.BasicConsume(queue: "confirm-queue",
                                             autoAck: false,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }

            }
        }
        #endregion

        #region Dead
        /// <summary>
        /// 什么是消息确认机制?
        /// MQ消息确认类似于数据库中用到的 commit 语句，用于告诉broker本条消息是被消费成功了还是失败了；
        /// 平时默认消息在被接收后就被自动确认了，需要在创建消费者时、设置 autoAck: false 即可使用手动确认模式；
        /// ====================================================================================
        /// 什么是死信队列?
        /// 死信队列是用于接收普通队列发生失败的消息，其原理与普通队列相同；
        /// > 失败消息如：被消费者拒绝的消息、TTL超时的消息、队列达到最大数量无法写入的消息；
        /// 死信队列创建方法：
        /// > 在创建普通队列时，在参数"x-dead-letter-exchange"中定义失败消息转发的目标交换机；
        /// > 再创建一个临时队列，订阅"x-dead-letter-exchange"中指定的交换机；
        /// > 此时的临时队列就能接收到普通队列失败的消息了；
        /// > 可在消息的 Properties.headers.x-death 属性中查询到消息投递源信息和消息被投递的次数；
        /// </summary>
        public class DeadExchange
        {
            private static string _exchangeNormal = "Exchange.Normal";  //定义一个用于接收 正常 消息的交换机
            private static string _exchangeRetry = "Exchange.Retry";    //定义一个用于接收 重试 消息的交换机
            private static string _exchangeFail = "Exchange.Fail";      //定义一个用于接收 失败 消息的交换机
            private static string _queueNormal = "Queue.Noraml";        //定义一个用于接收 正常 消息的队列
            private static string _queueRetry = "Queue.Retry";          //定义一个用于接收 重试 消息的队列
            private static string _queueFail = "Queue.Fail";            //定义一个用于接收 失败 消息的队列

            public static void TestDemo()
            {
                var connection = Common.RabbitMQHelper.GetConnection();
                var channel = connection.CreateModel();

                //声明交换机
                channel.ExchangeDeclare(exchange: _exchangeNormal, type: "topic");
                channel.ExchangeDeclare(exchange: _exchangeRetry, type: "topic");
                channel.ExchangeDeclare(exchange: _exchangeFail, type: "topic");

                //定义队列参数
                var queueNormalArgs = new Dictionary<string, object>();
                {
                    // 绑定失败交互交换机
                    queueNormalArgs.Add("x-dead-letter-exchange", _exchangeFail);   //指定死信交换机，用于将 Noraml 队列中失败的消息投递给 Fail 交换机
                }
                var queueRetryArgs = new Dictionary<string, object>();
                {
                    queueRetryArgs.Add("x-dead-letter-exchange", _exchangeNormal);  //指定死信交换机，用于将 Retry 队列中超时的消息投递给 Noraml 交换机
                    queueRetryArgs.Add("x-message-ttl", 6000); //定义 queueRetry 的消息最大停留时间 (原理是：等消息超时后由 broker 自动投递给当前绑定的死信交换机)                                                                             
                                                               //定义最大停留时间为防止一些 待重新投递 的消息、没有定义重试时间而导致内存溢出
                }
                var queueFailArgs = new Dictionary<string, object>();
                {
                    //暂无
                }

                //声明队列
                channel.QueueDeclare(queue: _queueNormal, durable: true, exclusive: false, autoDelete: false, arguments: queueNormalArgs);
                channel.QueueDeclare(queue: _queueRetry, durable: true, exclusive: false, autoDelete: false, arguments: queueRetryArgs);
                channel.QueueDeclare(queue: _queueFail, durable: true, exclusive: false, autoDelete: false, arguments: queueFailArgs);

                //为队列绑定交换机
                channel.QueueBind(queue: _queueNormal, exchange: _exchangeNormal, routingKey: "#");
                channel.QueueBind(queue: _queueRetry, exchange: _exchangeRetry, routingKey: "#");
                channel.QueueBind(queue: _queueFail, exchange: _exchangeFail, routingKey: "#");

                #region 创建一个普通消息消费者
                {
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, e) =>
                    {
                        var _sender = (EventingBasicConsumer)sender;            //消息传送者
                        var _channel = _sender.Model;                           //消息传送通道
                        var _message = (BasicDeliverEventArgs)e;                //消息传送参数
                        var _headers = _message.BasicProperties.Headers;        //消息头
                        var _content = Encoding.UTF8.GetString(_message.Body.ToArray());  //消息内容
                        var _death = default(Dictionary<string, object>);       //死信参数

                        if (_headers != null && _headers.ContainsKey("x-death"))
                            _death = (Dictionary<string, object>)(_headers["x-death"] as List<object>)[0];

                        try
                        #region 消息处理
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t(1.0)消息接收：\r\n\t[deliveryTag={_message.DeliveryTag}]\r\n\t[consumerID={_message.ConsumerTag}]\r\n\t[exchange={_message.Exchange}]\r\n\t[routingKey={_message.RoutingKey}]\r\n\t[content={_content}]");

                            throw new Exception("模拟消息处理失败效果。");

                            //处理成功时
                            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t(1.1)处理成功：\r\n\t[deliveryTag={_message.DeliveryTag}]");

                            //消息确认 (销毁当前消息)
                            _channel.BasicAck(deliveryTag: _message.DeliveryTag, multiple: false);
                        }
                        #endregion
                        catch (Exception ex)
                        #region 消息处理失败时
                        {
                            var retryCount = (long)(_death?["count"] ?? default(long)); //查询当前消息被重新投递的次数 (首次则为0)

                            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t(1.2)处理失败：\r\n\t[deliveryTag={_message.DeliveryTag}]\r\n\t[retryCount={retryCount}]");

                            if (retryCount >= 2)
                            #region 投递第3次还没消费成功时，就转发给 exchangeFail 交换机
                            {
                                //消息拒绝（投递给死信交换机，也就是上边定义的 ("x-dead-letter-exchange", _exchangeFail)）
                                _channel.BasicNack(deliveryTag: _message.DeliveryTag, multiple: false, requeue: false);
                            }
                            #endregion
                            else
                            #region 否则转发给 exchangeRetry 交换机
                            {
                                var interval = (retryCount + 1) * 10; //定义下一次投递的间隔时间 (单位：秒)
                                                                      //如：首次重试间隔10秒、第二次间隔20秒、第三次间隔30秒

                                //定义下一次投递的间隔时间 (单位：毫秒)
                                _message.BasicProperties.Expiration = (interval * 1000).ToString();

                                //将消息投递给 _exchangeRetry (会自动增加 death 次数)
                                _channel.BasicPublish(exchange: _exchangeRetry, routingKey: _message.RoutingKey, basicProperties: _message.BasicProperties, body: _message.Body);

                                //消息确认 (销毁当前消息)
                                _channel.BasicAck(deliveryTag: _message.DeliveryTag, multiple: false);
                            }
                            #endregion
                        }
                        #endregion
                    };
                    channel.BasicConsume(queue: _queueNormal, autoAck: false, consumer: consumer);
                }
                #endregion

                #region 创建一个失败消息消费者
                {
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, e) =>
                    {
                        var _message = (BasicDeliverEventArgs)e;                //消息传送参数
                        var _content = Encoding.UTF8.GetString(_message.Body.ToArray());  //消息内容

                        Console.WriteLine();
                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t(2.0)发现失败消息：\r\n\t[deliveryTag={_message.DeliveryTag}]\r\n\t[consumerID={_message.ConsumerTag}]\r\n\t[exchange={_message.Exchange}]\r\n\t[routingKey={_message.RoutingKey}]\r\n\t[content={_content}]");
                    };

                    channel.BasicConsume(queue: _queueFail, autoAck: true, consumer: consumer);
                }
                #endregion

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t正在运行中...");

                var cmd = default(string);
                while ((cmd = Console.ReadLine()) != "close")
                #region 模拟正常消息发布
                {
                    var msgProperties = channel.CreateBasicProperties();
                    var msgContent = $"消息内容_{DateTime.Now.ToString("HH:mm:ss.fff")}_{cmd}";

                    channel.BasicPublish(exchange: _exchangeNormal, routingKey: "亚洲.中国.经济", basicProperties: msgProperties, body: Encoding.UTF8.GetBytes(msgContent));

                    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t发送成功：{msgContent}");
                    Console.WriteLine();
                }
                #endregion

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t正在关闭...");

                channel.ExchangeDelete(_exchangeNormal);
                channel.ExchangeDelete(_exchangeRetry);
                channel.ExchangeDelete(_exchangeFail);
                channel.QueueDelete(_queueNormal);
                channel.QueueDelete(_queueRetry);
                channel.QueueDelete(_queueFail);
                //channel.Abort();
                channel.Close(200, "Goodbye!");
                channel.Dispose();
                connection.Close(200, "Goodbye!");
                connection.Dispose();

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t运行结束。");
                Console.ReadKey();
            }
        }
        #endregion

        #region Delay
        public class DelayConsumer
        {
            public static void ReceiveMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection("192.168.3.215", 5672);
                {
                    var channel = connection.CreateModel();
                    {

                        channel.ExchangeDeclare(exchange: "exchange-direct", type: "direct");
                        // 通过api自动产生队列名字
                        string name = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queue: name, exchange: "exchange-direct", routingKey: "routing-delay");

                        //回调，当consumer收到消息后会执行该函数
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(ea.RoutingKey);
                            Console.WriteLine(" [x] Received {0}", message);
                        };

                        //Console.WriteLine("name:" + name);
                        //消费队列"hello"中的消息
                        channel.BasicConsume(queue: name,
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                    }
                }

            }
        }
        #endregion

        #region Durable
        public class DurableConsumer
        {
            public static void ReceiveMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection("192.168.3.216", 5671);
                {
                    var channel = connection.CreateModel();
                    {
                        //1、 创建持久化队列
                        channel.QueueDeclare("durable_queue", true, false, false, null);
                        //2、 创建持久化的交换机
                        channel.ExchangeDeclare("durable_exchange", "fanout", true, false, null);
                        channel.QueueBind("durable_queue", "durable_exchange", "", null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" Durable Queue Received => {0}", message);
                        };
                        channel.BasicConsume("durable_queue", true, consumer);
                    }
                }

            }
        }
        #endregion

        #region Exchange
        #region Direct
        public class DirectConsumer
        {
            public static void ConsumerMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");
                var queueName = "direct_queue3";
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.QueueBind(queue: queueName,
                                          exchange: "direct_exchange",
                                          routingKey: "red");
                channel.QueueBind(queue: queueName,
                                          exchange: "direct_exchange",
                                          routingKey: "yellow");
                channel.QueueBind(queue: queueName,
                                          exchange: "direct_exchange",
                                          routingKey: "green");

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);

                    // 消费完成后需要手动签收消息，如果不写该代码就容易导致重复消费问题
                    channel.BasicAck(ea.DeliveryTag, true); // 可以降低每次签收性能损耗
                };

                // 消息签收模式
                // 手动签收 保证正确消费，不会丢消息(基于客户端而已)
                // 自动签收 容易丢消息 
                // 签收：意味着消息从队列中删除
                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        #endregion

        #region Fanout
        public class FanoutConsumer
        {
            public static void ConsumerMessage()
            {
                var connection = Common.RabbitMQHelper.GetClusterConnection();
                {
                    var channel = connection.CreateModel();
                    {
                        //申明exchange
                        channel.ExchangeDeclare(exchange: "fanout_exchange", type: "fanout");
                        // 创建队列
                        string queueName1 = "fanout_queue1";
                        channel.QueueDeclare(queueName1, false, false, false, null);
                        string queueName2 = "fanout_queue2";
                        channel.QueueDeclare(queueName2, false, false, false, null);
                        string queueName3 = "fanout_queue3";
                        channel.QueueDeclare(queueName3, false, false, false, null);
                        // 绑定到交互机
                        channel.QueueBind(queue: queueName1, exchange: "fanout_exchange", routingKey: "");
                        channel.QueueBind(queue: queueName2, exchange: "fanout_exchange", routingKey: "");
                        channel.QueueBind(queue: queueName3, exchange: "fanout_exchange", routingKey: "");


                        Console.WriteLine("[*] Waitting for fanout logs.");
                        //申明consumer
                        var consumer = new EventingBasicConsumer(channel);
                        //绑定消息接收后的事件委托
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine("[x] {0}", message);

                        };

                        channel.BasicConsume(queue: queueName3, autoAck: true, consumer: consumer);
                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
            }
        }
        #endregion

        #region Topic
        public class TopicConsumer
        {
            public static void ConsumerMessage()
            {
                var connection = Common.RabbitMQHelper.GetConnection();
                var channel = connection.CreateModel();
                var queueName = "topic_queue1";
                channel.ExchangeDeclare(exchange: "topic_exchange", type: "topic");
                channel.QueueDeclare(queueName, false, false, false, null);
                // 有个bug
                channel.QueueBind(queue: queueName,
                                          exchange: "topic_exchange",
                                          routingKey: "user.data.insert");

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        #endregion
        #endregion

        #region Noraml
        public class Receive
        {
            public static void ReceiveMessage()
            {
                // 消费者消费是队列中消息
                string queueName = "normal";
                var connection = Common.RabbitMQHelper.GetConnection("192.168.3.215", 5672);
                {
                    var channel = connection.CreateModel();
                    {
                        // 如果你先启动是消费端就会异常
                        channel.QueueDeclare(queueName, false, false, false, null);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" Normal Received => {0}", message);
                        };
                        channel.BasicConsume(queueName, true, consumer);
                    }
                }

            }
        }
        #endregion

        #region Priority
        public class PriorityConsumer
        {
            public static void ConsumerMessage()
            {
                string exchange = "pro.exchange";
                string queueName = "pro.queue";
                var connection = Common.RabbitMQHelper.GetConnection();
                {
                    var channel = connection.CreateModel();
                    {
                        channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false);
                        //x-max-priority属性必须设置，否则消息优先级不生效
                        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object> { { "x-max-priority", 10 } });
                        channel.QueueBind(queueName, exchange, queueName);

                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            byte[] body = ea.Body.ToArray();
                            string message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(message);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };

                        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    }
                }
            }
        }
        #endregion

        #region Worker
        public class WorkerReceive
        {
            public static void ReceiveMessage()
            {
                string queueName = "Worker_Queue";
                var connection = Common.RabbitMQHelper.GetConnection();
                {
                    var channel = connection.CreateModel();
                    {
                        channel.QueueDeclare(queueName, false, false, false, null);
                        var consumer = new EventingBasicConsumer(channel);
                        //设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不再分配任务。
                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); //能者多劳
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" Worker Queue Received => {0}", message);
                        };
                        channel.BasicConsume(queueName, true, consumer);
                    }

                }

            }
        }
        #endregion


        #region BackupExchange
        /// <summary>
        /// 使用交换机备份
        /// </summary>
        ///
        public class BackupExchange
        {
            public static void BackupMethod()
            {
                string queueName = "BACKUP_QUEUE";
                string exchangeName = "BACKUP_EXCHANGE";
                string backupQueue = "BACKUP_QUEUE_D";
                string backupExchangeName = "ALTERNATE_EXCHNAGE";
                string routeKey = "BACKUP_ROUTEKEY";
                using (var connection = Common.RabbitMQHelper.GetConnection("192.168.3.200", 5671))
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明交互交换机[指定备份交换机]
                        Dictionary<string, object> argument = new Dictionary<string, object>();
                        argument.Add("alternate-exchange", backupExchangeName);
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false, false, argument);
                        // 声明备份交互机
                        channel.ExchangeDeclare(backupExchangeName, ExchangeType.Fanout, false, false, null);
                        // 声明队列
                        channel.QueueDeclare(queueName, false, false, false, null);
                        // 声明备份队列
                        channel.QueueDeclare(backupQueue, false, false, false, null);

                        // 绑定队列
                        channel.QueueBind(queueName, exchangeName, routeKey);
                        channel.QueueBind(backupQueue, backupExchangeName, "");

                        //发送数据
                        for (int i = 0; i < 10; i++)
                        {
                            // 消息内容
                            string message = "This is Backup Exchange Model-->" + i;
                            var body = Encoding.UTF8.GetBytes(message);
                            //指定发送消息到哪个路由，以及他的路由键,消息等
                            if (i % 2 == 0)
                            {
                                channel.BasicPublish(exchangeName, routeKey, null, body);
                            }
                            else
                            {
                                //匹配不到队列[如果路由key找不到队列则启用备用交换机]
                                channel.BasicPublish(exchangeName, "kkkk", null, body);
                            }
                            Console.WriteLine(" [x] Sent '" + message + "'");
                            Thread.Sleep(200);
                        }
                    }
                }
            }
        }
        #endregion


        #region ReturnModel
        /// <summary>
        /// 演示消息没有正确路由到队列处理方式
        /// </summary>
        /// 
        public class ReturnModel
        {
            public static void Producer()
            {
                string queueName = "RE_QUEUE";
                string exchangeName = "RE_EXCHANGE";
                using (var connection = Common.RabbitMQHelper.GetConnection("192.168.3.200", 5671))
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明队列
                        channel.QueueDeclare(queueName, false, false, false, null);
                        // 声明交换机[交换机没有绑定队列的情况]
                        //channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
                        //channel.QueueBind(queueName, exchangeName, "");
                        // 声明交换机
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                        channel.QueueBind(queueName, exchangeName, "test_direct");

                        string message = "This is Return Model";
                        var body = Encoding.UTF8.GetBytes(message);
                        // 配置回调
                        channel.BasicReturn += (o, basic) =>
                        {
                            var rc = basic.ReplyCode; //消息失败的code
                            var rt = basic.ReplyText; //描述返回原因的文本。
                            var msg = Encoding.UTF8.GetString(basic.Body.Span.ToArray()); //失败消息的内容
                                                                                //在这里我们可能要对这条不可达消息做处理，比如是否重发这条不可达的消息呀，或者这条消息发送到其他的路由中等等
                            System.IO.File.AppendAllText("d:/return.txt", "调用了Return;ReplyCode:" + rc + ";ReplyText:" + rt + ";Body:" + msg + "\r\n");
                            Console.WriteLine("send message failed,不可达的消息消息监听.");
                        };

                        var properties = channel.CreateBasicProperties();
                        properties.MessageId = "fdsfdfs";
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: "test_direct1",
                                             mandatory: true, // 必须设置该参数为true
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }
        }
        #endregion

        #region TransactionModel
        public class TransactionModel
        {
            public static void TxProducer()
            {
                string queueName = "ORIGN_QUEUE";
                using (var connection = Common.RabbitMQHelper.GetConnection("192.168.3.200", 5671))
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明队列
                        channel.QueueDeclare(queueName, false, false, false, null);
                        try
                        {
                            // 开启事务
                            channel.TxSelect();
                            // 发送信息
                            channel.BasicPublish("", queueName, false, null, Encoding.UTF8.GetBytes("这个是事务消息1"));
                            // 提交事务
                            channel.TxCommit();

                            channel.BasicPublish("", queueName, false, null, Encoding.UTF8.GetBytes("这个是事务消息2"));
                            // 模拟异常
                            int i = 1;
                            int x = i / 0;
                            channel.TxCommit();
                            Console.WriteLine("消息发送成功");
                        }
                        catch (Exception)
                        {
                            if (channel.IsOpen)
                            {
                                // 回滚事务
                                channel.TxRollback();
                                Console.WriteLine("消息已经回滚");
                            }
                            throw;
                        }
                    }
                }
            }
            public static void ConfirmProducer()
            {
                string queueName = "ORIGN_QUEUE";
                using (var connection = Common.RabbitMQHelper.GetConnection("192.168.3.215", 5672))
                {
                    using (var channel = connection.CreateModel())
                    {
                        // 声明队列
                        channel.QueueDeclare(queueName, false, false, false, null);
                        string message = "This is Confirm Model";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.ConfirmSelect();
                        channel.BasicAcks += (sender, e) =>
                        {
                            Console.Write("ACK received");
                        };

                        var properties = channel.CreateBasicProperties();

                        channel.BasicPublish(exchange: "",
                                             routingKey: queueName,
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }
        }
        #endregion

    }
}

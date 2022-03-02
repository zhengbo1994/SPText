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
    public class RBMQHelper
    {
        string exchangeName = "demoexchange";
        string queueName = "demoqueue";
        string exchangeType = ExchangeType.Direct;
        string routingKey = "demoqueue";

        string userName = "test";
        string password = "test";
        string hostName = "127.0.0.1";
        int port = 5672;
        string virtualHost = "vhost";

        public delegate void MQMsgDelegate(string msg);
        public event MQMsgDelegate MQMsg;

        public delegate void MQErrorDeletegate(string error);
        public event MQErrorDeletegate MQError;

        /// <summary>
        /// 发布消息队列
        /// </summary>
        private Queue<string> ProducerQueue = new Queue<string>();

        private object obj = new object();
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            lock (obj)
            {
                ProducerQueue.Enqueue(msg);
            }
        }

        /// <summary>
        /// RabbitMQ
        /// </summary>
        /// <param name="exchangeName">消息交换机</param>
        /// <param name="queueName">消息队列</param>
        /// <param name="exchangeType">交换器类型</param>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="hostName">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="virtualHost">虚拟主机</param>
        public RBMQHelper(string exchangeName, string queueName, string exchangeType, string routingKey, string userName, string password, string hostName, int port, string virtualHost)
        {
            this.exchangeName = exchangeName;
            this.queueName = queueName;
            this.exchangeType = exchangeType;
            this.routingKey = routingKey;
            this.userName = userName;
            this.password = password;
            this.hostName = hostName;
            this.port = port;
            this.virtualHost = virtualHost;
        }

        /// <summary>
        /// 开始消费
        /// </summary>
        public void Consumer()
        {
            try
            {

                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = userName;
                factory.Password = password;
                factory.HostName = hostName;
                factory.Port = port;
                factory.VirtualHost = virtualHost;

                //factory.AutomaticRecoveryEnabled = true;
                using (var connection = factory.CreateConnection())
                {

                    using (var channel = connection.CreateModel())
                    {
                        //设置交换器的类型
                        channel.ExchangeDeclare(exchangeName, exchangeType);

                        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                        channel.QueueDeclare(queueName, false, false, false, null);

                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(queueName, exchangeName, routingKey, null);

                        //流量控制
                        channel.BasicQos(0, 2, false);

                        while (true)
                        {
                            //消费数据
                            var consumer = new EventingBasicConsumer(channel);

                            //false为手动应答，true为自动应答
                            channel.BasicConsume(queueName, false, consumer);

                            consumer.Received += (ch, ea) =>
                            {
                                var body = ea.Body.ToArray();

                                MQMsg(Encoding.UTF8.GetString(body));

                                //Console.WriteLine("已接收： {0}", Encoding.UTF8.GetString(body));

                                //手动应答时使用
                                channel.BasicAck(ea.DeliveryTag, false);
                            };

                            string consumerTag = channel.BasicConsume(queueName, false, consumer);
                            channel.BasicCancel(consumerTag);

                            Thread.Sleep(1);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MQError(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 开始发布
        /// </summary>
        public void Producer()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = userName;
                factory.Password = password;
                factory.HostName = hostName;
                factory.Port = port;
                factory.VirtualHost = virtualHost;

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //设置交换器的类型
                        channel.ExchangeDeclare(exchangeName, exchangeType);

                        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                        channel.QueueDeclare(queueName, false, false, false, null);

                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(queueName, exchangeName, routingKey, null);

                        //消息特点
                        var properties = channel.CreateBasicProperties();
                        properties.ContentType = "text/plain";
                        properties.DeliveryMode = 2;

                        while (true)
                        {
                            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                            watch.Start();//开始计时

                            Console.WriteLine("队列内数据量:" + (ProducerQueue.Count));//输出时间 毫秒
                            lock (obj)
                            {
                                if (ProducerQueue.Count > 0)
                                {
                                    while (ProducerQueue.Count > 0)
                                    {
                                        var sendMsg = ProducerQueue.Dequeue();

                                        //发送消息
                                        byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(sendMsg);
                                        channel.BasicPublish(exchangeName, routingKey, properties, messageBodyBytes);
                                        //Console.WriteLine("写入数据：" + sendMsg);

                                        //MQMsg(sendMsg +"待写入："+ ProducerQueue.Count);

                                        Thread.Sleep(1);
                                    }
                                }
                            }
                            watch.Stop();//停止计时

                            Console.WriteLine("耗时:" + (watch.ElapsedMilliseconds));//输出时间 毫秒


                            Thread.Sleep(1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MQError(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

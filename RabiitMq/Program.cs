using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabiitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            //连接对象
            var factory = new ConnectionFactory()
            {
                //主机名
                HostName = "127.0.0.1",
                //用户名
                UserName = "bobo",
                //密码
                Password = "520520",
            };


            //SendTx(factory);

            // Received(factory);


            SendConfirm(factory);


        }


        public static void SendTx(ConnectionFactory factory)
        {
            //打开连接
            using (var connection = factory.CreateConnection())
            {
                //创建model
                using (var channel = connection.CreateModel())
                {
                    //声明交换机
                    //exchange:交换机名称，type：交换机类型,durable：是否持久化，autoDelete:是否自动删除，arguments：参数
                    channel.ExchangeDeclare(exchange: "directExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    //queue：队列名称，durable:是否排他队列
                    channel.QueueDeclare(queue: "ChinaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //绑定队列
                    channel.QueueBind(queue: "ChinaQueue", exchange: "directExchange", routingKey: "China", arguments: null);


                    //发送消息


                    //开启事务
                    channel.TxSelect();

                    try
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            var message = "中国" + i;
                            var btye = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "directExchange", routingKey: "China", basicProperties: null, body: btye);
                            
                        }


                        //int ii = 0;
                        //int j = 1;
                        //int k = j / ii;

                        //提交事务
                        channel.TxCommit();

                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);

                        //回滚事务
                        channel.TxRollback();
                    }                               
                }
            }
        }



        public static void SendConfirm(ConnectionFactory factory)
        {
            //打开连接
            using (var connection = factory.CreateConnection())
            {
                //创建model
                using (var channel = connection.CreateModel())
                {
                    //声明交换机
                    //exchange:交换机名称，type：交换机类型,durable：是否持久化，autoDelete:是否自动删除，arguments：参数
                    channel.ExchangeDeclare(exchange: "directExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    //queue：队列名称，durable:是否排他队列
                    channel.QueueDeclare(queue: "ChinaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //绑定队列
                    channel.QueueBind(queue: "ChinaQueue", exchange: "directExchange", routingKey: "China", arguments: null);

                    //发送消息                  
                    try
                    {

                        var message = "中国";
                        var btye = Encoding.UTF8.GetBytes(message);

                        //开启消息确认模式
                        channel.ConfirmSelect();

                        //消息持久化

                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish(exchange: "directExchange", routingKey: "China", basicProperties: properties, body: btye);

                        //如果一条消息或多条消息都确认发送 那么为true 否则失败
                        if (channel.WaitForConfirms())
                        {
                            Console.WriteLine("消息发送成功");
                        }
                        else
                        {

                        }

                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);


                    }
                }
            }
        }

        public static void Received(ConnectionFactory factory)
        {   
            //创建连接
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明交换机
                    channel.ExchangeDeclare(exchange: "directExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    //queue：队列名称，durable:是否排他队列
                    channel.QueueDeclare(queue: "ChinaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //绑定队列
                    channel.QueueBind(queue: "ChinaQueue", exchange: "directExchange", routingKey: "China", arguments: null);

                    //定义消费者
                    var consumer = new EventingBasicConsumer(channel);

                    //监听事件
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());

                        Console.WriteLine($"接受到消息{message}");

                        if (message == "1")
                        {
                            //手动确定消息 消息正常消费，告诉Broker:你可以删除当前这一条消息了
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);


                        }
                        //出异常了 不确认消息 //requeue: true 重新放到队列当中去
                        else
                        {
                            channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                        }



                    };

                    //处理消息
                    channel.BasicConsume(
                        queue: "ChinaQueue",
                        autoAck: false,
                        consumer: consumer
                        );

                    Console.ReadLine();
                }
            }
        }
    }
}

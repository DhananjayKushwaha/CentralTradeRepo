using CentralTrade.Domain.Interfaces;
using CentralTrade.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CentralTrade.Domain.Services
{
    public class RabbitMqMessageHandler : IMessageHandler<IMessage>
    {
        public void Send(IMessage message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: message.Exchange, type: message.Type);

                    channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                    channel.BasicPublish(exchange: message.Exchange,
                                         routingKey: message.Key,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        //public static void RecieveOrders()
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.ExchangeDeclare(exchange: "trade_orders",
        //                                type: "direct");

        //        var queueName = channel.QueueDeclare().QueueName;

        //        //read all avaibale stock ids from repo
        //        List<Guid> stockIds = new List<Guid>();//set from db
        //        foreach (var stockId in stockIds)
        //        {
        //            channel.QueueBind(queue: queueName,
        //                                  exchange: "trade_orders",
        //                                  routingKey: stockId.ToString());
        //        }

        //        Console.WriteLine(" [*] Waiting for messages.");

        //        var consumer = new EventingBasicConsumer(channel);
        //        consumer.Received += (model, ea) =>
        //        {
        //            var body = ea.Body;
        //            var message = Encoding.UTF8.GetString(body.ToArray());
        //            var routingKey = ea.RoutingKey;
        //            Console.WriteLine(" [x] Received '{0}':'{1}'",
        //                              routingKey, JsonSerializer.Deserialize<Order>(message));

        //            //process the message & store 
        //        };

        //        channel.BasicConsume(queue: queueName,
        //                             autoAck: true,
        //                             consumer: consumer);
        //    }
        //}

        //public static void RecieveStockUpdates(Guid stockId)
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.ExchangeDeclare(exchange: "trade_stocks",
        //                                type: "direct");

        //        var queueName = channel.QueueDeclare().QueueName;

        //        channel.QueueBind(queue: queueName,
        //                              exchange: "trade_stocks",
        //                              routingKey: stockId.ToString());

        //        Console.WriteLine(" [*] Waiting for messages.");

        //        var consumer = new EventingBasicConsumer(channel);
        //        consumer.Received += (model, ea) =>
        //        {
        //            var body = ea.Body;
        //            var message = Encoding.UTF8.GetString(body.ToArray());
        //            var routingKey = ea.RoutingKey;
        //            Console.WriteLine(" [x] Received '{0}':'{1}'",
        //                              routingKey, JsonSerializer.Deserialize<StockUpdate>(message));
        //        };

        //        //update repository
        //        channel.BasicConsume(queue: queueName,
        //                             autoAck: true,
        //                             consumer: consumer);
        //    }
        //}
    }
}

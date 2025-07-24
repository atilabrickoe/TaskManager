using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagerDomain.Interfaces;

namespace Repository.Messaging
{
    public class NotificationService : INotificationService
    {
        //private readonly IConnection _connection;
        //private readonly IModel _channel;
        private readonly string _queueName = "task_notifications";

        public NotificationService()
        {
        }

        public async Task NotifyUserAsync(Guid userId, string message)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false);
            var payload = JsonSerializer.Serialize(new { userId, message });
            var body = Encoding.UTF8.GetBytes(payload);
            await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
            //return Task.CompletedTask;
        }
    }
}

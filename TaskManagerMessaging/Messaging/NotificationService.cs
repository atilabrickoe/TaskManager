using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TaskManagerDomain.Interfaces;

namespace TaskManagerMessaging.Messaging
{
    public class NotificationService : INotificationService
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _logger = logger;

            var rabbitSection = configuration.GetSection("RabbitMQ");
            var hostName = rabbitSection.GetSection("HostName").Value ?? "localhost";
            _queueName = rabbitSection.GetSection("QueueName").Value ?? "TaskManagerQueue";

            _factory = new ConnectionFactory { HostName = hostName };
        }

        public async Task NotifyUserAsync(NotificationMessageDto notification)
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var payload = JsonSerializer.Serialize(notification);
                var body = Encoding.UTF8.GetBytes(payload);

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _queueName,
                    body: body);

                _logger.LogInformation("Message published to queue {Queue}", _queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending message to RabbitMQ.");
            }
        }
    }
}

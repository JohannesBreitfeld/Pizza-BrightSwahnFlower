using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace PizzaMail
{
    public class RabbitMqInitializer : IHostedService
    {
        private readonly ILogger<RabbitMqInitializer> _logger;
        private readonly IConfiguration _config;

        public RabbitMqInitializer(ILogger<RabbitMqInitializer> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initializing RabbitMQ…");
            Console.WriteLine("Initializing RabbitMQ…");

            string uri = _config["RABBITMQ_CONNECTION"];
            var factory = new ConnectionFactory() { Uri = new Uri(uri) };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            string queueName = "orders.email.queue";
            string exchangeName = "orders.exchange";
            string routingKey = "order.created";

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, durable: true);
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queueName, exchangeName, routingKey);

            _logger.LogInformation("RabbitMQ queue initialized.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

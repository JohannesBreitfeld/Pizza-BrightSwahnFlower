using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PizzaMail.Contracts;
using RabbitMQ.Client;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PizzaMail
{
    public class EmailFunction
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RabbitMqInitializer> _logger;
        private readonly IConfiguration _configuration;

        public EmailFunction(IHttpClientFactory httpClientFactory, ILogger<RabbitMqInitializer> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("MailerSend");
            _logger = logger;
            _configuration = configuration;
        }

        [Function("RabbitMqEmailFunction")]
        public async Task Run([RabbitMQTrigger("orders.email.queue", ConnectionStringSetting = "RABBITMQ_CONNECTION")] byte[] message)
        {
            string json = Encoding.UTF8.GetString(message);
            var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            if (orderCreatedEvent is null)
            {
                _logger.LogError("Couldn't deserialize json, {json}", json);
            }
            else
            {
                string orderItemsWithQuantity = string.Join(", ", orderCreatedEvent.Data.OrderItems.Select(oi =>
                {
                    var unit = oi.Quantity > 1 ? "pcs" : "pc";
                    return $"{oi.ProductName}: {oi.Quantity}{unit}";
                }));
                int totalNumberOfItems = orderCreatedEvent.Data.OrderItems.Sum(oi => oi.Quantity);
                decimal orderTotal = orderCreatedEvent.Data.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);

                var requestBody = new
                {
                    from = new { email = _configuration["MAILERSEND_FROM_EMAIL"] },
                    to = new[] { new { email = orderCreatedEvent.Data.CustomerEmail } },
                    subject = "Order confirmation",
                    text = $"We got your order for {orderItemsWithQuantity}.\nTotal number of items: {totalNumberOfItems}\nSum: {orderTotal.ToString("C", CultureInfo.GetCultureInfo("sv-SE"))}"
                };

                _logger.LogInformation($"Attempting to send email for order {orderCreatedEvent.Data.OrderId} to {orderCreatedEvent.Data.CustomerEmail}", orderCreatedEvent.Data.OrderId);

                var response = await _httpClient.PostAsJsonAsync("v1/email", requestBody);
            }
        }
    }
}

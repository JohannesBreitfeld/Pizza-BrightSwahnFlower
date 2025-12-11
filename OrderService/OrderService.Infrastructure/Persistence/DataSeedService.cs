using OrderService.Core.Contracts;
using OrderService.Core.Domain;

namespace OrderService.Infrastructure.Persistence;

public static class DataSeedService
{
    public static async Task SeedOrdersAsync(IOrderRepository orderRepository)
    {
        var random = new Random();

        var sampleOrders = Enumerable.Range(1, 5).Select(i =>
        {
            var orderId = Guid.NewGuid().ToString();

            return new Order
            {
                Id = orderId,
                OrderId = orderId,
                CustomerEmail = $"customer{i}@example.com",
                CreatedAt = DateTime.UtcNow.AddMinutes(-i * 5),

                Items = new List<OrderItem>
            {
                    new OrderItem
                    {
                        ProductId = Guid.NewGuid().ToString(),
                        ProductName = $"Sample Product {i}",
                        Quantity = random.Next(1, 4),
                        UnitPrice = random.Next(50, 300)
                    }
            }
            };
        }).ToList();

        foreach (var order in sampleOrders)
        {
            order.TotalAmount = order.Items.Sum(x => x.UnitPrice * x.Quantity);
            order.TaxAmount = Math.Round(order.TotalAmount * 0.25m, 2);
        }

        foreach (var order in sampleOrders)
        {
            await orderRepository.CreateAsync(order);
        }
    }
}



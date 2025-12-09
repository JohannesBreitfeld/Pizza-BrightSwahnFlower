using OrderService.Core.Domain;

namespace OrderService.Core.Events;

public class OrderCreatedEvent
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public string EventType { get; set; } = "Created";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public OrderCreatedEventData Data { get; set; } = null!;
}

public class OrderCreatedEventData
{
    public string OrderId { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}

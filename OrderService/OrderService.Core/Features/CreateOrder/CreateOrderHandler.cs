using System.Text.Json;
using MediatR;
using OrderService.Core.Contracts;
using OrderService.Core.Domain;
using OrderService.Core.Events;

namespace OrderService.Core.Features.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOutboxRepository _outboxRepository;

    public CreateOrderHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var orderItems = request.Items.Select(item => new OrderItem
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        var totalAmount = orderItems.Sum(item => item.Quantity * item.UnitPrice);
        var taxAmount = totalAmount * 0.25m;
        var order = new Order
        {
            Id = orderId, 
            OrderId = orderId, 
            CustomerEmail = request.CustomerEmail,
            Items = orderItems,
            TotalAmount = totalAmount,
            TaxAmount = taxAmount,
            CreatedAt = DateTime.UtcNow
        };

        var orderCreatedEvent = new OrderCreatedEvent
        {
            Data = new OrderCreatedEventData
            {
                OrderId = orderId,
                CustomerEmail = request.CustomerEmail,
                TotalAmount = totalAmount,
                TaxAmount = taxAmount,
                OrderItems = orderItems
            }
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = orderId, 
            EventType = orderCreatedEvent.EventType,
            Payload = JsonSerializer.Serialize(orderCreatedEvent),
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false,
            RetryCount = 0
        };

        await _outboxRepository.CreateWithOrderAsync(order, outboxMessage, cancellationToken);

        return new CreateOrderResponse
        {
            OrderId = orderId,
            CustomerEmail = request.CustomerEmail,
            TotalAmount = totalAmount,
            TaxAmount = taxAmount,
            CreatedAt = order.CreatedAt
        };
    }
}

using OrderService.Core.Domain;

namespace OrderService.Core.Contracts;

public interface IOutboxRepository
{
    Task CreateWithOrderAsync(Order order, OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 10, CancellationToken cancellationToken = default);
    Task MarkAsProcessedAsync(string id, string orderId, CancellationToken cancellationToken = default);
    Task IncrementRetryCountAsync(string id, string orderId, CancellationToken cancellationToken = default);
}

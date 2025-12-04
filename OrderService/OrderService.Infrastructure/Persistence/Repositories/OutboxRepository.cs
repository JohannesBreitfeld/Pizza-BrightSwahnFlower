using Microsoft.Azure.Cosmos;
using OrderService.Core.Contracts;
using OrderService.Core.Domain;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly Container _container;

    public OutboxRepository(CosmosDbService cosmosDbService)
    {
        _container = cosmosDbService.Container;
    }

    public async Task CreateWithOrderAsync(Order order, OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        // Create a transactional batch to ensure both documents are created atomically
        var batch = _container.CreateTransactionalBatch(new PartitionKey(order.OrderId));

        batch.CreateItem(order);
        batch.CreateItem(outboxMessage);

        var response = await batch.ExecuteAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create order and outbox message. Status: {response.StatusCode}");
        }
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new QueryDefinition(
            "SELECT * FROM c WHERE c.type = @type AND c.isProcessed = @isProcessed ORDER BY c.createdAt OFFSET 0 LIMIT @batchSize")
            .WithParameter("@type", "OutboxMessage")
            .WithParameter("@isProcessed", false)
            .WithParameter("@batchSize", batchSize);

        var iterator = _container.GetItemQueryIterator<OutboxMessage>(query);
        var messages = new List<OutboxMessage>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            messages.AddRange(response);
        }

        return messages;
    }

    public async Task MarkAsProcessedAsync(string id, string orderId, CancellationToken cancellationToken = default)
    {
        var message = await _container.ReadItemAsync<OutboxMessage>(
            id,
            new PartitionKey(orderId),
            cancellationToken: cancellationToken);

        message.Resource.IsProcessed = true;
        message.Resource.ProcessedAt = DateTime.UtcNow;

        await _container.ReplaceItemAsync(
            message.Resource,
            id,
            new PartitionKey(orderId),
            cancellationToken: cancellationToken);
    }

    public async Task IncrementRetryCountAsync(string id, string orderId, CancellationToken cancellationToken = default)
    {
        var message = await _container.ReadItemAsync<OutboxMessage>(
            id,
            new PartitionKey(orderId),
            cancellationToken: cancellationToken);

        message.Resource.RetryCount++;

        await _container.ReplaceItemAsync(
            message.Resource,
            id,
            new PartitionKey(orderId),
            cancellationToken: cancellationToken);
    }
}

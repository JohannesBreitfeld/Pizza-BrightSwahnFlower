using Microsoft.Azure.Cosmos;
using OrderService.Core.Contracts;
using OrderService.Core.Domain;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly Container _container;

    public OrderRepository(CosmosDbService cosmosDbService)
    {
        _container = cosmosDbService.Container;
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var response = await _container.CreateItemAsync(
            order,
            new PartitionKey(order.OrderId),
            cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = 'Order'");
        var iterator = _container.GetItemQueryIterator<Order>(query);
        var results = new List<Order>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<Order>> GetAllPagedAsync(int skip = 0, int take = 20, CancellationToken cancellationToken = default)
    {
        var query = new QueryDefinition(
      "SELECT * FROM c WHERE c.type = 'Order' OFFSET @skip LIMIT @take")
            .WithParameter("@skip", skip)
            .WithParameter("@take", take);
        var iterator = _container.GetItemQueryIterator<Order>(query);
        var results = new List<Order>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }
}

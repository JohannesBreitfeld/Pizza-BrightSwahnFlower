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
}

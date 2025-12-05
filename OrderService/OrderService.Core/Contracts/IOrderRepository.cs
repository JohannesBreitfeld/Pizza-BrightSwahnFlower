using OrderService.Core.Domain;

namespace OrderService.Core.Contracts;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default);
}

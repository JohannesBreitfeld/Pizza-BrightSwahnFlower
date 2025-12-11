using OrderService.Core.Domain;

namespace OrderService.Core.Contracts;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetAllPagedAsync(int skip = 0, int take = 20, CancellationToken cancellationToken = default);
}

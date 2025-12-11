using MediatR;
using OrderService.Core.Contracts;
using OrderService.Core.Domain;
using OrderService.Core.Mapping;

namespace OrderService.Core.Features.GetAllOrders;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, GetAllOrdersResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetAllOrdersResponse> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders = Enumerable.Empty<Order>();

        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            var skip = (request.Page.Value - 1) * request.PageSize.Value;
            orders = await _orderRepository.GetAllPagedAsync(skip, request.PageSize.Value);
        }
        else
        {
            orders = await _orderRepository.GetAllAsync();
        }
        var ordersResponse = orders.ToResponse();

        return new GetAllOrdersResponse(ordersResponse);
    }
}

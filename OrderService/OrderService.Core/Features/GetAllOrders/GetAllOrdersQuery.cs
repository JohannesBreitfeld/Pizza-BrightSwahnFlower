using MediatR;

namespace OrderService.Core.Features.GetAllOrders;

public sealed record GetAllOrdersQuery : IRequest<GetAllOrdersResponse>
{
    public GetAllOrdersQuery(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int? Page { get; init; } = null;
    public int? PageSize { get; init; } = null;

}

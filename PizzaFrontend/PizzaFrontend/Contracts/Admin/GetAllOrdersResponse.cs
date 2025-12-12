namespace PizzaFrontend.Contracts.Admin;

public sealed record GetAllOrdersResponse(List<OrderDTO> Orders);

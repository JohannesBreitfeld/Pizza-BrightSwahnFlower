using PizzaFrontend.Contracts.Orders;

namespace PizzaFrontend.Interfaces
{
    public interface IOrderClient
    {
        Task<HttpResponseMessage> PlaceOrderAsync(CreateOrderRequest request);
    }
}

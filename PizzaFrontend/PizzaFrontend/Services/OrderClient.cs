using PizzaFrontend.Contracts.Orders;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services
{
    public class OrderClient(HttpClient client) : IOrderClient
    {
        public async Task<HttpResponseMessage> PlaceOrderAsync(CreateOrderRequest request)
        {
            return await client.PostAsJsonAsync("", request);
        }
    }
}

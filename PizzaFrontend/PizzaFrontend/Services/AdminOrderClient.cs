using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services;

public class AdminOrderClient(HttpClient client) : IAdminOrderClient
{
    public async Task<HttpResponseMessage> GetAllOrdersAsync(int page = 1, int pageSize = 20)
    {
        return await client.GetAsync($"?page={page}&pageSize={pageSize}");
    }

    public async Task<HttpResponseMessage> GetOrderByIdAsync(string orderId)
    {
        return await client.GetAsync($"{orderId}");
    }
}

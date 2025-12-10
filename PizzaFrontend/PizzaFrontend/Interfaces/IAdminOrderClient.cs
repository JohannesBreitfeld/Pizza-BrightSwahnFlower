namespace PizzaFrontend.Interfaces;

public interface IAdminOrderClient
{
    Task<HttpResponseMessage> GetAllOrdersAsync(int page = 1, int pageSize = 20);
    Task<HttpResponseMessage> GetOrderByIdAsync(string orderId);
}

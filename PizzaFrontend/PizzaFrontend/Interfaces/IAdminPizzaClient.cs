using PizzaFrontend.Contracts.Admin;
using PizzaFrontend.Contracts.Pizzas;

namespace PizzaFrontend.Interfaces;

public interface IAdminPizzaClient
{
    Task<HttpResponseMessage> GetAllPizzasAsync();
    Task<HttpResponseMessage> GetPizzaByIdAsync(int id);
    Task<HttpResponseMessage> CreatePizzaAsync(CreatePizzaRequest request);
    Task<HttpResponseMessage> UpdatePizzaAsync(int id, UpdatePizzaRequest request);
    Task<HttpResponseMessage> DeletePizzaAsync(int id);
    Task<HttpResponseMessage> GetAllIngredientsAsync();
}

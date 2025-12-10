using PizzaFrontend.Contracts.Admin;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services;

public class AdminPizzaClient(HttpClient client) : IAdminPizzaClient
{
    public async Task<HttpResponseMessage> GetAllPizzasAsync()
    {
        return await client.GetAsync("");
    }

    public async Task<HttpResponseMessage> GetPizzaByIdAsync(int id)
    {
        return await client.GetAsync($"{id}");
    }

    public async Task<HttpResponseMessage> CreatePizzaAsync(CreatePizzaRequest request)
    {
        return await client.PostAsJsonAsync("", request);
    }

    public async Task<HttpResponseMessage> UpdatePizzaAsync(int id, UpdatePizzaRequest request)
    {
        return await client.PutAsJsonAsync($"{id}", request);
    }

    public async Task<HttpResponseMessage> DeletePizzaAsync(int id)
    {
        return await client.DeleteAsync($"{id}");
    }

    public async Task<HttpResponseMessage> GetAllIngredientsAsync()
    {
        return await client.GetAsync("ingredients");
    }
}

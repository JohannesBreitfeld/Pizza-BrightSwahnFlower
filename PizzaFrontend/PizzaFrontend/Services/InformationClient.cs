using PizzaFrontend.Contracts.Pizzas;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services
{
    public class InformationClient(HttpClient client) : IInformationClient
    {
        public async Task<IEnumerable<PizzaDTO>> GetPizzasAsync()
        {
            var pizzas = await client.GetFromJsonAsync<IEnumerable<PizzaDTO>>("");
            return pizzas ?? Enumerable.Empty<PizzaDTO>();
        }
    }
}

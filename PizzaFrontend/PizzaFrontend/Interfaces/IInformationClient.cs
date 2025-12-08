using PizzaFrontend.Contracts.Pizzas;

namespace PizzaFrontend.Interfaces
{
    public interface IInformationClient
    {
        Task<IEnumerable<PizzaDTO>> GetPizzasAsync();
    }
}

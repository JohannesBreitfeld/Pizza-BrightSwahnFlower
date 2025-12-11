using PizzaInformationService.Domain.Entities;

namespace PizzaInformationService.Domain.Interfaces
{
    public interface IPizzaInformationRepository
    {
        Task<List<Pizza>> GetAllPizzaAsync(CancellationToken ct = default);
        Task<Pizza?> GetPizzaByIdAsync(int id, CancellationToken ct = default);
        Task<Pizza> CreatePizzaAsync(Pizza pizza, CancellationToken ct = default);
    }
}

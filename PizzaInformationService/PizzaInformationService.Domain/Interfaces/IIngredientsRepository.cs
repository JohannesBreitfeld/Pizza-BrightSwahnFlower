using PizzaInformationService.Domain.Entities;

namespace PizzaInformationService.Domain.Interfaces
{
    public interface IIngredientsRepository
    {
        Task<List<Ingredient>> GetAllIngredientsAsync(CancellationToken ct = default);
        Task<Ingredient?> GetIngredientByIdAsync(int id, CancellationToken ct = default);
        Task<List<Ingredient>> GetIngredientsByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}

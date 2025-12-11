using Microsoft.EntityFrameworkCore;
using PizzaInformationService.Domain.Entities;
using PizzaInformationService.Domain.Interfaces;
using PizzaInformationService.Infrastructure.Persistance;

namespace PizzaInformationService.Infrastructure.Repositories
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly PizzaInformationDbContext _context;

        public IngredientsRepository(PizzaInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync(CancellationToken ct = default)
        {
            return await _context.Ingredients.AsNoTracking().ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);
        }

        public async Task<List<Ingredient>> GetIngredientsByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
                return await _context.Ingredients.Where(i => ids.Contains(i.Id)).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PizzaInformationService.Domain.Entities;
using PizzaInformationService.Domain.Interfaces;
using PizzaInformationService.Infrastructure.Persistance;

namespace PizzaInformationService.Infrastructure.Repositories
{
    public class PizzaInformationRepository : IPizzaInformationRepository
    {
        private readonly PizzaInformationDbContext _context;
        public PizzaInformationRepository(PizzaInformationDbContext context)
        {
            _context = context;
        }

        public async Task<Pizza> CreatePizzaAsync(Pizza pizza, CancellationToken ct = default)
        {
            var created = await _context.Pizzas.AddAsync(pizza);

            await _context.SaveChangesAsync(ct);

            return created.Entity;
        }

        public async Task<List<Pizza>> GetAllPizzaAsync(CancellationToken ct = default)
        {
            return await _context.Pizzas.AsNoTracking().Include(p => p.Ingredients).ToListAsync(ct);
        }

        public async Task<Pizza?> GetPizzaByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Pizzas.AsNoTracking().Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.Id == id, ct);
        }
    }
}

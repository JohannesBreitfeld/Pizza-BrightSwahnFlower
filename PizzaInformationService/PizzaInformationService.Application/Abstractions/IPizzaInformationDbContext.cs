using PizzaInformationService.Domain.Entities;

namespace PizzaInformationService.Application.Abstractions
{
    public interface IPizzaInformationDbContext
    {
        IQueryable<Domain.Entities.Pizza> Pizzas { get; }
        IQueryable<Ingredient> Ingredients { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

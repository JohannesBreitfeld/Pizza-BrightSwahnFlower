using Microsoft.EntityFrameworkCore;
using PizzaInformationService.Domain.Entities;
using PizzaInformationService.Infrastructure.Persistance;
using PizzaInformationService.Infrastructure.Repositories;

namespace PizzaInformationService.Test.Repositories
{
    public class PizzaInformationRepositoryTest
    {
        private readonly PizzaInformationDbContext _context;
        private readonly PizzaInformationRepository _repository;

        public PizzaInformationRepositoryTest()
        {
            // Create a unique in-memory database for this test class
            var options = new DbContextOptionsBuilder<PizzaInformationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PizzaInformationDbContext(options);

            // Seed some data
            _context.Pizzas.AddRange(
                new Pizza { Id = 1, Name = "Margherita", Price = 10, ImageUrl = "test" },
                new Pizza { Id = 2, Name = "Pepperoni", Price = 12, ImageUrl = "test" }
            );
            _context.SaveChanges();

            _repository = new PizzaInformationRepository(_context);
        }

        [Fact]
        public async Task GetAllPizza_ShouldContainPizzas()
        {
            var result = await _repository.GetAllPizzaAsync();

            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetPizzaById_ShouldReturnPizza()
        {
            var result = await _repository.GetPizzaByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPizzaById_ShouldReturnNull_WhenPizzaNotFound()
        {
            var result = await _repository.GetPizzaByIdAsync(999);

            Assert.Null(result);
        }
    }
}

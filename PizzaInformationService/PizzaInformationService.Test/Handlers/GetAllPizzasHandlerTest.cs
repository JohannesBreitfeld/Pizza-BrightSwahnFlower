using Moq;
using PizzaInformationService.Application.Pizza.GetAllPizzas;
using PizzaInformationService.Domain.Entities;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Test.Handlers
{
    public class GetAllPizzasHandlerTest
    {
        [Fact]
        public async Task GetAllPizzas_ShouldReturnPizzas()
        {
            // Arrange
            var mockRepository = new Mock<IPizzaInformationRepository>();
            mockRepository.Setup(repo => repo.GetAllPizzaAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Pizza>
                          {
                              new Pizza { Id = 1, Name = "Margherita", ImageUrl="test" },
                              new Pizza { Id = 2, Name = "Pepperoni", ImageUrl="test" }
                          });
            var handler = new GetAllPizzasHandler(mockRepository.Object);
            var request = new GetAllPizzasQuery();
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async Task GetAllPizzas_ShouldCallRepositoryOnce()
        {
            // Arrange
            var mockRepository = new Mock<IPizzaInformationRepository>();
            var handler = new GetAllPizzasHandler(mockRepository.Object);
            var request = new GetAllPizzasQuery();
            // Act
            await handler.Handle(request, CancellationToken.None);
            // Assert
            mockRepository.Verify(repo => repo.GetAllPizzaAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

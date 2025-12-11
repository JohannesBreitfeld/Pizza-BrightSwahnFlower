using Moq;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Entities;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Test.Handlers
{
    public class GetPizzaByIdHandlertest
    {

        [Fact]
        public async Task GetPizzaById_ShouldReturnPizza()
        {
            // Arrange
            var mockRepository = new Mock<IPizzaInformationRepository>();
            mockRepository.Setup(repo => repo.GetPizzaByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new Pizza { Id = 1, Name = "Margherita", ImageUrl = "test" });
            var handler = new GetPizzaByIdHandler(mockRepository.Object);
            var request = new GetPizzaByIdQuery(1);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Margherita", result.Name);
        }

        [Fact]
        public async Task GetPizzaById_ShouldReturnNull_WhenPizzaNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IPizzaInformationRepository>();
            mockRepository.Setup(repo => repo.GetPizzaByIdAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Pizza?)null);
            var handler = new GetPizzaByIdHandler(mockRepository.Object);
            var request = new GetPizzaByIdQuery(999);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetPizzaById_ShouldCallRepositoryOnce()
        {
            // Arrange
            var mockRepository = new Mock<IPizzaInformationRepository>();
            var handler = new GetPizzaByIdHandler(mockRepository.Object);
            var request = new GetPizzaByIdQuery(1);
            // Act
            await handler.Handle(request, CancellationToken.None);
            // Assert
            mockRepository.Verify(repo => repo.GetPizzaByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

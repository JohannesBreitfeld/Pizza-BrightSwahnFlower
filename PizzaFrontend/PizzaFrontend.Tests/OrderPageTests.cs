using Moq;
using MudBlazor;
using PizzaFrontend.Contracts.Orders;
using PizzaFrontend.Contracts.Pizzas;
using PizzaFrontend.Interfaces;
using PizzaFrontend.Models;
using PizzaFrontend.Tests.TestableClasses;
using System.Net;
using System.Net.Http.Json;

namespace PizzaFrontend.Tests
{
    public class OrderPageTests
    {
        [Fact]
        public void SelectPizza_MarksSelectedCorrectly()
        {
            var page = new TestableOrderPage();
            var pizza1 = new Pizza { Name = "Margherita" };
            var pizza2 = new Pizza { Name = "Pepperoni" };

            page.SelectPizza(pizza1);
            page.SelectPizza(pizza2);

            Assert.False(pizza1.IsSelected);
            Assert.True(pizza2.IsSelected);
        }

        [Fact]
        public void OrderButtonIsDisabled_WhenNoSelection_ReturnsTrue()
        {
            var page = new TestableOrderPage();
            Assert.True(page.OrderButtonIsDisabled);
        }

        [Fact]
        public async Task OnAfterRenderAsync_LoadsPizzas_WhenFirstRender()
        {
            var mockApi = new Mock<IInformationClient>();
            var fakeSnackBar = new FakeSnackbar();

            var pizzaDtos = new List<PizzaDTO>
        {
            new PizzaDTO(1, "Margherita", new List<IngredientDTO> { new IngredientDTO(1, "Cheese")}, "image1.png", 99),
            new PizzaDTO(2, "Kangaroo", new List<IngredientDTO> { new IngredientDTO(1, "Cheese")}, "image2.png", 129),
        };

            mockApi.Setup(x => x.GetPizzasAsync()).ReturnsAsync(pizzaDtos);

            var page = new TestableOrderPage
            {
                InformationApi = mockApi.Object,
                Snackbar = fakeSnackBar
            };

            await page.CallOnAfterRenderAsync(firstRender: true);

            Assert.False(page.IsLoadingPizzas);
            Assert.Equal(2, page.Pizzas.Count);
            Assert.Equal("Margherita", page.Pizzas[0].Name);
            Assert.Equal("Kangaroo", page.Pizzas[1].Name);
            Assert.Empty(fakeSnackBar.Messages);
        }
        [Fact]
        public async Task OnAfterRenderAsync_ShowsErrorSnackbar_OnApiException()
        {
            var mockApi = new Mock<IInformationClient>();
            var fakeSnackBar = new FakeSnackbar();
            mockApi.Setup(x => x.GetPizzasAsync()).ThrowsAsync(new Exception("API Error"));
            var page = new TestableOrderPage
            {
                InformationApi = mockApi.Object,
                Snackbar = fakeSnackBar
            };

            await page.CallOnAfterRenderAsync(firstRender: true);

            Assert.False(page.IsLoadingPizzas);
            Assert.Empty(page.Pizzas);
            Assert.Single(fakeSnackBar.Messages);
            Assert.Equal("Couldn't retrieve pizzas, no connection to server", fakeSnackBar.Messages[0].Message);
            Assert.Equal(Severity.Error, fakeSnackBar.Messages[0].Severity);
        }

        [Fact]
        public async Task PlaceOrderAsync_Success_ShowsSuccessSnackbar()
        {
            // Arrange
            var mockOrderApi = new Mock<IOrderClient>();
            var fakeSnackbar = new FakeSnackbar();

            var responseContent = new CreateOrderResponse("12345", "test@example.com", 100, 1, DateTime.Now);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(responseContent)
            };

            mockOrderApi
                .Setup(x => x.PlaceOrderAsync(It.IsAny<CreateOrderRequest>()))
                .ReturnsAsync(httpResponse);

            var page = new TestableOrderPage
            {
                OrderApi = mockOrderApi.Object,
                Snackbar = fakeSnackbar
            };

            page.SetEmail("test@example.com");
            page.SetSelectedPizza(new Pizza
            {
                Id = 1,
                Name = "Test Pizza",
                Price = 100
            });

            // Act
            await page.PlaceOrderAsync();

            // Assert
            Assert.Single(fakeSnackbar.Messages);
            Assert.Equal($"Order {responseContent.OrderId} successful", fakeSnackbar.Messages[0].Message);
            Assert.Equal(Severity.Normal, fakeSnackbar.Messages[0].Severity);
        }

        [Fact]
        public async Task PlaceOrderAsync_Failure_ShowsErrorSnackbar()
        {
            // Arrange
            var mockOrderApi = new Mock<IOrderClient>();
            var fakeSnackbar = new FakeSnackbar();

            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            mockOrderApi
                .Setup(x => x.PlaceOrderAsync(It.IsAny<CreateOrderRequest>()))
                .ReturnsAsync(httpResponse);

            var page = new TestableOrderPage
            {
                OrderApi = mockOrderApi.Object,
                Snackbar = fakeSnackbar
            };

            page.SetEmail("test@example.com");
            page.SetSelectedPizza(new Pizza
            {
                Id = 1,
                Name = "Test Pizza",
                Price = 100
            });

            // Act
            await page.PlaceOrderAsync();

            // Assert
            Assert.Single(fakeSnackbar.Messages);
            Assert.Equal("Order failed", fakeSnackbar.Messages[0].Message);
            Assert.Equal(Severity.Error, fakeSnackbar.Messages[0].Severity);
        }
    }
}

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PizzaFrontend.Contracts.Orders;
using PizzaFrontend.Interfaces;
using PizzaFrontend.Models;

namespace PizzaFrontend.Components.Pages.LandingPage
{
    public partial class OrderPage : ComponentBase
    {
        [Inject]
        protected internal virtual IOrderClient OrderApi { get; set; } = default!;
        [Inject]
        protected internal virtual IInformationClient InformationApi { get; set; } = default!;

        [Inject]
        protected internal virtual ISnackbar Snackbar { get; set; } = default!;

        protected Pizza? _selectedPizza;
        protected bool _isLoading = true;
        protected string _email = string.Empty;
        protected bool _orderButtonIsDisabled => _selectedPizza is null || string.IsNullOrEmpty(_email);
        protected List<Pizza> _pizzas = [];

        protected virtual void RefreshUI()
        {
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                var pizzaDTOs = await InformationApi.GetPizzasAsync();

                _pizzas = pizzaDTOs.Select(dto => new Pizza
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Ingredients = dto.Ingredients.Select(i => new Ingredient
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList(),
                    ImageUrl = dto.ImageUrl
                }).ToList();
            }
            catch
            {
                Snackbar.Add("Couldn't retrieve pizzas, no connection to server", Severity.Error);
            }

            _isLoading = false;
            RefreshUI();
        }

        public void SelectPizza(Pizza pizza)
        {
            if (_selectedPizza is not null)
            {
                _selectedPizza.IsSelected = false;
            }

            pizza.IsSelected = true;
            _selectedPizza = pizza;
            RefreshUI();
        }

        public async Task PlaceOrderAsync()
        {
            var pizzas = new List<OrderItemDTO>()
        {
            new OrderItemDTO(_selectedPizza!.Id.ToString(), _selectedPizza.Name, 1, _selectedPizza.Price)
        };
            var request = new CreateOrderRequest(_email, pizzas);

            try
            {
                var result = await OrderApi.PlaceOrderAsync(request);

                if (result.IsSuccessStatusCode)
                {
                    var orderResponse = await result.Content.ReadFromJsonAsync<CreateOrderResponse>();

                    if (orderResponse is not null)
                    {
                        Snackbar.Add($"Order {orderResponse.OrderId} successful");
                    }
                }
                else
                {
                    Snackbar.Add("Order failed", Severity.Error);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"Order failed: {e.Message}", Severity.Error);
            }

        }
    }
}

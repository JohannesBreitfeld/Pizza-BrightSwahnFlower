using PizzaFrontend.Components.Pages.LandingPage;
using PizzaFrontend.Models;

namespace PizzaFrontend.Tests.TestableClasses
{
    
    public class TestableOrderPage : OrderPage
    {
        public void SetSelectedPizza(Pizza pizza) => _selectedPizza = pizza;
        public void SetEmail(string email) => _email = email;
        public List<Pizza> Pizzas => _pizzas;
        public bool OrderButtonIsDisabled => _orderButtonIsDisabled;
        public bool IsLoadingPizzas => _isLoading;
        protected override void RefreshUI()
        {
        }
        public async Task CallOnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}

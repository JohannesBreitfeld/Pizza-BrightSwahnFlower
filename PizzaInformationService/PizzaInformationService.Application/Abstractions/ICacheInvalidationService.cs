namespace PizzaInformationService.Application.Abstractions
{
    public interface ICacheInvalidationService
    {
        Task InvalidatePizzaCacheAsync();
        Task InvalidateIngredientsCacheAsync();
    }
}

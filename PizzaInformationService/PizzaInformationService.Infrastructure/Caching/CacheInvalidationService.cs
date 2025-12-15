using PizzaInformationService.Application.Abstractions;

namespace PizzaInformationService.Infrastructure.Caching
{
    public class CacheInvalidationService : ICacheInvalidationService
    {
        private readonly ICacheService _cache;

        public CacheInvalidationService(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task InvalidatePizzaCacheAsync()
        {
            await _cache.RemoveAsync(CacheKeys.AllPizzas);
        }

        public async Task InvalidateIngredientsCacheAsync()
        {
            await _cache.RemoveAsync(CacheKeys.AllIngredients);
        }
    }

}

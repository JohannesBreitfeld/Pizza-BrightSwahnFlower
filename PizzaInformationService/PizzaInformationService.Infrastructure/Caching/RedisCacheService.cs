using Microsoft.Extensions.Caching.Distributed;
using PizzaInformationService.Application.Abstractions;
using System.Text.Json;

namespace PizzaInformationService.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T?> GetAsync<T>(string key)
        {

            try
            {
                var data = await _cache.GetStringAsync(key);

                if (string.IsNullOrEmpty(data)) return default;

                var serializedObject = JsonSerializer.Deserialize<T>(data);

                return serializedObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"REDIS EXCEPTION: {ex}");
                throw;
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);
        }
    }
}

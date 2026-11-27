// The Cache-Aside pattern (also known as lazy loading) is a caching strategy where your application code manages both the cache and the database directly, loading data into the cache only on demand.

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

public sealed record Product(int Id, string Name, decimal Price);

public class ProductService(IDistributedCache cache, IProductRepository repository)
{
    public async Task<Product?> GetProductAsync(int id, CancellationToken ct = default)
    {
        string cacheKey = $"product:{id}";

        // 1. Check the cache first
        string? cachedJson = await cache.GetStringAsync(cacheKey, ct);
        if (!string.IsNullOrEmpty(cachedJson))
        {
            return JsonSerializer.Deserialize<Product>(cachedJson);
        }

        // 2. Cache miss: fetch from the primary database
        Product? product = await repository.GetByIdAsync(id, ct);
        if (product is null)
        {
            return null;
        }

        // 3. Populate the cache with an expiration policy (TTL)
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        };

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), options, ct);

        return product;
    }

    public async Task UpdateProductAsync(Product product, CancellationToken ct = default)
    {
        // 1. Update the database (source of truth)
        await repository.UpdateAsync(product, ct);

        // 2. Invalidate the cache entry so the next read fetches fresh data
        string cacheKey = $"product:{product.Id}";
        await cache.RemoveAsync(cacheKey, ct);
    }
}
// If you are on .NET 9 or newer, Microsoft's HybridCache implements the Cache-Aside pattern out of the box—handling serialization, two-tier caching (in-memory + Redis), and cache stampede protection automatically:
using Microsoft.Extensions.Caching.Hybrid;

public class ModernProductService(HybridCache cache, IProductRepository repository)
{
    public async Task<Product?> GetProductAsync(int id, CancellationToken ct = default)
    {
        return await cache.GetOrCreateAsync(
            $"product:{id}",
            async cancel => await repository.GetByIdAsync(id, cancel),
            cancellationToken: ct
        );
    }
}


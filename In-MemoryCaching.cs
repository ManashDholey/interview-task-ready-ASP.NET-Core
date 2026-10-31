// To implement caching in ASP.NET Core, you choose a caching layer based on your infrastructure—ranging from a single-instance In-Memory Cache to a 
// multi-instance Distributed Cache (Redis), or the modern HybridCache introduced in .NET 9

// 1. In-Memory Caching (Best for Single Servers)

// This strategy stores data directly within the application's RAM. It is highly performant but disappears when the application restarts and is not shared across multi-server setups.
// Configuration (Program.cs)

var builder = WebApplication.CreateBuilder(args);

// Register the In-Memory cache service
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024; // Optional: Limits the total items/size to prevent memory leaks
}); 
using Microsoft.Extensions.Caching.Memory;

public class ProductService
{
    private readonly IMemoryCache _cache;
    private readonly DbContext _context;

    public ProductService(IMemoryCache cache, DbContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        string cacheKey = $"product-{id}";

        // Try to get the value from cache, if missing, fetch and save
        if (!_cache.TryGetValue(cacheKey, out Product? product))
        {
            product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)) // Fixed lifetime
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))  // Extends if accessed before timeout
                    .SetSize(1); // Required if SizeLimit is configured

                _cache.Set(cacheKey, product, cacheOptions);
            }
        }
        return product;
    }
}


// 2. Distributed Caching (Best for Cloud & Microservices)

// A distributed cache is hosted externally (e.g., using Redis or SQL Server). It ensures data remains consistent across multiple application instances.
// Configuration (Program.cs)

// Requires package: Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "MyApp_";
});

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public async Task<Product?> GetProductAsync(int id, IDistributedCache cache)
{
    string cacheKey = $"product-{id}";
    string? cachedData = await cache.GetStringAsync(cacheKey);

    if (!string.IsNullOrEmpty(cachedData))
    {
        return JsonSerializer.Deserialize<Product>(cachedData);
    }

    var product = await _context.Products.FindAsync(id);
    if (product != null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), options);
    }
    return product;
}

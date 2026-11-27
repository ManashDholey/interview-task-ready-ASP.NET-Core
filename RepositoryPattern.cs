// The Repository pattern in C# is a design pattern that creates an abstraction layer between your application's business logic and its data access layer (such as Entity Framework Core, Dapper, or an external API).

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    void Add(Order order);
    void Remove(Order order);
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public void Add(Order order) => _dbContext.Orders.Add(order);

    public void Remove(Order order) => _dbContext.Orders.Remove(order);
}

// builder.Services.AddScoped<IOrderRepository, OrderRepository>();





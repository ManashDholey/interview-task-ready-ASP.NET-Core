1. Clean Architecture – Basic Interview Questions
// Q1. What is Clean Architecture?
// Answer:

// Clean Architecture is a software architectural design pattern introduced by Robert C. Martin (Uncle Bob).

// It organizes an application into different layers, keeping business logic independent of external technologies such 
// as databases, frameworks, APIs, and UI.

// The main objective is to build applications that are:
// 1. Maintainable.
// 2. Testable.
// 3. Scalable.
// 4. Flexible.
// 5. Independent of frameworks.
// 6. Independent of databases.
// 7. Easy to modify and extend.
// The four commonly used layers are:
// - Domain Layer
// - Application Layer
// - Infrastructure Layer
// - Presentation Layer
// The most important principle is the Dependency Rule: dependencies must point inward, toward the core business logic.
// Q2. Why do we need Clean Architecture?
// Answer:

// In traditional applications, business logic, database queries, API controllers, and validation may all exist in the same 
// project.

// This creates tight coupling.

// For example:
// - Changing SQL Server to PostgreSQL becomes difficult.
// - Unit testing business logic requires database access.
// - Controllers become too large.
// - Business rules get duplicated.
// - Maintenance becomes complicated.
// Clean Architecture solves these problems by separating responsibilities.

// Real-world example:

// Suppose we have an e-commerce application.

// Today, we use SQL Server. Tomorrow, the business decides to move to PostgreSQL.

// With Clean Architecture, we can replace the Infrastructure implementation without changing the core business rules or application use cases.
// Q3. What are the main principles of Clean Architecture?
// Answer:

// The important principles are:
// Principle	Explanation
// Separation of Concerns	Each layer has a specific responsibility
// Dependency Inversion	High-level modules don't depend on low-level implementations
// Single Responsibility	A class should have one primary responsibility
// Dependency Rule	Dependencies point inward
// Framework Independence	Business logic doesn't depend on frameworks
// Database Independence	Business logic doesn't depend on a particular database
// Testability	Business logic can be tested independently

// Q4. What are the four layers of Clean Architecture?
// Answer:

// 1. Domain Layer

// Contains core business logic.

// Examples:
// - Entities
// - Value Objects
// - Domain Events
// - Business Rules
// - Domain Services
// - Domain Exceptions
// 2. Application Layer

// Contains application use cases and coordinates business operations.

// Examples:
// - Commands
// - Queries
// - Handlers
// - DTOs
// - Interfaces
// - Application Services
// - Validation
// 3. Infrastructure Layer

// Contains external technology implementations.

// Examples:
// - Entity Framework Core
// - SQL Server
// - Repository Implementations
// - External API Integrations
// - Email Services
// - File Storage
// - Redis
// 4. Presentation Layer

// Contains the user-facing entry point.

// Examples:
// - ASP.NET Core Web API
// - Controllers
// - Middleware
// - Authentication
// - Authorization
// - API Request/Response Models
// Q5. Explain the Dependency Rule in Clean Architecture.
// Answer:

// The Dependency Rule states that source-code dependencies must point toward the inner layers.

// For example:
// - Presentation can depend on Application.
// - Infrastructure can depend on Application and Domain.
// - Application can depend on Domain.
// - Domain should not depend on Application, Infrastructure, or Presentation.
// The Domain Layer is the innermost layer.







// milanjovanovic.tech


// The important point is that Infrastructure implements interfaces defined by the inner layers rather than the inner layers depending on Infrastructure implementations.
// Q6. Is Clean Architecture the same as Domain-Driven Design (DDD)?
// Answer:

// No. They are related but different concepts.

// Clean Architecture focuses on structuring software and managing dependencies.

// Domain-Driven Design (DDD) focuses on understanding and modeling complex business domains.

// For example:

// In an e-commerce application:
// - DDD helps define Customer, Order, Product, Payment, and business rules.
// - Clean Architecture determines where these models and business rules belong.
// We can use DDD with Clean Architecture, but DDD is not mandatory.
// Q7. What is the difference between Clean Architecture and Layered Architecture?
// Answer:
// Layered Architecture	Clean Architecture
// Organizes code into layers	Organizes code around business rules and use cases
// Often uses top-to-bottom dependencies	Uses inward dependencies
// Business logic may depend on infrastructure	Business logic is independent of infrastructure
// Database may be central	Business logic is central
// Testing can be difficult	Supports isolated testing
// Can create tight coupling	Encourages loose coupling

// Interview tip: Traditional layered architecture often follows Presentation → Business Logic → Data Access. Clean Architecture applies Dependency Inversion to ensure infrastructure details do not control business logic.

// 2. Domain Layer – Interview Questions
// Q8. What is the Domain Layer?
// Answer:

// The Domain Layer is the core of the application.

// It contains business entities and rules that represent the actual business requirements.

// It should not depend on:
// - ASP.NET Core
// - Entity Framework Core
// - SQL Server
// - MediatR
// - External APIs
// - Infrastructure implementations
// The Domain Layer should contain pure business logic as much as practical.
// Q9. What are Entities?
// Answer:

// An Entity is an object that has a unique identity and represents a business concept.

// For example, consider a Customer entity.
public class Customer
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public Customer(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        Id = 0;
        Name = name;
        Email = email;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        Email = email;
    }
}


// Even if the Customer's name changes, its identity remains the same.
// Q10. What is a Value Object?
// Answer:

// A Value Object is an object defined by its values rather than a unique identity.

// For example:
// - Address
// - Money
// - Email Address
// - Coordinates
// - Date Range
// Example:
// public record Address(
//     string Street,
//     string City,
//     string Country,
//     string PostalCode);


// Two Address objects with identical values can be considered equal.

// Entity vs Value Object:
// Entity	Value Object
// Has unique identity	No independent identity
// Identity-based equality	Value-based equality
// Can change over time	Preferably immutable
// Example: Customer	Example: Address

// Q11. What is a Domain Service?
// Answer:

// A Domain Service contains business logic that does not naturally belong to a single entity or value object.

// Example:
public class PricingService
{
    public decimal CalculateDiscount(
        decimal amount,
        decimal discountPercentage)
    {
        return amount * discountPercentage / 100;
    }
}


// A Domain Service is useful when a business operation involves multiple domain objects or represents a domain concept that does not fit naturally inside one entity.
// Q12. What is a Domain Event?
// Answer:
// 
// A Domain Event represents something important that happened in the business domain.

// Examples:
// - OrderPlaced
// - PaymentCompleted
// - CustomerRegistered
// - ProductStockUpdated
// Example:
public record OrderPlacedEvent(
    int OrderId,
    DateTime OccurredOn);


// Domain Events help separate business operations.

// For example, after an order is placed:
// 1. Save the order.
// 2. Update inventory.
// 3. Send an email.
// 4. Notify the shipping service.
// These operations can be coordinated through event handlers.

// Important: A domain event is not automatically the same as an integration event. Integration events communicate business changes across system boundaries.
// Q13. What is an Aggregate Root?
// Answer:

// An Aggregate Root is the main entity through which an aggregate is accessed and modified.

// For example:

// An Order aggregate may contain:
// - Order
// - OrderItems
// - ShippingAddress
// The Order entity acts as the Aggregate Root.

// Other objects should not be modified independently if doing so would violate aggregate business rules.
// Q14. Should we put Entity Framework Core models inside the Domain Layer?
// Answer:

// In strict Clean Architecture, the Domain Layer should not depend on Entity Framework Core.

// Domain entities should represent business concepts, not database structures.

// Entity Framework Core mappings can be defined in Infrastructure using Fluent API.

// Example:
public class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(
        EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}


//This keeps database-specific configurations outside the Domain Layer.

// 3. Application Layer – Interview Questions
// Q15. What is the Application Layer?
// Answer:

// The Application Layer coordinates the business use cases of the application.

// It defines what the application can do.

// Examples:
// - Create Customer
// - Update Customer
// - Delete Customer
// - Place Order
// - Process Payment
// - Get Customer Details
// It contains:
// - Commands
// - Queries
// - Handlers
// - DTOs
// - Application Interfaces
// - Validators
// - Application Services
// It depends on Domain but should not depend on Infrastructure implementations.
// Q16. What are Commands and Queries?
// Answer:

// Commands and Queries are commonly used in CQRS.

// Command: Performs an operation that changes application state.

// Examples:
// - CreateCustomerCommand
// - UpdateCustomerCommand
// - DeleteCustomerCommand
// Query: Retrieves information without changing application state.

// Examples:
// - GetCustomerByIdQuery
// - GetAllCustomersQuery
// - GetOrderDetailsQuery
// Command	Query
// Changes state	Reads data
// Usually returns success or an identifier	Returns requested data
// Example: CreateOrder	Example: GetOrder
// Can trigger domain events	Should not change business state

// Q17. What is CQRS?
// Answer:

// CQRS stands for Command Query Responsibility Segregation.

// It separates operations that modify data from operations that read data.

// For example:
// CreateCustomerCommand
//        |
//        v
// CreateCustomerHandler
//        |
//        v
// Customer Repository
//        |
//        v
// Database


// For reading:
// GetCustomerQuery
//        |
//        v
// GetCustomerHandler
//        |
//        v
// Database
//        |
//        v
// Customer DTO


// Benefits:
// - Clear separation of read/write operations.
// - Better organization of use cases.
// - Independent optimization of queries.
// - Easier maintenance.
// CQRS does not necessarily require separate databases. It can be implemented using a single SQL Server database.
// Q18. What is MediatR?
// Answer:

MediatR is a .NET library that implements the Mediator pattern.

It allows objects to communicate through a mediator instead of calling each other directly.

In Clean Architecture, it is often used to dispatch commands and queries to their handlers.

Example:
public record CreateCustomerCommand(
    string Name,
    string Email) : IRequest<int>;


Handler:
public class CreateCustomerHandler
    : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerHandler(
        ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = new Customer(
            request.Name,
            request.Email);

        await _repository.AddAsync(
            customer,
            cancellationToken);

        return customer.Id;
    }
}


Controller:
[HttpPost]
public async Task<IActionResult> Create(
    CreateCustomerCommand command)
{
    var id = await _mediator.Send(command);

    return Ok(id);
}


The controller does not need to know how the customer is created.

// Note: MediatR is an optional library, not a mandatory part of Clean Architecture.
// Q19. What is the difference between a Command Handler and a Query Handler?
// Answer:

// A Command Handler processes commands and changes application state.

// A Query Handler retrieves information.

// Example:
public class CreateOrderHandler
    : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Validate business operation
        // Create order
        // Save order

        return 1;
    }
}


// Query example:
public class GetOrderHandler
    : IRequestHandler<GetOrderQuery, OrderDto>
{
    public async Task<OrderDto> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken)
    {
        // Retrieve order
        // Return DTO

        return new OrderDto();
    }
}


// Q20. What are DTOs?
// Answer:

// DTO stands for Data Transfer Object.

// DTOs transfer data between different layers or system boundaries.

// Example:
public class CustomerDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}


// Why do we use DTOs?
// 1. Avoid exposing internal domain entities.
// 2. Control which fields are returned.
// 3. Avoid over-posting.
// 4. Separate API contracts from database models.
// 5. Improve security and maintainability.
// Q21. What is AutoMapper?
// Answer:

// AutoMapper is a .NET object-to-object mapping library.

// It converts one object type into another.

//Example:
public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerRequest, Customer>();
    }
}


//Usage:
var customerDto = _mapper.Map<CustomerDto>(customer);


// It reduces repetitive mapping code.

// However, for complex domain logic, explicit mapping is often clearer.
// Q22. What is the difference between DTO, Entity, and ViewModel?
// DTO	Entity	ViewModel
// Transfers data	Represents business concepts	Represents UI requirements
// Used across boundaries	Used in Domain	Used in presentation
// Usually no business behavior	Contains business behavior	Contains display-specific data
// Example: CustomerDto	Customer	CustomerViewModel

// Q23. Where should validation be implemented?
// Answer:

// Validation can happen at multiple levels.

// Presentation Layer:
// - Required request fields.
// - Input format.
// - Request structure.
// Application Layer:
// - Use-case validation.
// - Customer existence.
// - Duplicate email checks.
// - Request-specific rules.
// Domain Layer:
// - Core business invariants.
// - Entity state validation.
// - Rules that must always remain true.
// Example using FluentValidation:
public class CreateCustomerValidator
    : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}


//Remember: A validator cannot replace domain invariants. Critical business rules must remain protected even when the application handler is bypassed.

//4. Infrastructure Layer – Interview Questions
// Q24. What is the Infrastructure Layer?
// Answer:

// Infrastructure contains implementations of external technical requirements.

// Examples:
// - SQL Server
// - Entity Framework Core
// - Repository Implementations
// - Redis
// - SMTP
// - Azure Blob Storage
// - External REST APIs
// - Message Brokers
// It implements abstractions defined by the Application or Domain layers.
// Q25. What is the Repository Pattern?
// Answer:

// The Repository Pattern provides an abstraction for accessing and persisting domain objects.

// Instead of allowing application use cases to depend directly on database implementation details, we define an interface.

// Example:

// Domain or Application abstraction:
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken);
}


// Infrastructure implementation:
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return _context.Customers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(
            customer,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}


// Q26. What is the Unit of Work Pattern?
// Answer:

// Unit of Work coordinates multiple database operations within a single transaction.

// For example:

// When placing an order:
// 1. Create Order.
// 2. Add Order Items.
// 3. Update Product Stock.
// 4. Save all changes together.
// If an operation fails, the transaction can be rolled back.

// Entity Framework Core's DbContext already provides Unit of Work-like behavior through change tracking and SaveChanges.
//
// Therefore, creating another Unit of Work abstraction around every EF Core operation is not always necessary.
// Q27. What is DbContext in Entity Framework Core?
// Answer:

// DbContext represents a session with the database.

// It is responsible for:
// - Querying data.
// - Tracking entities.
// - Saving changes.
// - Managing entity relationships.
// - Coordinating database operations.
Example:
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}


// Q28. Should DbContext be registered as Singleton or Scoped?
// Answer:

// DbContext should normally be registered as Scoped in ASP.NET Core.
// services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(connectionString));


// Why Scoped?
// - One instance per HTTP request by default.
// - Supports a consistent change-tracking session.
// - Avoids sharing the same context across concurrent requests.
// - Prevents thread-safety issues associated with sharing a context.
// DbContext is not thread-safe.
// Q29. What is the difference between Repository Pattern and DbContext?
// Answer:
Repository	DbContext
Abstraction for data access	EF Core database session
Hides persistence implementation	Provides ORM functionality
Can encapsulate domain-specific queries	Provides LINQ queries and tracking
Optional abstraction	EF Core central component

// A repository is not always necessary when working with EF Core. Some teams use DbContext directly in application handlers, especially for simpler applications and read queries.
// Q30. How do you handle external API integrations in Clean Architecture?
// Answer:

// Define an interface in the Application Layer and implement it in Infrastructure.

// Application:
public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(
        decimal amount,
        CancellationToken cancellationToken);
}


// Infrastructure:
public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ProcessPaymentAsync(
        decimal amount,
        CancellationToken cancellationToken)
    {
        // Call external payment API
        return true;
    }
}


// Register it using:
services.AddHttpClient<IPaymentService, PaymentService>();



// 5. Presentation Layer – Interview Questions
// Q31. What is the Presentation Layer?
// Answer:

// The Presentation Layer is responsible for communicating with users or external clients.

// In ASP.NET Core, it typically contains:
// - Controllers
// - API Endpoints
// - Request Models
// - Response Models
// - Middleware
// - Authentication
// - Authorization
// - API Configuration
// It should not contain complex business logic.
// Q32. What is the responsibility of a Controller?
// Answer:

// A controller receives HTTP requests and delegates the work to the Application Layer.

// Example:
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ISender _sender;

    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetCustomerByIdQuery(id),
            cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}


// A controller should generally be thin.
// Q33. What is Middleware in ASP.NET Core?
// Answer:

///Middleware is a component that processes HTTP requests and responses in the ASP.NET Core pipeline.

Examples:
- Exception Handling
- Authentication
- Authorization
- Logging
- CORS
- Rate Limiting
Example:
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


The middleware order matters because requests pass through the configured pipeline.
Q34. Where should global exception handling be implemented?
Answer:

Global exception handling should generally be implemented in the Presentation Layer using ASP.NET Core middleware or the built-in exception-handling infrastructure.

Example:
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(
                new { message = "Internal server error" });
        }
    }
}


In production, prefer standardized Problem Details responses and ensure sensitive exception details are not exposed.

6. Dependency Injection – Important Interview Questions
Q35. What is Dependency Injection?
Answer:

Dependency Injection is a technique in which an object receives its dependencies from an external source rather than creating them itself.

Example without DI:
public class CustomerService
{
    private readonly CustomerRepository _repository =
        new CustomerRepository();
}


This creates tight coupling.

With DI:
public class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }
}


Now the class depends on an abstraction rather than a concrete implementation.
Q36. What are the three DI lifetimes in .NET?
Answer:
Lifetime	Meaning	Example
Transient	New instance every time requested	Stateless helper
Scoped	One instance per scope/request	DbContext
Singleton	One instance for the application lifetime	Thread-safe cache

Example:
services.AddTransient<IEmailService, EmailService>();

services.AddScoped<ICustomerRepository, CustomerRepository>();

services.AddSingleton<ICacheService, CacheService>();


Q37. What is the Dependency Inversion Principle?
Answer:

Dependency Inversion Principle is one of the SOLID principles.

It states:
1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
2. Abstractions should not depend on details. Details should depend on abstractions.
Example:

Application defines:
public interface IEmailService
{
    Task SendAsync(string email);
}


Infrastructure implements:
public class SmtpEmailService : IEmailService
{
    public Task SendAsync(string email)
    {
        // SMTP implementation
        return Task.CompletedTask;
    }
}


Application does not need to know how SMTP works.
Q38. Where should Dependency Injection registrations happen?
Answer:

A common approach is to organize registrations inside each project.

Infrastructure:
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Default")));

        services.AddScoped<ICustomerRepository,
            CustomerRepository>();

        return services;
    }
}


Application:
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);

        return services;
    }
}


Presentation / Program.cs:
builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);


This keeps project registrations organized.

7. Project Structure – Real-World .NET Example
Q39. How do you structure a Clean Architecture project in ASP.NET Core?
Answer:

A typical solution structure looks like this:
MyECommerce.sln

MyECommerce.Domain

Core

Entities/

ValueObjects/

Enums/

Events/

Exceptions/


MyECommerce.Application

Features/

  Customers/Commands/

  Customers/Queries/

  Orders/Commands/

Interfaces/

DTOs/

Behaviors/


MyECommerce.Infrastructure

Persistence/

Repositories/

Configurations/

Services/

Migrations/


MyECommerce.WebAPI

Controllers/

Middleware/

Extensions/

Program.cs

appsettings.json



Project references:
Project	References
Domain	None
Application	Domain
Infrastructure	Application, Domain
WebAPI	Application, Infrastructure

This is a common implementation, not the only valid structure.
Q40. Why should Domain not reference Infrastructure?
Answer:

Because Domain contains the core business rules.

If Domain references Infrastructure, it becomes dependent on external technology.

For example, if Domain directly uses:
using Microsoft.EntityFrameworkCore;


then business logic becomes coupled to EF Core.

This violates the Dependency Rule.

Instead, Domain or Application defines the abstraction, and Infrastructure implements it.
Q41. Can Application Layer reference Infrastructure Layer?
Answer:

In strict Clean Architecture, Application should not reference Infrastructure.

Application defines interfaces, and Infrastructure implements them.

The composition root, usually the Web API project, connects the implementations using Dependency Injection.
Q42. Can Infrastructure Layer reference Domain Layer?
Answer:

Yes.

Infrastructure needs to work with domain entities and implement interfaces defined by the inner layers.

For example:
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
}


The implementation belongs to Infrastructure, while the interface belongs to an inner layer.

8. Testing – Interview Questions
Q43. Why is Clean Architecture easier to test?
Answer:

Because business logic is separated from infrastructure and external dependencies.

We can test:
- Domain entities without databases.
- Application handlers using mocked dependencies.
- Infrastructure repositories with integration tests.
- Controllers using API integration tests.
Q44. What are Unit Tests?
Answer:

Unit Tests verify a small unit of functionality independently.

Example:
[Fact]
public void Customer_Should_Have_Valid_Name()
{
    var customer = new Customer(
        "John",
        "john@example.com");

    Assert.Equal("John", customer.Name);
}


Q45. What are Integration Tests?
Answer:

Integration Tests verify whether multiple components work together.

For example:
- Web API + Application Layer.
- Repository + EF Core.
- EF Core + Database.
- External API integration.
ASP.NET Core supports integration testing through WebApplicationFactory.
Q46. Should we mock DbContext in unit tests?
Answer:

Generally, avoid mocking EF Core's DbContext and IQueryable behavior for repository tests.

Better approaches:
- Mock repository interfaces when testing application handlers.
- Test repository behavior using a real relational database or a suitable test database.
- Use WebApplicationFactory for API integration tests.
EF Core's InMemory provider is not a full replacement for relational database testing because its behavior differs from relational providers.

9. Onion Architecture – Interview Questions
Q47. What is Onion Architecture?
Answer:

Onion Architecture is a software architecture pattern introduced by Jeffrey Palermo.

It organizes the application into concentric layers around the Domain Model.

The domain is at the center, and infrastructure and presentation are on the outside.

Its primary goal is to keep business logic independent of external technologies.







pereiren.medium.com


Q48. What are the layers of Onion Architecture?
Answer:

A common Onion Architecture structure includes:
1. Domain Model Layer.
2. Domain Services Layer.
3. Application Services Layer.
4. Infrastructure Layer.
5. Presentation Layer.
The exact naming and grouping can differ between implementations.
Q49. What is the difference between Onion Architecture and Clean Architecture?
Answer:

Both architectures prioritize business logic and dependency inversion.
Onion Architecture	Clean Architecture
Organizes around concentric rings	Organizes around concentric layers
Domain model is central	Entities and business rules are central
Focuses on dependency direction	Explicitly describes the Dependency Rule
Infrastructure stays outside	Infrastructure stays outside
Often uses Domain Services and Application Services	Often uses Use Cases and Application Services

In practical .NET development, their implementations can look almost identical.
Q50. What is the difference between Domain Services and Application Services?
Answer:

Domain Services contain business rules that do not naturally belong to a single entity.

Application Services coordinate use cases and orchestrate operations.

Example:
- Domain Service: Calculate complex order pricing.
- Application Service: Create order, call pricing service, persist order, and publish events.

10. N-Tier / Layered Architecture – Interview Questions
Q51. What is N-Tier Architecture?
Answer:

N-Tier Architecture is an architectural style that divides an application into multiple tiers.

A typical ASP.NET Core application has:
1. Presentation Tier.
2. Business Logic Tier.
3. Data Access Tier.
4. Database Tier.
The purpose is to separate responsibilities and make applications easier to maintain.







felix-pappe.medium.com


Q52. What is the difference between N-Tier and N-Layer Architecture?
Answer:

These terms are often used interchangeably, but technically they are different.

N-Layer: Logical separation of application responsibilities in code.

N-Tier: Physical or deployment separation into different runtime tiers or servers.

Example:

An application may have:
- Presentation Layer.
- Business Layer.
- Data Access Layer.
All three may run inside one deployed application.

That is layered architecture, not necessarily three-tier deployment.
Q53. What are the common layers in N-Tier Architecture?
Answer:

Presentation Layer:
Handles HTTP requests and responses.

Business Logic Layer:
Contains business rules and workflows.

Data Access Layer:
Communicates with the database.

Database Layer:
Stores application data.
Q54. What are the advantages of N-Tier Architecture?
Answer:
- Separation of responsibilities.
- Easier maintenance.
- Reusability.
- Better organization.
- Independent deployment possibilities.
- Improved security boundaries.
- Easier team collaboration.
Q55. What are the disadvantages of N-Tier Architecture?
Answer:
- More complexity.
- Additional communication overhead if physically distributed.
- More configuration.
- Possible performance overhead.
- Risk of creating unnecessary layers.
- Changes may require modifications across multiple layers.
Q56. Explain a typical N-Tier project structure in .NET.
MyApplication
│
├── MyApplication.Presentation
│   ├── Controllers
│   └── Program.cs
│
├── MyApplication.Business
│   ├── Services
│   └── Interfaces
│
├── MyApplication.DataAccess
│   ├── Repositories
│   └── AppDbContext
│
└── MyApplication.Database
    └── SQL Server


Q57. What is the difference between N-Tier and Clean Architecture?
Answer:

N-Tier usually separates an application by technical responsibility.

Clean Architecture separates business rules and use cases from infrastructure and enforces inward dependency direction.

For example:

In traditional N-Tier, Business Logic may depend directly on a Data Access implementation.

In Clean Architecture, Application defines an interface and Infrastructure implements it.
Q58. Can N-Tier Architecture and Clean Architecture be used together?
Answer:

Yes.

N-Tier provides separation of responsibilities and deployment boundaries, while Clean Architecture provides dependency rules.

A solution can have logical layers, physical deployment tiers, and Clean Architecture dependency principles simultaneously.

11. Important Architecture Comparison Questions
Q59. Compare N-Tier, Onion, and Clean Architecture.
Feature	N-Tier	Onion	Clean
Main focus	Separation into tiers	Domain-centered design	Business-rule independence
Core	Business Logic	Domain Model	Entities and Use Cases
Dependency direction	Varies	Inward	Inward
Infrastructure dependency	May be direct	Abstracted	Abstracted
Testability	Depends on implementation	High	High
Domain independence	Not guaranteed	Yes	Yes
Typical use	Enterprise applications	Domain-centric systems	Complex business applications

Q60. Which architecture is best for ASP.NET Core applications?
Answer:

There is no universally best architecture.

It depends on:
- Business complexity.
- Team size.
- Project lifetime.
- Maintenance requirements.
- Deployment requirements.
- Performance requirements.
For a small CRUD application, simple layered architecture may be sufficient.

For applications with complex business rules, Clean Architecture or Onion Architecture may provide better separation.

For distributed enterprise systems, modular architecture or microservices may be appropriate when justified by business and operational needs.

12. Advanced and Senior-Level Interview Questions
Q61. What is the difference between Clean Architecture and Microservices?
Answer:

Clean Architecture describes the internal structure and dependencies of an application.

Microservices describe how a system is divided into independently deployable services.

For example:

An e-commerce system may have:
- Customer Service.
- Order Service.
- Payment Service.
- Inventory Service.
Each service can independently use Clean Architecture internally.

They solve different problems.
Q62. Can we use Clean Architecture in a Monolithic Application?
Answer:

Yes.

Clean Architecture does not require microservices.

A monolithic application can have separate Domain, Application, Infrastructure, and Presentation projects.

A modular monolith is also a good approach for many enterprise systems.
Q63. What is a Modular Monolith?
Answer:

A Modular Monolith is a single deployable application divided into independent business modules.

Example:
ECommerce
│
├── Customers
├── Orders
├── Payments
├── Inventory
└── Shipping


Each module has its own responsibilities and boundaries.

Benefits:
- Easier deployment than microservices.
- Lower operational complexity.
- Better separation of business functionality.
- Can evolve into services if necessary.
Q64. What is the difference between Modular Monolith and Microservices?
Modular Monolith	Microservices
Single deployment	Independent deployments
Usually one application process	Multiple services/processes
Easier transactions	Distributed transaction challenges
Simpler operations	More operational complexity
Module communication can be in-process	Often uses APIs or messaging

Q65. What is Vertical Slice Architecture?
Answer:

Vertical Slice Architecture organizes code by business feature or use case rather than only by technical layer.

Example:
Features
│
├── Customers
│   ├── CreateCustomer
│   │   ├── Command.cs
│   │   ├── Handler.cs
│   │   └── Validator.cs
│   │
│   └── GetCustomer
│       ├── Query.cs
│       └── Handler.cs
│
└── Orders
    ├── CreateOrder
    └── GetOrder


It reduces the need to navigate through multiple technical folders when implementing one feature.
Q66. What is the difference between Clean Architecture and Vertical Slice Architecture?
Answer:

Clean Architecture focuses on dependency direction and separation of concerns.

Vertical Slice Architecture focuses on organizing code around individual features.

They can be combined.

For example, a Clean Architecture solution can use feature-based folders inside its Application Layer.
Q67. What is the Outbox Pattern?
Answer:

The Outbox Pattern solves the problem of reliably publishing messages after database changes.

Suppose we create an order and publish an OrderCreated message.

If the database save succeeds but message publishing fails, another service may never receive the event.

The Outbox Pattern stores the event in an Outbox table within the same database transaction as the business operation.

A background worker later publishes the event.

This improves reliability.
Q68. What is the difference between Domain Events and Integration Events?
Answer:

Domain Events communicate something that happened inside a domain boundary.

Integration Events communicate something that happened to other systems or services.
Domain Event	Integration Event
Internal business event	Cross-system event
Usually in-process	Usually asynchronous messaging
Example: OrderPlaced	Example: OrderCreatedIntegrationEvent
Domain-centric	System communication

Q69. What is the Specification Pattern?
Answer:

The Specification Pattern encapsulates reusable business query or validation criteria.

Example:
public class ActiveCustomerSpecification
{
    public bool IsSatisfiedBy(Customer customer)
    {
        return customer.IsActive;
    }
}


It can help reuse complex business conditions without duplicating logic.
Q70. What is the difference between a Domain Model and an Anemic Domain Model?
Answer:

A Rich Domain Model contains data and business behavior together.

An Anemic Domain Model primarily contains properties, while business logic exists in separate service classes.

Rich Domain Model:
public class Order
{
    public decimal TotalAmount { get; private set; }

    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException();

        TotalAmount -= TotalAmount * percentage / 100;
    }
}


Anemic Domain Model:
public class Order
{
    public decimal TotalAmount { get; set; }
}


Business behavior is implemented elsewhere.

Neither style is automatically wrong, but a rich domain model is often valuable when business rules are complex.

13. Scenario-Based Real-World Interview Questions
These questions are particularly important for senior .NET interviews.
Q71. Suppose your application currently uses SQL Server, but the business wants to migrate to PostgreSQL. How would Clean Architecture help?
Answer:

I would keep database-specific code in Infrastructure.

I would:
1. Keep Domain entities independent of EF Core.
2. Keep Application interfaces independent of database implementation.
3. Replace the SQL Server EF Core provider with PostgreSQL.
4. Update database configurations and migrations.
5. Verify repository implementations.
6. Run integration tests.
7. Validate data migration.
The Domain and Application use cases should require minimal or no changes.
Q72. Your controller contains 500 lines of business logic. How would you improve it?
Answer:

I would move business operations out of the controller.

I would:
1. Create separate use cases.
2. Move business rules into Domain entities or Domain Services.
3. Move orchestration into Application handlers.
4. Introduce DTOs.
5. Use Dependency Injection.
6. Add unit tests.
The controller should only handle HTTP concerns and delegate operations.
Q73. Your application has duplicate validation logic in multiple places. How would you solve it?
Answer:

I would identify the type of validation.
- Request validation belongs in Presentation.
- Use-case validation belongs in Application.
- Business invariants belong in Domain.
I would use FluentValidation for application request validation and enforce important business rules inside domain objects.
Q74. How would you design an Order Management System using Clean Architecture?
Answer:

I would create the following Domain entities:
- Customer
- Order
- OrderItem
- Product
- Payment
Application use cases:
- CreateOrderCommand.
- GetOrderQuery.
- CancelOrderCommand.
- ProcessPaymentCommand.
Infrastructure:
- AppDbContext.
- OrderRepository.
- PaymentGateway.
- EmailService.
Presentation:
- OrdersController.
- PaymentsController.
- CustomersController.
The order creation workflow would be:
HTTP Request
     |
     v
OrdersController
     |
     v
CreateOrderCommand
     |
     v
CreateOrderHandler
     |
     v
Validate Order
     |
     v
Domain Business Rules
     |
     v
Order Repository
     |
     v
SQL Server
     |
     v
HTTP Response


Q75. How would you handle payment failure after an order is created?
Answer:

I would design a clear order and payment state machine.

For example:
Pending Payment
       |
       v
Payment Processing
       |
   +---+---+
   |       |
   v       v
Paid    Failed
   |       |
   v       v
Confirm  Retry / Cancel
Order


For distributed payment workflows, I would consider:
- Idempotency keys.
- Retry policies.
- Timeouts.
- Compensation.
- Saga Pattern.
- Outbox Pattern.
- Payment reconciliation.
I would never assume that an external payment API call and a local database transaction form one atomic transaction.
Q76. What happens if Infrastructure is unavailable?
Answer:

I would implement:
- Proper exception handling.
- Timeouts.
- Retry policies for transient failures.
- Circuit Breaker Pattern where appropriate.
- Logging and monitoring.
- Graceful failure responses.
For external HTTP calls, .NET's resilience capabilities can be used to configure retries, timeouts, and circuit breakers.

Retries should be applied carefully to avoid duplicating non-idempotent operations.
Q77. How would you improve a slow ASP.NET Core API?
Answer:

I would investigate before changing architecture.

My approach:
1. Check application logs.
2. Analyze SQL queries.
3. Check database indexes.
4. Use pagination.
5. Avoid N+1 queries.
6. Use AsNoTracking() for read-only EF Core queries.
7. Use asynchronous I/O.
8. Introduce caching where appropriate.
9. Reduce unnecessary object mapping.
10. Profile CPU and memory usage.
I would use measurements rather than assume the architecture is the performance bottleneck.
Q78. How do you prevent circular dependencies between projects?
Answer:

I would review project references and enforce the dependency direction.

For example:
Domain
  ↑
Application
  ↑
Infrastructure

WebAPI → Application
WebAPI → Infrastructure


More precisely, Application and Infrastructure both reference Domain, while Infrastructure also references Application for its abstractions.

If two projects require each other, I would extract the shared abstraction into an appropriate inner layer or redesign the boundary.
Q79. How would you secure a Clean Architecture Web API?
Answer:

I would implement:
- JWT authentication or another appropriate identity solution.
- Role-based authorization.
- Policy-based authorization.
- Input validation.
- HTTPS.
- Secrets management.
- Rate limiting.
- Secure error handling.
- SQL injection protection through parameterized queries.
- Proper logging without exposing sensitive information.
Authentication and authorization infrastructure generally belong in the Presentation and supporting Infrastructure concerns, while application use cases enforce relevant authorization requirements.
Q80. How would you handle transactions across multiple repositories?
Answer:

I would ensure all repositories use the same scoped DbContext when they participate in the same database transaction.

I can use EF Core transactions:
await using var transaction =
    await dbContext.Database.BeginTransactionAsync();

try
{
    // Perform multiple operations

    await dbContext.SaveChangesAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}


For multiple independent services or databases, I would consider Saga and Outbox patterns rather than relying on a local database transaction.

14. Coding Questions Frequently Asked in Interviews
Q81. Implement a Repository Pattern in ASP.NET Core.
public interface IRepository<T>
    where T : class
{
    Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<List<T>> GetAllAsync(
        CancellationToken cancellationToken);

    Task AddAsync(
        T entity,
        CancellationToken cancellationToken);

    void Update(T entity);

    void Delete(T entity);
}


Implementation:
public class Repository<T> : IRepository<T>
    where T : class
{
    protected readonly AppDbContext Context;

    public Repository(AppDbContext context)
    {
        Context = context;
    }

    public Task<T?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return Context.Set<T>()
            .FindAsync(new object[] { id }, cancellationToken)
            .AsTask();
    }

    public Task<List<T>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return Context.Set<T>()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        T entity,
        CancellationToken cancellationToken)
    {
        await Context.Set<T>().AddAsync(
            entity,
            cancellationToken);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}


Note that this repository does not call SaveChangesAsync on each operation. A unit-of-work boundary can coordinate persistence.
Q82. Implement Dependency Injection for Repository.
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));


Q83. Implement a Generic Service.
public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerAsync(
        int id,
        CancellationToken cancellationToken);
}


Implementation:
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(
        ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> GetCustomerAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (customer == null)
            return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }
}


Q84. How do you implement a clean exception-handling strategy?
Answer:

I would:
1. Define meaningful application or domain exceptions.
2. Use middleware for global exception handling.
3. Map expected exceptions to suitable HTTP status codes.
4. Return Problem Details.
5. Log unexpected errors.
6. Avoid returning stack traces to clients.
Q85. How do you implement pagination?
Answer:

Pagination should generally happen at the database level.
public async Task<List<Customer>> GetCustomersAsync(
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken)
{
    return await _context.Customers
        .AsNoTracking()
        .OrderBy(x => x.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
}


For large datasets and stable sequential navigation, keyset pagination can be more efficient than offset pagination.

15. Common Mistakes in Clean Architecture
Q86. What are the common mistakes developers make?
Answer:
Mistake	Correct approach
Putting all business logic in controllers	Move use cases into Application
Putting all logic in Application	Keep domain invariants in Domain
Domain references EF Core	Keep Domain independent
Exposing EF entities through APIs	Use DTOs
Application references Infrastructure	Depend on abstractions
Creating unnecessary generic repositories	Use abstractions where valuable
Overusing MediatR	Use it only when it improves organization
Too many interfaces	Introduce meaningful abstractions
Duplicate validation everywhere	Separate validation by responsibility
No integration testing	Test actual infrastructure boundaries
Using microservices unnecessarily	Start with a modular monolith when appropriate

Q87. Can Clean Architecture become overengineered?
Answer:

Yes.

For a small CRUD application, creating four projects, multiple repositories, generic services, factories, handlers, validators, and mapping layers for every operation can add unnecessary complexity.

The objective is not to create more layers.

The objective is to protect business logic and make the system easier to maintain.

16. Most Important Rapid-Fire Questions
These are useful for the final round of a .NET interview.
No.	Question	Short answer
1	What is Clean Architecture?	Architecture that isolates business rules from external dependencies
2	Who introduced it?	Robert C. Martin
3	What is Onion Architecture?	Domain-centered architecture with inward dependencies
4	Who introduced Onion Architecture?	Jeffrey Palermo
5	What is N-Tier?	Separation into logical or physical tiers
6	What is DIP?	Depend on abstractions, not implementations
7	What is a DTO?	Data Transfer Object
8	What is CQRS?	Separation of command and query operations
9	What is MediatR?	Mediator pattern library for .NET
10	What is a Domain Event?	An important business event
11	What is an Aggregate Root?	Entry point to an aggregate
12	What is a Value Object?	Object identified by its values
13	What is a Repository?	Abstraction for persistence operations
14	What is Unit of Work?	Coordinates persistence operations
15	What is DI?	Dependency Injection
16	What is IoC?	Inversion of Control
17	What is a Domain Service?	Business logic not belonging to one entity
18	What is an Application Service?	Coordinates a use case
19	What is Infrastructure?	External technology implementations
20	What is Vertical Slice?	Feature-oriented code organization
21	What is Modular Monolith?	One deployment with independent modules
22	What is Microservices?	Independently deployable services
23	What is Outbox Pattern?	Reliable event publishing using persisted messages
24	What is Saga Pattern?	Coordination of distributed transactions
25	What is FluentValidation?	A .NET validation library
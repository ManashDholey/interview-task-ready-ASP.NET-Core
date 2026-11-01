// In ASP.NET Core, the Map() extension method is used to create a separate branch in the middleware pipeline based on a specific URL path.

// It allows you to execute different middleware for different URL paths instead of executing the same middleware pipeline for every request.
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Middleware for all requests
app.Use(async (context, next) =>
{
    Console.WriteLine("Main Pipeline - Before");

    await next();

    Console.WriteLine("Main Pipeline - After");
});

// Create a separate branch for /admin
app.Map("/admin", adminApp =>
{
    adminApp.Run(async context =>
    {
        await context.Response.WriteAsync(
            "Welcome to Admin Section");
    });
});

// Create a separate branch for /api
app.Map("/api", apiApp =>
{
    apiApp.Run(async context =>
    {
        await context.Response.WriteAsync(
            "Welcome to API Section");
    });
});

// Default endpoint
app.Run(async context =>
{
    await context.Response.WriteAsync("Welcome to Home Page");
});

app.Run();
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
// Option 2: Factory-Based Approach (Using IMiddleware)If you want compile-time safety and cleaner DI capabilities, you can explicitly implement the IMiddleware interface from the Microsoft.AspNetCore.Http namespace.csharpusing Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class FactoryMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 1. Logic executed before the next middleware
        
        await next(context); // 2. Call next middleware
        
        // 3. Logic executed after the next middleware
    }
}

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "HTTP Request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        await _next(context);
    }
}

//Extension Method — MiddlewareExtensions.cs

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
//Register Middleware — Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRequestLogging();

app.MapControllers();

app.Run();

// ============================================================
//  ASP.NET Core - Complete Middleware Pipeline Example
//  All 10 middleware in the correct production order
// ============================================================

// ───────────────────────────────────────────
// Program.cs
// ───────────────────────────────────────────

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────
// SERVICE REGISTRATIONS
// ───────────────────────────────────────────

builder.Services.AddControllers();

// ── 6. CORS ──────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://myfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── 7. Authentication ────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(
                                               builder.Configuration["Jwt:Key"]!))
        };
    });

// ── 8. Authorization ─────────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    // Only authenticated users can access by default
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Custom role-based policy
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// ── 9. Custom Middlewares ─────────────────────────────────────
// (Registered as services if they have dependencies)
builder.Services.AddSingleton<RequestLoggingMiddleware>();
builder.Services.AddSingleton<RateLimitingMiddleware>();

var app = builder.Build();


// ============================================================
//  MIDDLEWARE PIPELINE  (ORDER MATTERS!)
// ============================================================

// ── 1. ExceptionHandler ──────────────────────────────────────
// Must be FIRST — wraps everything to catch unhandled exceptions.
if (app.Environment.IsDevelopment())
{
    // In development: shows detailed error page
    app.UseDeveloperExceptionPage();
}
else
{
    // In production: returns a clean error response
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode  = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                """{"error": "An unexpected error occurred. Please try again later."}""");
        });
    });
}

// ── 2. HSTS ──────────────────────────────────────────────────
// HTTP Strict Transport Security — tells browsers to always use HTTPS.
// Only enabled in production (browsers cache this aggressively).
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();   // Default max-age = 30 days
}

// ── 3. HttpsRedirection ──────────────────────────────────────
// Redirects all HTTP requests to HTTPS (301/307).
app.UseHttpsRedirection();

// ── 4. Static Files ──────────────────────────────────────────
// Serves files from wwwroot/ (CSS, JS, images, etc.)
// SHORT-CIRCUITS the pipeline — no auth check needed for public assets.
app.UseStaticFiles();

// Optional: serve files from a custom folder
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(builder.Environment.ContentRootPath, "MyAssets")),
//     RequestPath  = "/assets"
// });

// ── 5. Routing ───────────────────────────────────────────────
// Matches the incoming URL to a known endpoint.
// Must come BEFORE CORS, Auth, and Endpoints.
app.UseRouting();

// ── 6. CORS ──────────────────────────────────────────────────
// Handles cross-origin requests.
// Must be AFTER Routing (needs route info) and BEFORE Auth.
app.UseCors("AllowSpecificOrigin");

// ── 7. Authentication ────────────────────────────────────────
// Reads the request (e.g. JWT token) and identifies WHO the user is.
// Sets HttpContext.User with claims.
app.UseAuthentication();

// ── 8. Authorization ─────────────────────────────────────────
// Checks WHAT the identified user is allowed to do.
// Must come AFTER Authentication.
app.UseAuthorization();

// ── 9. Custom Middlewares ─────────────────────────────────────
// Your app-specific middleware runs AFTER all security is settled.
app.UseMiddleware<RequestLoggingMiddleware>();   // Custom1 — logs every request
app.UseMiddleware<RateLimitingMiddleware>();     // Custom2 — limits requests per IP

// ── 10. Endpoint ─────────────────────────────────────────────
// Maps controller routes / minimal API endpoints.
// Must be LAST — this is the actual handler that returns the response.
app.MapControllers();

// Minimal API endpoint example
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }))
   .AllowAnonymous();  // Override fallback policy for this route

app.Run();


// ============================================================
//  CUSTOM MIDDLEWARE CLASSES  (Middleware 9)
// ============================================================

// ── Custom1: Request Logging Middleware ─────────────────────
public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // -- Before the next middleware --
        _logger.LogInformation(
            "[{Time}] Incoming: {Method} {Path} | User: {User}",
            DateTime.UtcNow,
            context.Request.Method,
            context.Request.Path,
            context.User.Identity?.Name ?? "Anonymous");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        await next(context);  // Call the next middleware in the chain

        // -- After the next middleware (response phase) --
        stopwatch.Stop();
        _logger.LogInformation(
            "[{Time}] Response: {StatusCode} in {Elapsed}ms",
            DateTime.UtcNow,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}

// ── Custom2: Rate Limiting Middleware ────────────────────────
public class RateLimitingMiddleware : IMiddleware
{
    // Simple in-memory store: IP → request count per minute
    private static readonly Dictionary<string, (int Count, DateTime Window)> _cache = new();
    private const int MaxRequestsPerMinute = 60;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        lock (_cache)
        {
            if (_cache.TryGetValue(ip, out var entry))
            {
                // Reset window if older than 1 minute
                if (DateTime.UtcNow - entry.Window > TimeSpan.FromMinutes(1))
                    _cache[ip] = (1, DateTime.UtcNow);
                else
                    _cache[ip] = (entry.Count + 1, entry.Window);
            }
            else
            {
                _cache[ip] = (1, DateTime.UtcNow);
            }

            if (_cache[ip].Count > MaxRequestsPerMinute)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = "60";
                return; // Short-circuit — don't call next()
            }
        }

        await next(context);
    }
}

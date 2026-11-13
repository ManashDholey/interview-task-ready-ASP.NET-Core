using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

// Configure these values using a secure configuration provider (for example,
// user secrets or environment variables), not a committed production secret.
// {
//   "Jwt": {
//     "Secret": "a-secret-key-at-least-32-bytes-long",
//     "Issuer": "https://yourdomain.com",
//     "Audience": "https://yourdomain.com"
//   }
// }

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

// This middleware validates the bearer token and populates HttpContext.User.
// It must run before authorization evaluates endpoint policies.
app.UseJwtClaimsMiddleware();
app.UseAuthorization();

app.MapGet("/me", (HttpContext context) =>
{
    var claims = context.User.Claims.Select(claim => new
    {
        claim.Type,
        claim.Value
    });

    return Results.Ok(new
    {
        Name = context.User.Identity?.Name,
        Subject = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
        Claims = claims
    });
}).RequireAuthorization();

app.Run();

public interface IJwtService
{
    ClaimsPrincipal? ValidateAndExtractClaims(string token);
}

public sealed class JwtService : IJwtService
{
    private readonly TokenValidationParameters _validationParameters;

    public JwtService(IConfiguration configuration)
    {
        var secret = configuration["Jwt:Secret"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(secret) ||
            string.IsNullOrWhiteSpace(issuer) ||
            string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "Jwt:Secret, Jwt:Issuer, and Jwt:Audience must be configured.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(secret);
        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException(
                "Jwt:Secret must be at least 32 bytes for a 256-bit signing key.");
        }

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            AuthenticationType = "Bearer",
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    }

    public ClaimsPrincipal? ValidateAndExtractClaims(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(token))
        {
            return null;
        }

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                _validationParameters,
                out _);

            return principal;
        }
        catch (SecurityTokenException)
        {
            // Invalid signature, issuer, audience, lifetime, or malformed JWT.
            return null;
        }
        catch (ArgumentException)
        {
            // Invalid token input.
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }
}

public sealed class JwtClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public JwtClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Resolve the service from InvokeAsync so scoped implementations are safe.
    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();

        if (AuthenticationHeaderValue.TryParse(
                authorizationHeader,
                out var authorization) &&
            string.Equals(
                authorization.Scheme,
                "Bearer",
                StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(authorization.Parameter))
        {
            var principal = jwtService.ValidateAndExtractClaims(
                authorization.Parameter);

            if (principal is not null)
            {
                context.User = principal;
            }
        }

        // Missing or invalid tokens do not terminate the request pipeline;
        // authorization can reject unauthenticated requests as appropriate.
        await _next(context);
    }
}

public static class JwtClaimsMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtClaimsMiddleware(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtClaimsMiddleware>();
    }
}

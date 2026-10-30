// How can you configure JWT authentication in ASP.NET Core Web API?
// Here is the complete step-by-step implementation guide using the latest .NET standards.

// 1. Install the NuGet Package

// Open your terminal or NuGet Package Manager and install the required JWT Bearer library:
// bash
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
// Use code with caution.

// 2. Configure JWT Settings in appsettings.json

// Add your JWT settings—including the Issuer, Audience, and a secure Secret Key (which must be at least 256 bits / 32 characters long)—to your configuration file:
// json
// {
//   "Jwt": {
//     "Issuer": "https://yourdomain.com",
//     "Audience": "https://yourdomain.com",
//     "Key": "YOUR_SUPER_SECRET_KEY_THAT_IS_LONG_ENOUGH_32_BYTES"
//   }
// }
// Use code with caution.

// 3. Register Services in Program.cs

// In your Program.cs, extract the configuration keys and add the authentication services using AddAuthentication() and AddJwtBearer().

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Extract JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// 2. Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    // Set the default schemes to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Optional: Removes the default 5-minute grace period for expiration
    };
});

builder.Services.AddAuthorization(); // Add Authorization middleware services
builder.Services.AddControllers();

var app = builder.Build();

// 4. Configure the Middleware Pipeline

// Enable authentication in the HTTP request pipeline. Order matters immensely here: app.UseAuthentication() must always come before app.UseAuthorization()

app.UseHttpsRedirection();

// Attach middleware to the pipeline
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // Or MapGroup for Minimal APIs

app.Run();

// How can you configure JWT authentication in ASP.NET Core Web API?
    // You can configure JWT authentication by following these steps:
    // 1. Install the Microsoft.AspNetCore.Authentication.JwtBearer package.
    // 2. Add JWT settings to appsettings.json.
    // 3. Register authentication and authorization services in Program.cs.
    // 4. Configure the middleware pipeline to use authentication and authorization.
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpiryInMinutes"])
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}   

// How can you authorize API endpoints using JWT tokens?
// 2. Authorize an endpoint using [Authorize]
// If you want only authenticated users to access an API, use the [Authorize] attribute.
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok("Employee data retrieved successfully");
    }
}
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    // All authenticated users
    [Authorize]
    [HttpGet("all")]
    public IActionResult GetEmployees()
    {
        return Ok("All employees");
    }

    // Only Admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok("Employee deleted successfully");
    }

    // Admin or Manager
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public IActionResult AddEmployee()
    {
        return Ok("Employee added successfully");
    }
}
// A Refresh Token in ASP.NET Core is a long-lived credential used to generate a new JWT access token when the existing access token expires, without requiring the user to log in again.

// It helps maintain a secure and seamless user experience while reducing the need for repeated authentication.
// Step 1: Create RefreshToken model
// Create a class named RefreshToken.cs.
public class RefreshToken
{
    public int Id { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? RevokedDate { get; set; }

    public bool IsRevoked => RevokedDate != null;

    public ApplicationUser? User { get; set; }
}

// Step 2: Add RefreshToken table to DbContext
// In ApplicationDbContext.cs:

public DbSet<RefreshToken> RefreshTokens { get; set; }

// Step 3: Generate Refresh Token
// Create a method that generates a cryptographically secure random token.
using System.Security.Cryptography;

public string GenerateRefreshToken()
{
    var randomBytes = new byte[64];

    using var randomNumberGenerator =
        RandomNumberGenerator.Create();

    randomNumberGenerator.GetBytes(randomBytes);

    return Convert.ToBase64String(randomBytes);
}

// Step 5: Create Login API
// After successful authentication, generate both tokens and store the refresh-token hash in the database.

[HttpPost("login")]
public async Task<IActionResult> Login(LoginModel model)
{
    var user = await _userManager.FindByEmailAsync(model.Email);

    if (user == null ||
        !await _userManager.CheckPasswordAsync(
            user, model.Password))
    {
        return Unauthorized("Invalid credentials");
    }

    var roles = await _userManager.GetRolesAsync(user);

    var accessToken =
        _jwtService.GenerateAccessToken(user, roles);

    var refreshToken =
        _jwtService.GenerateRefreshToken();

    var refreshTokenHash = Convert.ToHexString(
        SHA256.HashData(
            Encoding.UTF8.GetBytes(refreshToken)
        )
    );

    var refreshTokenEntity = new RefreshToken
    {
        TokenHash = refreshTokenHash,
        UserId = user.Id,
        CreatedDate = DateTime.UtcNow,
        ExpiryDate = DateTime.UtcNow.AddDays(7)
    };

    _context.RefreshTokens.Add(refreshTokenEntity);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        accessToken,
        refreshToken,
        expiresIn = 900
    });
}    

// Step 6: Create Refresh Token API
// This API validates the refresh token and issues a new access token.
[HttpPost("refresh")]
public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
{
    var tokenHash = Convert.ToHexString(
        SHA256.HashData(
            Encoding.UTF8.GetBytes(request.RefreshToken)
        )
    );

    var storedToken = await _context.RefreshTokens
        .Include(x => x.User)
        .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

    if (storedToken == null ||
        storedToken.IsRevoked ||
        storedToken.ExpiryDate <= DateTime.UtcNow ||
        storedToken.User == null)
    {
        return Unauthorized("Invalid refresh token");
    }

    var user = storedToken.User;

    // Revoke the old refresh token
    storedToken.RevokedDate = DateTime.UtcNow;

    // Generate new tokens
    var roles = await _userManager.GetRolesAsync(user);

    var newAccessToken =
        _jwtService.GenerateAccessToken(user, roles);

    var newRefreshToken =
        _jwtService.GenerateRefreshToken();

    var newRefreshTokenHash = Convert.ToHexString(
        SHA256.HashData(
            Encoding.UTF8.GetBytes(newRefreshToken)
        )
    );

    var newRefreshTokenEntity = new RefreshToken
    {
        TokenHash = newRefreshTokenHash,
        UserId = user.Id,
        CreatedDate = DateTime.UtcNow,
        ExpiryDate = DateTime.UtcNow.AddDays(7)
    };

    _context.RefreshTokens.Add(newRefreshTokenEntity);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        accessToken = newAccessToken,
        refreshToken = newRefreshToken
    });
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
// How to implement refresh tokens with JWT tokens in asp.net core?
// 1. Generate a refresh token along with the access token during login.
// 2. Store the refresh token securely in the database with a hash.
// 3. Create an API endpoint to accept the refresh token and validate it.
// 4. If valid, revoke the old refresh token and issue a new access token and refresh token.
// 5. Return the new tokens to the client.
// 6. Repeat the process whenever the client requests a new access token using the refresh token.


// 3. How can you customize ASP.NET Core Identity to meet specific application requirements?
// ASP.NET Core Identity is customizable. We can extend the default user model, configure password policies, customize authentication, add custom claims, and implement additional user-management functionality.
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
// Customization 1: Extend IdentityUser
// Suppose our application requires additional employee information.

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    public string Department { get; set; }

    public DateTime DateOfJoining { get; set; }
}

// Customization 2: Configure password policies
// In Program.cs:
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;

    options.Password.RequireDigit = true;

    options.Password.RequireUppercase = true;

    options.Password.RequireLowercase = true;

    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;

    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(15);
});


// This configures password complexity and account lockout policies.
// Customization 3: Configure custom user and role types
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Here, we register our custom user class and the role type.
// Customization 4: Add custom claims
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName!),
    new Claim("Department", user.Department),
    new Claim("EmployeeCode", "EMP101")
};


// These claims can be added to an authenticated user's identity through the appropriate sign-in or claims-generation workflow.
// Customization 5: Customize database context
public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}


// We can also add application-specific DbSet properties and configure relationships using EF Core.
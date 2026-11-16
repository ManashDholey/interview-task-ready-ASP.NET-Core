var builder = WebApplication.CreateBuilder(args);

// 1. Add a backing cache store (In-Memory is used here for single-server setups)
builder.Services.AddDistributedMemoryCache();

// 2. Register session services and configure options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Default is 20 minutes
    options.Cookie.HttpOnly = true;                 // Prevents client-side script access
    options.Cookie.IsEssential = true;              // Bypasses GDPR consent checks if required
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Enable Session Middleware (Must be after UseRouting and before endpoints)
app.UseSession();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();

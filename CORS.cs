// CORS stands for Cross-Origin Resource Sharing.

// CORS is a browser security mechanism that allows or restricts 
// web applications running on one origin from accessing resources hosted on another origin.
// 2. Why is CORS necessary?
// CORS is necessary because browsers enforce the Same-Origin Policy (SOP).
// The Same-Origin Policy prevents a website from freely accessing resources from another origin.
// 4. How do you enable CORS in ASP.NET Core Web API?
// Let's implement CORS in ASP.NET Core using Program.cs.

// Step 1: Configure CORS policy
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Place UseCors() after routing 
// (if explicitly configured) and before authentication 
// and authorization middleware in the usual endpoint-routing setup.
// Step 2: Enable CORS middleware
var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngularApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

// What are CORS policies?
// A CORS policy is a set of rules that defines which origins, HTTP methods and HTTP headers are allowed to access an API from a browser.

// ASP.NET Core supports different ways to configure CORS policies.
// Example: Specific origin policy

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpecificOrigin", policy =>
    {
        policy
            .WithOrigins("https://myfrontend.com")
            .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH")
            .WithHeaders("Content-Type", "Authorization");
    });
});


// Part 1: Swagger / OpenAPI
// 1. What is Swagger/OpenAPI?
// OpenAPI is a standard specification used to describe RESTful APIs. It defines API endpoints, HTTP methods, 
// request parameters, request bodies, response formats, authentication requirements, and status codes.

// Swagger is a set of tools that works with the OpenAPI specification to document, visualize, and test REST APIs.

// For example, if we develop an Employee Management API, Swagger displays all available endpoints:
// - GET /api/employees – Get all employees.
// - GET /api/employees/{id} – Get employee by ID.
// - POST /api/employees – Create an employee.
// - PUT /api/employees/{id} – Update an employee.
// - DELETE /api/employees/{id} – Delete an employee.

// 3. How can you enable Swagger in ASP.NET Core?
// Let's implement Swagger in an ASP.NET Core Web API.

// Step 1: Install the NuGet package

// For the Swashbuckle approach:
// dotnet add package Swashbuckle.AspNetCore
// Step 2: Configure Swagger in Program.cs

// For ASP.NET Core 6, 7, 8 and later minimal hosting:
var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
// Step 3: Create an EmployeeController
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = new[]
        {
            new { Id = 1, Name = "Manash" },
            new { Id = 2, Name = "Rahul" }
        };

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        return Ok(new
        {
            Id = id,
            Name = "Manash"
        });
    }
}

// How can you version your API using Swagger in ASP.NET Core?
// API Versioning allows us to maintain multiple versions of the same API without breaking existing clients.

// For example:
// - API v1: /api/v1/employees
// - API v2: /api/v2/employees
// Suppose we have an Employee API. In version 1, we return employee name and ID. In version 2, we introduce additional fields such as email and department.

// Instead of modifying the existing API and breaking older applications, we can maintain both versions.
// Step 1: Install NuGet packages
// dotnet add package Asp.Versioning.Mvc

// dotnet add package Asp.Versioning.Mvc.ApiExplorer

// dotnet add package Swashbuckle.AspNetCore
// Step 2: Configure API versioning in Program.cs
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);

        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";

        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Employee API V1");

    options.SwaggerEndpoint(
        "/swagger/v2/swagger.json",
        "Employee API V2");
});

app.MapControllers();

app.Run();

// Step 3: Configure Swagger documents for each API version
// Create a class called ConfigureSwaggerOptions.cs.
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = "Employee API",
                    Version = description.ApiVersion.ToString()
                });
        }
    }
}
// Register this class in Program.cs:
// builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
// This ensures that Swagger generates separate documentation for each API version.

// Step 4: Create versioned Employee APIs
// EmployeeController.cs

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/employees")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class EmployeeController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetEmployeesV1()
    {
        return Ok(new
        {
            Id = 1,
            Name = "Manash"
        });
    }

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetEmployeesV2()
    {
        return Ok(new
        {
            Id = 1,
            Name = "Manash",
            Email = "manash@example.com",
            Department = "IT"
        });
    }
}
// Step 5: Register middleware and run the application
// Make sure these registrations are included:
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
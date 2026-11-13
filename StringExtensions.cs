public static class StringExtensions 
{
    // The 'this string str' marks this as an extension method for 'string'
    public static int WordCount(this string str) 
    {
        return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// How you call it:
string sentence = "C# extension methods are powerful";
int count = sentence.WordCount(); // Looks like a native instance method!

namespace MyProject.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}


using MyProject.Extensions;

class Program
{
    static void Main()
    {
        string name = "Manash";

        bool result = name.IsNullOrEmpty();

        Console.WriteLine(result); // False
    }
    
public static class IntegerExtensions
{
    public static bool IsEven(this int number)
    {
        return number % 2 == 0;
    }
}

int number = 10;

Console.WriteLine(number.IsEven()); // True

    
namespace MyProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection
            AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}

    
using MyProject.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Calling our custom extension method
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapControllers();

app.Run();

    
    
}

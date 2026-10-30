public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Level 1: Company has a collection of Employees
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Foreign key for Company
    public int CompanyId { get; set; }
    public Company Company { get; set; }

    // Level 2: Employee has a single Laptop (One-to-One or Optional One-to-Many)
    public Laptop Laptop { get; set; }
    // Many-to-Many: An employee can work on many projects
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}

public class Laptop
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string SerialNumber { get; set; }

    // Foreign key for Employee
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Many-to-Many: A project has many employees
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
// Key Relationships
// Company to Employees: One-to-many (ICollection<Employee>). This enables .Include(c => c.Employees).

// Employee to Laptop: One-to-one or dependent relationship (Laptop Laptop). This enables .ThenInclude(e => e.Laptop).

using Microsoft.EntityFrameworkCore;

public class CompanyService
{
    private readonly AppDbContext _context;

    public CompanyService(AppDbContext context)
    {
        _context = context;
    }

    // Synchronous version (matches your snippet)
    public List<Company> GetCompaniesWithLaptops()
    {
        return _context.Companies
            .Include(c => c.Employees)
                .ThenInclude(e => e.Laptop)
            .ToList();
    }

    // Asynchronous version (Recommended for web applications/APIs)
    public async Task<List<Company>> GetCompaniesWithLaptopsAsync()
    {
        return await _context.Companies
            .Include(c => c.Employees)
                .ThenInclude(e => e.Laptop)
            .ToListAsync();
    }
    public async Task<List<Company>> GetCompaniesWithEmployeesAndProjectsAsync()
{
    return await _context.Companies
        .Include(c => c.Employees)                 // Level 1: Company -> Employees
            .ThenInclude(e => e.Projects)          // Level 2: Employee -> Projects (Many-to-Many)
        .ToListAsync();
}
}
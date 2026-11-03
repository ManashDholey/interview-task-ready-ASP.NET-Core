int[] numbers = {1, 2, 3, 1, 3};

        // LINQ Query to find duplicate elements
        var duplicates = numbers.GroupBy(x => x)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key);

        // Output the results
        Console.WriteLine("Duplicate elements: " + string.Join(", ", duplicates));

//Frequency
var data = "Manash Dholey";

var result = data
    .ToCharArray()
    .GroupBy(e => e)
    .Where(e => e.Key != ' ')
    .Select(g => new
    {
        Character = g.Key,
        Frequency = g.Count()
    });

foreach (var item in result)
{
    Console.WriteLine($"{item.Character} = {item.Frequency}");
}
 int[] salary = { 6000, 1000, 3000, 2000, 1000, 7000 };

        int n = 4; // Find the 3rd highest distinct salary

        var result1 = salary
            .Distinct()
            .OrderByDescending(s => s)
            .Skip(n - 1)
            .FirstOrDefault();

        Console.WriteLine($"{n}rd highest salary: {result1}");         
//To paginate data with LINQ, use Skip() and Take() 

int pageSize =  10; 

int pageNumber =  2; 

var pagedResults =  context.Products 
.OrderBy( p = > p.Name) 
.Skip(( pageNumber- 1) * pageSize) 
.Take( pageSize) 
.ToList(); 

//💡 Sharing some commonly asked C# coding interview questions that cover arrays, LINQ, and string operations.

//1️⃣ Addition & Subtraction of doubles
double num1 = 1.000001;
double num2 = 0.000001;

double sum = num1 + num2;
double difference = num1 - num2;

//2️⃣ Declare a 3D Array
int[,,] array3D = new int[3, 3, 3];

//3️⃣ Print duplicate values in an array using LINQ
char[] duplicate_values = { 'a','b','a','c','d','e','b' };

var duplicates = duplicate_values
    .GroupBy(x => x)
    .Where(g => g.Count() > 1)
    .Select(y => y.Key);

Console.WriteLine(string.Join(", ", duplicates));

//4️⃣ Reverse a string without using strrev
string input = "hello";
string reversed = new string(input.Reverse().ToArray());
Console.WriteLine(reversed);  // olleh

//5️⃣ Find 2nd / 3rd / 4th Highest Number using LINQ
int[] numbers = { 10, 5, 20, 8, 20, 15 };

var result = numbers
    .Distinct()
    .OrderByDescending(x => x)
    .ToList();

Console.WriteLine("2nd Highest: " + result[1]);
Console.WriteLine("3rd Highest: " + result[2]);
Console.WriteLine("4th Highest: " + result[3]);
//(Commonly asked with Employee Salaries → e.g., "Find the 3rd Highest Salary")

//6️⃣ Print repeated characters in a string using LINQ
string stringValue = "Malayalam";

var repeated = stringValue.ToLower()
    .GroupBy(c => c)
    .Where(g => g.Count() > 1)
    .Select(g => g.Key);

Console.WriteLine(string.Join(", ", repeated));
//Another example:
string dept = "IT Department";
var repeatedDept = dept
    .Replace(" ", "")
    .ToLower()
    .GroupBy(c => c)
    .Where(g => g.Count() > 1)
    .Select(g => g.Key);

Console.WriteLine(string.Join(", ", repeatedDept));

//7️⃣ LINQ with JOINS (Example: Employees & Departments)
var employees = new[] {
    new { Id = 1, Name = "Mani", DeptId = 1 },
    new { Id = 2, Name = "Raj", DeptId = 2 }
};

var departments = new[] {
    new { DeptId = 1, DeptName = "IT" },
    new { DeptId = 2, DeptName = "HR" }
};

var query = from e in employees
            join d in departments on e.DeptId equals d.DeptId
            select new { e.Name, d.DeptName };

foreach (var item in query)
    Console.WriteLine($"{item.Name} - {item.DeptName}");
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

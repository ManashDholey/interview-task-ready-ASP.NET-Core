// Positional record (automatically creates init-only properties and a constructor)
public record Person(string FirstName, string LastName, string[] Phones);

// Usage:
var person1 = new Person("Amit", "Sharma", new[] { "9876543210" });
var person2 = new Person("Amit", "Sharma", new[] { "9876543210" });

// Value equality evaluates to true, even though they are different objects in memory
Console.WriteLine(person1 == person2); // True

// Using the 'with' expression for non-destructive mutation
var person3 = person1 with { FirstName = "Rahul" };
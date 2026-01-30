# 💡 Hints, Tips & C# Reference Guide

> **Complete C# reference for students!** Everything you need to implement your projects, from basics to advanced patterns.

## 📖 Table of Contents

1. [C# Language Fundamentals](#c-language-fundamentals)
2. [Common Patterns](#common-patterns)
3. [LINQ Deep Dive](#linq-deep-dive)
4. [Collections & Data Structures](#collections--data-structures)
5. [Object-Oriented Programming](#object-oriented-programming)
6. [Modern C# Features](#modern-c-features)
   - 🎯 **Delegates and Events** - Function pointers and event handling
   - 📦 **Records** - Immutable data with `with` expressions and deconstruction
   - ❓ **Nullability** - `?`, `??`, `?.`, `!` operators explained
   - 🔍 **Pattern Matching** - All 13 patterns with examples
7. [Advanced C# Features](#-advanced-c-features)
   - 🔧 **Extension Methods** - Add functionality to existing types
   - 📊 **Indexers and Ranges** - `^` and `..` operators
   - 💬 **String Interpolation** - Advanced formatting
   - 📍 **Local Functions** - Functions within functions
   - 🏷️ **nameof and typeof** - Reflection helpers
   - 🔀 **Switch Expressions** - Modern pattern matching
   - ⚡ **yield return** - Lazy evaluation
   - 🔒 **const vs readonly vs static** - Constants explained
8. [Design Patterns](#design-patterns)
9. [Common Mistakes](#common-mistakes)
10. [Debugging Techniques](#debugging-techniques)
11. [Performance Tips](#performance-tips)

---

## 🔤 C# Language Fundamentals

### Variables and Types

```csharp
// Value types (stored on stack)
int age = 25;
double price = 9.99;
bool isActive = true;
char grade = 'A';
decimal money = 100.50m;  // Use 'm' suffix for decimal

// Reference types (stored on heap)
string name = "John";
object obj = new object();
int[] numbers = new int[5];

// Nullable types
int? maybeNumber = null;  // Can be null
string? maybeText = null;  // C# 8+ nullable reference types

// var (type inference)
var count = 5;           // Inferred as int
var text = "Hello";      // Inferred as string
var list = new List<int>(); // Inferred as List<int>

// const (compile-time constant)
const double PI = 3.14159;
const string AppName = "MyApp";

// readonly (runtime constant)
readonly DateTime startTime = DateTime.Now;
```

### Type Conversion

```csharp
// Implicit conversion (safe, no data loss)
int i = 5;
double d = i;  // OK: int → double

// Explicit conversion (cast)
double d = 9.99;
int i = (int)d;  // 9 (truncates)

// Parse (string → number)
int num = int.Parse("123");
double price = double.Parse("9.99");

// TryParse (safe parsing)
if (int.TryParse("123", out int result))
    Console.WriteLine(result);

// Convert class
int i = Convert.ToInt32("123");
double d = Convert.ToDouble("9.99");
string s = Convert.ToString(123);

// ToString (any type → string)
int num = 123;
string text = num.ToString();
string formatted = num.ToString("N2");  // 123.00
```

### Operators

```csharp
// Arithmetic
int sum = a + b;
int diff = a - b;
int product = a * b;
int quotient = a / b;
int remainder = a % b;

// Increment/Decrement
i++;  // Post-increment: use then add
++i;  // Pre-increment: add then use
i--;  // Post-decrement
--i;  // Pre-decrement

// Compound assignment
x += 5;  // x = x + 5
x -= 3;  // x = x - 3
x *= 2;  // x = x * 2
x /= 4;  // x = x / 4

// Comparison
bool eq = a == b;   // Equal
bool neq = a != b;  // Not equal
bool gt = a > b;    // Greater than
bool lt = a < b;    // Less than
bool gte = a >= b;  // Greater or equal
bool lte = a <= b;  // Less or equal

// Logical
bool and = a && b;  // AND (short-circuit)
bool or = a || b;   // OR (short-circuit)
bool not = !a;      // NOT

// Null-coalescing
string result = name ?? "Default";  // If name is null, use "Default"
int count = nullableInt ?? 0;       // If null, use 0

// Null-conditional
string? upper = name?.ToUpper();    // If name is null, return null
int? length = text?.Length;         // Safe navigation

// Ternary operator
string status = age >= 18 ? "Adult" : "Minor";
int max = a > b ? a : b;
```

### Control Flow

```csharp
// If-else
if (condition)
{
    // Do something
}
else if (otherCondition)
{
    // Do something else
}
else
{
    // Default case
}

// Switch statement (traditional)
switch (value)
{
    case 1:
        Console.WriteLine("One");
        break;
    case 2:
        Console.WriteLine("Two");
        break;
    default:
        Console.WriteLine("Other");
        break;
}

// Switch expression (C# 8+)
string result = value switch
{
    1 => "One",
    2 => "Two",
    _ => "Other"  // Default case
};

// Pattern matching in switch
string Classify(object obj) => obj switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer",
    string s => $"String: {s}",
    null => "Null value",
    _ => "Unknown type"
};

// For loop
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

// Foreach loop
foreach (var item in collection)
{
    Console.WriteLine(item);
}

// While loop
while (condition)
{
    // Do something
}

// Do-while loop
do
{
    // Do something at least once
}
while (condition);

// Break and continue
for (int i = 0; i < 10; i++)
{
    if (i == 5) continue;  // Skip to next iteration
    if (i == 8) break;     // Exit loop
    Console.WriteLine(i);
}
```

---

## 🎯 Common Patterns

### Pattern 1: Parameter Validation

**Always validate inputs!**

```csharp
// Check for null
if (value is null)
    throw new ArgumentNullException(nameof(value));

// Check for empty string
if (string.IsNullOrWhiteSpace(text))
    throw new ArgumentException("Text cannot be empty", nameof(text));

// Check for negative numbers
if (number < 0)
    throw new ArgumentException("Number must be positive", nameof(number));

// Check for zero
if (divisor == 0)
    throw new DivideByZeroException("Cannot divide by zero");

// Check array length
if (args.Length < 2)
    throw new ArgumentException("Need at least 2 arguments");

// Check range
if (seat < 1 || seat > capacity)
    throw new ArgumentOutOfRangeException(nameof(seat));
```

---

## 📊 LINQ Deep Dive

### Essential LINQ Methods

```csharp
using System.Linq;

// Filtering
var filtered = list.Where(x => x.Price > 10);
var adults = people.Where(p => p.Age >= 18);

// Projection (transformation)
var names = movies.Select(m => m.Title);
var upperNames = names.Select(n => n.ToUpper());

// Sorting
var sorted = list.OrderBy(x => x.EndTime);
var descending = list.OrderByDescending(x => x.Price);
var multiSort = list.OrderBy(x => x.Category)
                    .ThenBy(x => x.Price);

// Aggregation
double sum = numbers.Sum();
double average = numbers.Average();
int count = numbers.Count();
int min = numbers.Min();
int max = numbers.Max();

// Quantifiers
bool hasAny = list.Any(x => x.Price > 100);
bool allPositive = numbers.All(x => x > 0);
bool containsItem = list.Contains(item);

// Element operations
var first = list.First();              // Throws if empty
var firstOrNull = list.FirstOrDefault(); // Returns null/default
var last = list.Last();
var single = list.Single();            // Throws if != 1 element

// Set operations
var union = list1.Union(list2);        // All unique items
var intersect = list1.Intersect(list2); // Common items
var except = list1.Except(list2);      // In list1 but not list2
var distinct = list.Distinct();        // Remove duplicates

// Partitioning
var first5 = list.Take(5);
var skip10 = list.Skip(10);
var page = list.Skip(20).Take(10);     // Pagination

// Grouping
var grouped = list.GroupBy(x => x.Category);
foreach (var group in grouped)
{
    Console.WriteLine($"Category: {group.Key}");
    foreach (var item in group)
        Console.WriteLine($"  {item}");
}

// Joining
var joined = customers.Join(
    orders,
    c => c.Id,           // Customer key
    o => o.CustomerId,   // Order key
    (c, o) => new { c.Name, o.Total }
);

// Conversion
var array = list.ToArray();
var list = array.ToList();
var dict = list.ToDictionary(x => x.Id);
var lookup = list.ToLookup(x => x.Category);
```

### Advanced LINQ

```csharp
// Aggregate (custom reduction)
int sum = numbers.Aggregate((acc, x) => acc + x);
int product = numbers.Aggregate(1, (acc, x) => acc * x);

// Zip (combine two sequences)
var combined = seq1.Zip(seq2, (a, b) => a + b);

// SelectMany (flatten nested collections)
var allWords = sentences.SelectMany(s => s.Split(' '));

// DefaultIfEmpty (prevent empty sequence)
var result = list.DefaultIfEmpty(defaultItem);

// SequenceEqual (compare sequences)
bool equal = list1.SequenceEqual(list2);

// Reverse
var reversed = list.Reverse();

// Concat (combine sequences)
var combined = list1.Concat(list2);

// OfType (filter by type)
var strings = objects.OfType<string>();

// Cast (convert all elements)
var typed = objects.Cast<int>();
```

### LINQ Query Syntax (Alternative)

```csharp
// Method syntax (preferred)
var result = list.Where(x => x.Price > 10)
                 .OrderBy(x => x.Name)
                 .Select(x => x.Name);

// Query syntax (SQL-like)
var result = from item in list
             where item.Price > 10
             orderby item.Name
             select item.Name;

// Both are equivalent!
```

### Performance Tips for LINQ

```csharp
// ❌ Bad: Multiple enumerations
var expensive = items.Where(x => x.Price > 100);
int count = expensive.Count();      // Enumerates once
var first = expensive.First();      // Enumerates again!

// ✅ Good: Materialize once
var expensive = items.Where(x => x.Price > 100).ToList();
int count = expensive.Count();      // No enumeration
var first = expensive.First();      // No enumeration

// ❌ Bad: Inefficient filtering
var result = list.Where(x => x.Age > 18)
                 .Where(x => x.City == "NYC");

// ✅ Good: Single filter
var result = list.Where(x => x.Age > 18 && x.City == "NYC");

// ❌ Bad: Unnecessary conversion
var array = list.ToArray();
var count = array.Length;

// ✅ Good: Direct count
var count = list.Count();
```

---

### Pattern 3: DateTime and TimeSpan

**Working with dates and times:**

```csharp
// Current time
DateTime now = DateTime.Now;

// Specific date and time
DateTime specific = new DateTime(2025, 11, 1, 10, 30, 0);

// Parse from string
DateTime parsed = DateTime.Parse("2025-11-01T10:30:00");

// Date only (no time)
DateOnly date = new DateOnly(2025, 11, 1);
DateOnly fromDateTime = DateOnly.FromDateTime(dateTime);

// Time span (duration)
TimeSpan duration = TimeSpan.FromMinutes(148);
TimeSpan hours = TimeSpan.FromHours(2.5);

// Add time
DateTime later = now.AddHours(2);
DateTime tomorrow = now.AddDays(1);

// Subtract times
TimeSpan diff = endTime - startTime;

// Compare times
if (show.StartTime >= lastEndTime)
    // No overlap

// Format for display
string formatted = dateTime.ToString("yyyy-MM-dd HH:mm");
```

---

## 📦 Collections & Data Structures

### Choosing the Right Collection

| Collection | Use When | Time Complexity | Allows Duplicates | Ordered |
|------------|----------|----------------|-------------------|---------|
| `List<T>` | General purpose, indexed access | Add: O(1), Search: O(n) | Yes | Yes |
| `HashSet<T>` | Fast lookups, unique items | All: O(1) average | No | No |
| `Dictionary<K,V>` | Key-value pairs | All: O(1) average | Keys: No, Values: Yes | No |
| `Queue<T>` | FIFO processing | All: O(1) | Yes | Yes |
| `Stack<T>` | LIFO processing | All: O(1) | Yes | Yes |
| `LinkedList<T>` | Frequent insert/delete | Insert/Delete: O(1) | Yes | Yes |
| `SortedSet<T>` | Sorted unique items | All: O(log n) | No | Yes |
| `SortedDictionary<K,V>` | Sorted key-value pairs | All: O(log n) | Keys: No | Yes |

### List<T> - Dynamic Array

```csharp
using System.Collections.Generic;

List<string> names = new();

// Adding
names.Add("Alice");
names.AddRange(new[] { "Bob", "Charlie" });
names.Insert(1, "David");  // Insert at index

// Accessing
string first = names[0];
string last = names[^1];    // C# 8+ index from end
names[0] = "Alicia";        // Modify

// Removing
names.Remove("Bob");         // Remove by value
names.RemoveAt(0);          // Remove by index
names.RemoveAll(n => n.StartsWith("C")); // Remove matching
names.Clear();              // Remove all

// Searching
bool contains = names.Contains("Alice");
int index = names.IndexOf("Bob");  // -1 if not found
string? found = names.Find(n => n.Length > 5);

// Capacity management
names.Capacity = 100;       // Pre-allocate
names.TrimExcess();         // Reduce capacity

// Iteration
foreach (var name in names)
    Console.WriteLine(name);

// ForEach method
names.ForEach(n => Console.WriteLine(n));
```

### HashSet<T> - Fast Lookups

```csharp
HashSet<int> numbers = new();

// Adding
numbers.Add(5);
bool added = numbers.Add(5);  // Returns false (already exists)
numbers.UnionWith(new[] { 1, 2, 3 });

// Checking
bool contains = numbers.Contains(5);  // O(1) - very fast!

// Set operations
var set1 = new HashSet<int> { 1, 2, 3 };
var set2 = new HashSet<int> { 3, 4, 5 };

set1.UnionWith(set2);        // {1, 2, 3, 4, 5}
set1.IntersectWith(set2);    // {3}
set1.ExceptWith(set2);       // {1, 2}
bool isSubset = set1.IsSubsetOf(set2);
bool overlaps = set1.Overlaps(set2);

// When to use:
// - Seat booking (check if seat already taken)
// - Unique items (no duplicates)
// - Fast membership testing
```

### Dictionary<TKey, TValue> - Key-Value Store

```csharp
Dictionary<string, int> ages = new();

// Adding
ages["Alice"] = 25;
ages.Add("Bob", 30);
bool added = ages.TryAdd("Charlie", 35);  // C# 8+

// Accessing
int age = ages["Alice"];
bool exists = ages.TryGetValue("Bob", out int bobAge);

// Checking
bool hasKey = ages.ContainsKey("Alice");
bool hasValue = ages.ContainsValue(25);

// Removing
ages.Remove("Alice");
ages.Clear();

// Iteration
foreach (var kvp in ages)
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");

foreach (var key in ages.Keys)
    Console.WriteLine(key);

foreach (var value in ages.Values)
    Console.WriteLine(value);

// Complex keys
Dictionary<(int x, int y), CrossRoad> grid = new();
grid[(0, 0)] = new CrossRoad();
var crossroad = grid[(0, 0)];
```

### Queue<T> - First In, First Out

```csharp
Queue<Vehicle> queue = new();

// Enqueue (add to back)
queue.Enqueue(vehicle1);
queue.Enqueue(vehicle2);

// Dequeue (remove from front)
Vehicle next = queue.Dequeue();

// Peek (look without removing)
Vehicle front = queue.Peek();

// Check
bool hasItems = queue.Count > 0;
bool isEmpty = queue.Count == 0;

// Use cases:
// - Traffic light queues
// - Task processing
// - BFS algorithm
```

### Stack<T> - Last In, First Out

```csharp
Stack<string> stack = new();

// Push (add to top)
stack.Push("First");
stack.Push("Second");

// Pop (remove from top)
string top = stack.Pop();  // "Second"

// Peek (look without removing)
string topItem = stack.Peek();

// Use cases:
// - Undo functionality
// - Path reconstruction (Dijkstra)
// - DFS algorithm
// - Expression evaluation
```

### PriorityQueue<TElement, TPriority> - Ordered Processing

```csharp
// C# 10+ (.NET 6+)
PriorityQueue<string, int> pq = new();

// Enqueue with priority (lower number = higher priority)
pq.Enqueue("Important", priority: 1);
pq.Enqueue("Normal", priority: 5);
pq.Enqueue("Urgent", priority: 0);

// Dequeue (removes highest priority)
string next = pq.Dequeue();  // "Urgent"

// Peek
string highest = pq.Peek();

// Use cases:
// - Dijkstra's algorithm
// - Task scheduling
// - Emergency vehicle priority
```

### SortedSet<T> - Always Sorted

```csharp
SortedSet<int> sorted = new();

// Add (maintains sort order)
sorted.Add(5);
sorted.Add(2);
sorted.Add(8);
// Contents: {2, 5, 8}

// Min and Max
int min = sorted.Min;
int max = sorted.Max;

// Range operations
var subset = sorted.GetViewBetween(3, 7);  // {5}

// Use cases:
// - Maintaining sorted data
// - Range queries
// - Ordered unique values
```

### Collection Initialization Syntax

```csharp
// Collection initializer
List<int> numbers = new() { 1, 2, 3, 4, 5 };

// Dictionary initializer
Dictionary<string, int> ages = new()
{
    ["Alice"] = 25,
    ["Bob"] = 30,
    ["Charlie"] = 35
};

// Or with Add syntax
Dictionary<string, int> ages2 = new()
{
    { "Alice", 25 },
    { "Bob", 30 }
};

// HashSet initializer
HashSet<int> unique = new() { 1, 2, 3, 4, 5 };
```

---

### Pattern 5: String Handling

**Common string operations:**

```csharp
// Split by space
string[] parts = input.Split(' ');

// Split and handle quotes
// "add-room \"Room A\" 100" → ["add-room", "Room A", "100"]
// See App.cs Split() method for implementation

// Join array into string
string combined = string.Join(", ", items);

// Check if empty
if (string.IsNullOrWhiteSpace(text))
    // Empty!

// Format with interpolation
string msg = $"Booked {count} seats in {room.Name}";

// Format numbers
decimal price = 9.99m;
string formatted = $"${price:F2}";  // $9.99

// Trim whitespace
string clean = text.Trim();

// To upper/lower
string upper = text.ToUpper();
string lower = text.ToLower();

// Contains substring
if (text.Contains("error"))
    // Handle error

// Starts with / ends with
if (file.EndsWith(".cs"))
    // C# file
```

---

### Pattern 6: Error Handling

**Try-catch blocks:**

```csharp
try
{
    // Code that might throw exception
    int result = int.Parse(input);
    double quotient = numerator / denominator;
}
catch (FormatException)
{
    Console.WriteLine("Invalid number format");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Cannot divide by zero");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

**Try-Parse pattern (better for expected failures):**

```csharp
if (int.TryParse(input, out int result))
{
    // Success! Use result
}
else
{
    // Failed to parse
    Console.WriteLine("Please enter a valid number");
}
```

---

### Pattern 7: Math Operations

**Common math functions:**

```csharp
using System;

// Absolute value
double abs = Math.Abs(-5.0);  // 5.0

// Square root
double sqrt = Math.Sqrt(16.0);  // 4.0

// Power
double pow = Math.Pow(2, 3);  // 8.0

// Logarithms
double log10 = Math.Log10(100);  // 2.0
double ln = Math.Log(Math.E);    // 1.0

// Trig functions (all in radians!)
double sin = Math.Sin(Math.PI / 2);  // 1.0
double cos = Math.Cos(0);            // 1.0
double tan = Math.Tan(Math.PI / 4);  // 1.0

// Convert degrees to radians
double radians = degrees * Math.PI / 180.0;

// Convert radians to degrees
double degrees = radians * 180.0 / Math.PI;

// Rounding
double rounded = Math.Round(3.7);   // 4.0
double ceiling = Math.Ceiling(3.1); // 4.0
double floor = Math.Floor(3.9);     // 3.0

// Min and max
double min = Math.Min(5, 3);  // 3.0
double max = Math.Max(5, 3);  // 5.0

// Constants
double pi = Math.PI;  // 3.14159...
double e = Math.E;    // 2.71828...
```

---

## 🏗️ Object-Oriented Programming

### Classes and Objects

```csharp
// Class definition
public class Person
{
    // Fields (private by convention)
    private string _name;
    private int _age;
    
    // Properties (public access)
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    // Auto-property (compiler generates field)
    public int Age { get; set; }
    
    // Read-only property
    public int BirthYear => DateTime.Now.Year - Age;
    
    // Constructor
    public Person(string name, int age)
    {
        _name = name;
        _age = age;
    }
    
    // Methods
    public void Greet()
    {
        Console.WriteLine($"Hello, I'm {Name}");
    }
    
    // Method overloading
    public void Greet(string otherName)
    {
        Console.WriteLine($"Hello {otherName}, I'm {Name}");
    }
}

// Creating objects
Person person = new Person("Alice", 25);
person.Greet();
```

### Inheritance

```csharp
// Base class
public class Animal
{
    public string Name { get; set; }
    
    public virtual void MakeSound()
    {
        Console.WriteLine("Some sound");
    }
}

// Derived class
public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }
    
    public void Fetch()
    {
        Console.WriteLine($"{Name} is fetching!");
    }
}

// Usage
Dog dog = new Dog { Name = "Buddy" };
dog.MakeSound();  // "Woof!"
dog.Fetch();

// Polymorphism
Animal animal = new Dog { Name = "Max" };
animal.MakeSound();  // "Woof!" (calls Dog's version)
```

### Interfaces

```csharp
// Interface definition
public interface IOperation
{
    string Name { get; }
    double Evaluate(params double[] args);
}

// Interface implementation
public class Add : IOperation
{
    public string Name => "add";
    
    public double Evaluate(params double[] args)
    {
        return args.Length == 0 ? 0 : args.Sum();
    }
}

// Multiple interfaces
public interface ILoggable
{
    void Log(string message);
}

public class Calculator : IOperation, ILoggable
{
    public string Name => "calc";
    public double Evaluate(params double[] args) => 0;
    public void Log(string message) => Console.WriteLine(message);
}

// Interface as type
IOperation op = new Add();
double result = op.Evaluate(2, 3);
```

### Abstract Classes

```csharp
// Abstract class (cannot instantiate)
public abstract class Shape
{
    public abstract double Area { get; }
    public abstract double Perimeter { get; }
    
    // Concrete method
    public void Display()
    {
        Console.WriteLine($"Area: {Area}, Perimeter: {Perimeter}");
    }
}

// Concrete implementation
public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double Area => Math.PI * Radius * Radius;
    public override double Perimeter => 2 * Math.PI * Radius;
    
    public Circle(double radius)
    {
        Radius = radius;
    }
}

// Usage
Shape shape = new Circle(5.0);
shape.Display();
```

### Encapsulation

```csharp
public class BankAccount
{
    // Private field
    private decimal _balance;
    
    // Read-only property
    public decimal Balance => _balance;
    
    // Method with validation
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
        _balance += amount;
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
        if (_balance < amount)
            return false;  // Insufficient funds
        
        _balance -= amount;
        return true;
    }
}
```

### Static Members

```csharp
public class MathHelper
{
    // Static field
    private static int _callCount = 0;
    
    // Static property
    public static int CallCount => _callCount;
    
    // Static method
    public static double Square(double x)
    {
        _callCount++;
        return x * x;
    }
    
    // Static constructor (runs once)
    static MathHelper()
    {
        Console.WriteLine("MathHelper initialized");
    }
}

// Usage (no instance needed)
double result = MathHelper.Square(5);
Console.WriteLine(MathHelper.CallCount);
```

---

## 🚀 Modern C# Features

### Records (C# 9+)

```csharp
// Record (immutable by default)
public record Movie(string Title, TimeSpan Duration);

// Usage
Movie movie = new Movie("Inception", TimeSpan.FromMinutes(148));
Console.WriteLine(movie.Title);

// Value equality (not reference)
var m1 = new Movie("Avatar", TimeSpan.FromMinutes(162));
var m2 = new Movie("Avatar", TimeSpan.FromMinutes(162));
bool equal = m1 == m2;  // true!

// With expressions (create modified copy)
Movie longer = movie with { Duration = TimeSpan.FromMinutes(180) };

// Record with body
public record Person(string Name, int Age)
{
    public bool IsAdult => Age >= 18;
}

// Mutable record
public record class MutablePerson
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

### Pattern Matching

```csharp
// Type pattern
object obj = "Hello";
if (obj is string s)
    Console.WriteLine(s.Length);

// Property pattern
if (person is { Age: >= 18, Name: "Alice" })
    Console.WriteLine("Adult named Alice");

// Positional pattern
if (point is (0, 0))
    Console.WriteLine("Origin");

// Relational patterns
int score = 85;
string grade = score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "F"
};

// List patterns (C# 11+)
int[] numbers = { 1, 2, 3 };
if (numbers is [1, 2, 3])
    Console.WriteLine("Matches exactly");

if (numbers is [1, .., 3])
    Console.WriteLine("Starts with 1, ends with 3");
```

### Tuples

```csharp
// Creating tuples
(int, string) tuple1 = (1, "one");
var tuple2 = (Age: 25, Name: "Alice");

// Accessing
int id = tuple1.Item1;
string name = tuple2.Name;

// Deconstruction
var (age, name) = tuple2;
Console.WriteLine($"{name} is {age} years old");

// Returning multiple values
public (int min, int max) GetMinMax(int[] numbers)
{
    return (numbers.Min(), numbers.Max());
}

var (min, max) = GetMinMax(new[] { 1, 5, 3, 9, 2 });

// Discards
var (_, maximum) = GetMinMax(numbers);  // Ignore minimum
```

### Null Safety (C# 8+)

```csharp
// Nullable reference types
#nullable enable

string nonNull = "Hello";      // Cannot be null
string? maybeNull = null;      // Can be null

// Null-forgiving operator
string text = maybeNull!;  // "I know it's not null!"

// Null-conditional operator
int? length = text?.Length;

// Null-coalescing operator
string result = text ?? "Default";

// Null-coalescing assignment
text ??= "Default";  // Assign if null
```

### Init-Only Properties (C# 9+)

```csharp
public class Person
{
    public string Name { get; init; }  // Can set during initialization only
    public int Age { get; init; }
}

// Usage
var person = new Person { Name = "Alice", Age = 25 };
// person.Name = "Bob";  // Compile error!
```

### Target-Typed New (C# 9+)

```csharp
// Old way
Person person = new Person("Alice", 25);

// New way (type inferred)
Person person = new("Alice", 25);

// In collections
List<Person> people = new()
{
    new("Alice", 25),
    new("Bob", 30)
};
```

### Top-Level Statements (C# 9+)

```csharp
// No need for class or Main method!
using System;

Console.WriteLine("Hello, World!");
var numbers = new[] { 1, 2, 3, 4, 5 };
Console.WriteLine(numbers.Sum());

// Functions can be defined below
int Add(int a, int b) => a + b;
```

### File-Scoped Namespaces (C# 10+)

```csharp
// Old way
namespace MyApp
{
    public class MyClass
    {
        // ...
    }
}

// New way
namespace MyApp;

public class MyClass
{
    // ...
}
```

### String Interpolation

```csharp
string name = "Alice";
int age = 25;

// Basic interpolation
string message = $"Hello, {name}!";

// With expressions
string info = $"{name} is {age} years old";

// With formatting
decimal price = 9.99m;
string formatted = $"Price: ${price:F2}";  // $9.99

// With alignment
string aligned = $"{name,-10} {age,5}";  // Left/right align

// Verbatim + interpolation
string path = $@"C:\Users\{name}\Documents";

// Raw string literals (C# 11+)
string json = """
    {
        "name": "{name}",
        "age": {age}
    }
    """;
```

### Lambda Expressions

```csharp
// Expression lambda
Func<int, int> square = x => x * x;

// Statement lambda
Func<int, int, int> add = (a, b) =>
{
    int result = a + b;
    return result;
};

// Multiple parameters
Func<int, int, int> multiply = (a, b) => a * b;

// No parameters
Func<int> getRandom = () => new Random().Next();

// With LINQ
var filtered = numbers.Where(n => n > 5);
var squared = numbers.Select(n => n * n);

// As event handler
button.Click += (sender, args) => Console.WriteLine("Clicked!");
```

### Local Functions

```csharp
public int Calculate(int[] numbers)
{
    // Local function
    int Square(int x) => x * x;
    
    return numbers.Select(Square).Sum();
}

// Local function with capture
public void Process(List<int> items)
{
    int threshold = 10;
    
    // Captures 'threshold' from outer scope
    bool IsValid(int item) => item > threshold;
    
    var valid = items.Where(IsValid);
}
```

### Expression-Bodied Members

```csharp
public class Person
{
    private string _name;
    
    // Expression-bodied property
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException();
    }
    
    // Expression-bodied method
    public string GetGreeting() => $"Hello, I'm {Name}";
    
    // Expression-bodied constructor
    public Person(string name) => _name = name;
    
    // Expression-bodied finalizer
    ~Person() => Console.WriteLine("Disposed");
}
```

### Delegates and Events

```csharp
// Delegate definition (function pointer type)
public delegate void LogHandler(string message);
public delegate int MathOperation(int a, int b);

// Using delegates
public class Calculator
{
    public void Process(int a, int b, MathOperation operation)
    {
        int result = operation(a, b);
        Console.WriteLine($"Result: {result}");
    }
}

// Usage with method
int Add(int a, int b) => a + b;
calc.Process(5, 3, Add);  // Result: 8

// Usage with lambda
calc.Process(5, 3, (a, b) => a * b);  // Result: 15

// Func and Action (built-in delegates)
Func<int, int, int> multiply = (a, b) => a * b;  // Returns value
Action<string> log = msg => Console.WriteLine(msg);  // No return
Predicate<int> isEven = n => n % 2 == 0;  // Returns bool

// Multicast delegates (multiple methods)
LogHandler logger = null;
logger += ConsoleLog;     // Add method
logger += FileLog;        // Add another
logger += EmailLog;       // Add another
logger("Message");        // Calls all three!
logger -= FileLog;        // Remove one

void ConsoleLog(string msg) => Console.WriteLine(msg);
void FileLog(string msg) => File.AppendAllText("log.txt", msg);
void EmailLog(string msg) => SendEmail("admin@example.com", msg);

// Events (special delegates)
public class Button
{
    // Define event
    public event EventHandler? Clicked;
    
    // Raise event
    public void Click()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

// Subscribe to event
var button = new Button();
button.Clicked += (sender, args) => Console.WriteLine("Button clicked!");
button.Click();  // Triggers event

// Custom event args
public class TemperatureChangedEventArgs : EventArgs
{
    public double OldTemp { get; set; }
    public double NewTemp { get; set; }
}

public class Thermostat
{
    public event EventHandler<TemperatureChangedEventArgs>? TemperatureChanged;
    
    private double _temperature;
    public double Temperature
    {
        get => _temperature;
        set
        {
            var args = new TemperatureChangedEventArgs
            {
                OldTemp = _temperature,
                NewTemp = value
            };
            _temperature = value;
            TemperatureChanged?.Invoke(this, args);
        }
    }
}

// Usage
var thermostat = new Thermostat();
thermostat.TemperatureChanged += (sender, args) =>
{
    Console.WriteLine($"Temp changed: {args.OldTemp}°C → {args.NewTemp}°C");
};
thermostat.Temperature = 25.0;  // Triggers event
```

### Records - Deep Dive

```csharp
// Basic record (positional syntax)
public record Movie(string Title, TimeSpan Duration);

// Record with additional members
public record Person(string Name, int Age)
{
    // Computed property
    public bool IsAdult => Age >= 18;
    
    // Method
    public void Greet() => Console.WriteLine($"Hi, I'm {Name}");
    
    // ToString override (optional)
    public override string ToString() => $"{Name} ({Age})";
}

// Record with primary constructor and additional properties
public record Book(string Title, string Author)
{
    public int Pages { get; init; }  // Additional init-only property
    public decimal Price { get; set; }  // Mutable property
}

// Usage
var book = new Book("1984", "Orwell") { Pages = 328, Price = 9.99m };

// Record equality (value-based, not reference-based!)
var movie1 = new Movie("Inception", TimeSpan.FromMinutes(148));
var movie2 = new Movie("Inception", TimeSpan.FromMinutes(148));
Console.WriteLine(movie1 == movie2);  // TRUE! (value equality)

// With expressions (non-destructive mutation)
var original = new Person("Alice", 25);
var updated = original with { Age = 26 };  // Creates NEW instance
Console.WriteLine(original.Age);  // 25 (unchanged)
Console.WriteLine(updated.Age);   // 26 (new value)

// Multiple properties
var movie3 = movie1 with 
{ 
    Title = "Inception 2", 
    Duration = TimeSpan.FromMinutes(160) 
};

// Deconstruction
var (title, duration) = movie1;
Console.WriteLine($"{title} is {duration.TotalMinutes} minutes");

// Record deconstruction with custom deconstructors
public record Point(int X, int Y)
{
    // Add custom deconstructor for polar coordinates
    public void Deconstruct(out double r, out double theta)
    {
        r = Math.Sqrt(X * X + Y * Y);
        theta = Math.Atan2(Y, X);
    }
}

// Usage - choose which deconstructor
var point = new Point(3, 4);
var (x, y) = point;           // Positional: (3, 4)
var (r, theta) = point;       // Polar: (5, 0.927...)

// Record inheritance
public record Vehicle(string Brand, string Model);
public record Car(string Brand, string Model, int Doors) : Vehicle(Brand, Model);
public record Truck(string Brand, string Model, int Capacity) : Vehicle(Brand, Model);

var car = new Car("Toyota", "Camry", 4);
var truck = car with { Doors = 2 };  // Compile error! Doors not in base

// Proper inheritance with 'with'
var vehicle = new Car("Honda", "Civic", 4);
var newCar = vehicle with { Brand = "Toyota" };  // Works!

// Struct records (value type records)
public readonly record struct Point2D(int X, int Y);
public record struct MutablePoint(int X, int Y);  // Mutable struct record

// Anonymous types vs records
var anon = new { Name = "Alice", Age = 25 };  // Anonymous type
var rec = new Person("Alice", 25);            // Record

// Records are better because:
// - Can add methods and properties
// - Can inherit from other records
// - More flexible
// - Named type (better for APIs)

// Practical example: Immutable updates
public record GameState(int Score, int Level, string PlayerName)
{
    public GameState AddPoints(int points) => this with { Score = Score + points };
    public GameState NextLevel() => this with { Level = Level + 1 };
    public GameState Rename(string name) => this with { PlayerName = name };
}

var game = new GameState(0, 1, "Player1");
game = game.AddPoints(100);      // Score: 100
game = game.AddPoints(50);       // Score: 150
game = game.NextLevel();         // Level: 2
game = game.Rename("Alice");     // Name: Alice
```

### Nullability - Complete Guide

```csharp
// Enable nullable reference types (C# 8+)
#nullable enable

// Non-nullable reference type
string nonNull = "Hello";      // Cannot be null
// nonNull = null;             // Compiler warning!

// Nullable reference type
string? maybeNull = null;      // Can be null
maybeNull = "Hello";           // Can also be non-null

// Nullable value types
int? nullableInt = null;       // Nullable<int>
double? nullableDouble = 5.5;

// Null-conditional operator (?.)
string? text = GetText();
int? length = text?.Length;    // If text is null, length is null
                               // If text is not null, length is text.Length

// Chain null-conditional operators
Person? person = GetPerson();
string? city = person?.Address?.City;  // Safe navigation

// Null-conditional with methods
string? result = person?.GetName()?.ToUpper();

// Null-conditional with indexers
string? first = names?[0];

// Null-coalescing operator (??)
string name = userName ?? "Guest";  // If userName is null, use "Guest"
int count = nullableInt ?? 0;       // If nullableInt is null, use 0

// Chain null-coalescing
string display = firstName ?? middleName ?? lastName ?? "Unknown";

// Null-coalescing assignment (??=)
string? value = null;
value ??= "Default";  // Assigns "Default" only if value is null
value ??= "Other";    // Does nothing (value is already "Default")

// Practical usage
private Dictionary<string, User> _cache = new();
public User GetUser(string id)
{
    return _cache[id] ??= LoadUserFromDatabase(id);  // Cache pattern!
}

// Null-forgiving operator (!)
string? maybeNullable = GetString();
string definitelyNotNull = maybeNullable!;  // "I know it's not null!"
// Use carefully - can cause NullReferenceException!

// When to use null-forgiving:
// - After null check
string? input = GetInput();
if (input is not null)
{
    ProcessString(input!);  // We know it's not null here
}

// - When you KNOW value is not null
string? config = Configuration["Key"];  // You set this in config
string value = config!;  // You're certain it exists

// Null checking patterns

// Pattern 1: if statement
string? name = GetName();
if (name is not null)
{
    Console.WriteLine(name.ToUpper());  // Safe
}

// Pattern 2: null-conditional + null-coalescing
Console.WriteLine(name?.ToUpper() ?? "UNKNOWN");

// Pattern 3: pattern matching
if (name is string n)  // Type test with declaration
{
    Console.WriteLine(n.ToUpper());  // n is not null here
}

// Pattern 4: Guard clause
string? input = GetInput();
if (input is null)
    throw new ArgumentNullException(nameof(input));
// input is not null from here on

// Working with nullable value types
int? age = GetAge();

// Check for null
if (age.HasValue)
{
    Console.WriteLine(age.Value);  // Safe access
}

// Or use GetValueOrDefault
int actualAge = age.GetValueOrDefault(0);

// Or null-coalescing
int safeAge = age ?? 0;

// Nullable and LINQ
int?[] numbers = { 1, null, 3, null, 5 };
var nonNullNumbers = numbers.Where(n => n.HasValue)
                            .Select(n => n!.Value);  // or n.Value

// Better: use OfType
var nonNull = numbers.OfType<int>();  // Filters out nulls!

// Nullable return types
public string? FindUser(int id)  // May return null
{
    return users.FirstOrDefault(u => u.Id == id)?.Name;
}

// Nullable parameters
public void Process(string? optionalParam = null)
{
    if (optionalParam is null)
    {
        // Handle null case
        return;
    }
    // Use optionalParam safely
}

// Collections and nullability
List<string> nonNullableList = new();      // List of non-null strings
List<string?> nullableList = new();        // List can contain nulls
List<string>? maybeNullList = null;        // List itself may be null
List<string?>? bothNullable = null;        // Both nullable

// Dictionary with nullable values
Dictionary<string, User?> users = new();   // Values can be null
users["john"] = null;  // OK
User? user = users["john"];  // Might be null

// Null checking in properties
public class Person
{
    private string? _name;
    
    public string Name
    {
        get => _name ?? "Unknown";
        set => _name = value ?? throw new ArgumentNullException();
    }
}

// Null-checking in constructors
public class Product
{
    public string Name { get; }
    public decimal Price { get; }
    
    public Product(string? name, decimal? price)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }
}

// Practical patterns for your projects

// Pattern: Repository GetOrDefault
public Show? GetShow(Guid id)  // Returns null if not found
{
    return _shows.FirstOrDefault(s => s.Id == id);
}

// Usage
var show = store.GetShow(id);
if (show is null)
{
    Console.WriteLine("Show not found");
    return;
}
Console.WriteLine(show.Movie.Title);  // Safe!

// Pattern: Try-Get pattern
public bool TryGetShow(Guid id, out Show? show)
{
    show = _shows.FirstOrDefault(s => s.Id == id);
    return show is not null;
}

// Usage
if (store.TryGetShow(id, out var show))
{
    Console.WriteLine(show!.Movie.Title);  // We know it's not null
}

// Disable nullable warnings (use sparingly!)
#nullable disable
string canBeNull = null;  // No warning
#nullable restore
```

### Pattern Matching - All Patterns

```csharp
// 1. TYPE PATTERNS
object obj = "Hello";

// Simple type pattern
if (obj is string)
    Console.WriteLine("It's a string");

// Type pattern with declaration
if (obj is string s)
    Console.WriteLine(s.ToUpper());  // Can use 's'

// Type pattern in switch
string Describe(object obj) => obj switch
{
    int i => $"Integer: {i}",
    string s => $"String: {s}",
    double d => $"Double: {d}",
    null => "Null",
    _ => "Unknown type"
};

// 2. CONSTANT PATTERNS
int number = 5;

if (number is 5)
    Console.WriteLine("It's five");

string Grade(int score) => score switch
{
    100 => "Perfect!",
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "F"
};

// 3. RELATIONAL PATTERNS (C# 9+)
int age = 25;

if (age is >= 18 and < 65)
    Console.WriteLine("Working age");

if (age is < 0 or > 120)
    Console.WriteLine("Invalid age");

string Category(int age) => age switch
{
    < 0 => "Invalid",
    < 13 => "Child",
    < 20 => "Teenager",
    < 65 => "Adult",
    _ => "Senior"
};

// Combining relational patterns
if (score is >= 0 and <= 100)
    Console.WriteLine("Valid score");

// 4. LOGICAL PATTERNS
// and, or, not

if (value is not null)
    Console.WriteLine(value);

if (number is > 0 and < 100)
    Console.WriteLine("In range");

if (day is "Saturday" or "Sunday")
    Console.WriteLine("Weekend!");

if (user is not null and { Age: >= 18 })
    Console.WriteLine("Adult user");

// 5. PROPERTY PATTERNS
public record Person(string Name, int Age, string City);

var person = new Person("Alice", 25, "NYC");

// Simple property pattern
if (person is { Age: 25 })
    Console.WriteLine("Person is 25 years old");

// Multiple properties
if (person is { Age: >= 18, City: "NYC" })
    Console.WriteLine("Adult in NYC");

// Nested properties
public record Address(string City, string State);
public record Employee(string Name, Address Address);

var emp = new Employee("Bob", new Address("Boston", "MA"));

if (emp is { Address: { State: "MA" } })
    Console.WriteLine("Massachusetts employee");

// Property pattern in switch
string Describe(Person p) => p switch
{
    { Age: < 18 } => "Minor",
    { Age: >= 18, City: "NYC" } => "Adult in NYC",
    { Age: >= 18 } => "Adult",
    _ => "Unknown"
};

// 6. POSITIONAL PATTERNS
public record Point(int X, int Y);

var point = new Point(0, 0);

// Positional pattern
if (point is (0, 0))
    Console.WriteLine("Origin");

// Positional with declaration
if (point is (var x, var y))
    Console.WriteLine($"X: {x}, Y: {y}");

// Positional in switch
string Location(Point p) => p switch
{
    (0, 0) => "Origin",
    (var x, 0) => $"X-axis at {x}",
    (0, var y) => $"Y-axis at {y}",
    (var x, var y) when x == y => "Diagonal",
    (var x, var y) => $"Point ({x}, {y})"
};

// 7. VAR PATTERNS
// Capture value in variable
if (obj is var o)
{
    // o is same as obj, but in new scope
    Console.WriteLine(o?.ToString());
}

// Useful in switch when you need the value
string Process(object obj) => obj switch
{
    var o when o.ToString().Length > 10 => "Long",
    var o => o.ToString()  // Capture for use
};

// 8. DISCARD PATTERN (_)
// Don't care about value
string Classify(object obj) => obj switch
{
    int _ => "Some integer",
    string _ => "Some string",
    _ => "Unknown"
};

// Positional with discard
if (point is (_, var y))  // Don't care about X
    Console.WriteLine($"Y: {y}");

// 9. LIST PATTERNS (C# 11+)
int[] numbers = { 1, 2, 3, 4, 5 };

// Exact match
if (numbers is [1, 2, 3, 4, 5])
    Console.WriteLine("Exact match");

// Match first and last
if (numbers is [1, .., 5])
    Console.WriteLine("Starts with 1, ends with 5");

// Match first element
if (numbers is [1, ..])
    Console.WriteLine("Starts with 1");

// Match last element
if (numbers is [.., 5])
    Console.WriteLine("Ends with 5");

// Match length
if (numbers is [_, _, _])
    Console.WriteLine("Has 3 elements");

// Capture elements
if (numbers is [var first, .., var last])
    Console.WriteLine($"First: {first}, Last: {last}");

// Match middle
if (numbers is [1, var second, var third, ..])
    Console.WriteLine($"Second: {second}, Third: {third}");

// List pattern in switch
string Describe(int[] arr) => arr switch
{
    [] => "Empty",
    [var x] => $"Single: {x}",
    [var x, var y] => $"Pair: {x}, {y}",
    [1, .., 5] => "1 to 5",
    [.., var last] => $"Ends with {last}",
    _ => "Other"
};

// 10. TUPLE PATTERNS
var tuple = (1, "Hello");

if (tuple is (1, "Hello"))
    Console.WriteLine("Match");

if (tuple is (var num, var text))
    Console.WriteLine($"{num}: {text}");

// Tuple pattern in switch
string Describe((int x, int y) point) => point switch
{
    (0, 0) => "Origin",
    (0, _) => "Y-axis",
    (_, 0) => "X-axis",
    var (x, y) when x == y => $"Diagonal at {x}",
    var (x, y) => $"Point ({x}, {y})"
};

// 11. WHEN GUARDS (with any pattern)
string Classify(int number) => number switch
{
    var n when n < 0 => "Negative",
    var n when n == 0 => "Zero",
    var n when n > 0 and n < 10 => "Small positive",
    var n when n >= 10 => "Large positive",
    _ => "Unknown"
};

// Complex when guard
if (person is { Age: var age } when age >= 18 && age < 65)
    Console.WriteLine("Working age");

// 12. RECURSIVE PATTERNS
public record Node(int Value, Node? Left, Node? Right);

int Depth(Node? node) => node switch
{
    null => 0,
    { Left: null, Right: null } => 1,
    { Left: var left, Right: var right } => 
        1 + Math.Max(Depth(left), Depth(right))
};

// 13. PRACTICAL EXAMPLES

// Example 1: Command processing
string ProcessCommand(string cmd) => cmd.Trim().ToLower() switch
{
    "exit" or "quit" => "Goodbye!",
    "help" or "?" => "Available commands: help, exit, list",
    var c when c.StartsWith("add ") => $"Adding {c[4..]}",
    var c when c.StartsWith("delete ") => $"Deleting {c[7..]}",
    _ => "Unknown command"
};

// Example 2: HTTP status handling
string StatusMessage(int status) => status switch
{
    200 => "OK",
    201 => "Created",
    400 => "Bad Request",
    401 => "Unauthorized",
    404 => "Not Found",
    >= 400 and < 500 => "Client Error",
    >= 500 => "Server Error",
    _ => "Unknown"
};

// Example 3: Shape area calculation
public abstract record Shape;
public record Circle(double Radius) : Shape;
public record Rectangle(double Width, double Height) : Shape;
public record Triangle(double Base, double Height) : Shape;

double Area(Shape shape) => shape switch
{
    Circle { Radius: var r } => Math.PI * r * r,
    Rectangle { Width: var w, Height: var h } => w * h,
    Triangle { Base: var b, Height: var h } => 0.5 * b * h,
    _ => throw new ArgumentException("Unknown shape")
};

// Example 4: Validation
bool IsValidEmail(string email) => email switch
{
    null or "" => false,
    var e when !e.Contains('@') => false,
    var e when e.Split('@').Length != 2 => false,
    var e when e.Split('@')[1].Length < 3 => false,
    _ => true
};

// Example 5: Traffic light state
public enum LightState { Red, Yellow, Green }

LightState NextState(LightState current) => current switch
{
    LightState.Red => LightState.Green,
    LightState.Green => LightState.Yellow,
    LightState.Yellow => LightState.Red,
    _ => throw new InvalidOperationException()
};

// Example 6: Vehicle processing (for TrafficSim)
public enum Vehicle { Car, Truck, Bus, Motorcycle }

int GetCapacityUsage(Vehicle v) => v switch
{
    Vehicle.Motorcycle => 1,
    Vehicle.Car => 2,
    Vehicle.Truck or Vehicle.Bus => 3,
    _ => throw new ArgumentException()
};

// Example 7: Booking validation (for CinemaApp)
bool CanBook(Show show, int seatNumber) => (show, seatNumber) switch
{
    (null, _) => false,
    (_, < 1) => false,
    ({ Room.Capacity: var capacity }, var seat) when seat > capacity => false,
    ({ BookedSeats: var booked }, var seat) when booked.Contains(seat) => false,
    _ => true
};
```

---

## 🐛 Common Mistakes and Solutions

### Mistake 1: Off-by-One Errors

```csharp
// ❌ Wrong: Excludes last element
for (int i = 0; i < array.Length - 1; i++)

// ✅ Correct: Includes all elements
for (int i = 0; i < array.Length; i++)

// ❌ Wrong: Seat 0 is invalid
if (seat >= 1 && seat < capacity)

// ✅ Correct: Seats 1 to capacity (inclusive)
if (seat >= 1 && seat <= capacity)
```

---

### Mistake 2: Comparing Floating Point

```csharp
// ❌ Wrong: Floating point comparison
if (result == 0.0)

// ✅ Better: Use epsilon for comparison
const double epsilon = 0.0001;
if (Math.Abs(result) < epsilon)
```

---

### Mistake 3: Modifying Collection While Iterating

```csharp
// ❌ Wrong: Modifies list while iterating
foreach (var item in list)
{
    if (condition)
        list.Remove(item);  // CRASH!
}

// ✅ Correct: Create copy first
foreach (var item in list.ToList())
{
    if (condition)
        list.Remove(item);
}

// ✅ Or use RemoveAll
list.RemoveAll(item => condition);
```

---

### Mistake 4: Null Reference

```csharp
// ❌ Wrong: Might be null
var show = shows.FirstOrDefault(s => s.Id == id);
Console.WriteLine(show.Movie.Title);  // NullReferenceException!

// ✅ Correct: Check for null
var show = shows.FirstOrDefault(s => s.Id == id);
if (show is not null)
    Console.WriteLine(show.Movie.Title);

// ✅ Or use null-conditional operator
Console.WriteLine(show?.Movie.Title ?? "Not found");
```

---

### Mistake 5: Integer Division

```csharp
// ❌ Wrong: Integer division
int a = 5;
int b = 2;
double result = a / b;  // 2.0 (not 2.5!)

// ✅ Correct: Cast to double first
double result = (double)a / b;  // 2.5
```

---

### Mistake 6: DateTime Comparison

```csharp
// ❌ Wrong: Compares exact DateTime
if (show.StartTime == date)  // Will never match!

// ✅ Correct: Compare DateOnly
DateOnly showDate = DateOnly.FromDateTime(show.StartTime);
if (showDate == date)  // Works!
```

---

### Mistake 7: Empty Array Handling

```csharp
// ❌ Wrong: Throws exception on empty array
double sum = args.Sum();  // OK
double first = args[0];   // Index out of range!

// ✅ Correct: Check length first
if (args.Length == 0)
    return defaultValue;
double first = args[0];
```

---

## 🎓 Design Pattern Quick Reference

### Strategy Pattern (SciCalc)

**When to use:** Multiple algorithms for same task

```csharp
// 1. Define interface
public interface IOperation
{
    string Name { get; }
    double Evaluate(params double[] args);
}

// 2. Implement strategies
public class Add : IOperation { ... }
public class Sub : IOperation { ... }

// 3. Use polymorphically
IOperation op = GetOperation("add");
double result = op.Evaluate(2, 3);
```

---

### Factory Pattern (SciCalc)

**When to use:** Create objects based on input

```csharp
public static class OperationFactory
{
    private static Dictionary<string, IOperation> ops = new()
    {
        ["add"] = new Add(),
        ["sub"] = new Sub(),
    };
    
    public static IOperation? Create(string name)
    {
        return ops.GetValueOrDefault(name);
    }
}
```

---

### Repository Pattern (CinemaApp)

**When to use:** Abstract data access

```csharp
// 1. Define interface
public interface IShowStore
{
    void Add(Show show);
    Show? Find(Guid id);
    Show[] FindByDate(DateOnly date);
}

// 2. Implement (in-memory, database, etc.)
public class InMemoryShowStore : IShowStore
{
    private List<Show> _shows = new();
    
    public void Add(Show show)
    {
        _shows.Add(show);
    }
    // ...
}
```

---

### State Machine (TrafficSim)

**When to use:** Object behavior depends on state

```csharp
public enum LightState { Red, Green, Yellow }

public class TrafficLight
{
    private LightState _state = LightState.Red;
    
    public void Iterate()
    {
        _state = _state switch
        {
            LightState.Red => LightState.Green,
            LightState.Green => LightState.Yellow,
            LightState.Yellow => LightState.Red,
            _ => LightState.Red
        };
    }
}
```

---

## 🔍 Debugging Techniques

### Technique 1: Print Debugging

```csharp
Console.WriteLine($"DEBUG: Variable x = {x}");
Console.WriteLine($"DEBUG: Array length = {args.Length}");
Console.WriteLine($"DEBUG: Entering method {nameof(TryBook)}");
```

### Technique 2: Breakpoint Debugging

1. Set breakpoint (click left margin in IDE)
2. Run in debug mode (F5)
3. Step through code (F10 = step over, F11 = step into)
4. Inspect variables in watch window

### Technique 3: Unit Tests

```csharp
[Fact]
public void Add_WithTwoNumbers_ReturnsSum()
{
    var add = new Add();
    var result = add.Evaluate(2, 3);
    Assert.Equal(5, result);
}
```

---

## 📚 Helpful Resources

### Official Documentation
- [C# Language Reference](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [LINQ Overview](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
- [DateTime and TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.datetime)

### Algorithms
- [Greedy Algorithms](https://en.wikipedia.org/wiki/Greedy_algorithm)
- [Dijkstra's Algorithm](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm)
- [Interval Scheduling](https://en.wikipedia.org/wiki/Interval_scheduling)

### Visualizers
- [Algorithm Visualizer](https://algorithm-visualizer.org/)
- [Dijkstra Visualizer](https://www.cs.usfca.edu/~galles/visualization/Dijkstra.html)

---

## 💪 Challenge Yourself

### After Basic Implementation:

1. **Add unit tests** for all operations
2. **Implement persistence** (save/load from JSON)
3. **Add more features:**
   - SciCalc: Variables, memory, history
   - CinemaApp: User accounts, reviews, seat maps
   - TrafficSim: Emergency vehicles, accidents, rush hour
4. **Optimize algorithms** (better time/space complexity)
5. **Add GUI** (use Avalonia or WPF)
6. **Try bonus challenges** (see docs/ folder)

---

## 🔧 Advanced C# Features

### Extension Methods

```csharp
// Extension methods add functionality to existing types without modifying them

// Define extension methods (must be in static class)
public static class StringExtensions
{
    // 'this' keyword on first parameter makes it an extension method
    public static bool IsValidEmail(this string email)
    {
        return email.Contains('@') && email.Split('@').Length == 2;
    }
    
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }
    
    public static int WordCount(this string text)
    {
        return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// Usage - looks like a regular method!
string email = "user@example.com";
bool isValid = email.IsValidEmail();  // Calls extension method

string longText = "This is a very long text";
string shortened = longText.Truncate(10);  // "This is a ..."

// Extension methods for your projects
public static class CollectionExtensions
{
    // For CinemaApp - check if list is empty
    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return !source.Any();
    }
    
    // For TrafficSim - get random element
    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Shared.Next(list.Count)];
    }
    
    // For any project - safe index access
    public static T? GetSafe<T>(this List<T> list, int index)
    {
        return index >= 0 && index < list.Count ? list[index] : default;
    }
}

// LINQ is full of extension methods!
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var even = numbers.Where(n => n % 2 == 0);  // Where is an extension method!
var doubled = numbers.Select(n => n * 2);   // Select is an extension method!
```

### Indexers and Ranges

```csharp
// Index from end operator (^)
string text = "Hello World";
char lastChar = text[^1];      // 'd' (last character)
char secondLast = text[^2];    // 'l' (second from end)

int[] numbers = { 1, 2, 3, 4, 5 };
int last = numbers[^1];        // 5
int secondLast = numbers[^2];  // 4

// Range operator (..)
string hello = text[0..5];     // "Hello" (start to 5)
string world = text[6..11];    // "World" (6 to end)
string all = text[..];         // entire string
string fromSecond = text[1..]; // "ello World" (from index 1)
string toFourth = text[..4];   // "Hell" (up to index 4)

// Combining Index and Range
string lastWord = text[^5..];  // "World" (last 5 chars)
string middle = text[2..^2];   // "llo Wor" (skip 2 from each end)

// With arrays
int[] slice = numbers[1..4];   // { 2, 3, 4 }
int[] last3 = numbers[^3..];   // { 3, 4, 5 }

// Custom indexers in your classes
public class CinemaRoom
{
    private Dictionary<int, Seat> _seats = new();
    
    // Indexer - access like an array!
    public Seat this[int seatNumber]
    {
        get => _seats[seatNumber];
        set => _seats[seatNumber] = value;
    }
    
    // You can have multiple indexers with different types
    public Seat this[int row, int column]
    {
        get => _seats[row * 10 + column];
    }
}

// Usage
var room = new CinemaRoom();
room[1] = new Seat(1, false);        // Set using indexer
var seat = room[1];                   // Get using indexer
var seatByPosition = room[1, 5];     // Multi-dimensional indexer

// Practical example for TrafficSim
public class IntersectionGrid
{
    private Vehicle?[,] _grid;
    
    public IntersectionGrid(int rows, int cols)
    {
        _grid = new Vehicle?[rows, cols];
    }
    
    // 2D indexer
    public Vehicle? this[int x, int y]
    {
        get => _grid[x, y];
        set => _grid[x, y] = value;
    }
}

var grid = new IntersectionGrid(10, 10);
grid[5, 5] = new Vehicle("Car");
```

### String Interpolation Advanced

```csharp
// Basic interpolation
string name = "Alice";
int age = 25;
string message = $"Hello, {name}! You are {age} years old.";

// Expressions in interpolation
string result = $"5 + 3 = {5 + 3}";
string upper = $"UPPERCASE: {name.ToUpper()}";

// Format specifiers
decimal price = 19.99m;
Console.WriteLine($"Price: {price:C}");        // Currency: $19.99
Console.WriteLine($"Price: {price:F2}");       // Fixed: 19.99
Console.WriteLine($"Price: {price:N}");        // Number: 19.99

int number = 42;
Console.WriteLine($"Hex: {number:X}");         // Hex: 2A
Console.WriteLine($"Padded: {number:D5}");     // Decimal: 00042

double percent = 0.75;
Console.WriteLine($"Percent: {percent:P}");    // 75.00%
Console.WriteLine($"Percent: {percent:P0}");   // 75%

// Alignment
Console.WriteLine($"|{name,10}|");             // Right-aligned in 10 chars
Console.WriteLine($"|{name,-10}|");            // Left-aligned in 10 chars

// Date formatting
DateTime now = DateTime.Now;
Console.WriteLine($"Date: {now:yyyy-MM-dd}");       // 2025-10-16
Console.WriteLine($"Time: {now:HH:mm:ss}");         // 14:30:45
Console.WriteLine($"Custom: {now:MMM dd, yyyy}");   // Oct 16, 2025

// Conditional formatting
string status = age >= 18 ? "Adult" : "Minor";
Console.WriteLine($"Status: {status}");

// Verbatim string interpolation
string path = $@"C:\Users\{name}\Documents";

// Raw string literals (C# 11+)
string json = """
    {
        "name": "Alice",
        "age": 25
    }
    """;

// Multi-line with interpolation
string report = $"""
    Name: {name}
    Age: {age}
    Status: {status}
    """;

// Format table output
Console.WriteLine($"{"Name",-15} {"Age",5} {"City",-10}");
Console.WriteLine($"{name,-15} {age,5} {"NYC",-10}");
```

### Local Functions

```csharp
// Functions defined inside other functions

public int Calculate(int[] numbers, string operation)
{
    // Local function - only accessible within Calculate
    int Sum(int[] nums)
    {
        int total = 0;
        foreach (var n in nums) total += n;
        return total;
    }
    
    // Another local function
    int Product(int[] nums)
    {
        int result = 1;
        foreach (var n in nums) result *= n;
        return result;
    }
    
    // Use local functions
    return operation switch
    {
        "sum" => Sum(numbers),
        "product" => Product(numbers),
        _ => 0
    };
}

// Local functions can be recursive
public int Factorial(int n)
{
    int FactorialHelper(int num, int accumulator)
    {
        if (num <= 1) return accumulator;
        return FactorialHelper(num - 1, num * accumulator);
    }
    
    return FactorialHelper(n, 1);
}

// Local functions capture variables
public void ProcessItems(List<string> items)
{
    int count = 0;
    
    void IncrementAndLog()
    {
        count++;  // Can access 'count' from outer scope
        Console.WriteLine($"Processed {count} items");
    }
    
    foreach (var item in items)
    {
        // Process item
        IncrementAndLog();
    }
}

// Practical example for CinemaApp
public bool BookSeats(Show show, List<int> seatNumbers)
{
    // Local validation function
    bool IsValidSeat(int seatNum)
    {
        return seatNum > 0 && 
               seatNum <= show.Room.Capacity &&
               !show.BookedSeats.Contains(seatNum);
    }
    
    // Check all seats
    if (!seatNumbers.All(IsValidSeat))
        return false;
    
    // Book all seats
    foreach (var seat in seatNumbers)
        show.BookedSeats.Add(seat);
    
    return true;
}
```

### nameof and typeof

```csharp
// nameof - get string name of variable/type/member
// Useful for error messages and validation

public void ProcessUser(string userName)
{
    if (userName is null)
        throw new ArgumentNullException(nameof(userName));  // "userName"
    
    if (userName.Length < 3)
        throw new ArgumentException($"{nameof(userName)} too short");
}

// Refactoring-safe property names
public class Person
{
    public string Name { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrEmpty(Name))
            throw new InvalidOperationException(
                $"{nameof(Name)} is required");  // "Name"
    }
}

// typeof - get Type object
Type personType = typeof(Person);
Console.WriteLine(personType.Name);  // "Person"

// Compare types
if (obj.GetType() == typeof(string))
    Console.WriteLine("It's a string");

// Generic type checking
public void Process<T>(T item)
{
    Console.WriteLine($"Processing {typeof(T).Name}");
}

// Practical usage
public class ValidationException : Exception
{
    public string PropertyName { get; }
    
    public ValidationException(string propertyName, string message)
        : base($"{propertyName}: {message}")
    {
        PropertyName = propertyName;
    }
}

// Usage
if (movie.Duration <= TimeSpan.Zero)
    throw new ValidationException(
        nameof(movie.Duration), 
        "Duration must be positive");
```

### Switch Expressions vs Statements

```csharp
// Switch STATEMENT (old way)
string GetDayType(DayOfWeek day)
{
    string result;
    switch (day)
    {
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday:
            result = "Weekend";
            break;
        case DayOfWeek.Monday:
        case DayOfWeek.Tuesday:
        case DayOfWeek.Wednesday:
        case DayOfWeek.Thursday:
        case DayOfWeek.Friday:
            result = "Weekday";
            break;
        default:
            result = "Unknown";
            break;
    }
    return result;
}

// Switch EXPRESSION (modern way) - much cleaner!
string GetDayType2(DayOfWeek day) => day switch
{
    DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
    DayOfWeek.Monday or DayOfWeek.Tuesday or 
    DayOfWeek.Wednesday or DayOfWeek.Thursday or 
    DayOfWeek.Friday => "Weekday",
    _ => "Unknown"
};

// Switch expression with pattern matching
decimal CalculateFee(Vehicle vehicle) => vehicle switch
{
    { Type: VehicleType.Motorcycle } => 5.00m,
    { Type: VehicleType.Car, Passengers: <= 2 } => 10.00m,
    { Type: VehicleType.Car } => 15.00m,
    { Type: VehicleType.Bus } => 20.00m,
    _ => 0m
};

// Switch expression can be assigned
var category = age switch
{
    < 13 => "Child",
    < 20 => "Teen",
    < 65 => "Adult",
    _ => "Senior"
};

// Prefer switch expressions for:
// - Returning a value based on pattern
// - Cleaner, more concise code
// - Type checking and pattern matching

// Use switch statements for:
// - Multiple statements per case
// - Side effects (void methods)
// - Very complex logic per case
```

### Yield Return (Lazy Evaluation)

```csharp
// yield return creates iterators - values computed on-demand!

// Without yield (eager evaluation)
public static List<int> GetSquares(int max)
{
    var result = new List<int>();
    for (int i = 1; i <= max; i++)
    {
        result.Add(i * i);  // Computes ALL values immediately
    }
    return result;
}

// With yield (lazy evaluation)
public static IEnumerable<int> GetSquaresLazy(int max)
{
    for (int i = 1; i <= max; i++)
    {
        yield return i * i;  // Computes values only when requested
    }
}

// Usage difference
var eager = GetSquares(1000000);    // Computes 1M values NOW
var lazy = GetSquaresLazy(1000000); // Doesn't compute anything yet!

// Only computes what's needed
var first10 = lazy.Take(10).ToList();  // Only computes 10 values!

// Practical example: Infinite sequence
public static IEnumerable<int> Fibonacci()
{
    int a = 0, b = 1;
    while (true)  // Infinite loop!
    {
        yield return a;
        (a, b) = (b, a + b);
    }
}

// Take only what you need
var first20Fib = Fibonacci().Take(20).ToList();

// For TrafficSim: Generate vehicles on demand
public IEnumerable<Vehicle> GenerateTraffic()
{
    while (true)
    {
        yield return new Vehicle
        {
            Type = Random.Shared.Next(4) switch
            {
                0 => VehicleType.Car,
                1 => VehicleType.Truck,
                2 => VehicleType.Bus,
                _ => VehicleType.Motorcycle
            }
        };
    }
}

// Use only what simulation needs
var next100Vehicles = GenerateTraffic().Take(100);

// For CinemaApp: Generate time slots
public IEnumerable<DateTime> GetAvailableSlots(DateTime start)
{
    var current = start;
    while (current.Hour < 23)
    {
        yield return current;
        current = current.AddMinutes(30);
    }
}

// yield break - exit early
public IEnumerable<int> GetPrimes(int max)
{
    for (int num = 2; num <= max; num++)
    {
        if (num > 1000)
            yield break;  // Stop generating
            
        if (IsPrime(num))
            yield return num;
    }
}
```

### Const vs ReadOnly vs Static

```csharp
// const - compile-time constant (must be initialized)
public class Circle
{
    public const double Pi = 3.14159;  // Cannot change, ever!
    
    // const values are implicitly static
    // Circle.Pi ✓
}

// readonly - runtime constant (can be set in constructor)
public class Configuration
{
    public readonly string ConnectionString;  // Can only set once
    
    public Configuration(string connStr)
    {
        ConnectionString = connStr;  // OK in constructor
        // ConnectionString = "new";  // ERROR after construction!
    }
}

// static - shared across all instances
public class Counter
{
    public static int TotalCount = 0;  // Shared by all Counter objects
    public int InstanceCount = 0;      // Each instance has its own
    
    public Counter()
    {
        TotalCount++;    // Affects ALL instances
        InstanceCount++; // Only affects THIS instance
    }
}

var c1 = new Counter();  // TotalCount = 1, c1.InstanceCount = 1
var c2 = new Counter();  // TotalCount = 2, c2.InstanceCount = 1

// static readonly - shared constant, set at runtime
public class Settings
{
    public static readonly string AppName = GetAppName();
    
    static string GetAppName()
    {
        return "CinemaApp";  // Computed at runtime, but only once
    }
}

// When to use each:
// const - mathematical constants, never change (Pi, E, etc.)
// readonly - configuration that shouldn't change after construction
// static - shared data/methods across all instances
// static readonly - shared constants computed at runtime
```

---

## ⚡ Performance Tips

### Big O Complexity Quick Reference

```csharp
// O(1) - Constant time
dict[key]           // Dictionary lookup
hashSet.Contains()  // HashSet check
list[index]         // Array/List index access

// O(log n) - Logarithmic time
sortedSet.Contains()      // Binary search
Array.BinarySearch()      // Binary search

// O(n) - Linear time
list.Contains()     // List search
array.Sum()         // Iterate all elements
list.Where()        // Filter collection

// O(n log n) - Log-linear time
list.OrderBy()      // Efficient sorting
Array.Sort()        // QuickSort/MergeSort

// O(n²) - Quadratic time
// Nested loops over same collection
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++)
        // ...
```

### Optimization Techniques

```csharp
// ❌ Bad: Multiple enumerations
var expensive = items.Where(x => x.Price > 100);
int count = expensive.Count();  // Enumerates
var first = expensive.First();  // Enumerates again!

// ✅ Good: Materialize once
var expensive = items.Where(x => x.Price > 100).ToList();
int count = expensive.Count;    // No enumeration
var first = expensive[0];       // Direct access

// ❌ Bad: String concatenation in loop
string result = "";
for (int i = 0; i < 1000; i++)
    result += i.ToString();  // Creates new string each time!

// ✅ Good: StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
    sb.Append(i);
string result = sb.ToString();

// ❌ Bad: Unnecessary boxing
object obj = 5;  // Boxing (int → object)
int num = (int)obj;  // Unboxing

// ✅ Good: Use generics
List<int> numbers = new();  // No boxing

// ❌ Bad: Creating objects in loop
for (int i = 0; i < 1000; i++)
{
    var random = new Random();  // New instance each time!
    int value = random.Next();
}

// ✅ Good: Reuse objects
var random = new Random();
for (int i = 0; i < 1000; i++)
{
    int value = random.Next();
}

// ❌ Bad: Contains in List (O(n))
List<int> numbers = GetLargeList();
if (numbers.Contains(target))  // Slow!

// ✅ Good: Contains in HashSet (O(1))
HashSet<int> numbers = GetLargeList().ToHashSet();
if (numbers.Contains(target))  // Fast!
```

### Memory Management

```csharp
// Use structs for small, immutable data
public struct Point  // Value type
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// Use readonly when possible
public readonly struct ReadOnlyPoint
{
    public int X { get; }
    public int Y { get; }
}

// Span<T> for efficient array operations (C# 7.2+)
int[] numbers = { 1, 2, 3, 4, 5 };
Span<int> slice = numbers.AsSpan(1, 3);  // No allocation!

// ArrayPool for temporary arrays
using System.Buffers;

int[] temp = ArrayPool<int>.Shared.Rent(1000);
try
{
    // Use temp array
}
finally
{
    ArrayPool<int>.Shared.Return(temp);
}
```

### Algorithm Selection

```csharp
// Searching
// - Unsorted data: O(n) - linear search
// - Sorted data: O(log n) - binary search
// - Hash-based: O(1) - HashSet/Dictionary

// Sorting
// - Small arrays (<50): Insertion sort
// - General purpose: QuickSort/MergeSort O(n log n)
// - Nearly sorted: Insertion sort
// - Stable sort needed: MergeSort

// Data structure selection
// - Frequent lookups: Dictionary/HashSet
// - Sequential access: List
// - Ordered data: SortedSet/SortedDictionary
// - FIFO: Queue
// - LIFO: Stack
```

---

## 🎨 Best Practices

### Naming Conventions

```csharp
// PascalCase for public members
public class PersonManager { }
public void CalculateTotal() { }
public int TotalCount { get; set; }

// camelCase for private fields (with underscore)
private int _count;
private string _name;

// camelCase for parameters and locals
public void Process(int itemCount)
{
    var localVariable = 5;
}

// ALL_CAPS for constants
public const int MAX_SIZE = 100;

// Interfaces start with 'I'
public interface IOperation { }
public interface IRepository { }

// Descriptive names
// ❌ Bad
int d;  // What is 'd'?
var temp;
var x;

// ✅ Good
int daysUntilExpiry;
var temperatureCelsius;
var customerAge;
```

### Code Organization

```csharp
// One class per file
// File: Person.cs
public class Person { }

// Related classes in same namespace
namespace MyApp.Models
{
    public class Person { }
    public class Address { }
}

// Organize using statements
using System;                    // System first
using System.Collections.Generic;
using System.Linq;

using MyApp.Models;             // Then project namespaces
using MyApp.Services;

using Newtonsoft.Json;          // Then third-party
```

### Error Messages

```csharp
// ❌ Bad: Vague error
throw new Exception("Error!");

// ✅ Good: Specific and helpful
throw new ArgumentException(
    "Seat number must be between 1 and 100",
    nameof(seatNumber)
);

// ✅ Good: Include context
throw new InvalidOperationException(
    $"Cannot book seat {seatNumber} - already reserved by {bookedBy}"
);
```

### Comments and Documentation

```csharp
// ❌ Bad: Obvious comments
// Increment i
i++;

// ❌ Bad: Commented-out code
// var oldWay = DoSomething();
var newWay = DoSomethingBetter();

// ✅ Good: Explain WHY, not WHAT
// Use binary search because list is already sorted
int index = Array.BinarySearch(sorted, target);

// ✅ Good: XML documentation
/// <summary>
/// Calculates the shortest path between two points using Dijkstra's algorithm.
/// </summary>
/// <param name="start">Starting position</param>
/// <param name="goal">Target position</param>
/// <returns>List of positions representing the path</returns>
public List<Position> FindPath(Position start, Position goal)
{
    // Implementation
}
```

### SOLID Principles Quick Reference

```csharp
// Single Responsibility Principle
// ❌ Bad: Class does too much
public class UserManager
{
    public void CreateUser() { }
    public void DeleteUser() { }
    public void SendEmail() { }      // Should be separate!
    public void GenerateReport() { }  // Should be separate!
}

// ✅ Good: Focused responsibilities
public class UserRepository
{
    public void Create(User user) { }
    public void Delete(int id) { }
}

public class EmailService
{
    public void Send(string to, string subject, string body) { }
}

// Open/Closed Principle
// Open for extension, closed for modification
public interface IOperation
{
    double Evaluate(params double[] args);
}

// Add new operations without modifying existing code
public class Add : IOperation { }
public class Multiply : IOperation { }

// Liskov Substitution Principle
// Derived classes should be substitutable for base classes
public abstract class Shape
{
    public abstract double Area { get; }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    public override double Area => Math.PI * Radius * Radius;
}

// Interface Segregation Principle
// Many specific interfaces better than one general
// ❌ Bad: Fat interface
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

// ✅ Good: Segregated interfaces
public interface IWorkable { void Work(); }
public interface IFeedable { void Eat(); }

// Dependency Inversion Principle
// Depend on abstractions, not concretions
// ❌ Bad: Depends on concrete class
public class ReportGenerator
{
    private MySqlDatabase _db = new MySqlDatabase();  // Tight coupling!
}

// ✅ Good: Depends on interface
public class ReportGenerator
{
    private IDatabase _db;
    
    public ReportGenerator(IDatabase db)
    {
        _db = db;
    }
}
```

---

## 🔍 Testing Your Code

### Manual Testing Tips

```csharp
// Test edge cases
// - Empty input
// - Single item
// - Maximum capacity
// - Negative numbers
// - Null values
// - Boundary conditions

// Example: Testing Add operation
public void TestAdd()
{
    var add = new Add();
    
    // Normal case
    Assert.Equal(5, add.Evaluate(2, 3));
    
    // Edge case: empty
    Assert.Equal(0, add.Evaluate());
    
    // Edge case: single number
    Assert.Equal(5, add.Evaluate(5));
    
    // Edge case: negatives
    Assert.Equal(-1, add.Evaluate(-3, 2));
    
    // Edge case: many numbers
    Assert.Equal(10, add.Evaluate(1, 2, 3, 4));
}
```

### Debug Output

```csharp
// Use Console.WriteLine for debugging
Console.WriteLine($"DEBUG: count = {count}");
Console.WriteLine($"DEBUG: Entering {nameof(CalculatePath)}");

// Use Debug.Assert for assumptions
System.Diagnostics.Debug.Assert(count > 0, "Count should be positive");

// Use try-catch for error details
try
{
    var result = RiskyOperation();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    throw;  // Re-throw to preserve stack trace
}
```

---

## 📚 Additional Resources

### Official Documentation
- **C# Language Tour**: https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
- **C# Programming Guide**: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/
- **LINQ Documentation**: https://learn.microsoft.com/en-us/dotnet/csharp/linq/
- **.NET API Browser**: https://learn.microsoft.com/en-us/dotnet/api/

### Learning Paths
- **C# Fundamentals**: Learn variables, types, control flow
- **OOP in C#**: Master classes, inheritance, interfaces
- **LINQ Mastery**: Query and transform data efficiently
- **Async/Await**: Write asynchronous code (not needed for this project)
- **Design Patterns**: Improve code architecture

### Common Algorithms
- **Sorting**: QuickSort, MergeSort, BubbleSort
- **Searching**: Binary Search, Linear Search
- **Graph**: BFS, DFS, Dijkstra, A*
- **Greedy**: Interval Scheduling, Huffman Coding
- **Dynamic Programming**: Fibonacci, Knapsack

### Practice Platforms
- **LeetCode**: https://leetcode.com/
- **HackerRank**: https://www.hackerrank.com/domains/algorithms
- **Exercism C#**: https://exercism.org/tracks/csharp
- **Coding Game**: https://www.codingame.com/

---

## 🎯 Quick Lookup Table

### When to Use What

| Need | Use |
|------|-----|
| Fast lookups | `HashSet<T>`, `Dictionary<K,V>` |
| Ordered iteration | `List<T>`, `SortedSet<T>` |
| Unique items | `HashSet<T>`, `SortedSet<T>` |
| FIFO processing | `Queue<T>` |
| LIFO processing | `Stack<T>` |
| Key-value pairs | `Dictionary<K,V>` |
| Sorted data | `SortedSet<T>`, `SortedDictionary<K,V>` |
| Immutable data | `record`, `readonly struct` |
| Multiple return values | Tuples `(int, string)` |
| Optional values | Nullable `int?`, `string?` |
| Filter collection | LINQ `.Where()` |
| Transform collection | LINQ `.Select()` |
| Sum/Aggregate | LINQ `.Sum()`, `.Aggregate()` |
| Find first match | LINQ `.FirstOrDefault()` |
| Sort collection | LINQ `.OrderBy()` |

---

## 💡 Pro Tips for This Project

### SciCalc Tips
```csharp
// Use params for variable arguments
public double Evaluate(params double[] args)

// Use LINQ for array operations
return args.Length == 0 ? 0 : args.Sum();

// Convert degrees to radians
double radians = degrees * Math.PI / 180.0;

// Handle special cases first
if (args.Length == 0) return 0;
if (args.Length == 1) return args[0];
```

### CinemaApp Tips
```csharp
// Use HashSet for seat tracking (O(1) lookups)
private readonly HashSet<int> _bookedSeats = new();

// Check overlaps with time comparison
if (show.Start >= lastEnd)  // No overlap

// Sort by end time for greedy algorithm
var sorted = shows.OrderBy(s => s.End);

// Use DateOnly for date comparisons
DateOnly date = DateOnly.FromDateTime(show.Start);
```

### TrafficSim Tips
```csharp
// Use tuples for coordinates
var position = (x: 0, y: 0);
var neighbor = (position.x + 1, position.y);

// Use Dictionary for grid
Dictionary<(int, int), CrossRoad> grid = new();

// Use enum for states
public enum LightState { Red, Green, Yellow }

// Use Queue for traffic flow
Queue<Vehicle> queue = new();

// Use switch expression for state transitions
_state = _state switch
{
    LightState.Red => LightState.Green,
    LightState.Green => LightState.Yellow,
    LightState.Yellow => LightState.Red,
    _ => LightState.Red
};
```

---

## 🎓 Final Words

### Remember:
- **Start simple** - Get it working first, optimize later
- **Test frequently** - Run your code after every change
- **Read errors carefully** - They tell you what's wrong
- **Use debugger** - Step through code to understand flow
- **Ask for help** - Don't struggle alone for hours
- **Learn by doing** - Practice is the best teacher

### When Stuck:
1. **Read the error message** - What is it telling you?
2. **Check the TODO comments** - They have hints!
3. **Look at examples** - `skeleton-examples/` folder
4. **Search documentation** - Use Microsoft docs
5. **Try simpler version** - Break down the problem
6. **Ask for help** - Course forum, office hours

### Success Checklist:
- [ ] Code compiles without errors
- [ ] All test cases pass
- [ ] Edge cases handled
- [ ] Meaningful variable names
- [ ] Comments for complex logic
- [ ] No compiler warnings
- [ ] Follows C# conventions
- [ ] Design patterns used correctly

---

*Remember: Programming is learned by doing! Don't be afraid to experiment and make mistakes. That's how we learn! Every expert was once a beginner. Keep coding! 🚀*

---

**Last Updated**: October 16, 2025  
**Version**: 2.0 - Comprehensive C# Reference Edition

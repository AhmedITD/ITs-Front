# 🔑 Key Implementation Highlights

## 1. Show Class - Seat Booking Logic

```csharp
public sealed class Show
{
    public Guid Id { get; } = Guid.NewGuid();
    public Movie Movie { get; }
    public Room Room { get; }
    public DateTime Start { get; }
    public DateTime End => Start + Movie.Duration;  // Calculated property
    public decimal Price { get; }
    
    private readonly HashSet<int> _taken = new();  // O(1) lookup
    public int AvailableSeats => Room.Capacity - _taken.Count;
    
    public bool TryBook(params int[] seats)
    {
        // Atomic transaction - all or nothing
        if (seats.Length == 0) return false;
        if (seats.Any(s => s < 1 || s > Room.Capacity)) return false;
        if (seats.Any(_taken.Contains)) return false;  // Prevent double-booking
        
        foreach (var seat in seats)
            _taken.Add(seat);
        
        return true;
    }
}
```

**Key Points:**
- ✅ Immutable properties (except _taken which is private)
- ✅ HashSet for O(1) duplicate checking
- ✅ Atomic booking (no partial bookings)
- ✅ Clear separation of concerns

---

## 2. InMemoryShowStore - Repository Pattern

```csharp
public sealed class InMemoryShowStore : IShowStore
{
    private readonly List<Show> _shows = new();
    
    public IEnumerable<Show> All() => _shows;
    
    public void Add(Show show) => _shows.Add(show);
    
    public Show? Find(Guid id) => _shows.FirstOrDefault(s => s.Id == id);
}
```

**Key Points:**
- ✅ Simple list-based storage
- ✅ Easy to swap for JSON/Database implementation
- ✅ Implements IShowStore interface
- ✅ LINQ for queries

---

## 3. GreedyMarathonPlanner - Interval Scheduling

```csharp
public List<Show> Plan(DateOnly day, IEnumerable<Show> shows)
{
    // 1. Filter shows for the day
    var dayShows = shows
        .Where(s => DateOnly.FromDateTime(s.Start) == day)
        .OrderBy(s => s.End)      // SORT BY END TIME (key insight!)
        .ThenBy(s => s.Start)
        .ToList();
    
    // 2. Greedy selection
    var plan = new List<Show>();
    DateTime currentEndTime = DateTime.MinValue;
    
    foreach (var show in dayShows)
    {
        if (show.Start >= currentEndTime)  // No overlap?
        {
            plan.Add(show);
            currentEndTime = show.End;
        }
    }
    
    return plan;
}
```

**Why This Works:**
- ✅ Sorting by END TIME is key to greedy approach
- ✅ Finishing early leaves more time for next movie
- ✅ Proven optimal for interval scheduling problem
- ✅ Time complexity: O(n log n)

---

## 4. App.cs - REPL Command Loop

```csharp
public static void Run()
{
    Console.WriteLine("Commands: add-room, add-movie, add-show, list-shows, book, marathon, help, exit");
    
    for (;;)  // Infinite loop until exit
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line)) continue;
        
        var parts = Split(line);  // Handle quoted strings
        if (parts.Length == 0) continue;
        
        var cmd = parts[0].ToLowerInvariant();
        
        try
        {
            switch (cmd)
            {
                case "exit": return;
                case "help": PrintHelp(); break;
                case "add-room": HandleAddRoom(parts); break;
                case "add-movie": HandleAddMovie(parts); break;
                case "add-show": HandleAddShow(parts); break;
                case "list-shows": HandleListShows(parts); break;
                case "book": HandleBook(parts); break;
                case "marathon": HandleMarathon(parts); break;
                default: Console.WriteLine("Unknown command. Type 'help' for usage."); break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

**Key Points:**
- ✅ Clean command dispatch with switch
- ✅ Exception handling for user errors
- ✅ Split() helper handles quoted strings
- ✅ Case-insensitive command matching

---

## 5. Quote-Aware String Splitter

```csharp
private static string[] Split(string input)
{
    var list = new List<string>();
    bool inQuotes = false;
    var current = "";
    
    foreach (var ch in input)
    {
        if (ch == '"')
        {
            inQuotes = !inQuotes;  // Toggle quote mode
            continue;
        }
        
        if (char.IsWhiteSpace(ch) && !inQuotes)
        {
            // Space outside quotes - word boundary
            if (current.Length > 0)
            {
                list.Add(current);
                current = "";
            }
        }
        else
        {
            // Regular character or space inside quotes
            current += ch;
        }
    }
    
    if (current.Length > 0)
        list.Add(current);
    
    return list.ToArray();
}
```

**Handles:**
- `add-room "Room A" 100` → ["add-room", "Room A", "100"]
- `add-show "Inception" "Room A" 2025-11-01T10:00 9.99`

---

## 6. Command Handler Example - HandleBook

```csharp
private static void HandleBook(string[] parts)
{
    if (parts.Length < 3)
    {
        Console.WriteLine("Usage: book <id> seat1 seat2 ...");
        return;
    }
    
    if (!Guid.TryParse(parts[1], out var showId))
    {
        Console.WriteLine("Error: Invalid show ID");
        return;
    }
    
    var show = store.Find(showId);
    if (show == null)
    {
        Console.WriteLine("Error: Show not found");
        return;
    }
    
    var seats = new int[parts.Length - 2];
    for (int i = 0; i < seats.Length; i++)
    {
        if (!int.TryParse(parts[i + 2], out seats[i]))
        {
            Console.WriteLine($"Error: Invalid seat number '{parts[i + 2]}'");
            return;
        }
    }
    
    if (show.TryBook(seats))
    {
        Console.WriteLine($"✓ Booked seats {string.Join(", ", seats)}");
        Console.WriteLine($"  Available: {show.AvailableSeats}/{show.Room.Capacity}");
    }
    else
    {
        Console.WriteLine("✗ Booking failed: Invalid or taken seats");
    }
}
```

**Validation Steps:**
1. ✅ Check argument count
2. ✅ Parse GUID
3. ✅ Find show in store
4. ✅ Parse seat numbers
5. ✅ Attempt atomic booking
6. ✅ Report result to user

---

## 7. LINQ Usage Examples

### Listing Shows by Date
```csharp
var shows = store.All()
    .Where(s => DateOnly.FromDateTime(s.Start) == day)
    .OrderBy(s => s.Start)
    .ToList();
```

### Marathon Planning
```csharp
var dayShows = shows
    .Where(s => DateOnly.FromDateTime(s.Start) == day)
    .OrderBy(s => s.End)
    .ThenBy(s => s.Start)
    .ToList();
```

### Seat Validation
```csharp
if (seats.Any(s => s < 1 || s > Room.Capacity)) return false;
if (seats.Any(_taken.Contains)) return false;
```

---

## 8. Error Handling Pattern

Every handler follows this pattern:

```csharp
private static void HandleCommand(string[] parts)
{
    // 1. Validate argument count
    if (parts.Length != expected)
    {
        Console.WriteLine("Usage: ...");
        return;
    }
    
    // 2. Parse and validate each argument
    if (!Type.TryParse(parts[i], out var value))
    {
        Console.WriteLine($"Error: Invalid format for {description}");
        return;
    }
    
    // 3. Validate business logic
    if (!conditions.Are.Met)
    {
        Console.WriteLine("Error: Specific error message");
        return;
    }
    
    // 4. Perform operation
    DoOperation();
    
    // 5. Report success
    Console.WriteLine("✓ Operation completed");
}
```

---

## 9. Design Pattern Examples

### Repository Pattern
```
Interface: IShowStore
├── InMemoryShowStore (current)
├── JsonShowStore (future)
└── DatabaseShowStore (future)
```

### Strategy Pattern
```
Interface: IMarathonPlanner
├── GreedyMarathonPlanner (current)
├── MaxDurationPlanner (future)
└── PreferredGenrePlanner (future)
```

---

## 10. Data Structure Performance

| Operation | Data Structure | Complexity |
|-----------|-----------------|-----------|
| Check seat booked | HashSet<int> | O(1) |
| Add seat | HashSet<int> | O(1) |
| List shows for day | LINQ Where | O(n) |
| Sort shows by date | LINQ OrderBy | O(n log n) |
| Find show by ID | LINQ FirstOrDefault | O(n) |

**Future Optimization:**
- Use Dictionary<Guid, Show> for O(1) show lookup
- Use SortedSet for pre-sorted shows

---

## 🎯 Key Takeaways

1. **Separation of Concerns** - Each class has one responsibility
2. **Design Patterns** - Repository and Strategy patterns enable flexibility
3. **Input Validation** - All user input is validated before use
4. **Error Messages** - Clear, actionable error messages for users
5. **Algorithm Efficiency** - Greedy algorithm is O(n log n) and optimal
6. **LINQ Mastery** - Fluent API for clean data queries
7. **Immutable Data** - Records for Movie/Room prevent accidental changes
8. **Atomic Operations** - Booking is all-or-nothing (no partial bookings)

---

**All implementations follow professional C# coding standards and best practices.**

using System;
using System.Collections.Generic;
using System.Linq;
using CinemaApp.Models;
using CinemaApp.Services;

namespace CinemaApp;

/// <summary>
/// Main application class that handles user interaction.
/// This class demonstrates:
/// - COMPOSITION: Uses IShowStore and IMarathonPlanner
/// - DEPENDENCY INJECTION: Could easily swap implementations
/// - SINGLE RESPONSIBILITY: Only handles CLI and user commands
/// </summary>
public static class App
{
    // Application dependencies - could be injected for better testability
    private static readonly Dictionary<string, Room> rooms = new();
    private static readonly Dictionary<string, Movie> movies = new();
    private static readonly IShowStore store = new InMemoryShowStore();
    private static readonly IMarathonPlanner planner = new GreedyMarathonPlanner();

    //==========================================================================
    // STUDENT TODO: Implement main REPL command loop
    // ⭐⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 30-45 minutes
    //
    // STEPS:
    // 1. Print help/welcome message
    // 2. Infinite loop: for (;;)
    // 3. Read line from Console
    // 4. Check for 'exit' command → return
    // 5. Parse with Split(line) helper (handles quotes)
    // 6. Extract command: parts[0].ToLowerInvariant()
    // 7. Switch/dispatch to handler methods
    // 8. Wrap in try/catch to show errors
    //
    // HINT: Use switch expression or switch statement for command dispatch
    //==========================================================================
    /// <summary>
    /// Main application loop - reads and processes commands.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("Commands: add-room, add-movie, add-show, list-shows, book, marathon, help, exit");
        
        for (;;)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var parts = Split(line);
            if (parts.Length == 0) continue;
            
            var cmd = parts[0].ToLowerInvariant();
            
            try
            {
                switch (cmd)
                {
                    case "exit":
                        return;
                    case "help":
                        PrintHelp();
                        break;
                    case "add-room":
                        HandleAddRoom(parts);
                        break;
                    case "add-movie":
                        HandleAddMovie(parts);
                        break;
                    case "add-show":
                        HandleAddShow(parts);
                        break;
                    case "list-shows":
                        HandleListShows(parts);
                        break;
                    case "book":
                        HandleBook(parts);
                        break;
                    case "marathon":
                        HandleMarathon(parts);
                        break;
                    default:
                        Console.WriteLine("Unknown command. Type 'help' for usage.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
    
    private static void PrintHelp()
    {
        Console.WriteLine("Cinema Application Commands:");
        Console.WriteLine("  add-room \"name\" capacity        - Create a room");
        Console.WriteLine("  add-movie \"title\" HH:mm         - Create a movie");
        Console.WriteLine("  add-show \"movie\" \"room\" datetime price - Schedule a show");
        Console.WriteLine("  list-shows YYYY-MM-DD             - Show all shows for a date");
        Console.WriteLine("  book <id> seat1 seat2 ...         - Book seats for a show");
        Console.WriteLine("  marathon YYYY-MM-DD               - Plan non-overlapping movies");
        Console.WriteLine("  help                              - Display this help");
        Console.WriteLine("  exit                              - Quit");
    }

    //==========================================================================
    // STUDENT TODO: Implement add-room command handler
    // ⭐ DIFFICULTY: Easy | ⏱️ TIME: 15 minutes
    // USAGE: add-room "Room A" 100
    // STEPS:
    // 1. Check parts.Length == 3, else print usage and return
    // 2. Parse: name = parts[1], capacity = int.Parse(parts[2])
    // 3. Validate capacity > 0
    // 4. Create and store: rooms[name] = new Room(name, capacity)
    // 5. Print success message
    //==========================================================================
    /// <summary>
    /// Handles: add-room "Room A" 100
    /// Creates a new cinema room with specified capacity.
    /// </summary>
    private static void HandleAddRoom(string[] parts)
    {
        if (parts.Length != 3)
        {
            Console.WriteLine("Usage: add-room \"name\" capacity");
            return;
        }
        
        var name = parts[1];
        if (!int.TryParse(parts[2], out var capacity))
        {
            Console.WriteLine("Error: Capacity must be a number");
            return;
        }
        
        if (capacity <= 0)
        {
            Console.WriteLine("Error: Capacity must be positive");
            return;
        }
        
        var room = new Room(name, capacity);
        rooms[name] = room;
        Console.WriteLine($"✓ Created room: {room}");
    }

    //==========================================================================
    // STUDENT TODO: Implement add-movie command handler
    // ⭐ DIFFICULTY: Easy | ⏱️ TIME: 15 minutes
    // USAGE: add-movie "Inception" 02:28
    // STEPS:
    // 1. Check parts.Length == 3
    // 2. Parse: title = parts[1], duration = TimeSpan.Parse(parts[2])
    // 3. Store: movies[title] = new Movie(title, duration)
    // 4. Print success
    //==========================================================================
    /// <summary>
    /// Handles: add-movie "Inception" 02:28
    /// Creates a new movie with title and duration.
    /// </summary>
    private static void HandleAddMovie(string[] parts)
    {
        if (parts.Length != 3)
        {
            Console.WriteLine("Usage: add-movie \"title\" HH:mm");
            return;
        }
        
        var title = parts[1];
        if (!TimeSpan.TryParse(parts[2], out var duration))
        {
            Console.WriteLine("Error: Duration must be in HH:mm format");
            return;
        }
        
        var movie = new Movie(title, duration);
        movies[title] = movie;
        Console.WriteLine($"✓ Created movie: {movie}");
    }

    //==========================================================================
    // STUDENT TODO: Implement add-show command handler
    // ⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 25 minutes
    // USAGE: add-show "Inception" "Room A" 2025-11-01T10:00 9.99
    // STEPS:
    // 1. Check parts.Length == 5
    // 2. Look up movie: movies.TryGetValue(parts[1], out var movie)
    // 3. Look up room: rooms.TryGetValue(parts[2], out var room)
    // 4. Parse: start = DateTime.Parse(parts[3]), price = decimal.Parse(parts[4])
    // 5. Create: var show = new Show(movie, room, start, price)
    // 6. Store: store.Add(show)
    // 7. Print show ID
    //==========================================================================
    /// <summary>
    /// Handles: add-show "Inception" "Room A" 2025-11-01T10:00 9.99
    /// Creates a new show with all details.
    /// </summary>
    private static void HandleAddShow(string[] parts)
    {
        if (parts.Length != 5)
        {
            Console.WriteLine("Usage: add-show \"movie\" \"room\" datetime price");
            return;
        }
        
        var movieTitle = parts[1];
        if (!movies.TryGetValue(movieTitle, out var movie))
        {
            Console.WriteLine($"Error: Movie '{movieTitle}' not found");
            return;
        }
        
        var roomName = parts[2];
        if (!rooms.TryGetValue(roomName, out var room))
        {
            Console.WriteLine($"Error: Room '{roomName}' not found");
            return;
        }
        
        if (!DateTime.TryParse(parts[3], out var start))
        {
            Console.WriteLine("Error: Invalid datetime format");
            return;
        }
        
        if (!decimal.TryParse(parts[4], out var price))
        {
            Console.WriteLine("Error: Price must be a number");
            return;
        }
        
        var show = new Show(movie, room, start, price);
        store.Add(show);
        Console.WriteLine($"✓ Created show: {show}");
    }

    //==========================================================================
    // STUDENT TODO: Implement list-shows command handler
    // ⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 20 minutes
    // USAGE: list-shows 2025-11-01
    // STEPS:
    // 1. Check parts.Length == 2
    // 2. Parse: day = DateOnly.Parse(parts[1])
    // 3. Query: store.All().Where(s => DateOnly.FromDateTime(s.Start) == day).OrderBy(s => s.Start)
    // 4. If empty: print "No shows"
    // 5. Else: print header, loop through shows, print each
    //==========================================================================
    /// <summary>
    /// Handles: list-shows 2025-11-01
    /// Lists all shows for a specific date, ordered by start time.
    /// </summary>
    private static void HandleListShows(string[] parts)
    {
        if (parts.Length != 2)
        {
            Console.WriteLine("Usage: list-shows YYYY-MM-DD");
            return;
        }
        
        if (!DateOnly.TryParse(parts[1], out var day))
        {
            Console.WriteLine("Error: Invalid date format");
            return;
        }
        
        var shows = store.All()
            .Where(s => DateOnly.FromDateTime(s.Start) == day)
            .OrderBy(s => s.Start)
            .ToList();
        
        if (!shows.Any())
        {
            Console.WriteLine("No shows for this date");
            return;
        }
        
        Console.WriteLine($"\nShows for {day}:");
        foreach (var show in shows)
            Console.WriteLine(show);
        Console.WriteLine();
    }

    //==========================================================================
    // STUDENT TODO: Implement book command handler
    // ⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 25 minutes
    // USAGE: book showId 12 13 14
    // STEPS:
    // 1. Check parts.Length >= 3
    // 2. Parse: Guid.TryParse(parts[1], out var showId)
    // 3. Find: var show = store.Find(showId)
    // 4. Parse seats: parts.Skip(2).Select(int.Parse).ToArray()
    // 5. Try booking: if (show.TryBook(seats)) { success } else { failure }
    // 6. Print result and remaining seats
    //==========================================================================
    /// <summary>
    /// Handles: book showId 12 13 14
    /// Books specified seats for a show.
    /// </summary>
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

    //==========================================================================
    // STUDENT TODO: Implement marathon command handler
    // ⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 25 minutes
    // USAGE: marathon 2025-11-01
    // STEPS:
    // 1. Check parts.Length == 2
    // 2. Parse: day = DateOnly.Parse(parts[1])
    // 3. Plan: var plan = planner.Plan(day, store.All())
    // 4. If empty: print "No plan"
    // 5. Else: print header, loop through plan, calculate total duration, print summary
    // HINT: var totalDuration = TimeSpan.Zero; foreach: totalDuration += show.Movie.Duration
    //==========================================================================
    /// <summary>
    /// Handles: marathon 2025-11-01
    /// Plans a marathon ticket with maximum non-overlapping shows.
    /// </summary>
    private static void HandleMarathon(string[] parts)
    {
        if (parts.Length != 2)
        {
            Console.WriteLine("Usage: marathon YYYY-MM-DD");
            return;
        }
        
        if (!DateOnly.TryParse(parts[1], out var day))
        {
            Console.WriteLine("Error: Invalid date format");
            return;
        }
        
        var plan = planner.Plan(day, store.All());
        
        if (!plan.Any())
        {
            Console.WriteLine("No shows available for marathon");
            return;
        }
        
        Console.WriteLine($"\nMarathon plan for {day}:");
        var totalDuration = TimeSpan.Zero;
        for (int i = 0; i < plan.Count; i++)
        {
            var show = plan[i];
            Console.WriteLine($"  {i + 1}. {show.Movie.Title} ({show.Start:HH:mm}-{show.End:HH:mm}) - {show.Room.Name}");
            totalDuration += show.Movie.Duration;
        }
        Console.WriteLine($"Total watch time: {totalDuration.Hours}h {totalDuration.Minutes}m");
        Console.WriteLine();
    }

    /// <summary>
    /// Custom string splitter that handles quoted strings.
    /// Example: 'add-room "Room A" 100' → ["add-room", "Room A", "100"]
    /// </summary>
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
}

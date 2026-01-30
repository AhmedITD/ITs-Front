# 🌟 Bonus Challenge: Dynamic Programming Marathon Planner

> **Difficulty**: ⭐⭐⭐ Advanced  
> **Prerequisites**: Completed the basic greedy marathon planner, understand recursion  
> **Time**: 4-6 hours additional work  

## 📚 What You'll Learn

This bonus challenge upgrades your marathon planner from **greedy** (maximize number of shows) to **optimal** (maximize total value using dynamic programming). You'll explore:

- **Dynamic Programming (DP)** - solving optimization problems by breaking them into subproblems
- **Memoization vs Tabulation** - two approaches to DP
- **Weighted Interval Scheduling** - classic DP problem
- **Time-Space Tradeoffs** - understanding algorithmic complexity
- **Algorithm Comparison** - when greedy is good enough vs when you need DP

---

## 🎯 The Challenge

Transform your marathon planner from counting shows to maximizing total value (revenue, watch time, or custom scoring).

### Current Greedy Algorithm:
```
Shows: [A(10:00-12:00), B(11:00-13:00), C(12:30-14:00), D(14:30-16:00)]
Greedy Result: [A, C, D] → 3 shows
```

### New DP Algorithm with Values:
```
Shows: [A(10:00-12:00,$15), B(11:00-13:00,$25), C(12:30-14:00,$10), D(14:30-16:00,$8)]
Greedy Result: [A, C, D] → $33
DP Result: [B, D] → $33
But if values change: [A(10:00-12:00,$30), B(11:00-13:00,$25), C(12:30-14:00,$5), D(14:30-16:00,$8)]
Greedy: [A, C, D] → $43
DP: [A, D] → $38... wait, that's worse!

Actually with these values:
A conflicts with B
B conflicts with A and C
C conflicts with B and D
D conflicts with C

Compatible sets:
- A, D → $38
- A, C is impossible (they overlap 12:00-12:30)
Wait, let me recalculate:
A: 10:00-12:00
C: 12:30-14:00
These DON'T overlap! A ends at 12:00, C starts at 12:30.

So: A, C, D → $43 (Greedy got this right)
Or: B, D → $33

The DP should find A, C, D → $43 as optimal.
```

**When DP Matters**: When value ≠ count, optimal solution may skip some shows to get higher-value later shows.

---

## 🧠 Conceptual Foundation

### What is Dynamic Programming?

Dynamic Programming solves optimization problems by:
1. **Breaking into subproblems**: Solve smaller versions first
2. **Storing results**: Don't recalculate (memoization)
3. **Building up solution**: Combine subproblem solutions
4. **Guaranteeing optimality**: Considers all possibilities systematically

### The Weighted Interval Scheduling Problem

**Given**: 
- N activities with start time, end time, and value
- Activities can't overlap

**Goal**: 
- Select subset with maximum total value
- No overlapping allowed

**Why Greedy Fails**:
Greedy picks locally optimal choices (earliest end time), but might miss globally optimal solution.

**Example Where Greedy Fails**:
```
Shows:
1. 10:00-14:00, value=$100
2. 10:00-11:00, value=$10
3. 11:00-12:00, value=$10
4. 12:00-13:00, value=$10
5. 13:00-14:00, value=$10

Greedy (earliest end): Picks 2, 3, 4, 5 → $40
DP (optimal): Picks 1 → $100
```

### Key Insight

After sorting by end time, for each show `i`, we need to decide:
- **Take it**: Get its value + best solution using only shows that end before this one starts
- **Skip it**: Use best solution from previous shows

The maximum of these two choices is optimal!

---

## 💻 Implementation Guide

### Step 1: Add Value Property to Shows

First, extend your `Show` class to support custom values:

```csharp
namespace CinemaApp.Models;

public class Show
{
    public int Id { get; }
    public Movie Movie { get; }
    public Room Room { get; }
    public DateTime Start { get; }
    public DateTime End => Start + Movie.Duration;
    public decimal TicketPrice { get; }
    
    // NEW: Custom value for optimization
    // Could be: revenue, watch time, priority score, etc.
    public double Value { get; set; }
    
    // ... existing code ...
    
    /// <summary>
    /// Calculate revenue value (tickets sold * price).
    /// This could be used for revenue-maximizing marathon planning.
    /// </summary>
    public double CalculateRevenue()
    {
        return BookedSeats.Count * (double)TicketPrice;
    }
    
    /// <summary>
    /// Calculate watch time value (movie duration in minutes).
    /// This could be used for watch-time-maximizing marathon planning.
    /// </summary>
    public double CalculateWatchTime()
    {
        return Movie.Duration.TotalMinutes;
    }
}
```

---

### Step 2: Implement DP Marathon Planner (Memoization Approach)

```csharp
namespace CinemaApp.Services;

/// <summary>
/// Dynamic Programming marathon planner using memoization (top-down approach).
/// 
/// ALGORITHM: Weighted Interval Scheduling with DP
/// APPROACH: Memoization (recursive with caching)
/// 
/// TIME COMPLEXITY: O(n²) where n = number of shows
/// - O(n log n) for sorting
/// - O(n²) for finding previous compatible shows
/// - O(n) for DP recursion (each state computed once)
/// 
/// SPACE COMPLEXITY: O(n) for memoization cache + O(n) recursion stack
/// 
/// LEARNING OBJECTIVES:
/// - Understand recursive problem decomposition
/// - Learn memoization technique (caching subproblem results)
/// - Compare with greedy algorithm performance
/// </summary>
public class DPMarathonPlanner : IMarathonPlanner
{
    private readonly Dictionary<int, double> _memo = new();
    private List<Show> _sortedShows = new();
    private int[] _previousCompatible = Array.Empty<int>();

    /// <summary>
    /// Plan a marathon that maximizes total value (revenue, watch time, etc.).
    /// </summary>
    /// <param name="shows">Available shows for the day</param>
    /// <param name="valueFunc">Function to calculate value for each show</param>
    /// <returns>Optimal sequence of non-overlapping shows</returns>
    public List<Show> PlanMarathon(List<Show> shows, Func<Show, double> valueFunc)
    {
        if (shows.Count == 0) return new List<Show>();

        // Step 1: Sort by end time (required for DP to work)
        _sortedShows = shows.OrderBy(s => s.End).ToList();
        
        // Step 2: Assign values using provided function
        foreach (var show in _sortedShows)
        {
            show.Value = valueFunc(show);
        }

        // Step 3: Precompute previous compatible show for each show
        // previousCompatible[i] = largest index j where show[j] doesn't overlap with show[i]
        _previousCompatible = new int[_sortedShows.Count];
        for (int i = 0; i < _sortedShows.Count; i++)
        {
            _previousCompatible[i] = FindPreviousCompatible(i);
        }

        // Step 4: Compute optimal value using memoization
        _memo.Clear();
        ComputeOptimalValue(_sortedShows.Count - 1);

        // Step 5: Reconstruct the optimal solution
        return ReconstructSolution();
    }

    /// <summary>
    /// Convenience method using revenue as value.
    /// </summary>
    public List<Show> PlanMarathon(List<Show> shows)
    {
        return PlanMarathon(shows, show => show.CalculateRevenue());
    }

    /// <summary>
    /// Recursive DP function with memoization.
    /// 
    /// RECURRENCE RELATION:
    /// OPT(i) = max(
    ///   value[i] + OPT(prev[i]),  // Take show i
    ///   OPT(i-1)                   // Skip show i
    /// )
    /// 
    /// BASE CASE:
    /// OPT(-1) = 0 (no shows available)
    /// </summary>
    /// <param name="i">Index of last show to consider</param>
    /// <returns>Maximum value achievable using shows 0..i</returns>
    private double ComputeOptimalValue(int i)
    {
        // Base case: no shows to consider
        if (i < 0) return 0;

        // Check memo cache
        if (_memo.ContainsKey(i))
            return _memo[i];

        // Option 1: Take show i
        double takeValue = _sortedShows[i].Value + ComputeOptimalValue(_previousCompatible[i]);

        // Option 2: Skip show i
        double skipValue = ComputeOptimalValue(i - 1);

        // Store the maximum in memo cache
        double result = Math.Max(takeValue, skipValue);
        _memo[i] = result;

        return result;
    }

    /// <summary>
    /// Find the latest show that doesn't overlap with show at index i.
    /// Uses binary search for efficiency.
    /// 
    /// TIME COMPLEXITY: O(log n) per call
    /// </summary>
    private int FindPreviousCompatible(int i)
    {
        var currentShow = _sortedShows[i];
        
        // Binary search for latest show that ends before current show starts
        int left = 0, right = i - 1;
        int result = -1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            
            if (_sortedShows[mid].End <= currentShow.Start)
            {
                result = mid;  // This show is compatible, try to find a later one
                left = mid + 1;
            }
            else
            {
                right = mid - 1;  // This show overlaps, search earlier
            }
        }

        return result;
    }

    /// <summary>
    /// Reconstruct the actual sequence of shows from the DP solution.
    /// Works backwards from the last show, deciding at each step whether it was included.
    /// 
    /// TIME COMPLEXITY: O(n)
    /// </summary>
    private List<Show> ReconstructSolution()
    {
        var result = new List<Show>();
        int i = _sortedShows.Count - 1;

        while (i >= 0)
        {
            // Get values for take/skip decisions
            double takeValue = _sortedShows[i].Value + 
                ((_previousCompatible[i] >= 0) ? _memo[_previousCompatible[i]] : 0);
            double skipValue = (i > 0) ? _memo[i - 1] : 0;

            // If taking this show was optimal, include it
            if (takeValue >= skipValue)
            {
                result.Add(_sortedShows[i]);
                i = _previousCompatible[i];  // Jump to previous compatible
            }
            else
            {
                i--;  // Move to previous show
            }
        }

        result.Reverse();  // We built the list backwards
        return result;
    }
}
```

---

### Step 3: Implement DP Marathon Planner (Tabulation Approach)

```csharp
namespace CinemaApp.Services;

/// <summary>
/// Dynamic Programming marathon planner using tabulation (bottom-up approach).
/// 
/// ALGORITHM: Weighted Interval Scheduling with DP
/// APPROACH: Tabulation (iterative with table filling)
/// 
/// TIME COMPLEXITY: O(n²) where n = number of shows
/// SPACE COMPLEXITY: O(n) for DP table only (no recursion stack)
/// 
/// ADVANTAGES over Memoization:
/// - No recursion overhead (better for large inputs)
/// - Iterative (easier to debug)
/// - More cache-friendly memory access pattern
/// 
/// LEARNING OBJECTIVES:
/// - Understand iterative DP table construction
/// - Compare memoization vs tabulation approaches
/// - Learn when each approach is preferable
/// </summary>
public class DPMarathonPlannerTabulation : IMarathonPlanner
{
    /// <summary>
    /// Plan a marathon that maximizes total value using bottom-up DP.
    /// </summary>
    public List<Show> PlanMarathon(List<Show> shows, Func<Show, double> valueFunc)
    {
        if (shows.Count == 0) return new List<Show>();

        // Step 1: Sort by end time
        var sortedShows = shows.OrderBy(s => s.End).ToList();
        int n = sortedShows.Count;

        // Step 2: Assign values
        foreach (var show in sortedShows)
        {
            show.Value = valueFunc(show);
        }

        // Step 3: Precompute previous compatible shows
        var previousCompatible = new int[n];
        for (int i = 0; i < n; i++)
        {
            previousCompatible[i] = FindPreviousCompatible(sortedShows, i);
        }

        // Step 4: Build DP table iteratively (bottom-up)
        // dp[i] = maximum value using shows 0..i
        var dp = new double[n];
        dp[0] = sortedShows[0].Value;

        for (int i = 1; i < n; i++)
        {
            // Option 1: Take show i
            double takeValue = sortedShows[i].Value;
            if (previousCompatible[i] >= 0)
            {
                takeValue += dp[previousCompatible[i]];
            }

            // Option 2: Skip show i
            double skipValue = dp[i - 1];

            // Store the maximum
            dp[i] = Math.Max(takeValue, skipValue);
        }

        // Step 5: Reconstruct solution
        return ReconstructSolution(sortedShows, dp, previousCompatible);
    }

    public List<Show> PlanMarathon(List<Show> shows)
    {
        return PlanMarathon(shows, show => show.CalculateRevenue());
    }

    private int FindPreviousCompatible(List<Show> shows, int i)
    {
        var currentShow = shows[i];
        int left = 0, right = i - 1;
        int result = -1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            if (shows[mid].End <= currentShow.Start)
            {
                result = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return result;
    }

    private List<Show> ReconstructSolution(List<Show> shows, double[] dp, int[] previousCompatible)
    {
        var result = new List<Show>();
        int i = shows.Count - 1;

        while (i >= 0)
        {
            double takeValue = shows[i].Value;
            if (previousCompatible[i] >= 0)
            {
                takeValue += dp[previousCompatible[i]];
            }

            double skipValue = (i > 0) ? dp[i - 1] : 0;

            if (takeValue >= skipValue)
            {
                result.Add(shows[i]);
                i = previousCompatible[i];
            }
            else
            {
                i--;
            }
        }

        result.Reverse();
        return result;
    }
}
```

---

### Step 4: Comparison and Testing

```csharp
namespace CinemaApp.Services;

/// <summary>
/// Utility class to compare different marathon planning algorithms.
/// Useful for understanding when each algorithm performs best.
/// </summary>
public static class MarathonPlannerComparison
{
    public class PlannerResult
    {
        public string AlgorithmName { get; set; } = "";
        public List<Show> Shows { get; set; } = new();
        public double TotalValue { get; set; }
        public int ShowCount { get; set; }
        public long ExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// Compare all three algorithms on the same input.
    /// </summary>
    public static List<PlannerResult> CompareAlgorithms(
        List<Show> shows, 
        Func<Show, double> valueFunc)
    {
        var results = new List<PlannerResult>();

        // Test Greedy
        var greedy = new GreedyMarathonPlanner();
        var greedyResult = TimeExecution(
            "Greedy (Count)", 
            () => greedy.PlanMarathon(shows),
            valueFunc
        );
        results.Add(greedyResult);

        // Test DP Memoization
        var dpMemo = new DPMarathonPlanner();
        var memoResult = TimeExecution(
            "DP (Memoization)", 
            () => dpMemo.PlanMarathon(shows, valueFunc),
            valueFunc
        );
        results.Add(memoResult);

        // Test DP Tabulation
        var dpTab = new DPMarathonPlannerTabulation();
        var tabResult = TimeExecution(
            "DP (Tabulation)", 
            () => dpTab.PlanMarathon(shows, valueFunc),
            valueFunc
        );
        results.Add(tabResult);

        return results;
    }

    private static PlannerResult TimeExecution(
        string name, 
        Func<List<Show>> planFunc,
        Func<Show, double> valueFunc)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var shows = planFunc();
        sw.Stop();

        return new PlannerResult
        {
            AlgorithmName = name,
            Shows = shows,
            TotalValue = shows.Sum(s => valueFunc(s)),
            ShowCount = shows.Count,
            ExecutionTimeMs = sw.ElapsedMilliseconds
        };
    }

    /// <summary>
    /// Print comparison table to console.
    /// </summary>
    public static void PrintComparison(List<PlannerResult> results)
    {
        Console.WriteLine("\n=== Marathon Planner Comparison ===\n");
        Console.WriteLine($"{"Algorithm",-20} {"Shows",-8} {"Total Value",-15} {"Time (ms)",-10}");
        Console.WriteLine(new string('-', 60));

        foreach (var result in results)
        {
            Console.WriteLine(
                $"{result.AlgorithmName,-20} " +
                $"{result.ShowCount,-8} " +
                $"{result.TotalValue,-15:F2} " +
                $"{result.ExecutionTimeMs,-10}"
            );
        }

        Console.WriteLine();
    }
}
```

---

### Step 5: Example Usage in CLI

Add a new command to your `App.cs`:

```csharp
case "compare-planners":
    if (parts.Length != 2)
    {
        Console.WriteLine("Usage: compare-planners <date>");
        break;
    }
    
    if (!DateTime.TryParse(parts[1], out var compareDate))
    {
        Console.WriteLine("Invalid date format");
        break;
    }
    
    var allShows = _store.GetShowsByDate(compareDate);
    
    // Use revenue as value function
    var comparison = MarathonPlannerComparison.CompareAlgorithms(
        allShows, 
        show => show.CalculateRevenue()
    );
    
    MarathonPlannerComparison.PrintComparison(comparison);
    
    // Also show actual show sequences
    Console.WriteLine("\nDetailed Results:");
    foreach (var result in comparison)
    {
        Console.WriteLine($"\n{result.AlgorithmName}:");
        foreach (var show in result.Shows)
        {
            Console.WriteLine($"  - {show}");
        }
    }
    break;
```

---

## 🧪 Testing Your Implementation

```csharp
namespace CinemaApp.Tests;

public static class DPMarathonTests
{
    public static void RunAllTests()
    {
        TestBasicDP();
        TestGreedyVsDP();
        TestEdgeCases();
        TestPerformance();
        Console.WriteLine("✅ All DP marathon tests passed!");
    }

    /// <summary>
    /// Test basic DP functionality.
    /// </summary>
    private static void TestBasicDP()
    {
        var movies = new List<Movie>
        {
            new Movie("A", TimeSpan.FromHours(2)),
            new Movie("B", TimeSpan.FromHours(2)),
            new Movie("C", TimeSpan.FromHours(1.5))
        };

        var room = new Room("Room 1", 100);
        var baseDate = new DateTime(2025, 11, 1, 10, 0, 0);

        var shows = new List<Show>
        {
            new Show(1, movies[0], room, baseDate, 15m),                    // 10:00-12:00, $15
            new Show(2, movies[1], room, baseDate.AddHours(1), 25m),        // 11:00-13:00, $25
            new Show(3, movies[2], room, baseDate.AddHours(2.5), 10m)       // 12:30-14:00, $10
        };

        var planner = new DPMarathonPlanner();
        var result = planner.PlanMarathon(shows, s => (double)s.TicketPrice);

        // With values [15, 25, 10], optimal is [25] = 25
        // Because Show 2 overlaps with both Show 1 and Show 3
        Assert(result.Count == 1, "Should select 1 show");
        Assert(result[0].Id == 2, "Should select highest-value show");
    }

    /// <summary>
    /// Test case where DP beats greedy.
    /// </summary>
    private static void TestGreedyVsDP()
    {
        var longMovie = new Movie("Long", TimeSpan.FromHours(4));
        var shortMovie = new Movie("Short", TimeSpan.FromHours(1));
        
        var room = new Room("Room 1", 100);
        var baseDate = new DateTime(2025, 11, 1, 10, 0, 0);

        var shows = new List<Show>
        {
            // One long valuable show
            new Show(1, longMovie, room, baseDate, 100m),                  // 10:00-14:00, $100
            
            // Four short cheap shows
            new Show(2, shortMovie, room, baseDate, 10m),                  // 10:00-11:00, $10
            new Show(3, shortMovie, room, baseDate.AddHours(1), 10m),      // 11:00-12:00, $10
            new Show(4, shortMovie, room, baseDate.AddHours(2), 10m),      // 12:00-13:00, $10
            new Show(5, shortMovie, room, baseDate.AddHours(3), 10m)       // 13:00-14:00, $10
        };

        var greedy = new GreedyMarathonPlanner();
        var greedyResult = greedy.PlanMarathon(shows);
        var greedyValue = greedyResult.Sum(s => (double)s.TicketPrice);

        var dp = new DPMarathonPlanner();
        var dpResult = dp.PlanMarathon(shows, s => (double)s.TicketPrice);
        var dpValue = dpResult.Sum(s => (double)s.TicketPrice);

        Console.WriteLine($"Greedy: {greedyResult.Count} shows, ${greedyValue}");
        Console.WriteLine($"DP: {dpResult.Count} shows, ${dpValue}");

        Assert(dpValue > greedyValue, "DP should find better solution");
        Assert(dpResult.Count == 1 && dpResult[0].Id == 1, "DP should pick the long valuable show");
    }

    /// <summary>
    /// Test edge cases.
    /// </summary>
    private static void TestEdgeCases()
    {
        var planner = new DPMarathonPlanner();

        // Empty input
        var result1 = planner.PlanMarathon(new List<Show>(), s => 1.0);
        Assert(result1.Count == 0, "Empty input should return empty result");

        // Single show
        var movie = new Movie("A", TimeSpan.FromHours(2));
        var room = new Room("Room 1", 100);
        var show = new Show(1, movie, room, DateTime.Now, 10m);
        var result2 = planner.PlanMarathon(new List<Show> { show }, s => 1.0);
        Assert(result2.Count == 1, "Single show should be selected");
    }

    /// <summary>
    /// Test performance on larger inputs.
    /// </summary>
    private static void TestPerformance()
    {
        // Generate 100 random shows
        var random = new Random(42);
        var movies = Enumerable.Range(1, 100)
            .Select(i => new Movie($"Movie {i}", TimeSpan.FromMinutes(90 + random.Next(60))))
            .ToList();

        var room = new Room("Room 1", 100);
        var baseDate = new DateTime(2025, 11, 1, 8, 0, 0);

        var shows = new List<Show>();
        var currentTime = baseDate;
        for (int i = 0; i < 100; i++)
        {
            var movie = movies[i];
            var price = 10m + random.Next(20);
            shows.Add(new Show(i + 1, movie, room, currentTime, price));
            currentTime = currentTime.AddMinutes(30); // Shows every 30 min (overlapping)
        }

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var planner = new DPMarathonPlanner();
        var result = planner.PlanMarathon(shows, s => (double)s.TicketPrice);
        sw.Stop();

        Console.WriteLine($"DP on 100 shows: {result.Count} selected in {sw.ElapsedMilliseconds}ms");
        Assert(sw.ElapsedMilliseconds < 1000, "Should complete in under 1 second");
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Assertion failed: {message}");
    }
}
```

---

## 📊 Algorithm Comparison

### Greedy vs Dynamic Programming

| Aspect | Greedy | DP (Memoization) | DP (Tabulation) |
|--------|--------|------------------|-----------------|
| **Approach** | Pick earliest ending | Recursive + cache | Iterative + table |
| **Optimality** | ❌ Not guaranteed | ✅ Guaranteed optimal | ✅ Guaranteed optimal |
| **Time Complexity** | O(n log n) | O(n²) | O(n²) |
| **Space Complexity** | O(1) | O(n) + recursion | O(n) only |
| **Code Simplicity** | ⭐⭐⭐ Simple | ⭐⭐ Moderate | ⭐⭐ Moderate |
| **Best For** | Quick approximations | Small to medium inputs | Large inputs |
| **When to Use** | Value = count | Need true optimum | Production systems |

### When Greedy is Good Enough

Greedy works well when:
- All shows have similar value (price, duration)
- Maximizing count IS the goal
- Performance is critical (real-time systems)
- Approximate solution acceptable

### When You Need DP

DP is necessary when:
- Values vary significantly
- Need provably optimal solution
- Can afford O(n²) computation
- Value ≠ count (revenue, watch time, ratings)

---

## 🎨 Advanced Extensions

### 1. Multiple Value Functions

Allow users to choose optimization criteria:

```csharp
public enum OptimizationGoal
{
    MaximizeRevenue,      // Sum of ticket prices
    MaximizeWatchTime,    // Sum of movie durations
    MaximizeRating,       // Sum of movie ratings (add Rating to Movie)
    MaximizeOccupancy,    // Sum of booked seats
    Custom                // User-defined function
}

public List<Show> PlanMarathon(
    List<Show> shows, 
    OptimizationGoal goal,
    Func<Show, double>? customFunc = null)
{
    Func<Show, double> valueFunc = goal switch
    {
        OptimizationGoal.MaximizeRevenue => s => s.CalculateRevenue(),
        OptimizationGoal.MaximizeWatchTime => s => s.CalculateWatchTime(),
        OptimizationGoal.MaximizeRating => s => s.Movie.Rating * s.BookedSeats.Count,
        OptimizationGoal.MaximizeOccupancy => s => s.BookedSeats.Count,
        OptimizationGoal.Custom => customFunc ?? throw new ArgumentException("Custom function required"),
        _ => throw new NotSupportedException()
    };
    
    return PlanMarathon(shows, valueFunc);
}
```

### 2. Multi-Room Marathon

Extend to plan marathons across multiple rooms:

```csharp
/// <summary>
/// Plan marathon that can use multiple rooms simultaneously.
/// User can watch show in Room A, then immediately go to Room B.
/// </summary>
public class MultiRoomMarathonPlanner
{
    public List<Show> PlanMarathon(List<Show> shows, Func<Show, double> valueFunc)
    {
        // Group shows by room
        var byRoom = shows.GroupBy(s => s.Room.Name).ToList();
        
        // Plan optimal marathon for each room
        var planner = new DPMarathonPlanner();
        var roomMarathons = byRoom
            .Select(g => planner.PlanMarathon(g.ToList(), valueFunc))
            .ToList();
        
        // Merge all room marathons (they don't conflict since different rooms)
        return roomMarathons
            .SelectMany(m => m)
            .OrderBy(s => s.Start)
            .ToList();
    }
}
```

### 3. Constraints and Preferences

Add user constraints:

```csharp
public class ConstrainedMarathonPlanner
{
    public List<Show> PlanMarathon(
        List<Show> shows, 
        Func<Show, double> valueFunc,
        MarathonConstraints constraints)
    {
        // Filter shows based on constraints
        var eligible = shows.Where(s => 
            (constraints.MinDuration == null || s.Movie.Duration >= constraints.MinDuration) &&
            (constraints.MaxDuration == null || s.Movie.Duration <= constraints.MaxDuration) &&
            (constraints.PreferredGenres == null || constraints.PreferredGenres.Contains(s.Movie.Genre)) &&
            (constraints.MinRating == null || s.Movie.Rating >= constraints.MinRating)
        ).ToList();
        
        // Run DP on filtered shows
        var planner = new DPMarathonPlanner();
        return planner.PlanMarathon(eligible, valueFunc);
    }
}

public class MarathonConstraints
{
    public TimeSpan? MinDuration { get; set; }
    public TimeSpan? MaxDuration { get; set; }
    public List<string>? PreferredGenres { get; set; }
    public double? MinRating { get; set; }
    public int? MaxShows { get; set; }
}
```

### 4. Visualization

Print a timeline showing the marathon:

```csharp
public static void PrintMarathonTimeline(List<Show> marathon)
{
    if (marathon.Count == 0)
    {
        Console.WriteLine("No shows in marathon");
        return;
    }

    Console.WriteLine("\n=== Marathon Timeline ===\n");
    
    var totalValue = marathon.Sum(s => s.Value);
    var totalDuration = marathon.Last().End - marathon.First().Start;
    
    Console.WriteLine($"Total Shows: {marathon.Count}");
    Console.WriteLine($"Total Value: ${totalValue:F2}");
    Console.WriteLine($"Total Duration: {totalDuration.TotalHours:F1} hours");
    Console.WriteLine($"Start: {marathon.First().Start:HH:mm}");
    Console.WriteLine($"End: {marathon.Last().End:HH:mm}");
    Console.WriteLine();

    foreach (var show in marathon)
    {
        var bar = new string('█', (int)(show.Movie.Duration.TotalMinutes / 10));
        Console.WriteLine(
            $"{show.Start:HH:mm}-{show.End:HH:mm} " +
            $"[{bar}] " +
            $"{show.Movie.Title} " +
            $"(${show.Value:F2})"
        );
    }
    
    Console.WriteLine();
}
```

---

## 📚 Theory Deep Dive

### The DP Recurrence Relation Explained

For weighted interval scheduling:

```
Let OPT(i) = maximum value using shows 0..i
Let p(i) = latest show compatible with show i (ends before i starts)
Let v(i) = value of show i

Then:
OPT(i) = max(
    v(i) + OPT(p(i)),    // Include show i
    OPT(i-1)             // Exclude show i
)

Base case:
OPT(-1) = 0  (no shows = zero value)
```

**Why this works**:
1. **Optimal substructure**: Optimal solution for `i` shows uses optimal solutions for fewer shows
2. **Overlapping subproblems**: Same subproblems appear multiple times in recursion
3. **Memoization prevents recomputation**: Store results in cache

### Proof of Correctness (Sketch)

**Claim**: The DP algorithm finds the optimal solution.

**Proof by induction**:
- **Base case**: OPT(0) correctly picks show 0 (only option)
- **Inductive step**: Assume OPT(j) is correct for all j < i
  - If show i is in optimal solution, then remaining shows must form optimal solution (OPT(p(i)))
  - If show i is not in optimal solution, then optimal solution uses shows 0..i-1 (OPT(i-1))
  - Taking max of both cases gives optimal solution for i

**Conclusion**: OPT(n-1) is the optimal solution for all n shows.

---

## 🧠 Discussion Questions

Include answers to these in your README:

1. **When does greedy match DP optimality?**
   - What conditions must hold?

2. **Space optimization: Can we reduce from O(n) to O(1)?**
   - Hint: Do we need the entire DP table?

3. **What if shows can be partially attended?**
   - How would the problem change?

4. **Real-world application:**
   - Where else is weighted interval scheduling used?
   - Job scheduling, bandwidth allocation, conference planning

5. **Comparison with brute force:**
   - Brute force tries all 2^n subsets
   - DP only computes O(n) subproblems
   - Savings: exponential → polynomial

---

## 🏆 Grading Bonus Points

| Achievement | Bonus Points |
|-------------|--------------|
| DP Memoization implementation | +5 |
| DP Tabulation implementation | +3 |
| Algorithm comparison tool | +3 |
| Multiple value functions | +2 |
| Comprehensive testing | +3 |
| Performance analysis | +2 |
| Visualization/timeline | +2 |
| **Maximum Bonus** | **+20** |

---

## 📖 Further Reading

### Books
- **"Introduction to Algorithms"** (CLRS) - Chapter 15: Dynamic Programming
- **"Algorithm Design"** by Kleinberg & Tardos - Chapter 6: Dynamic Programming
- **"The Algorithm Design Manual"** by Skiena - DP chapter

### Online Resources
- [Weighted Interval Scheduling](https://en.wikipedia.org/wiki/Interval_scheduling) - Wikipedia
- [Dynamic Programming](https://www.geeksforgeeks.org/competitive-programming/dynamic-programming) - GeeksForGeeks
- [DP Patterns](https://leetcode.com/discuss/general-discussion/458695/dynamic-programming-patterns) - LeetCode
- [Visualizing DP](https://www.dpvisualizer.com/) - Python Interactive with possiblity to follow the problem on LeetCode

### Related Problems
- Knapsack Problem (0/1 and fractional)
- Longest Increasing Subsequence
- Edit Distance
- Coin Change
- Matrix Chain Multiplication

---

## ✅ Submission Checklist

- [ ] DP Memoization planner implemented
- [ ] DP Tabulation planner implemented
- [ ] Value property added to Show
- [ ] Comparison tool working
- [ ] Tests for correctness and performance
- [ ] CLI commands for comparing algorithms
- [ ] Documentation explaining approach
- [ ] Analysis of when greedy vs DP is better
- [ ] (Optional) One advanced extension

---

## 🚀 Get Started!

1. Create `src/Services/DPMarathonPlanner.cs`
2. Implement the memoization version first (easier to understand)
3. Test with small examples by hand
4. Add comparison tool
5. Implement tabulation version
6. Run performance tests
7. Choose an advanced extension

Good luck! Dynamic Programming is challenging but incredibly powerful once you understand it. Don't hesitate to ask questions! 🎉

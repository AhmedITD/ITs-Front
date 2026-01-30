# 🎓 Mini-Project Pack (Week 5, Solo) — Educational Skeleton Version

> **⚠️ IMPORTANT**: These are **skeleton/template** projects designed for learning!  
> All implementation files have been converted to educational scaffolding with TODO comments and hints.  
> **Your task**: Complete the implementations by following the detailed guides provided.

## 📦 What's Included

This repository contains **three complete project skeletons** with:
- ✅ **Full project structure** and build system
- ✅ **Skeleton implementations** with detailed TODO comments
- ✅ **Comprehensive documentation** (22,000+ lines of guides)
- ✅ **Test data files** with 900+ example commands
- ✅ **Difficulty ratings** (⭐ to ⭐⭐⭐⭐⭐) and time estimates
- ✅ **Step-by-step implementation guides**
- ✅ **Example skeleton templates** showing completed patterns

**Total Learning Time**: 48-64 hours over 8 weeks

---

## 🚀 Getting Started (5 Minutes)

### 1. Choose Your Project

| Project | Files | Difficulty | Main Topics | Estimated Time |
|---------|-------|------------|-------------|----------------|
| **SciCalc** | 15 files | ⭐-⭐⭐⭐ | LINQ, Interfaces, Factory Pattern | 12-20 hours |
| **CinemaApp** | 4 files | ⭐⭐-⭐⭐⭐⭐⭐ | Records, Greedy Algorithm, Repository | 10-15 hours |
| **TrafficSim** | 6 files | ⭐⭐-⭐⭐⭐⭐⭐ | State Machines, Dijkstra, Graphs | 12-20 hours |

**Recommendation**: Start with **SciCalc** if new to C#!

### 2. Build & Run

```bash
# Navigate to your chosen project
cd SciCalc        # or CinemaApp or TrafficSim

# Build the project (should succeed even with skeletons!)
dotnet build

# Run the application
dotnet run
```

**Expected**: The app starts but throws `NotImplementedException` when you try to use features.  
**Your goal**: Implement the missing functionality following the TODO comments!

### 3. Read the Documentation

📚 **Start Here**: 
1. **`QUICK_START.md`** - 15-minute introduction (read first!)
2. **`IMPLEMENTATION_GUIDE.md`** - Week-by-week roadmap with examples
3. **Project-specific `README.md`** - Inside each project folder
4. **`examples.txt`** - Test cases and usage examples (in each project's root)

---

## 🎯 Choose **ONE** Project

**Common rules (for all options)**

* **Language/runtime:** C# (.NET 8), console app only.
* **OOP:** clear modelling (classes/records), encapsulation, composition-first, sensible use of at least **one** small pattern (Strategy/Factory/Repository/Adapter).
* **Data structures & LINQ:** use the right collections and LINQ where it helps readability.
* **Persistence:** JSON or CSV where specified.
* **Errors:** validate inputs; catch and handle exceptions with friendly messages.
* **Git/GitHub workflow:** issues → small feature branches → PRs → peer review (or self-review notes if working solo) → squash merge.
* **Deliverables (by end of Week 5):**

  * Working console app + source code.
  * README with run steps, features, and sample commands/output.
  * At least **5 PRs** merged (setup, core feature, persistence, algorithm, polish).
  * Brief demo (≤5 min) OR screencast (≤3 min).

## 📊 Marking Rubric (100 points)

| Category | Points | What We're Looking For |
|----------|--------|------------------------|
| **Functionality** | 40 | Does it work? Meets all requirements? Produces correct outputs? |
| **Code Quality & OOP** | 25 | Clean code, proper OOP usage, design patterns applied correctly |
| **Git/GitHub Workflow** | 15 | Good issues, branches, PRs, meaningful commits |
| **Robustness** | 10 | Input validation, error handling, handles edge cases |
| **Documentation & Demo** | 10 | Clear README, good examples, effective demo video |

## 📁 Repository Layout (Already Set Up!)

All projects follow this professional structure:

```
its_project_1/
├── SciCalc/                      # Scientific Calculator Project
│   ├── src/
│   │   ├── Models/               # AngleMode enum
│   │   ├── Services/
│   │   │   ├── Operations/       # Add, Sub, Mul, Div, Sin, Cos, etc. (⭐-⭐⭐)
│   │   │   ├── IOperation.cs
│   │   │   ├── ITrigonometric.cs
│   │   │   └── OperationFactory.cs
│   │   ├── Calculator.cs         # Main REPL (⭐⭐⭐⭐)
│   │   └── Program.cs
│   ├── data/
│   │   └── test-cases.txt        # 200+ test commands
│   ├── examples.txt              # Usage examples
│   └── README.md
│
├── CinemaApp/                    # Cinema Booking System
│   ├── src/
│   │   ├── Models/               # Movie, Room (complete)
│   │   │   └── Show.cs           # Booking logic (⭐⭐⭐)
│   │   ├── Services/
│   │   │   ├── InMemoryShowStore.cs        # Repository (⭐)
│   │   │   └── GreedyMarathonPlanner.cs    # Algorithm (⭐⭐⭐⭐⭐)
│   │   ├── App.cs                # Command handlers (⭐⭐-⭐⭐⭐)
│   │   └── Program.cs
│   ├── data/
│   │   └── test-cases.txt        # 300+ test scenarios
│   ├── examples.txt              # Usage examples
│   └── README.md
│
├── TrafficSim/                   # Traffic Simulation
│   ├── src/
│   │   ├── Models/               # Vehicle, Side, LightState (complete)
│   │   │   └── TrafficLight.cs   # State machine (⭐⭐-⭐⭐⭐)
│   │   ├── Services/
│   │   │   ├── Street.cs         # Vehicle queue (⭐⭐)
│   │   │   ├── CrossRoad.cs      # Intersection (⭐⭐⭐)
│   │   │   ├── City.cs           # Grid management (⭐⭐)
│   │   │   ├── ShortestPath.cs   # Dijkstra algorithm (⭐⭐⭐⭐⭐)
│   │   │   └── App.cs            # CLI (⭐⭐)
│   │   └── Program.cs
│   ├── data/
│   │   └── test-cases.txt        # 400+ pathfinding tests
│   ├── examples.txt              # Usage examples & algorithm guide
│   └── README.md
│
├── skeleton-examples/            # Complete implementation examples
│   ├── Add.cs                    # ⭐ Easy template
│   ├── Show.cs                   # ⭐⭐⭐ Medium template
│   ├── GreedyMarathonPlanner.cs  # ⭐⭐⭐⭐⭐ Hard template
│   └── README.md
│
├── QUICK_START.md                # 15-minute getting started guide
├── IMPLEMENTATION_GUIDE.md       # Week-by-week roadmap
├── HINTS_AND_TIPS.md             # Solutions to common problems
├── QUICK_REFERENCE.md            # Fast lookup for all tasks
├── SKELETON_CONVERSION_COMPLETE.md  # Final status report
└── README.md                     # This file
```

### Why This Structure?
- **Models/** = Data classes and enums (Vehicle, Movie, Room, Side, etc.)
- **Services/** = Business logic (Calculator, Planner, Pathfinder, etc.)
- **Program.cs** = Entry point that starts everything
- **data/** = Test cases and sample data
- **examples.txt** = Usage guide with commands and expected outputs

## 📅 8-Week Learning Schedule

This skeleton package is designed for **8 weeks of learning** (~6-8 hours per week):

### **Weeks 1-2: SciCalc Basic Operations** (⭐-⭐⭐)
**Topics**: LINQ basics, simple validation, arithmetic operations  
**Files**: Add, Sub, Mul, Mod, Pow (5 files)  
**Time**: 4-6 hours total

- ✅ Implement basic arithmetic (add, sub, mul)
- ✅ Learn LINQ methods (Sum, Aggregate)
- ✅ Handle edge cases (empty arrays, single values)
- 🔄 **PR #1-2**: "Basic operations complete"

### **Weeks 2-3: SciCalc Advanced Operations** (⭐⭐-⭐⭐⭐)
**Topics**: Error handling, domain validation, logarithms, factorial  
**Files**: Div, Sqrt, Log10, Ln, IDiv, Fact (6 files)  
**Time**: 6-8 hours total

- ✅ Add division with zero check
- ✅ Implement domain validation (sqrt of negative)
- ✅ Learn Math library functions
- 🔄 **PR #3-4**: "Advanced math operations"

### **Weeks 3-4: SciCalc REPL + Trigonometry** (⭐⭐-⭐⭐⭐⭐)
**Topics**: Command parsing, pattern matching, angle modes  
**Files**: Sin, Cos, Tan, Calculator.cs (4 files)  
**Time**: 8-10 hours total

- ✅ Implement trigonometric functions
- ✅ Add angle mode switching (deg/rad)
- ✅ Build REPL command loop
- 🔄 **PR #5-6**: "Trig functions and REPL complete"

### **Weeks 4-5: CinemaApp Booking** (⭐-⭐⭐⭐)
**Topics**: HashSet validation, repository pattern, LINQ queries  
**Files**: Show, InMemoryShowStore, App handlers (3 files)  
**Time**: 6-8 hours total

- ✅ Implement seat booking with collision detection
- ✅ Add repository for show storage
- ✅ Create 6 command handlers
- 🔄 **PR #7-8**: "Booking system complete"

### **Weeks 5-6: CinemaApp Marathon** (⭐⭐⭐⭐⭐)
**Topics**: Greedy algorithms, interval scheduling  
**Files**: GreedyMarathonPlanner (1 file)  
**Time**: 8-12 hours total

- ✅ Implement greedy interval scheduling
- ✅ Sort by earliest finish time
- ✅ Handle overlapping shows
- 🔄 **PR #9**: "Marathon planning algorithm"

### **Weeks 6-7: TrafficSim Core** (⭐⭐-⭐⭐⭐)
**Topics**: State machines, queues, grid structures  
**Files**: TrafficLight, Street, CrossRoad, City (4 files)  
**Time**: 8-10 hours total

- ✅ Build traffic light state machine
- ✅ Implement vehicle queues
- ✅ Create grid of crossroads
- 🔄 **PR #10-11**: "Traffic simulation core"

### **Weeks 7-8: TrafficSim Pathfinding** (⭐⭐⭐⭐⭐)
**Topics**: Dijkstra's algorithm, graph traversal  
**Files**: ShortestPath, App (2 files)  
**Time**: 8-10 hours total

- ✅ Implement Dijkstra's algorithm
- ✅ Handle graph traversal
- ✅ Add CLI for pathfinding
- 🔄 **PR #12**: "Shortest path complete"

**Total**: 48-64 hours over 8 weeks

## 🎯 Success Tips

1. **Start Simple**: Get basic version working first, then add features
2. **Commit Often**: Make small commits with clear messages
3. **Test As You Go**: Run your app after each feature to catch bugs early
4. **Ask for Help**: Use course forum, office hours, or study groups
5. **Read the Examples**: Check `data/examples.txt` in each project folder

---

## Option 1 — Scientific Calculator (Console)

### Goal

Design a **scientific calculator** with basic arithmetic and **≥5 scientific operations** (e.g., `sin`, `cos`, `tan`, `sqrt`, `pow x^y`, `log10`, `ln`, `mod`, **integer division**, **factorial**). Operations accept **any number of numeric parameters** (e.g., `add 2.5 1 -2 24.5125 0.33`).

### Requirements

* **Input style (recommended):** command + space-separated arguments, e.g.:

  * `add 2.5 1 -2 24.5125 0.33`
  * `sin deg 30` or `sin rad 0.5235987756`
  * `pow 2 8` , `idiv 7 3`, `mod 7 3`, `fact 5`, `log10 1000`, `ln 2.71828`
* **Typing:** accept `int`/`double` seamlessly (parse to `double` internally).
* **Arity:** most functions support **variable arity** via `params double[]`.
* **Edge cases:** divide by zero; `sqrt` of negative; `fact` for non-integers or negatives (reject politely); domain for `log`.
* **Extensibility:** register operations via a **Strategy** + **Factory** combo.

### OOP & Patterns

* `IOperation { string Name {get;} string Help {get;} double Evaluate(params double[] args); }`
* `OperationFactory` maps command string → `IOperation` (Creational).
* `Calculator` owns a registry (`Dictionary<string, IOperation>`); can print help and dispatch.

### CLI commands (minimum)

* `add`, `sub`, `mul`, `div`, plus **≥5** scientific ops (`sin`, `cos`, `tan`, `sqrt`, `pow`, `log10`, `ln`, `mod`, `idiv`, `fact`).
* `mode` for angle unit (default **rad**, allow `deg`).
* `help`, `exit`.

### Minimal skeleton

```csharp
// Program.cs
using System.Globalization;

Calculator.Run();

public static class Calculator
{
    private static readonly Dictionary<string, IOperation> Ops = OperationFactory.CreateAll();
    private static AngleMode _angleMode = AngleMode.Radians;

    public static void Run()
    {
        Console.WriteLine("SciCalc — type 'help' or 'exit'");
        for(;;)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) return;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

            if (cmd == "help") { PrintHelp(); continue; }
            if (cmd == "mode" && parts.Length == 2)
            { _angleMode = parts[1].StartsWith("deg", true, CultureInfo.InvariantCulture) ? AngleMode.Degrees : AngleMode.Radians; Console.WriteLine($"Angle mode: {_angleMode}"); continue; }

            if (!Ops.TryGetValue(cmd, out var op))
            { Console.WriteLine("Unknown command. Try 'help'"); continue; }

            try
            {
                var args = parts.Skip(1).Select(ParseNumber).ToArray();
                var result = op switch
                {
                    ITrigonometric trig => trig.Evaluate(args, _angleMode),
                    _ => op.Evaluate(args)
                };
                Console.WriteLine(result.ToString("G17", CultureInfo.InvariantCulture));
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
    }

    private static double ParseNumber(string s)
    {
        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var d)) return d;
        throw new ArgumentException($"Not a number: '{s}'");
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Commands: " + string.Join(", ", Ops.Keys.OrderBy(x => x)) +
                          "\nAlso: help, mode (deg|rad), exit");
    }
}

public enum AngleMode { Degrees, Radians }

public interface IOperation
{
    string Name { get; }
    string Help { get; }
    double Evaluate(params double[] args);
}
public interface ITrigonometric : IOperation
{
    double Evaluate(double[] args, AngleMode mode);
}

// src/Services/OperationFactory.cs
public static class OperationFactory
{
    public static Dictionary<string, IOperation> CreateAll() => new()
    {
        ["add"] = new Add(), ["sub"] = new Sub(), ["mul"] = new Mul(), ["div"] = new Div(),
        ["sqrt"] = new Sqrt(), ["pow"] = new Pow(), ["log10"] = new Log10(), ["ln"] = new Ln(),
        ["mod"] = new Mod(), ["idiv"] = new IDiv(), ["fact"] = new Fact(),
        ["sin"] = new Sin(), ["cos"] = new Cos(), ["tan"] = new Tan(),
    };
}

// Example ops (put each in its own file under /Services/Operations)
public sealed class Add : IOperation { public string Name=>"add"; public string Help=>"add x y [z ...]";
    public double Evaluate(params double[] a) => a.Length==0 ? 0 : a.Sum(); }

public sealed class Div : IOperation { public string Name=>"div"; public string Help=>"div a b";
    public double Evaluate(params double[] a)
    { if (a.Length<2) throw new ArgumentException("Need at least 2 args");
      return a.Skip(1).Aggregate(a[0], (acc, x) => x==0 ? throw new DivideByZeroException() : acc/x); } }

public sealed class Sin : ITrigonometric { public string Name=>"sin"; public string Help=>"sin [deg|rad] value";
    public double Evaluate(params double[] args) => throw new NotSupportedException("Use trig overload");
    public double Evaluate(double[] args, AngleMode m)
    { if (args.Length!=1) throw new ArgumentException("Need 1 arg");
      var x = args[0]; if (m==AngleMode.Degrees) x = x*Math.PI/180.0; return Math.Sin(x); } }

public sealed class Fact : IOperation { public string Name=>"fact"; public string Help=>"fact n";
    public double Evaluate(params double[] a)
    { if (a.Length!=1) throw new ArgumentException("Need 1 arg");
      var n = a[0]; if (n<0 || n!=Math.Truncate(n)) throw new ArgumentException("n must be non-negative integer");
      long r=1; for (int i=1; i<= (int)n; i++) r*=i; return r; } }
```

### Acceptance tests (samples)

See `SciCalc/data/test-cases.txt` for 200+ comprehensive test cases including:

* **Basic arithmetic:** `add 2.5 1 -2 24.5125 0.33` → `26.3425`
* **Division types:** `div 7 2` → `3.5` ; `idiv 7 2` → `3`; `mod 7 2` → `1`
* **Angle modes:** `mode deg` → `Angle mode: Degrees`; `sin 30` → `0.5`
* **Advanced math:** `pow 2 8` → `256` ; `sqrt 9` → `3` ; `log10 1000` → `3`
* **Factorial:** `fact 5` → `120` ; `fact -1` → friendly error
* **Edge cases:** Division by zero, negative sqrt, factorial validation

**Full test suite**: All operations, error scenarios, and complex calculations available in test file.

---

## Option 2 — Manhattan Traffic Simulation (Console)

### Goal

Simulate traffic on a **Manhattan-like grid**: vehicles move between crossings; traffic lights alternate per axis; streets have capacities/flows; priority vehicles can pass on red; roadworks can close a road; end state prints each vehicle’s route/time and totals. The **final part** adds **shortest-path** routing (street travel times only) and optional reading from a config file. *(This mirrors the brief’s staged parts and concepts: vehicles, lights with flow/capacity/states, streets, crossroads, city grid, register, roadworks, final stats, and precomputed route planning.)*

### Core model (suggested)

* `Vehicle` (name/id, destination, priority, route history, travelTime).
* `TrafficLight` (state enum: Red/Yellow/Green; flow per minute; time per state; capacity; internal queue prioritising priority vehicles).
* `Street` (travelTime, capacity, vehicles with remaining time).
* `CrossRoad` (Name; four `TrafficLight`s + four `Street`s; `AddVehicle(side, vehicle)`, `Iterate()`: advance lights; move vehicles to opposite street respecting flow/yellow-half-flow; always serve priority vehicles first).
* `City` (grid of `CrossRoad`s; `AddVehicle(crossroadName, side, vehicle)`, `Iterate()`; allow random turns; roadworks to disable a street).
* `VehicleRegister` (lookup vehicle by name; track route/time).
* `TrafficFlowSimulation` (compose city + register; `Run(cycles)` or until all arrived; print totals; add/remove roadworks; precompute routes with **Dijkstra**).

### Behavioural notes

* Light timing (green durations per axis; yellow=1 min; red implied).
* Flow: X cars/min on green; **yellow is half flow (round down)**.
* Capacity: if full, cars queue upstream.
* Priority vehicles ignore red; still respect capacity constraints.

### OOP & Patterns

* Composition of components; `IClockTick`/`Iterate()` method across.
* **Strategy** (optional) for routing decisions (random vs shortest-path).
* **Repository** (optional) for reading config (if you load from file).

### Minimal skeleton (selected)

```csharp
public enum LightState { Red, Yellow, Green }
public enum Side { North, East, South, West }

public sealed class Vehicle
{
    public string Name { get; }
    public string Destination { get; private set; }
    public bool Priority { get; }
    public List<string> Route { get; } = new();
    public int TravelTime { get; private set; }
    public Vehicle(string name, string destination, bool priority=false)
    { Name = name; Destination = destination; Priority = priority; }
    public void AddHop(string cross) => Route.Add(cross);
    public void Tick(int minutes=1) => TravelTime += minutes;
    public override string ToString() => Priority
      ? $"Priority vehicle '{Name}' to {Destination}"
      : $"Vehicle '{Name}' to {Destination}";
}

public sealed class TrafficLight
{
    private readonly Queue<Vehicle> _q = new(); // extend to prioritise Priority vehicles
    public LightState State { get; private set; }
    private readonly Dictionary<LightState,int> _dur = new() {{LightState.Green,3},{LightState.Yellow,1},{LightState.Red,5}};
    private int _left = 3;
    public int FlowPerMinute { get; set; } = 30;
    public int Capacity { get; set; } = 50;

    public TrafficLight(LightState init = LightState.Red) { State = init; }
    public void SetTimeForState(LightState s, int minutes) => _dur[s]=minutes;

    public bool TryEnqueue(Vehicle v) { if (_q.Count >= Capacity) return false; _q.Enqueue(v); return true; }
    public IEnumerable<Vehicle> ReleaseThisMinute()
    {
        int allowed = State switch { LightState.Green => FlowPerMinute,
                                     LightState.Yellow => FlowPerMinute/2,
                                     _ => 0 };
        while (allowed-- > 0 && _q.Count>0) yield return _q.Dequeue();
    }

    public void Tick()
    {
        if (--_left > 0) return;
        State = State switch
        {
            LightState.Green => LightState.Yellow,
            LightState.Yellow => LightState.Red,
            LightState.Red => LightState.Green,
            _ => LightState.Red
        };
        _left = _dur[State];
    }
}

// CrossRoad, Street, City, VehicleRegister, and Dijkstra-based router follow similarly
```

### Shortest path (final part)

* Build a graph where nodes are crossroads; edges are streets with **weight = street travel time** (ignore lights waiting time).
* Pre-plan each vehicle’s route using **Dijkstra**; if roadworks close an edge, recompute affected routes (simple re-plan is fine for this scope).

### CLI (example)

* `add-vehicle <name> <startX,Y> <destX,Y> [priority]`
* `tick <minutes>` advances simulation; `status` prints crossings/queues; `roadworks <A>-<B> on|off`; `stats` prints per-vehicle routes & times + totals; `exit`.

### Acceptance tests (samples)

See `TrafficSim/examples.txt` for 400+ comprehensive test cases including:

* **Basic pathfinding:** Adjacent moves, diagonal paths, corner-to-corner
  - `path 0 0 1 0` → cost 1 (horizontal)
  - `path 0 0 0 1` → cost 1 (vertical)
  - `path 0 0 2 2` → cost 4 (Manhattan distance)
  
* **Algorithm verification:** Manhattan distance calculations
  - All shortest paths should equal |x2-x1| + |y2-y1|
  - Multiple valid paths with same cost are acceptable
  
* **Edge cases:** Same start/goal, grid boundaries
  - `path 1 1 1 1` → cost 0 (already at destination)
  - `path 0 0 4 4` → cost 8 (maximum distance in 5×5 grid)

**Full test suite**: See `TrafficSim/examples.txt` for complete testing guide with:
- Step-by-step Dijkstra verification
- Debugging tips and common mistakes
- Expected output formats
- Unit test examples for all classes

---

## Option 3 — Cinema Application (Console)

### Goal

Model a cinema with **rooms, screens, shows** (title, start/end times, room, price), seat selection/booking, and daily schedules. The final part adds a **Marathon Ticket**: a full-day pass that automatically composes a **sequence of non-overlapping shows** (movie after movie without gaps/conflicts).

### Core model

* `Cinema` (name; rooms; shows; booking service).
* `Room` (Id/Name, capacity, seat map strategy: simple numbered seats).
* `Movie` (Title, Duration, Rating?).
* `Show` (Id, Movie, Room, Start, End, Price; remaining seats).
* `Booking` (Id, ShowId, Seats[], Buyer).
* `IMarathonPlanner` → returns an ordered list of shows that fit a user’s day.

### OOP & Patterns

* **Repository** for show storage (in-memory JSON store).
* **Strategy** for marathon planning (start with greedy by earliest finish; stretch: maximise total watch time).
* **Adapter (optional)** if you later import from a CSV schedule.

### Algorithms

* **Marathon ticket:** classic interval scheduling. Greedy algorithm:

  1. Sort all shows for the day by **end time**.
  2. Iteratively pick the next show whose `Start >= lastEnd`.
  3. Result is a maximal set of non-overlapping shows (often near-optimal for count).
     *(Stretch: Weighted by duration if maximising watch time — DP on intervals.)*

### Minimal skeleton

```csharp
public sealed record Movie(string Title, TimeSpan Duration);
public sealed record Room(string Name, int Capacity);

public sealed class Show
{
    public Guid Id { get; } = Guid.NewGuid();
    public Movie Movie { get; }
    public Room Room { get; }
    public DateTime Start { get; }
    public DateTime End => Start + Movie.Duration;
    public decimal Price { get; }
    private readonly HashSet<int> _taken = new();
    public Show(Movie m, Room r, DateTime start, decimal price)
    { Movie=m; Room=r; Start=start; Price=price; }
    public bool TryBook(params int[] seats)
    {
        if (seats.Any(s => s<1 || s>Room.Capacity)) return false;
        if (seats.Any(_taken.Contains)) return false;
        foreach (var s in seats) _taken.Add(s);
        return true;
    }
}

public interface IShowStore { IEnumerable<Show> All(); void Add(Show s); }
public sealed class JsonShowStore : IShowStore
{
    private readonly List<Show> _shows = new(); // persist later if you like
    public IEnumerable<Show> All() => _shows;
    public void Add(Show s) => _shows.Add(s);
}

public interface IMarathonPlanner { List<Show> Plan(DateOnly day, IEnumerable<Show> shows); }

public sealed class GreedyMarathonPlanner : IMarathonPlanner
{
    public List<Show> Plan(DateOnly day, IEnumerable<Show> shows)
    {
        var dayShows = shows.Where(s => DateOnly.FromDateTime(s.Start)==day)
                            .OrderBy(s => s.End).ToList();
        var plan = new List<Show>();
        DateTime current = DateTime.MinValue;
        foreach (var s in dayShows)
        {
            if (s.Start >= current) { plan.Add(s); current = s.End; }
        }
        return plan;
    }
}
```

### CLI (example)

* `add-room "Room A" 100`
* `add-movie "Inception" 2:28`
* `add-show "Inception" "Room A" 2025-11-01T10:00 9.99`
* `list-shows 2025-11-01`
* `book <showId> 12 13 14`
* `marathon 2025-11-01` → prints selected show IDs in order.

### Acceptance tests (samples)

See `CinemaApp/data/test-cases.txt` for 300+ comprehensive test cases including:

* **Complete workflows:** Room setup → Movie addition → Show creation → Booking
* **Seat validation:** Same seat twice fails, invalid seat index fails
* **Marathon planning:**
  - Two overlapping shows → only one included
  - Sequential shows in different rooms → both allowed (no conflict)
  - Empty day → returns empty plan
  - Full day → maximizes number of shows watched
* **Edge cases:** Capacity limits, time overlaps, greedy algorithm verification

**Full test suite**: Complete booking scenarios, marathon test cases, and algorithm verification.

---

## Git/GitHub issue set (starter list for any project)

1. Initialise repo, `.gitignore`, README; scaffold console app.
2. Design model classes; print basic menu.
3. Implement core commands (MVP).
4. Persistence layer (JSON/CSV).
5. Algorithm feature (calc ops / shortest path / marathon planner).
6. Error handling & validation pass.
7. CLI help & UX polish.
8. README usage examples + sample data.
9. Final tidy-up (dead code, comments), tag `v1.0`.

---

## 📦 What You Get

### ✅ Complete Skeleton Code
- All 25 implementation files converted to educational templates
- Projects build successfully (even with skeletons!)
- Clear `NotImplementedException` markers showing what to implement

### ✅ Comprehensive Documentation (22,000+ lines)
- **QUICK_START.md** - Get running in 15 minutes
- **IMPLEMENTATION_GUIDE.md** - Week-by-week roadmap with examples
- **HINTS_AND_TIPS.md** - Solutions to common problems
- **QUICK_REFERENCE.md** - Fast lookup for all 25 files

### ✅ Test Data (900+ lines)
- **SciCalc**: 200+ arithmetic, trig, and edge case tests
- **CinemaApp**: 300+ booking and marathon scenarios
- **TrafficSim**: 400+ pathfinding and algorithm tests

### ✅ Example Templates
- Complete implementations at three difficulty levels
- Shows proper code structure and patterns
- Reference when stuck on implementation

---

## ✅ Success Criteria (Grading)

By end of Week 8 (or Week 5 for fast track), you should have:

### Functionality (40 points)
* [ ] All `NotImplementedException` replaced with working code
* [ ] All test cases from `data/test-cases.txt` pass
* [ ] Edge cases handled (empty input, invalid data, etc.)
* [ ] Compiles on .NET 8; no runtime crashes

### Code Quality & OOP (25 points)
* [ ] Clean, readable code with meaningful variable names
* [ ] Proper use of LINQ where appropriate
* [ ] Design patterns clearly implemented
* [ ] Comments explaining complex logic

### Git/GitHub Workflow (15 points)
* [ ] ≥12 PRs merged (one per major file/feature)
* [ ] Clear commit messages
* [ ] Good issue descriptions
* [ ] Feature branches used properly

### Robustness (10 points)
* [ ] Input validation on all user inputs
* [ ] Friendly error messages (no stack traces to user)
* [ ] Handles edge cases gracefully

### Documentation & Demo (10 points)
* [ ] Updated project README with your additions
* [ ] Example commands showing new features
* [ ] Short demo (≤5 min) or screencast (≤3 min)

---

## 🎯 Quick Tips

1. **Start with Easy Files (⭐)** - Build confidence first
2. **Read TODO Comments** - They contain step-by-step hints
3. **Use Example Templates** - Reference when stuck
4. **Test Frequently** - Run `dotnet run` after every change
5. **Check Test Cases** - Use `examples.txt` to verify
6. **Ask for Help** - Use office hours and course forum
7. **Commit Often** - Small commits are easier to review

---

## 📞 Getting Help

- **Documentation**: Start with `QUICK_START.md`
- **Examples**: Check `skeleton-examples/` folder
- **Test Cases**: Use `data/test-cases.txt` or `examples.txt` files
- **Common Issues**: See `HINTS_AND_TIPS.md`
- **Course Forum**: Ask questions early!
- **Office Hours**: Bring specific error messages

---

## 🎉 Ready to Start?

1. **Read** `QUICK_START.md` (15 minutes)
2. **Choose** your project (SciCalc recommended first)
3. **Build** and run it: `cd SciCalc && dotnet build && dotnet run`
4. **Pick** an easy file (⭐ difficulty)
5. **Implement** following the TODO comments
6. **Test** using examples from `data/test-cases.txt`
7. **Commit** and move to next file!

**Good luck! 🚀 You've got comprehensive scaffolding to guide you every step of the way!**

---

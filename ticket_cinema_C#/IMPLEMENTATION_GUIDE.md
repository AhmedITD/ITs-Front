# 📋 Implementation Guide for Skeleton Projects

## 🎯 Overview

This guide provides a **step-by-step roadmap** for implementing the skeleton code. Each project is broken down into manageable tasks with time estimates and dependencies.

---

## 🚀 Quick Start

### Prerequisites Checklist
- [ ] ✅ .NET 8 SDK installed (`dotnet --version`)
- [ ] ✅ Git configured (`git config --list`)
- [ ] ✅ Text editor or IDE ready (VS Code, Rider, Visual Studio)
- [ ] ✅ Basic C# knowledge (classes, methods, LINQ)
- [ ] ✅ Read through skeleton files to understand structure

### First Steps (Do This First!)
1. **Clone/Fork** the repository
2. **Read** `QUICK_START.md` for 15-minute introduction
3. **Look at examples** in `skeleton-examples/` folder
4. **Read test cases** in your project's `examples.txt` file
5. **Choose ONE project** (SciCalc, CinemaApp, or TrafficSim)
6. **Build the project** to verify setup: `dotnet build`
7. **Follow the week-by-week guide** below

### Test Data Files (Use These!)
Each project includes comprehensive test cases:
- **`SciCalc/data/test-cases.txt`** - 200+ arithmetic, trig, and edge case tests
- **`CinemaApp/data/test-cases.txt`** - 300+ booking and marathon scenarios  
- **`TrafficSim/examples.txt`** - 400+ pathfinding tests with algorithm guide

Use these files to verify your implementation at every step!

---

## 📅 Week-by-Week Implementation Schedule

### Week 1: Foundation & Simple Features (SciCalc Recommended)

#### Day 1: Setup (1-2 hours)
- [ ] Create GitHub repository
- [ ] Clone repository locally
- [ ] Verify project builds: `dotnet build`
- [ ] Run project to see skeleton: `dotnet run --project ProjectName.csproj`
- [ ] **Create Issue #1:** "Setup development environment"
- [ ] **Create Branch:** `feature/setup`
- [ ] Add `.gitignore` for C# (ignore `bin/`, `obj/`)
- [ ] **Create PR #1:** "Initial setup complete"

**Time:** 1-2 hours  
**Difficulty:** ⭐ Easy

---

#### Day 2: First Implementation (Add.cs) (2-3 hours)

**File:** `src/Services/Operations/Add.cs`

**Steps:**
1. Read the file completely, including all TODO comments
2. Look at `skeleton-examples/Add.cs` for reference
3. Implement the `Evaluate` method
4. Test with: `dotnet run --project SciCalc.csproj`
5. Try commands: `add 2 3`, `add`, `add 1 2 3 4`

**Implementation Checklist:**
- [ ] Handle empty arguments (return 0)
- [ ] Handle single argument (return that number)
- [ ] Handle multiple arguments (use LINQ Sum)
- [ ] Method compiles without errors
- [ ] All test cases pass

**Test Cases:**
```
add 2 3          → 5.0
add 1 2 3 4      → 10.0
add              → 0.0
add 5            → 5.0
add -5 5         → 0.0
```

**Time:** 30 minutes  
**Difficulty:** ⭐ Easy

**Git Workflow:**
- [ ] **Create Issue #2:** "Implement Add operation"
- [ ] **Create Branch:** `feature/add-operation`
- [ ] Implement and test
- [ ] Commit: `git commit -m "feat: implement Add operation"`
- [ ] **Create PR #2:** "Add operation complete with tests"
- [ ] Merge to main

---

#### Day 3: Similar Operations (Sub, Mul) (2-3 hours)

**Files:** `src/Services/Operations/Sub.cs`, `Mul.cs`

**Approach:** These are very similar to Add!
- Sub: Use LINQ or manual subtraction
- Mul: Use LINQ or manual multiplication

**Implementation Order:**
1. **Sub.cs** (20 min)
   - Similar to Add but subtract
   - Handle empty args: return 0
   - Handle single arg: return that number
   - Multiple args: `args[0] - args[1] - args[2] ...`
   
2. **Mul.cs** (20 min)
   - Use LINQ `Aggregate` method
   - Empty args: return 1 (multiplicative identity)
   - Single arg: return that number
   - Multiple: multiply all together

**Test Cases:**
```
sub 10 3         → 7.0
sub 10 3 2       → 5.0
mul 2 3          → 6.0
mul 2 3 4        → 24.0
mul              → 1.0
```

**Time:** 40 minutes total  
**Difficulty:** ⭐ Easy

**Git:** PR #3: "Implement Sub and Mul operations"

---

#### Day 4: Error Handling (Div) (1-2 hours)

**File:** `src/Services/Operations/Div.cs`

**New Challenge:** Division by zero!

**Steps:**
1. Implement basic division
2. Add validation: check if divisor is 0
3. Throw `DivideByZeroException` if zero
4. Test error cases

**Implementation Checklist:**
- [ ] Basic division works
- [ ] Division by zero throws exception
- [ ] Error message is clear
- [ ] Edge cases handled (negative numbers)

**Test Cases:**
```
div 10 2         → 5.0
div 15 3         → 5.0
div 10 0         → ERROR: Division by zero
div -10 2        → -5.0
```

**Time:** 30 minutes  
**Difficulty:** ⭐⭐ Medium

**Git:** PR #4: "Implement Div with error handling"

---

#### Day 5: Connect to Factory (2-3 hours)

**File:** `src/Services/OperationFactory.cs`

**Goal:** Register your operations in the factory.

**Current State:** Empty dictionary
```csharp
private static readonly Dictionary<string, IOperation> ops = new();
```

**Your Task:** Fill the dictionary
```csharp
private static readonly Dictionary<string, IOperation> ops = new()
{
    ["add"] = new Add(),
    ["sub"] = new Sub(),
    ["mul"] = new Mul(),
    ["div"] = new Div(),
};
```

**Testing:**
1. Run calculator: `dotnet run --project SciCalc.csproj`
2. Try all operations you implemented
3. Verify they work from command line

**Time:** 30 minutes  
**Difficulty:** ⭐ Easy

**Git:** PR #5: "Connect operations to factory"

---

### Week 2: Advanced Operations & Validation

#### Day 6-7: Math Library Operations (3-4 hours)

**Files:** `Sqrt.cs`, `Pow.cs`, `Log10.cs`, `Ln.cs`

**Pattern:** Use `Math` class from .NET
- `Sqrt`: `Math.Sqrt(x)` + validate x >= 0
- `Pow`: `Math.Pow(x, y)`
- `Log10`: `Math.Log10(x)` + validate x > 0
- `Ln`: `Math.Log(x)` + validate x > 0

**Implementation Order:**
1. **Sqrt.cs** (30 min) - Practice domain validation
2. **Pow.cs** (20 min) - Straightforward
3. **Log10.cs** (30 min) - Domain validation
4. **Ln.cs** (20 min) - Similar to Log10

**Common Pattern - Domain Validation:**
```csharp
if (x < 0)
    throw new ArgumentException("Cannot take sqrt of negative number");
return Math.Sqrt(x);
```

**Test Cases:**
```
sqrt 16          → 4.0
sqrt -1          → ERROR
pow 2 3          → 8.0
log10 100        → 2.0
ln 2.718281828   → 1.0 (approximately)
```

**Time:** 2 hours total  
**Difficulty:** ⭐⭐ Medium

**Git:** PR #6: "Implement Math library operations"

---

#### Day 8: Modulo and Integer Division (1-2 hours)

**Files:** `Mod.cs`, `IDiv.cs`

**Simple operations:**
- `Mod`: Use `%` operator
- `IDiv`: Use `/` then cast to int

**Implementation:**
```csharp
// Mod.cs
public double Evaluate(params double[] args)
{
    if (args.Length < 2)
        throw new ArgumentException("Need 2 numbers");
    return args[0] % args[1];
}

// IDiv.cs (similar but cast result to int)
```

**Test Cases:**
```
mod 10 3         → 1.0
idiv 10 3        → 3.0
```

**Time:** 30 minutes  
**Difficulty:** ⭐ Easy

**Git:** PR #7: "Implement Mod and IDiv"

---

#### Day 9: Factorial (2-3 hours)

**File:** `Fact.cs`

**Challenge:** Implement factorial (recursive OR iterative)

**Two Approaches:**

**Approach 1: Iterative (Recommended)**
```csharp
double result = 1;
for (int i = 2; i <= n; i++)
    result *= i;
return result;
```

**Approach 2: Recursive**
```csharp
if (n <= 1) return 1;
return n * Factorial(n - 1);
```

**Validation Needed:**
- Only accept non-negative integers
- Handle 0! = 1
- Handle large numbers (may overflow!)

**Test Cases:**
```
fact 5           → 120.0
fact 0           → 1.0
fact -1          → ERROR
fact 20          → 2432902008176640000.0
```

**Time:** 1 hour  
**Difficulty:** ⭐⭐⭐ Hard

**Git:** PR #8: "Implement factorial operation"

---

#### Day 10: Trigonometric Functions (3-4 hours)

**Files:** `Sin.cs`, `Cos.cs`, `Tan.cs`

**New Challenge:** Angle mode (degrees vs radians)!

**Pattern for all three:**
1. Check angle mode (from Calculator)
2. Convert degrees to radians if needed
3. Call Math.Sin/Cos/Tan
4. Handle special cases (tan at 90°)

**Angle Conversion:**
```csharp
if (angleMode == AngleMode.Degrees)
    radians = degrees * Math.PI / 180.0;
```

**Implementation Order:**
1. **Sin.cs** (40 min) - Full implementation
2. **Cos.cs** (30 min) - Similar to Sin
3. **Tan.cs** (40 min) - Add undefined value handling

**Test Cases:**
```
# In degrees mode:
sin 30           → 0.5
cos 60           → 0.5
tan 45           → 1.0
tan 90           → Undefined (or very large number)

# In radians mode:
sin 1.5708       → 1.0 (≈ π/2)
```

**Time:** 2 hours total  
**Difficulty:** ⭐⭐⭐ Hard

**Git:** PR #9: "Implement trig functions with angle mode"

---

### Week 3: CinemaApp or TrafficSim

**Choose your path:**
- **Easier:** CinemaApp (booking system + greedy algorithm)
- **Harder:** TrafficSim (state machines + Dijkstra)

---

## 🎬 CinemaApp Implementation Path

### Week 3, Day 1: Models (2-3 hours)

#### Step 1: Movie.cs (30 min)

**Simple record with validation:**
```csharp
public record Movie
{
    public string Title { get; init; }
    public TimeSpan Duration { get; init; }
    
    public Movie(string title, TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title required");
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive");
            
        Title = title;
        Duration = duration;
    }
}
```

**Test:**
```csharp
var movie = new Movie("Inception", TimeSpan.FromMinutes(148));
```

---

#### Step 2: Room.cs (20 min)

**Even simpler record:**
```csharp
public record Room
{
    public string Name { get; init; }
    public int Capacity { get; init; }
    
    public Room(string name, int capacity)
    {
        // TODO: Add validation
        // ...
    }
}
```

---

#### Step 3: Show.cs (2 hours)

**Complex class - See `skeleton-examples/Show.cs` for full guide**

**Implementation order:**
1. Add all properties (30 min)
2. Add constructor with validation (30 min)
3. Implement TryBook method (45 min)
4. Implement IsSeatAvailable (15 min)

**Critical parts:**
- Use `HashSet<int>` for booked seats
- Validate ALL seats before booking ANY
- Return bool for success/failure

**Git:** PR #10: "Implement all model classes"

---

### Week 3, Day 2-3: Repository (3-4 hours)

**File:** `src/Services/InMemoryShowStore.cs`

**Implements:** `IShowStore` interface

**Data structure:**
```csharp
private readonly List<Show> _shows = new();
```

**Methods to implement:**
1. `Add(Show show)` - Add to list
2. `Find(Guid id)` - Use LINQ FirstOrDefault
3. `FindByDate(DateOnly date)` - Filter by date
4. `GetAll()` - Return all shows

**Example implementation:**
```csharp
public Show? Find(Guid id)
{
    return _shows.FirstOrDefault(s => s.Id == id);
}
```

**Time:** 2 hours  
**Difficulty:** ⭐⭐ Medium

**Git:** PR #11: "Implement show repository"

---

### Week 3, Day 4-5: Greedy Algorithm (4-6 hours)

**File:** `src/Services/GreedyMarathonPlanner.cs`

**See `skeleton-examples/GreedyMarathonPlanner.cs` for detailed guide!**

**Algorithm steps:**
1. Filter shows by date
2. Sort by EndTime (earliest first)
3. Greedy selection (no overlaps)
4. Return as array

**This is the HARDEST part!**
- Read the full pseudocode
- Understand WHY it works
- Test thoroughly

**Time:** 3-4 hours  
**Difficulty:** ⭐⭐⭐⭐ Very Hard

**Git:** PR #12: "Implement greedy marathon planner"

---

### Week 3, Day 6-7: CLI Integration (3-4 hours)

**File:** `src/Services/App.cs`

**Implement command handlers:**
1. `HandleAddRoom` - Parse and create room
2. `HandleAddMovie` - Parse and create movie
3. `HandleAddShow` - Create show with all details
4. `HandleListShows` - Display shows for date
5. `HandleBook` - Book seats
6. `HandleMarathon` - Call planner

**Pattern for each handler:**
```csharp
private static void HandleAddRoom(string[] parts)
{
    // 1. Validate arguments
    if (parts.Length < 3) { ... }
    
    // 2. Parse data
    string name = parts[1];
    int capacity = int.Parse(parts[2]);
    
    // 3. Create object
    var room = new Room(name, capacity);
    
    // 4. Store it
    rooms[name] = room;
    
    // 5. Confirm to user
    Console.WriteLine($"✓ Room '{name}' created");
}
```

**Time:** 4 hours  
**Difficulty:** ⭐⭐⭐ Hard

**Git:** PR #13: "Implement all CLI commands"

---

## 🚦 TrafficSim Implementation Path

### Week 3, Day 1-2: Simple Models (3-4 hours)

#### Vehicle.cs (1 hour)
- Properties: Name, Destination, Priority
- RouteHistory: List<string>
- Add method to record visit

#### Street.cs (1 hour)
- Properties: Name, TravelTime, Capacity
- Queue of vehicles
- Methods: AddVehicle, RemoveVehicle

**Git:** PR #10: "Implement Vehicle and Street"

---

### Week 3, Day 3-4: State Machine (4-6 hours)

**File:** `src/Models/TrafficLight.cs`

**Challenge:** Implement traffic light state machine

**States:** Red → Green → Yellow → Red

**Visual diagram in file shows transitions**

**Methods to implement:**
1. `Iterate()` - Advance state
2. `CanPass(Vehicle)` - Check if vehicle can pass
3. Queue management

**Time:** 4 hours  
**Difficulty:** ⭐⭐⭐⭐ Very Hard

**Git:** PR #11: "Implement traffic light state machine"

---

### Week 3, Day 5-7: Dijkstra's Algorithm (6-8 hours)

**File:** `src/Services/ShortestPath.cs`

**THIS IS THE HARDEST PART OF ALL PROJECTS!**

**Algorithm:** Dijkstra's shortest path

**See full pseudocode in skeleton file**

**Resources:**
- Wikipedia: Dijkstra's algorithm
- Visualizer: https://www.cs.usfca.edu/~galles/visualization/Dijkstra.html
- Video: Search YouTube for "Dijkstra visualization"

**Implementation steps:**
1. Initialize distances and priority queue (2 hours)
2. Main loop: extract minimum (2 hours)
3. Relax edges (2 hours)
4. Test thoroughly (2 hours)

**Time:** 6-8 hours  
**Difficulty:** ⭐⭐⭐⭐⭐ Expert

**Git:** PR #12: "Implement Dijkstra's algorithm"

---

## 🧪 Testing Strategy

### Unit Testing (Optional but Recommended)

**Create test project:**
```bash
dotnet new xunit -n SciCalc.Tests
cd SciCalc.Tests
dotnet add reference ../SciCalc/SciCalc.csproj
```

**Example test:**
```csharp
[Fact]
public void Add_WithMultipleNumbers_ReturnsSum()
{
    var add = new Add();
    var result = add.Evaluate(2, 3, 4);
    Assert.Equal(9, result);
}
```

### Integration Testing

**Use data/examples.txt:**
1. Run each command manually
2. Verify output matches expected
3. Test edge cases
4. Test error cases

### Acceptance Testing

**User stories:**
- "As a user, I can add two numbers"
- "As a user, I can book seats for a show"
- "As a user, I can plan a marathon ticket"

---

## 📊 Progress Tracking

### Use GitHub Issues

**Create issues for each feature:**
- Issue #1: Setup environment
- Issue #2: Implement Add operation
- Issue #3: Implement Sub and Mul
- ... etc.

### Use Project Board

**Columns:**
- 📋 To Do
- 🚧 In Progress
- ✅ Done

Move issues as you work!

---

## 🆘 Getting Help

### When Stuck:

1. **Read the TODO comments again** - they have hints!
2. **Look at skeleton-examples/** - full guidance there
3. **Check HINTS_AND_TIPS.md** - common patterns
4. **Use debugger** - step through code
5. **Print debug statements** - see what's happening
6. **Google the error message** - someone else had it too!
7. **Ask instructor/peers** - that's why we're here!

### Debugging Tips:

```csharp
Console.WriteLine($"DEBUG: args.Length = {args.Length}");
Console.WriteLine($"DEBUG: First arg = {args[0]}");
```

---

## 🎯 Definition of Done

### For Each Feature:

- [ ] ✅ Code compiles without errors
- [ ] ✅ All test cases pass
- [ ] ✅ No TODO comments left in implementation
- [ ] ✅ XML comments added/updated
- [ ] ✅ Edge cases handled
- [ ] ✅ Error messages are user-friendly
- [ ] ✅ Git commit with clear message
- [ ] ✅ PR created and reviewed

### For Project Completion:

- [ ] ✅ All core features working
- [ ] ✅ At least 5 PRs merged
- [ ] ✅ README updated with examples
- [ ] ✅ Demo video recorded (or prepared)
- [ ] ✅ No NotImplementedException remaining
- [ ] ✅ Code follows OOP principles
- [ ] ✅ Design pattern clearly used

---

## 🏆 Success Metrics

### Week 1 Goal:
- SciCalc: 4-5 operations working

### Week 2 Goal:
- SciCalc: All 14 operations complete
- OR CinemaApp: Models done

### Week 3 Goal:
- Full project working
- All PRs merged
- Demo ready

---

## 📅 Timeline Summary

**Total Time Estimates:**

| Project | Easy Path | Full Implementation |
|---------|-----------|-------------------|
| SciCalc | 15-20 hours | 25-30 hours |
| CinemaApp | 20-25 hours | 30-35 hours |
| TrafficSim | 25-30 hours | 35-45 hours |

**Weekly Breakdown:**
- Week 1: 8-12 hours (foundation)
- Week 2: 10-15 hours (core features)
- Week 3: 8-12 hours (polish + demo)

**Total: 25-40 hours depending on project and speed**

---

*This guide provides a complete roadmap. Follow it step-by-step and you'll succeed! 🚀*

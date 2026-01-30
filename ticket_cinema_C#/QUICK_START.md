# 🚀 Quick Start Guide for Students

> **New to C#?** This guide will help you get started with your project in 15 minutes!

## ⚡ 5-Minute Setup

### 1. Install .NET 8 SDK
Visit: https://dotnet.microsoft.com/download/dotnet/8.0

**Check installation:**
```bash
dotnet --version
```
Should show: `8.0.x` or higher

### 2. Choose Your Project

Pick **ONE** project based on your interest:

| Project | Best For | Difficulty | Main Concepts |
|---------|----------|------------|---------------|
| **SciCalc** | Math lovers | ⭐ Easy | Interfaces, Factory Pattern |
| **CinemaApp** | Business logic fans | ⭐⭐ Medium | Records, Algorithms, Booking |
| **TrafficSim** | Algorithm enthusiasts | ⭐⭐⭐ Hard | State Machines, Graphs, Dijkstra |

**Recommendation**: Start with **SciCalc** if this is your first C# project!

### 3. Run the Project

```bash
# Navigate to your chosen project
cd SciCalc        # or CinemaApp or TrafficSim

# Run it!
dotnet run --project src/Program.cs
```

🎉 **Success!** You should see the application start.

---

## 📖 Understanding the Structure (2 minutes)

Every project follows this pattern:

```
YourProject/
├── src/                  # ALL your code goes here
│   ├── Models/           # Data classes (Vehicle, Movie, etc.)
│   ├── Services/         # Logic classes (Calculator, Planner)
│   └── Program.cs        # Starts everything (has Main method)
├── data/                 # Example files and test data
└── README.md             # Project documentation
```

### What goes where?

**Models/** = "What things exist?"
- Example: `Vehicle`, `Movie`, `Room`, `Show`
- These are usually simple data containers
- Think: **nouns**

**Services/** = "What can we do?"
- Example: `Calculator`, `OperationFactory`, `MarathonPlanner`
- These contain your business logic
- Think: **verbs**

---

## 🎯 Your First Task (5 minutes)

### Step 1: Read the Examples
```bash
# From your project folder
cat examples.txt              # Mac/Linux (TrafficSim)
cat data/test-cases.txt       # Mac/Linux (SciCalc, CinemaApp)

type examples.txt             # Windows (TrafficSim)
type data\test-cases.txt      # Windows (SciCalc, CinemaApp)
```

**What's in these files?**
- **SciCalc**: 200+ test commands with expected results
- **CinemaApp**: 300+ booking scenarios and marathon tests
- **TrafficSim**: 400+ pathfinding tests + algorithm explanations

### Step 2: Try Some Commands

**For SciCalc:** (from `data/test-cases.txt`)
```
> add 5 3 2
NotImplementedException: TODO: Implement addition
(You need to implement this!)

After implementation:
> add 5 3 2
10

> mode deg
> sin 30
0.5

> help
Available commands: add, sub, mul, div, sqrt, pow, sin, cos, tan...
```

**For CinemaApp:** (from `data/test-cases.txt`)
```
> add-room "Room A" 100
NotImplementedException: TODO: Implement room addition
(You need to implement this!)

After implementation:
> add-room "Room A" 100
Room 'Room A' added with 100 seats

> add-movie "Inception" 02:28
> list-shows 2025-11-01
```

**For TrafficSim:** (from `examples.txt`)
```
> path 0 0 2 2
NotImplementedException: TODO: Implement Dijkstra's algorithm
(You need to implement this!)

After implementation:
> path 0 0 2 2
Shortest path from (0,0) to (2,2):
(0,0) → (1,0) → (2,0) → (2,1) → (2,2)
Total cost: 4
```

**💡 Tip**: All projects will throw `NotImplementedException` until you implement the methods!

### Step 3: Read ONE File

Pick a simple file to understand:
- **SciCalc**: `src/Services/Operations/Add.cs`
- **CinemaApp**: `src/Models/Movie.cs`
- **TrafficSim**: `src/Models/Vehicle.cs`

**Look for:**
- ✅ XML comments (`///`) explaining what it does
- ✅ Inline comments explaining WHY
- ✅ Pattern labels (FACTORY, STRATEGY, etc.)

---

## 🏗️ Week 1 Roadmap

### Monday: Setup & Explore
- [ ] Install .NET 8
- [ ] Run the project
- [ ] Try all example commands
- [ ] Read project README

### Tuesday: Understand Structure
- [ ] Read all files in `Models/`
- [ ] Read all files in `Services/`
- [ ] Draw a diagram showing how classes connect
- [ ] Identify which design patterns are used

### Wednesday: Make Changes
- [ ] Create a branch: `git checkout -b feature/my-first-change`
- [ ] Add a small feature (suggestions below)
- [ ] Test it works
- [ ] Commit: `git commit -m "Add my feature"`

### Thursday: Polish & Document
- [ ] Add error handling for your feature
- [ ] Write comments explaining your code
- [ ] Update README with your feature
- [ ] Create pull request

### Friday: Review & Demo
- [ ] Review your own PR
- [ ] Test edge cases
- [ ] Fix any bugs
- [ ] Prepare 2-minute demo

---

## 💡 First Feature Ideas

### SciCalc - Easy Additions:
1. **Add `abs` operation** (absolute value)
   - File: `src/Services/Operations/Abs.cs`
   - Formula: Returns positive version of number
   
2. **Add `max` operation** (find maximum)
   - File: `src/Services/Operations/Max.cs`
   - Use: `max 5 2 9 1` → `9`

3. **Add calculation history**
   - Store last 10 results
   - New command: `history`

### CinemaApp - Easy Additions:
1. **Add room capacity check**
   - Show warning if room is 80%+ booked
   
2. **Add movie genre**
   - Extend `Movie` record with genre
   - Filter shows by genre

3. **Add customer names to bookings**
   - Track who booked which seats

### TrafficSim - Easy Additions:
1. **Add vehicle status command**
   - Show current location of all vehicles
   
2. **Add traffic statistics**
   - Average travel time
   - Number of vehicles at each crossroad

3. **Add visualization**
   - Print ASCII grid showing vehicle positions

---

## 🔧 Common Commands

### Building & Running
```bash
# Build the project (compile)
dotnet build

# Run the project
dotnet run

# Clean build artifacts
dotnet clean

# Run and watch for changes (auto-restart)
dotnet watch run
```

### Git Commands
```bash
# Check status
git status

# Create new branch
git checkout -b feature/my-feature

# Add files
git add .

# Commit with message
git commit -m "Add new feature"

# Push to GitHub
git push origin feature/my-feature

# Switch back to main
git checkout main
```

### File Operations
```bash
# Create new file
touch src/Models/MyClass.cs        # Mac/Linux
type nul > src\Models\MyClass.cs   # Windows

# View file
cat src/Models/MyClass.cs          # Mac/Linux
type src\Models\MyClass.cs         # Windows
```

---

## 🐛 Troubleshooting

### Problem: "dotnet: command not found"
**Solution**: Install .NET 8 SDK from microsoft.com

### Problem: "Build FAILED"
**Solutions**:
1. Check for syntax errors (missing semicolons, brackets)
2. Run `dotnet clean` then `dotnet build`
3. Check that all `using` statements are correct
4. Verify file is in correct namespace

### Problem: "Could not find Program.cs"
**Solution**: You're in wrong directory. Use `cd` to navigate:
```bash
cd SciCalc              # Go into project
cd src                  # Go into src folder
cd ..                   # Go back up one level
pwd                     # Show current directory
```

### Problem: "Namespace not found"
**Solution**: Add `using` statement at top of file:
```csharp
using SciCalc.Models;
using SciCalc.Services;
```

### Problem: "Interface not implemented"
**Solution**: Make sure your class has ALL methods from interface:
```csharp
// Interface says:
public interface IOperation {
    string Name { get; }
    double Evaluate(params double[] args);
}

// Your class must have BOTH:
public class MyOp : IOperation {
    public string Name => "myop";  // ✅ Has Name
    public double Evaluate(...) { } // ✅ Has Evaluate
}
```

---

## 📚 Learning Resources

### C# Basics
- **Microsoft C# Tutorial**: https://learn.microsoft.com/en-us/dotnet/csharp/
- **C# Syntax Cheatsheet**: https://www.codecademy.com/learn/learn-c-sharp/modules/csharp-hello-world/cheatsheet

### Design Patterns
- **Strategy Pattern**: Objects doing the same task differently
- **Factory Pattern**: One place creates all objects
- **Repository Pattern**: Hide how data is stored

### Useful C# Features in This Project
- `record`: Immutable data class with auto-equality
- `params`: Accept variable number of arguments
- `??`: Null-coalescing operator (provide default)
- `?.`: Null-conditional operator (safe access)
- `=>`: Expression-bodied members (shorthand)

---

## ✅ Week 1 Checklist

Copy this into your own notes and check off as you go:

```markdown
## Setup
- [ ] .NET 8 SDK installed
- [ ] Project runs successfully
- [ ] Can execute example commands
- [ ] GitHub repository created

## Understanding
- [ ] Read main README.md
- [ ] Read project-specific README
- [ ] Understand folder structure
- [ ] Identified design patterns used
- [ ] Drew class diagram

## First Changes
- [ ] Created feature branch
- [ ] Added small feature
- [ ] Tested feature works
- [ ] Added comments to new code
- [ ] Committed changes

## Pull Request
- [ ] Created PR on GitHub
- [ ] Wrote clear PR description
- [ ] Added testing notes
- [ ] Reviewed own code
- [ ] Merged PR

## Documentation
- [ ] Updated README with new feature
- [ ] Added example command
- [ ] Documented any issues encountered
- [ ] Prepared demo script
```

---

## 🎓 Pro Tips

1. **Read Code Top-to-Bottom**
   - Start with `Program.cs` (entry point)
   - Follow the flow from there
   - Understand what calls what

2. **Comment As You Go**
   - If you figured something out, write it down
   - Future you will thank present you

3. **Test After Every Change**
   - Don't write 100 lines then test
   - Test every 5-10 lines
   - Catch bugs early

4. **Use `Console.WriteLine()` for Debugging**
   ```csharp
   Console.WriteLine($"Debug: value = {myValue}");
   ```

5. **Ask Questions**
   - Use course forum
   - Attend office hours
   - Study with classmates

6. **Start Small**
   - Better to finish 80% well than 100% poorly
   - Get basics working first
   - Add extras if time permits

---

## 🎯 Success Criteria

By end of Week 5, you should be able to:

✅ Explain the OOP principles in your project  
✅ Point out where design patterns are used  
✅ Add a new feature without breaking existing code  
✅ Handle errors gracefully  
✅ Create a pull request with clear description  
✅ Demo your project in 5 minutes  

---

## 🚀 Ready to Start?

1. Choose your project
2. Run the examples
3. Read the code
4. Make your first change
5. Have fun! 🎉

**Remember**: Everyone starts as a beginner. The best way to learn is by doing. Don't be afraid to make mistakes - that's how you learn!

Good luck! 💪

---

## 📞 Need Help?
Discord or Contact Assistants 

You're not alone in this journey! 🤝

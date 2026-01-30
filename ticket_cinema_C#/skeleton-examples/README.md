# 📚 Skeleton Examples

This folder contains **three fully-annotated skeleton examples** showing different levels of complexity and guidance.

---

## 🎯 Purpose

These examples demonstrate:
- ✅ How to structure skeleton code
- ✅ Appropriate level of guidance for each difficulty
- ✅ Educational annotation format
- ✅ Balance between hints and solutions

Use these as **templates** when converting other files!

---

## 📁 Files

### 1. **Add.cs** - Simple Operation ⭐

**Difficulty:** Easy  
**Time:** 10-15 minutes  
**Learning:** Basic LINQ, params keyword

**What students implement:**
```csharp
// One-liner solution possible!
return args.Length == 0 ? 0 : args.Sum();
```

**Guidance level:** Maximum scaffolding
- Step-by-step instructions
- Multiple hints
- Code snippets
- Common mistakes
- One-line solution teaser

**Use as template for:**
- Sub.cs, Mul.cs (similar simple operations)
- Other arithmetic operations
- Simple model properties

---

### 2. **Show.cs** - Business Logic Class ⭐⭐⭐

**Difficulty:** Medium  
**Time:** 45-60 minutes  
**Learning:** Class design, encapsulation, collections, validation

**What students implement:**
- 7 properties (some computed)
- Constructor with validation
- TryBook method (atomic booking)
- IsSeatAvailable method
- Optional ToString override

**Guidance level:** Medium
- Detailed TODO for complex parts
- Code structure suggestions
- Multiple implementation hints
- Common mistakes highlighted
- Pattern explanations

**Use as template for:**
- Movie.cs, Room.cs (simpler models)
- Vehicle.cs, Street.cs (TrafficSim models)
- Other classes with business logic

---

### 3. **GreedyMarathonPlanner.cs** - Complex Algorithm ⭐⭐⭐⭐

**Difficulty:** Hard  
**Time:** 2-3 hours  
**Learning:** Greedy algorithms, interval scheduling, LINQ, algorithm analysis

**What students implement:**
```csharp
// Greedy interval scheduling algorithm
// 1. Filter by date
// 2. Sort by end time
// 3. Select non-overlapping shows
```

**Guidance level:** Maximum
- Complete pseudocode
- Visual examples
- Step-by-step implementation guide
- Multiple code snippets
- Theory explanation
- Proof sketch
- Complexity analysis
- Links to resources
- Common pitfalls

**Use as template for:**
- ShortestPath.cs (Dijkstra algorithm)
- TrafficLight.cs (state machine)
- Other complex algorithms

---

## 🎓 Annotation Structure

Every skeleton example follows this format:

```csharp
//==============================================================================
// FILE: ClassName.cs
// LOCATION: Where it goes in project
// PURPOSE: What this class does
//
// 📚 LEARNING OBJECTIVES:
//   ✅ What students will learn
//
// 📋 PREREQUISITES:
//   ✅ What they need to know first
//
// ⭐ DIFFICULTY: 1-5 stars
// ⏱️ ESTIMATED TIME: Hours needed
//
// 🧪 TEST CASES:
//   Input → Expected output
//==============================================================================

using ...;

namespace ...;

/// <summary>
/// Class description
/// </summary>
public class ClassName
{
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Implementation instructions
    /// 
    /// 📝 REQUIREMENTS:
    /// What needs to be implemented
    /// 
    /// 💡 HINTS:
    /// How to approach it
    /// 
    /// 🧪 TEST CASES:
    /// Verification inputs/outputs
    /// 
    /// ⚡ COMPLEXITY:
    /// Time and space analysis
    /// 
    /// 🚨 COMMON MISTAKES:
    /// What to avoid
    /// =======================================================
    /// </summary>
    public ReturnType Method(params)
    {
        throw new NotImplementedException("Brief reminder");
    }
}

/*
 * 🎓 LEARNING NOTES:
 * Additional theory, context, alternatives
 */
```

---

## 📊 Comparison Table

| File | Lines | Difficulty | Time | Guidance | Key Concepts |
|------|-------|------------|------|----------|--------------|
| Add.cs | 189 | ⭐ Easy | 15 min | Maximum | LINQ, params |
| Show.cs | 428 | ⭐⭐⭐ Medium | 60 min | Medium | OOP, validation |
| GreedyMarathonPlanner.cs | 315 | ⭐⭐⭐⭐ Hard | 2-3 hrs | Maximum | Algorithms, proofs |

---

## 🎯 How to Use These Templates

### For Instructors Converting Files:

1. **Choose similar file from examples**
   - Simple operation? Use Add.cs pattern
   - Business logic class? Use Show.cs pattern
   - Complex algorithm? Use GreedyMarathonPlanner.cs pattern

2. **Copy annotation structure**
   - File header with metadata
   - TODO comment format
   - Learning notes at bottom

3. **Adjust guidance level**
   - Simple files: Less hints
   - Complex files: More hints
   - Match difficulty to guidance

4. **Test comprehensiveness**
   - Can student understand task from TODO alone?
   - Are hints helpful but not solutions?
   - Do test cases cover edge cases?

### For Students Learning:

1. **Read entire file first** (don't skip to TODO!)
   - Understand class purpose
   - Note learning objectives
   - Check prerequisites
   - Review test cases

2. **Plan before coding**
   - Read requirements carefully
   - Understand algorithm (if applicable)
   - Sketch out approach
   - Ask questions if confused

3. **Implement step by step**
   - Follow numbered steps in TODO
   - Use hints when stuck
   - Test frequently
   - Refactor after working

4. **Verify with test cases**
   - Run all provided test cases
   - Test edge cases
   - Handle errors gracefully
   - Ensure correctness

---

## 📈 Progression Through Examples

### Learning Path:

**Stage 1: Simple (Add.cs)**
- Goal: Get comfortable with skeleton format
- Focus: Basic C# and LINQ
- Success: Complete in < 20 minutes

**Stage 2: Medium (Show.cs)**
- Goal: Design more complex classes
- Focus: OOP principles, validation
- Success: All methods working, tests passing

**Stage 3: Complex (GreedyMarathonPlanner.cs)**
- Goal: Implement non-trivial algorithms
- Focus: Algorithm analysis, correctness
- Success: Understand WHY it works, not just HOW

---

## 🧪 Testing Your Skeletons

After converting a file, verify:

### Compilation Test:
```bash
dotnet build
# Should succeed even with NotImplementedException
```

### Structure Test:
- [ ] Has file header with metadata?
- [ ] Has learning objectives?
- [ ] Has difficulty and time estimate?
- [ ] Has test cases?

### TODO Test:
- [ ] Are requirements clear?
- [ ] Are hints helpful but not complete?
- [ ] Is step-by-step guide provided?
- [ ] Are common mistakes noted?

### Student Test:
- [ ] Give to 1-2 students
- [ ] Can they understand the task?
- [ ] Are hints sufficient?
- [ ] Is time estimate accurate?

---

## 💡 Best Practices

### DO:
✅ Provide complete pseudocode for complex algorithms  
✅ Show multiple solution approaches  
✅ Link to external resources  
✅ Explain WHY, not just HOW  
✅ Include complexity analysis  
✅ Note common pitfalls  
✅ Give realistic time estimates  

### DON'T:
❌ Give complete implementation  
❌ Use vague hints ("just think about it")  
❌ Omit test cases  
❌ Skip error handling guidance  
❌ Forget to explain design decisions  
❌ Assume prior knowledge  

---

## 🔄 Maintenance

### After Each Semester:

1. **Collect feedback**
   - Which hints were most helpful?
   - Which parts were confusing?
   - Were time estimates accurate?

2. **Update accordingly**
   - Clarify confusing TODOs
   - Add missing hints
   - Adjust time estimates
   - Add new test cases

3. **Keep examples fresh**
   - Update to latest C# features
   - Refresh external links
   - Add new learning notes

---

## 📚 Additional Resources

### For Creating Skeletons:
- `../SKELETON_REFACTORING_PLAN.md` - Overall strategy
- `../SKELETON_CONVERSION_COMPLETE_GUIDE.md` - Full process
- `../IMPLEMENTATION_GUIDE.md` - Student perspective

### For Understanding Concepts:
- `../HINTS_AND_TIPS.md` - Code patterns
- `../README.md` - Project overview
- `../QUICK_START.md` - Getting started

---

## 🎯 Quick Reference Card

```
┌─────────────────────────────────────────────────┐
│  SKELETON TEMPLATE SELECTION GUIDE              │
├─────────────────────────────────────────────────┤
│                                                  │
│  Simple operation/method? → Use Add.cs          │
│                                                  │
│  Business logic class?    → Use Show.cs         │
│                                                  │
│  Complex algorithm?       → Use Marathon.cs     │
│                                                  │
│  State machine?           → Adapt Marathon.cs   │
│                                                  │
│  Graph algorithm?         → Adapt Marathon.cs   │
│                                                  │
└─────────────────────────────────────────────────┘

Guidance Formula:
  Difficulty ↑ = Guidance ↑
  (Harder tasks need more scaffolding)
```

---

## 🏆 Success Criteria

A skeleton example is successful when:

✅ **Students can start immediately** (clear task)  
✅ **Students can make progress** (helpful hints)  
✅ **Students can finish** (sufficient guidance)  
✅ **Students understand deeply** (learning notes)  
✅ **Students can verify correctness** (test cases)  

---

*These examples represent hundreds of hours of pedagogical refinement. Use them as gold standards for your skeleton conversions! 🌟*

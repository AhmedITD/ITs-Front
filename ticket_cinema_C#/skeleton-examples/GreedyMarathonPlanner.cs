//==============================================================================
// FILE: GreedyMarathonPlanner.cs
// LOCATION: src/Services/GreedyMarathonPlanner.cs
// PURPOSE: Plans optimal marathon ticket using greedy interval scheduling
//
// 📚 LEARNING OBJECTIVES:
//   ✅ Understand and implement greedy algorithms
//   ✅ Learn interval scheduling problem and solution
//   ✅ Practice LINQ for filtering and sorting
//   ✅ Work with DateTime and TimeSpan types
//
// 📋 PREREQUISITES:
//   ✅ Understand LINQ (Where, OrderBy, ToArray)
//   ✅ Know what greedy algorithms are
//   ✅ Familiar with DateTime comparisons
//   ✅ Understand the problem: non-overlapping intervals
//
// ⭐ DIFFICULTY: Hard (Algorithm implementation)
// ⏱️ ESTIMATED TIME: 2-3 hours
//
// 🧪 TEST CASES:
//   Shows: [10:00-12:00, 11:00-13:00, 12:30-14:00, 14:00-16:00]
//   Expected: [10:00-12:00, 12:30-14:00, 14:00-16:00]  (3 shows)
//   
//   Shows: [09:00-11:00, 10:00-12:00, 11:30-13:00]
//   Expected: [09:00-11:00, 11:30-13:00]  (2 shows)
//==============================================================================

using System;
using System.Linq;
using CinemaApp.Models;

namespace CinemaApp.Services;

/// <summary>
/// Plans marathon tickets using greedy interval scheduling algorithm.
/// Selects maximum number of non-overlapping shows for a given day.
/// This is an optimal solution for the interval scheduling problem.
/// </summary>
public sealed class GreedyMarathonPlanner : IMarathonPlanner
{
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Implement greedy interval scheduling algorithm.
    /// 
    /// 📝 PROBLEM DESCRIPTION:
    /// Given a list of shows (each with start and end time), select the
    /// maximum number of shows that don't overlap. A "Marathon Ticket"
    /// means watching as many movies as possible in one day.
    /// 
    /// 📚 ALGORITHM: Greedy Interval Scheduling
    /// 
    /// The key insight: Always pick the show that ends EARLIEST.
    /// Why? It leaves the most time for future shows.
    /// 
    /// Visual Example:
    ///   Timeline: 09:00 ─────────────────────────── 20:00
    ///   
    ///   Show A:   [10:00 ══════════ 12:00]
    ///   Show B:           [11:00 ══════════ 13:00]
    ///   Show C:                  [12:30 ════════ 14:30]
    ///   Show D:                             [14:00 ════ 16:00]
    ///   
    ///   Sorted by end time: A, B, C, D
    ///   
    ///   Step 1: Pick A (ends 12:00)
    ///   Step 2: Skip B (starts 11:00, overlaps with A)
    ///   Step 3: Pick C (starts 12:30, doesn't overlap)
    ///   Step 4: Skip D (starts 14:00, overlaps with C)
    ///   
    ///   Result: [A, C]  ← Maximum 2 shows
    /// 
    /// 🎯 ALGORITHM PSEUDOCODE:
    /// 
    ///   function PlanMarathon(shows, date):
    ///       // Step 1: Filter shows for given date
    ///       relevantShows ← shows where DateOnly.FromDateTime(show.StartTime) == date
    ///       
    ///       // Step 2: Sort by end time (earliest ending first)
    ///       sorted ← relevantShows sorted by EndTime ascending
    ///       
    ///       // Step 3: Greedy selection
    ///       result ← empty list
    ///       lastEndTime ← DateTime.MinValue
    ///       
    ///       for each show in sorted:
    ///           if show.StartTime >= lastEndTime:
    ///               // No overlap! Add this show
    ///               add show to result
    ///               lastEndTime ← show.EndTime
    ///           else:
    ///               // Overlaps with previous show, skip it
    ///               continue
    ///       
    ///       return result as array
    /// 
    /// 💡 IMPLEMENTATION HINTS:
    /// 
    /// Hint 1: Filter shows for the date
    ///   var relevantShows = shows.Where(s => 
    ///       DateOnly.FromDateTime(s.StartTime) == date);
    /// 
    /// Hint 2: Sort by EndTime (earliest first)
    ///   var sorted = relevantShows.OrderBy(s => s.EndTime);
    /// 
    /// Hint 3: Check for overlap
    ///   if (currentShow.StartTime >= lastEndTime)
    ///       // No overlap!
    /// 
    /// Hint 4: Track last end time
    ///   DateTime lastEndTime = DateTime.MinValue;
    ///   // Update after each selected show:
    ///   lastEndTime = show.EndTime;
    /// 
    /// Hint 5: Build result list
    ///   List<Show> result = new();
    ///   result.Add(show);
    ///   return result.ToArray();
    /// 
    /// 📖 HELPFUL LINKS:
    /// - Interval Scheduling: https://en.wikipedia.org/wiki/Interval_scheduling
    /// - DateTime comparison: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.compare
    /// - LINQ OrderBy: https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderby
    /// - Greedy algorithms: https://en.wikipedia.org/wiki/Greedy_algorithm
    /// 
    /// ⚡ COMPLEXITY ANALYSIS:
    /// - Time Complexity: O(n log n) where n = number of shows
    ///   - Filtering: O(n)
    ///   - Sorting: O(n log n)  ← dominant term
    ///   - Greedy selection: O(n)
    /// - Space Complexity: O(n) for result list
    /// 
    /// 🎓 WHY THIS WORKS (Proof Sketch):
    /// 
    /// Theorem: Greedy choice is optimal.
    /// 
    /// Proof idea:
    /// 1. Suppose optimal solution O exists that differs from greedy G
    /// 2. Let f₁ be first show in O, g₁ be first show in G
    /// 3. Since G picks earliest-ending, g₁.end ≤ f₁.end
    /// 4. We can replace f₁ with g₁ in O without conflicts
    /// 5. This gives a solution as good as O
    /// 6. By induction, G is optimal
    /// 
    /// 🔍 STEP-BY-STEP IMPLEMENTATION GUIDE:
    /// 
    /// Step 1: Filter shows (use LINQ Where)
    ///   var relevantShows = shows.Where(s => ...);
    /// 
    /// Step 2: Sort by end time (use LINQ OrderBy)
    ///   var sorted = relevantShows.OrderBy(s => s.EndTime);
    /// 
    /// Step 3: Initialize result and tracking variables
    ///   List<Show> result = new();
    ///   DateTime lastEndTime = DateTime.MinValue;
    /// 
    /// Step 4: Loop through sorted shows
    ///   foreach (var show in sorted)
    ///   {
    ///       // Check for overlap...
    ///   }
    /// 
    /// Step 5: Check overlap and add if no conflict
    ///   if (show.StartTime >= lastEndTime)
    ///   {
    ///       result.Add(show);
    ///       lastEndTime = show.EndTime;
    ///   }
    /// 
    /// Step 6: Return as array
    ///   return result.ToArray();
    /// 
    /// 🧪 TESTING STRATEGY:
    /// 
    /// Test 1: No shows on date → return empty array
    /// Test 2: One show → return that show
    /// Test 3: All shows overlap → return only first (earliest ending)
    /// Test 4: No overlaps → return all shows
    /// Test 5: Mixed case → return maximum non-overlapping subset
    /// 
    /// 🚨 COMMON MISTAKES:
    /// 
    /// ❌ Sorting by start time instead of end time
    ///    → Doesn't give optimal solution!
    /// 
    /// ❌ Using > instead of >= for overlap check
    ///    → Shows ending exactly when next starts ARE allowed!
    /// 
    /// ❌ Forgetting to filter by date first
    ///    → Algorithm includes shows from other days
    /// 
    /// ❌ Not updating lastEndTime
    ///    → All shows after first are skipped
    /// 
    /// =======================================================
    /// </summary>
    /// <param name="shows">All available shows (not yet filtered)</param>
    /// <param name="date">The date to plan marathon for</param>
    /// <returns>Array of non-overlapping shows, maximum count</returns>
    public Show[] PlanMarathon(Show[] shows, DateOnly date)
    {
        // TODO: Remove this and implement the algorithm!
        throw new NotImplementedException(
            "STUDENT TODO: Implement greedy interval scheduling. " +
            "Steps: 1) Filter by date, 2) Sort by EndTime, 3) Greedy selection. " +
            "See detailed pseudocode and hints in comments above."
        );
        
        // Suggested structure (remove comments and implement):
        
        // Step 1: Filter shows for the given date
        // var relevantShows = shows.Where(s => /* check date */);
        
        // Step 2: Sort by end time (earliest first)
        // var sorted = relevantShows.OrderBy(s => /* sort key */);
        
        // Step 3: Initialize result and tracking
        // List<Show> result = new();
        // DateTime lastEndTime = DateTime.MinValue;
        
        // Step 4: Greedy selection loop
        // foreach (var show in sorted)
        // {
        //     if (/* no overlap? */)
        //     {
        //         /* add to result */
        //         /* update lastEndTime */
        //     }
        // }
        
        // Step 5: Convert to array and return
        // return result.ToArray();
    }
}

/*
 * 🎓 LEARNING NOTES:
 * 
 * Why Greedy Works Here (But Not Always):
 * 
 * Greedy algorithms make locally optimal choices hoping for global optimum.
 * They DON'T always work! But for interval scheduling, greedy IS optimal.
 * 
 * Key insight: Picking earliest-ending show ALWAYS leaves maximum room
 * for future selections. No other choice could possibly be better.
 * 
 * Counter-example where greedy fails:
 * - Knapsack problem: Picking highest value/weight ratio isn't always optimal
 * - You need Dynamic Programming for knapsack!
 * 
 * Related Problems:
 * 1. Weighted interval scheduling → Use Dynamic Programming (see bonus!)
 * 2. Room/machine scheduling → Same greedy algorithm works
 * 3. Activity selection → Classic greedy algorithm application
 * 
 * Real-World Applications:
 * - Conference scheduling (meeting rooms)
 * - CPU task scheduling
 * - Classroom assignment
 * - TV program selection
 * - Flight scheduling
 * 
 * Alternative Approaches (Don't use these, but good to know):
 * 1. Brute force: Try all 2^n subsets → O(2^n) time, way too slow!
 * 2. Dynamic Programming: Overkill for unweighted case, use for weighted
 * 3. Branch and bound: More complex, no benefit here
 * 
 * Pattern Recognition:
 * When you see "maximum number of non-overlapping" → Think GREEDY
 * When you see "maximum value/weight" → Think DYNAMIC PROGRAMMING
 * 
 * Bonus Challenge (After completing this):
 * See docs/dp-marathon-bonus.md for weighted interval scheduling!
 * What if each show has a different ticket price and we want max total value?
 */

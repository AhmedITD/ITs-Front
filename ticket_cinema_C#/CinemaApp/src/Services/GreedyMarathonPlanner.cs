//==============================================================================
// FILE: GreedyMarathonPlanner.cs
// ⭐⭐⭐⭐⭐ DIFFICULTY: Hard | ⏱️ TIME: 90-120 minutes
// 🧪 TEST: See skeleton-examples/GreedyMarathonPlanner.cs for tests & complete reference
//==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using CinemaApp.Models;

namespace CinemaApp.Services;

/// <summary>
/// Greedy algorithm for marathon planning.
/// This is a classic INTERVAL SCHEDULING algorithm from computer science.
/// 
/// Algorithm:
/// 1. Sort all shows by END time (earliest ending first)
/// 2. Pick the first show
/// 3. For each remaining show:
///    - If it starts after the previous show ends, pick it
///    - Otherwise, skip it
/// 
/// This maximizes the NUMBER of movies you can watch.
/// It's "greedy" because it always picks the next available show without looking ahead.
/// 
/// Time Complexity: O(n log n) due to sorting
/// Space Complexity: O(n) for storing the plan
/// </summary>
public sealed class GreedyMarathonPlanner : IMarathonPlanner
{
    //==========================================================================
    // STUDENT TODO: Implement greedy interval scheduling algorithm
    //
    // LEARNING OBJECTIVES:
    // - Understand greedy algorithms and when they produce optimal results
    // - Apply INTERVAL SCHEDULING problem pattern
    // - Use LINQ for filtering and sorting
    // - Practice algorithm complexity analysis
    //
    // IMPLEMENTATION STEPS:
    // 1. Filter shows for the given day:
    //    var dayShows = shows.Where(s => DateOnly.FromDateTime(s.Start) == day)
    //
    // 2. Sort by END TIME (earliest ending first) - this is KEY to greedy approach:
    //    .OrderBy(s => s.End).ThenBy(s => s.Start).ToList()
    //
    // 3. Greedy selection loop:
    //    - Initialize: var plan = new List<Show>();
    //    - Track: DateTime currentEndTime = DateTime.MinValue;
    //    - For each show in dayShows:
    //      * If show.Start >= currentEndTime:
    //        + plan.Add(show)
    //        + currentEndTime = show.End
    //      * Else: skip (overlaps with schedule)
    //
    // 4. Return plan
    //
    // WHY SORT BY END TIME?
    // - Finishing early leaves more time for other movies
    // - This gives us the MAXIMUM number of non-overlapping shows
    // - Proven optimal for interval scheduling problem!
    //
    // HINTS:
    // - Use LINQ Where + OrderBy + ThenBy
    // - currentEndTime tracks when we're free to watch next movie
    // - See skeleton-examples/GreedyMarathonPlanner.cs for complete solution
    //==========================================================================
    
    /// <summary>
    /// Plans a marathon ticket using the greedy interval scheduling algorithm.
    /// </summary>
    public List<Show> Plan(DateOnly day, IEnumerable<Show> shows)
    {
        var dayShows = shows
            .Where(s => DateOnly.FromDateTime(s.Start) == day)
            .OrderBy(s => s.End)
            .ThenBy(s => s.Start)
            .ToList();
        
        var plan = new List<Show>();
        DateTime currentEndTime = DateTime.MinValue;
        
        foreach (var show in dayShows)
        {
            if (show.Start >= currentEndTime)
            {
                plan.Add(show);
                currentEndTime = show.End;
            }
        }
        
        return plan;
    }
}

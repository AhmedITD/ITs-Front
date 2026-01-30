using System;
using System.Collections.Generic;
using CinemaApp.Models;

namespace CinemaApp.Services;

/// <summary>
/// Interface for marathon ticket planning algorithms.
/// This is the STRATEGY PATTERN - defines a family of algorithms.
/// 
/// Different strategies could include:
/// - GreedyMarathonPlanner: Maximize number of movies
/// - LongestDurationPlanner: Maximize total watch time
/// - PreferredGenrePlanner: Prioritize certain movie types
/// 
/// The strategy can be swapped at runtime without changing other code.
/// </summary>
public interface IMarathonPlanner
{
    /// <summary>
    /// Plans a marathon ticket for a given day.
    /// Returns an ordered list of non-overlapping shows.
    /// </summary>
    /// <param name="day">The date to plan for</param>
    /// <param name="shows">Available shows to choose from</param>
    /// <returns>Ordered list of shows that don't overlap</returns>
    List<Show> Plan(DateOnly day, IEnumerable<Show> shows);
}

using System;
using System.Collections.Generic;
using System.Linq;

//==============================================================================
// FILE: Show.cs
// ⭐⭐⭐ DIFFICULTY: Medium | ⏱️ TIME: 45-60 minutes
// 🧪 TEST: See skeleton-examples/Show.cs for complete reference
//==============================================================================

namespace CinemaApp.Models;

/// <summary>
/// Represents a specific showing of a movie in a room at a particular time.
/// This class demonstrates:
/// - ENCAPSULATION: Private _taken set, public read-only properties
/// - DATA VALIDATION: TryBook checks seat validity and availability
/// - BUSINESS LOGIC: Manages seat booking without external dependencies
/// </summary>
public sealed class Show
{
    public Guid Id { get; } = Guid.NewGuid();
    public Movie Movie { get; }
    public Room Room { get; }
    public DateTime Start { get; }
    public DateTime End => Start + Movie.Duration;
    public decimal Price { get; }
    
    private readonly HashSet<int> _taken = new();
    public int AvailableSeats => Room.Capacity - _taken.Count;
    
    public Show(Movie movie, Room room, DateTime start, decimal price)
    {
        Movie = movie;
        Room = room;
        Start = start;
        Price = price;
    }
    
    public bool TryBook(params int[] seats)
    {
        if (seats.Length == 0) return false;
        if (seats.Any(s => s < 1 || s > Room.Capacity)) return false;
        if (seats.Any(_taken.Contains)) return false;
        
        foreach (var seat in seats)
            _taken.Add(seat);
        
        return true;
    }
    
    public override string ToString() => 
        $"[{Id:N}] {Movie.Title} | {Room.Name} | {Start:yyyy-MM-dd HH:mm}-{End:HH:mm} | ${Price:F2} | Seats: {AvailableSeats}/{Room.Capacity}";
}

//==============================================================================
// FILE: Show.cs
// LOCATION: src/Models/Show.cs
// PURPOSE: Represents a cinema show with booking functionality
//
// 📚 LEARNING OBJECTIVES:
//   ✅ Understand class vs record (when to use which)
//   ✅ Learn encapsulation (private fields, public properties)
//   ✅ Practice validation in constructors
//   ✅ Work with collections (HashSet for seat tracking)
//   ✅ Implement business logic methods
//
// 📋 PREREQUISITES:
//   ✅ Know difference between class and record
//   ✅ Understand properties and fields
//   ✅ Familiar with HashSet<T> collection
//   ✅ Know how to use Guid for IDs
//
// ⭐ DIFFICULTY: Medium
// ⏱️ ESTIMATED TIME: 45-60 minutes
//
// 🧪 TEST CASES:
//   Room capacity: 100
//   Book seats [1, 2, 3] → Success, 97 remaining
//   Book seat [2] again → Fail (already booked)
//   Book seat [101] → Fail (out of range)
//==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaApp.Models;

/// <summary>
/// Represents a specific showing of a movie in a room at a particular time.
/// Handles seat booking and availability tracking.
/// 
/// Why CLASS instead of RECORD?
/// - Shows are MUTABLE (booking state changes)
/// - We need IDENTITY (same show over time, different state)
/// - Records are for IMMUTABLE data (Movie, Room)
/// </summary>
public sealed class Show
{
    //==========================================================================
    // PART 1: PROPERTIES
    //==========================================================================
    
    /// <summary>
    /// Unique identifier for this show.
    /// TODO: Add property
    /// Hint: public Guid Id { get; }
    /// </summary>
    // TODO: Add Id property here
    
    /// <summary>
    /// The movie being shown.
    /// TODO: Add property
    /// Hint: public Movie Movie { get; }
    /// </summary>
    // TODO: Add Movie property here
    
    /// <summary>
    /// The room where show takes place.
    /// TODO: Add property
    /// Hint: public Room Room { get; }
    /// </summary>
    // TODO: Add Room property here
    
    /// <summary>
    /// When the show starts.
    /// TODO: Add property
    /// </summary>
    // TODO: Add StartTime property (DateTime)
    
    /// <summary>
    /// Ticket price for this show.
    /// TODO: Add property
    /// </summary>
    // TODO: Add Price property (decimal)
    
    /// <summary>
    /// When the show ends (calculated from start + movie duration).
    /// TODO: Add computed property
    /// Hint: public DateTime EndTime => StartTime + Movie.Duration;
    /// </summary>
    // TODO: Add EndTime computed property
    
    //==========================================================================
    // PART 2: SEAT BOOKING STATE (Private Fields)
    //==========================================================================
    
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Private field to track which seats are booked.
    /// 
    /// 💡 WHY HashSet<int>?
    /// - Fast O(1) lookup: "Is seat 42 taken?"
    /// - Automatically prevents duplicates
    /// - Better than List<int> for this use case
    /// 
    /// Example usage:
    ///   private readonly HashSet<int> _bookedSeats = new();
    ///   _bookedSeats.Add(12);      // Book seat 12
    ///   _bookedSeats.Contains(12); // Check if seat 12 is booked
    /// =======================================================
    /// </summary>
    // TODO: Add private HashSet<int> field called _bookedSeats
    
    /// <summary>
    /// How many seats are still available.
    /// TODO: Implement calculated property
    /// Hint: Room.Capacity - _bookedSeats.Count
    /// </summary>
    // TODO: Add public int AvailableSeats { get; }
    
    //==========================================================================
    // PART 3: CONSTRUCTOR
    //==========================================================================
    
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Create a new show with validation.
    /// 
    /// 📝 REQUIREMENTS:
    /// 1. Accept: Movie, Room, StartTime, Price as parameters
    /// 2. Generate new Guid for Id
    /// 3. Validate: movie not null, room not null, price >= 0
    /// 4. Initialize bookedSeats as empty HashSet
    /// 5. Assign all properties
    /// 
    /// 💡 VALIDATION EXAMPLES:
    ///   if (movie is null)
    ///       throw new ArgumentNullException(nameof(movie));
    ///   
    ///   if (price < 0)
    ///       throw new ArgumentException("Price cannot be negative", nameof(price));
    /// 
    /// 🎯 CONSTRUCTOR STRUCTURE:
    ///   public Show(Movie movie, Room room, DateTime startTime, decimal price)
    ///   {
    ///       // Validate parameters
    ///       // Generate Id
    ///       // Assign properties
    ///       // Initialize _bookedSeats
    ///   }
    /// =======================================================
    /// </summary>
    // TODO: Implement constructor here
    
    //==========================================================================
    // PART 4: BOOKING METHODS
    //==========================================================================
    
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Attempts to book specified seats for this show.
    /// 
    /// 📝 REQUIREMENTS:
    /// 1. Validate all seats are in valid range (1 to Room.Capacity)
    /// 2. Check none of the seats are already booked
    /// 3. If all valid, book ALL seats (add to _bookedSeats)
    /// 4. If any validation fails, book NONE (atomic operation)
    /// 5. Return true if successful, false otherwise
    /// 
    /// 🎯 ALGORITHM:
    /// 
    ///   function TryBook(seats):
    ///       // Step 1: Validate all seats BEFORE booking any
    ///       for each seat in seats:
    ///           if seat < 1 OR seat > Room.Capacity:
    ///               return false  // Invalid seat number
    ///           if _bookedSeats.Contains(seat):
    ///               return false  // Already booked
    ///       
    ///       // Step 2: All valid! Now book them
    ///       for each seat in seats:
    ///           _bookedSeats.Add(seat)
    ///       
    ///       return true
    /// 
    /// 💡 IMPLEMENTATION HINTS:
    /// 
    /// Hint 1: Check range
    ///   if (seat < 1 || seat > Room.Capacity)
    ///       return false;
    /// 
    /// Hint 2: Check if already booked
    ///   if (_bookedSeats.Contains(seat))
    ///       return false;
    /// 
    /// Hint 3: Validate ALL before booking ANY
    ///   // First loop: just check
    ///   foreach (var seat in seats) { /* validate only */ }
    ///   // Second loop: book all
    ///   foreach (var seat in seats) { _bookedSeats.Add(seat); }
    /// 
    /// Hint 4: Use LINQ for validation (alternative)
    ///   if (seats.Any(s => s < 1 || s > Room.Capacity))
    ///       return false;
    ///   if (seats.Any(s => _bookedSeats.Contains(s)))
    ///       return false;
    /// 
    /// 🚨 COMMON MISTAKES:
    /// 
    /// ❌ Booking seats before validating ALL
    ///    Problem: If seat 3 invalid, seats 1-2 already booked!
    ///    Solution: Validate first, then book
    /// 
    /// ❌ Not checking seat range
    ///    Problem: Negative seats or seats > capacity
    ///    Solution: Check: seat >= 1 && seat <= Room.Capacity
    /// 
    /// ❌ Returning void instead of bool
    ///    Problem: Caller doesn't know if booking succeeded
    ///    Solution: Return bool success indicator
    /// 
    /// ⚡ COMPLEXITY:
    /// - Time: O(n) where n = number of seats to book
    /// - Space: O(1) excluding the storage in _bookedSeats
    /// =======================================================
    /// </summary>
    /// <param name="seats">Array of seat numbers to book (e.g., [12, 13, 14])</param>
    /// <returns>True if booking successful, false if any seat invalid or taken</returns>
    public bool TryBook(params int[] seats)
    {
        // TODO: Implement booking logic
        throw new NotImplementedException(
            "STUDENT TODO: Implement seat booking with validation. " +
            "Steps: 1) Validate all seats, 2) If valid, book all seats, 3) Return success status."
        );
        
        // Suggested structure:
        
        // Step 1: Validate all seats first (don't book any yet!)
        // foreach (var seat in seats)
        // {
        //     // Check range
        //     // Check if already booked
        //     // If either fails, return false
        // }
        
        // Step 2: All valid! Now book them
        // foreach (var seat in seats)
        // {
        //     /* add to booked set */
        // }
        
        // Step 3: Success!
        // return true;
    }
    
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Checks if a specific seat is available.
    /// 
    /// 📝 SIMPLE METHOD:
    /// Just check if seat is NOT in the booked set.
    /// 
    /// 💡 ONE-LINE IMPLEMENTATION:
    ///   return !_bookedSeats.Contains(seatNumber);
    /// 
    /// 🎯 BONUS: Add validation
    /// You could also check if seat is in valid range:
    ///   if (seatNumber < 1 || seatNumber > Room.Capacity)
    ///       return false;  // Invalid seats are "unavailable"
    ///   return !_bookedSeats.Contains(seatNumber);
    /// =======================================================
    /// </summary>
    /// <param name="seatNumber">Seat to check</param>
    /// <returns>True if seat is available, false if booked or invalid</returns>
    public bool IsSeatAvailable(int seatNumber)
    {
        // TODO: Implement availability check
        throw new NotImplementedException(
            "STUDENT TODO: Check if seat is available. " +
            "Hint: Check if seat is NOT in _bookedSeats set."
        );
    }
    
    //==========================================================================
    // PART 5: DISPLAY METHOD (OPTIONAL BUT HELPFUL)
    //==========================================================================
    
    /// <summary>
    /// Returns a string representation of this show.
    /// Useful for debugging and displaying to users.
    /// 
    /// TODO (Optional): Override ToString()
    /// Example: "Inception at 2025-11-01 10:00 in Room A ($9.99, 87/100 seats available)"
    /// </summary>
    public override string ToString()
    {
        // TODO (Optional): Format show information nicely
        // Hint: Use string interpolation
        // return $"{Movie.Title} at {StartTime:yyyy-MM-dd HH:mm} in {Room.Name} (${Price:F2}, {AvailableSeats}/{Room.Capacity} seats available)";
        
        return base.ToString(); // Default implementation
    }
}

/*
 * 🎓 LEARNING NOTES:
 * 
 * Class vs Record Decision Tree:
 * 
 * Use RECORD when:
 * ✅ Data is immutable (doesn't change after creation)
 * ✅ Value equality makes sense (two with same data are "equal")
 * ✅ Simple data containers
 * Examples: Movie, Room, Configuration, DTO
 * 
 * Use CLASS when:
 * ✅ Data is mutable (changes over time)
 * ✅ Identity matters (same object, different states)
 * ✅ Contains behavior/business logic
 * Examples: Show (booking state changes), services, repositories
 * 
 * Why HashSet for booked seats?
 * 
 * Comparison:
 * - List<int>: O(n) to check if seat exists
 * - HashSet<int>: O(1) to check if seat exists ← BETTER!
 * - Array: Fixed size, awkward to add seats
 * - Dictionary<int, bool>: Overkill, HashSet is simpler
 * 
 * Why TryBook returns bool instead of throwing exception?
 * 
 * Try* Pattern in C#:
 * - Returns bool for success/failure
 * - Doesn't throw exceptions for expected failures
 * - Caller can handle failure gracefully
 * - Examples: int.TryParse(), Dictionary.TryGetValue()
 * 
 * Alternative: ThrowingBook() that throws BookingException
 * - Use when failure is exceptional/unexpected
 * - Forces caller to handle with try-catch
 * - More "aggressive" error handling
 * 
 * Atomic Operations:
 * "All or nothing" - either book ALL seats or book NONE
 * Why? Prevents partial bookings if one seat is invalid
 * Example: Booking seats [10, 11, 999]
 *   ❌ Bad: Books 10, 11, then fails on 999 → inconsistent!
 *   ✅ Good: Validates all first, fails without booking any
 * 
 * Real-World Considerations:
 * - Concurrency: What if two users book same seat simultaneously?
 *   → Need locking or database transactions
 * - Persistence: This is in-memory, need to save to database
 * - Seat maps: Could use 2D array for seat layout
 * - Pricing: Dynamic pricing, discounts, seat categories
 * 
 * Design Patterns Used:
 * - Encapsulation: Private _bookedSeats, public methods
 * - Value Object: Movie and Room (immutable)
 * - Entity: Show (has identity via Guid)
 * - Try* Pattern: TryBook for graceful failure handling
 */

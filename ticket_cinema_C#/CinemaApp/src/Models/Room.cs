namespace CinemaApp.Models;

/// <summary>
/// Represents a cinema room with a name and seating capacity.
/// Uses C# 'record' type for immutable data.
/// In a real cinema, rooms don't change their capacity frequently.
/// </summary>
/// <param name="Name">The room identifier (e.g., "Room A", "IMAX Theater")</param>
/// <param name="Capacity">Maximum number of seats available</param>
public sealed record Room(string Name, int Capacity)
{
    /// <summary>
    /// Provides a readable string representation of the room.
    /// </summary>
    public override string ToString() => $"{Name} (Capacity: {Capacity})";
}

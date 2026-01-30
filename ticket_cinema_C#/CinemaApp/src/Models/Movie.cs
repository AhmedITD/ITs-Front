namespace CinemaApp.Models;

/// <summary>
/// Represents a movie with its title and duration.
/// Uses C# 'record' type - immutable data container with built-in equality comparison.
/// Records are perfect for simple data objects that don't need to change after creation.
/// </summary>
/// <param name="Title">The movie's title (e.g., "Inception")</param>
/// <param name="Duration">How long the movie runs (e.g., 2 hours 28 minutes)</param>
public sealed record Movie(string Title, TimeSpan Duration)
{
    /// <summary>
    /// Provides a readable string representation of the movie.
    /// </summary>
    public override string ToString() => $"{Title} ({Duration.Hours}h {Duration.Minutes}m)";
}

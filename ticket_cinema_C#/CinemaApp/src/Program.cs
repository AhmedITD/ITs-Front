using System;
using CinemaApp.Services;

namespace CinemaApp;

/// <summary>
/// Entry point for the Cinema Application.
/// This console application manages a cinema with rooms, movies, shows, and bookings.
/// Includes a Marathon Ticket feature that automatically schedules non-overlapping movies.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Cinema Application ===");
        Console.WriteLine("Manage rooms, movies, shows, and bookings");
        Console.WriteLine();
        
        // Start the application - all logic is in the App class
        App.Run();
    }
}

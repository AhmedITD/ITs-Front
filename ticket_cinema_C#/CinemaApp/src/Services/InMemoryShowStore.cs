//==============================================================================
// FILE: InMemoryShowStore.cs
// ⭐ DIFFICULTY: Easy | ⏱️ TIME: 20-30 minutes
// 🧪 TEST: Create store, Add shows, All() returns them, Find(id) works
//==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using CinemaApp.Models;

namespace CinemaApp.Services;

/// <summary>
/// In-memory implementation of IShowStore.
/// Stores shows in a List - data is lost when application closes.
/// 
/// For persistence, you could create:
/// - JsonShowStore (saves to JSON file)
/// - CsvShowStore (saves to CSV file)
/// - DatabaseShowStore (saves to database)
/// 
/// This demonstrates the REPOSITORY PATTERN's flexibility.
/// </summary>
public sealed class InMemoryShowStore : IShowStore
{
    //==========================================================================
    // STUDENT TODO: Implement IShowStore using List<Show>
    //
    // STEPS:
    // 1. Create field: private readonly List<Show> _shows = new();
    // 2. Implement All(): return _shows (as IEnumerable)
    // 3. Implement Add(Show show): _shows.Add(show)
    // 4. Implement Find(Guid id): use LINQ FirstOrDefault
    //
    // HINTS:
    // - All() just returns the list (interface hides implementation)
    // - Find uses: _shows.FirstOrDefault(show => show.Id == id)
    // - FirstOrDefault returns null if not found (Show? nullable)
    //==========================================================================
    
    private readonly List<Show> _shows = new();
    
    public IEnumerable<Show> All()
    {
        return _shows;
    }
    
    public void Add(Show show)
    {
        _shows.Add(show);
    }
    
    public Show? Find(Guid id)
    {
        return _shows.FirstOrDefault(s => s.Id == id);
    }
}

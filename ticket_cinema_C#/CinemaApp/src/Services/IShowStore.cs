using System;
using System.Collections.Generic;
using CinemaApp.Models;

namespace CinemaApp.Services;

/// <summary>
/// Interface for show storage - defines what operations a show store must support.
/// This is the REPOSITORY PATTERN - abstracts data storage details.
/// 
/// Benefits:
/// - Easy to swap implementations (in-memory, JSON file, database)
/// - Makes testing easier (can create mock implementations)
/// - Separates data access from business logic
/// </summary>
public interface IShowStore
{
    /// <summary>
    /// Retrieves all shows in the system.
    /// </summary>
    /// <returns>Enumerable collection of all shows</returns>
    IEnumerable<Show> All();
    
    /// <summary>
    /// Adds a new show to the store.
    /// </summary>
    /// <param name="show">The show to add</param>
    void Add(Show show);
    
    /// <summary>
    /// Finds a show by its unique ID.
    /// </summary>
    /// <param name="id">The show's GUID</param>
    /// <returns>The show if found, null otherwise</returns>
    Show? Find(Guid id);
}

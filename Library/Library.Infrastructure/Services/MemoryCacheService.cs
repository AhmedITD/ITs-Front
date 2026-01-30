using Library.Application.Interfaces.Services;

namespace Library.Infrastructure.Services;

/// <summary>
/// Singleton in-memory cache using Dictionary&lt;string, object&gt;. Does not use IMemoryCache.
/// </summary>
public class MemoryCacheService : IMemoryCacheService
{
    private readonly Dictionary<string, object> _cache = new();
    private readonly object _lock = new();

    public T? Get<T>(string key)
    {
        lock (_lock)
        {
            if (!_cache.TryGetValue(key, out var entryObj) || entryObj is not CacheEntry entry)
                return default;

            if (DateTime.UtcNow >= entry.ExpiresAt)
            {
                _cache.Remove(key);
                return default;
            }

            return entry.Value is T typed ? typed : default;
        }
    }

    public void Set<T>(string key, T value, TimeSpan duration)
    {
        lock (_lock)
        {
            _cache[key] = new CacheEntry(value!, DateTime.UtcNow.Add(duration));
        }
    }

    public void Remove(string key)
    {
        lock (_lock)
        {
            _cache.Remove(key);
        }
    }

    private sealed class CacheEntry(object value, DateTime expiresAt)
    {
        public object Value { get; } = value;
        public DateTime ExpiresAt { get; } = expiresAt;
    }
}
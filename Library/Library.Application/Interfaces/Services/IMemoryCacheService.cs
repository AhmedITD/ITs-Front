namespace Library.Application.Interfaces.Services;

public interface IMemoryCacheService
{
    // T is generic: Cache a String, a Book, a List<int>...
    T? Get<T>(string key);
    
    // Set with expiration
    void Set<T>(string key, T value, TimeSpan expiration);
    
    void Remove(string key);
}
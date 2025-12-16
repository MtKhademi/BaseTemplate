namespace CacheModule.Services;

internal class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly ConcurrentDictionary<string, byte> _keys = new();

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        if (_memoryCache.TryGetValue(key, out var value) && value is T tValue)
            return Task.FromResult<T?>(tValue);
        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken ct = default)
    {
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpiration.HasValue)
            options.SetAbsoluteExpiration(absoluteExpiration.Value);

        _memoryCache.Set(key, value, options);
        _keys.TryAdd(key, 0);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        _memoryCache.Remove(key);
        _keys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveAllAsync(CancellationToken ct = default)
    {
        foreach (var key in _keys.Keys)
        {
            _memoryCache.Remove(key);
        }
        _keys.Clear();
        return Task.CompletedTask;
    }
}
using Application.Games.Contracts;
using Application.Games.DTOs;
using Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching;

public sealed class GameSearchMemoryCache(
    IMemoryCache memoryCache,
    IOptions<RawgOptions> rawgOptions) : IGameSearchCache
{
    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(Math.Max(1, rawgOptions.Value.CacheMinutes));

    public bool TryGet(string query, int limit, out IReadOnlyCollection<GameDetailsDto>? value)
    {
        return memoryCache.TryGetValue(GetCacheKey(query, limit), out value);
    }

    public void Set(string query, int limit, IReadOnlyCollection<GameDetailsDto> value)
    {
        memoryCache.Set(GetCacheKey(query, limit), value, _ttl);
    }

    private static string GetCacheKey(string query, int limit) =>
        $"game-search:{query.Trim().ToLowerInvariant()}:{limit}";
}

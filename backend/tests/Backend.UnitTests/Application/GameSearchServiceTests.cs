using Application.Common.Exceptions;
using Application.Games.Contracts;
using Application.Games.DTOs;
using Application.Games.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Backend.UnitTests.Application;

public sealed class GameSearchServiceTests
{
    [Fact]
    public async Task SearchByNameAsync_ShouldThrowValidation_WhenQueryIsEmpty()
    {
        var service = new GameSearchService(new FakeRawgGateway(), new FakeCache(), NullLogger<GameSearchService>.Instance);

        var action = async () => await service.SearchByNameAsync("  ", 10, CancellationToken.None);

        await Assert.ThrowsAsync<ValidationAppException>(action);
    }

    [Fact]
    public async Task SearchByNameAsync_ShouldReturnFromCache_WhenCached()
    {
        var cache = new FakeCache
        {
            CachedItems = [new GameDetailsDto("Cached", "desc", "image", [])]
        };

        var service = new GameSearchService(new FakeRawgGateway(), cache, NullLogger<GameSearchService>.Instance);

        var result = await service.SearchByNameAsync("elden ring", 10, CancellationToken.None);

        Assert.Equal("Cached", result.Single().Name);
    }

    private sealed class FakeCache : IGameSearchCache
    {
        public IReadOnlyCollection<GameDetailsDto>? CachedItems { get; set; }

        public bool TryGet(string query, int limit, out IReadOnlyCollection<GameDetailsDto>? value)
        {
            value = CachedItems;
            return value is not null;
        }

        public void Set(string query, int limit, IReadOnlyCollection<GameDetailsDto> value)
        {
            CachedItems = value;
        }
    }

    private sealed class FakeRawgGateway : IRawgGateway
    {
        public Task<IReadOnlyCollection<RawgGameSummary>> SearchGamesAsync(string name, int limit, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyCollection<RawgGameSummary>>([new RawgGameSummary(1, "Elden Ring")]);

        public Task<RawgGameDetails?> GetGameDetailsAsync(int gameId, CancellationToken cancellationToken)
            => Task.FromResult<RawgGameDetails?>(new RawgGameDetails("Elden Ring", "desc", "img"));

        public Task<IReadOnlyCollection<RawgAchievement>> GetAchievementsAsync(int gameId, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyCollection<RawgAchievement>>([new RawgAchievement("A", "B", "C")]);
    }
}

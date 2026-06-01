using System.Text.RegularExpressions;
using Application.Common.Exceptions;
using Application.Games.Contracts;
using Application.Games.DTOs;
using Microsoft.Extensions.Logging;

namespace Application.Games.Services;

public sealed class GameSearchService(
    IRawgGateway rawgGateway,
    IGameSearchCache cache,
    ILogger<GameSearchService> logger) : IGameSearchService
{
    private static readonly Regex HtmlRegex = new("<.*?>", RegexOptions.Compiled);
    private const int DefaultLimit = 10;
    private const int MaxLimit = 20;
    private const int MaxConcurrentRawgRequests = 3;

    public async Task<IReadOnlyCollection<GameDetailsDto>> SearchByNameAsync(
        string name,
        int limit,
        CancellationToken cancellationToken)
    {
        var query = name?.Trim() ?? string.Empty;
        var safeLimit = NormalizeLimit(limit);

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ValidationAppException("The query parameter 'name' is required.");
        }

        if (cache.TryGet(query, safeLimit, out var cached) && cached is not null)
        {
            logger.LogInformation("Cache hit for game query {Query} with limit {Limit}", query, safeLimit);
            return cached;
        }

        var summaries = await rawgGateway.SearchGamesAsync(query, safeLimit, cancellationToken);
        if (summaries.Count == 0)
        {
            throw new NotFoundException($"No games found for '{query}'.");
        }

        using var throttle = new SemaphoreSlim(MaxConcurrentRawgRequests);
        var gameTasks = summaries.Select(summary => BuildGameDetailsAsync(summary, throttle, cancellationToken));
        var games = (await Task.WhenAll(gameTasks))
            .Where(item => item is not null)
            .Select(item => item!)
            .ToArray();

        if (games.Length == 0)
        {
            throw new ExternalServiceException("RAWG returned an empty details payload.");
        }

        cache.Set(query, safeLimit, games);
        return games;
    }

    private async Task<GameDetailsDto?> BuildGameDetailsAsync(
        RawgGameSummary summary,
        SemaphoreSlim throttle,
        CancellationToken cancellationToken)
    {
        await throttle.WaitAsync(cancellationToken);

        try
        {
            var details = await rawgGateway.GetGameDetailsAsync(summary.Id, cancellationToken);
            if (details is null)
            {
                logger.LogWarning("RAWG returned null details for game id {GameId}", summary.Id);
                return null;
            }

            var achievements = await rawgGateway.GetAchievementsAsync(summary.Id, cancellationToken);

            return new GameDetailsDto(
                details.Name,
                Sanitize(details.Description),
                details.BackgroundImage,
                achievements
                    .Select(a => new AchievementDto(a.Title, Sanitize(a.Description), a.Image))
                    .ToArray());
        }
        finally
        {
            throttle.Release();
        }
    }

    private static int NormalizeLimit(int limit)
    {
        if (limit <= 0)
        {
            return DefaultLimit;
        }

        return Math.Min(limit, MaxLimit);
    }

    private static string Sanitize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var noHtml = HtmlRegex.Replace(value, string.Empty);
        return System.Net.WebUtility.HtmlDecode(noHtml).Trim();
    }
}

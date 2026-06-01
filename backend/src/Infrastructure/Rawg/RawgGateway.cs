using System.Net.Http.Json;
using Application.Common.Exceptions;
using Application.Games.Contracts;
using Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Rawg;

public sealed class RawgGateway(
    HttpClient httpClient,
    IOptions<RawgOptions> options,
    ILogger<RawgGateway> logger) : IRawgGateway
{
    private readonly RawgOptions _options = options.Value;

    public async Task<IReadOnlyCollection<RawgGameSummary>> SearchGamesAsync(
        string name,
        int limit,
        CancellationToken cancellationToken)
    {
        var path = $"games?search={Uri.EscapeDataString(name)}&page_size={limit}&key={_options.ApiKey}";
        var payload = await SendAsync<RawgSearchResponse>(path, cancellationToken);

        return payload.Results
            .Select(x => new RawgGameSummary(x.Id, x.Name))
            .ToArray();
    }

    public async Task<RawgGameDetails?> GetGameDetailsAsync(int gameId, CancellationToken cancellationToken)
    {
        var path = $"games/{gameId}?key={_options.ApiKey}";
        var payload = await SendAsync<RawgGameDetailsResponse>(path, cancellationToken);
        return new RawgGameDetails(payload.Name, payload.Description, payload.BackgroundImage);
    }

    public async Task<IReadOnlyCollection<RawgAchievement>> GetAchievementsAsync(int gameId, CancellationToken cancellationToken)
    {
        var path = $"games/{gameId}/achievements?page_size={int.MaxValue}&key={_options.ApiKey}";
        var payload = await SendAsync<RawgAchievementsResponse>(path, cancellationToken);

        return payload.Results
            .Select(x => new RawgAchievement(x.Name, x.Description, x.Image))
            .ToArray();
    }

    private async Task<T> SendAsync<T>(string path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey) && string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            throw new ExternalServiceException("RAWG API key or Base URL is missing. Configure Rawg:ApiKey and Rawg:BaseUrl.");
        }

        using var response = await httpClient.GetAsync(_options.BaseUrl + path, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning("RAWG error {StatusCode}: {Body}", response.StatusCode, body);
            throw new ExternalServiceException("RAWG API returned an error response.");
        }

        var payload = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        return payload ?? throw new ExternalServiceException("RAWG API returned an invalid payload.");
    }
}

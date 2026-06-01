using System.Text.Json.Serialization;

namespace Infrastructure.Rawg;

internal sealed class RawgSearchResponse
{
    [JsonPropertyName("results")]
    public List<RawgSearchResult> Results { get; init; } = [];
}

internal sealed class RawgSearchResult
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

internal sealed class RawgGameDetailsResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("background_image")]
    public string BackgroundImage { get; init; } = string.Empty;
}

internal sealed class RawgAchievementsResponse
{
    [JsonPropertyName("results")]
    public List<RawgAchievementResult> Results { get; init; } = [];
}

internal sealed class RawgAchievementResult
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; init; } = string.Empty;
}

namespace Infrastructure.Configuration;

public sealed class RawgOptions
{
    public const string SectionName = "Rawg";

    public string BaseUrl { get; init; } = "https://api.rawg.io/api";
    public string ApiKey { get; init; } = string.Empty;
    public int CacheMinutes { get; init; } = 5;
}

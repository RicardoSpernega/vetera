namespace Application.Games.Contracts;

public interface IRawgGateway
{
    Task<IReadOnlyCollection<RawgGameSummary>> SearchGamesAsync(string name, int limit, CancellationToken cancellationToken);
    Task<RawgGameDetails?> GetGameDetailsAsync(int gameId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<RawgAchievement>> GetAchievementsAsync(int gameId, CancellationToken cancellationToken);
}

public sealed record RawgGameSummary(int Id, string Name);
public sealed record RawgGameDetails(string Name, string Description, string BackgroundImage);
public sealed record RawgAchievement(string Title, string Description, string Image);

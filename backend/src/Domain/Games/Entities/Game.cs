namespace Domain.Games.Entities;

public sealed class Game
{
    public Game(string name, string description, string backgroundImage, IReadOnlyCollection<Achievement> achievements)
    {
        Name = string.IsNullOrWhiteSpace(name) ? "Unknown" : name.Trim();
        Description = description?.Trim() ?? string.Empty;
        BackgroundImage = backgroundImage?.Trim() ?? string.Empty;
        Achievements = achievements;
    }

    public string Name { get; }
    public string Description { get; }
    public string BackgroundImage { get; }
    public IReadOnlyCollection<Achievement> Achievements { get; }
}

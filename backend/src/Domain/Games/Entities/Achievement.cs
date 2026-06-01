namespace Domain.Games.Entities;

public sealed class Achievement
{
    public Achievement(string title, string description, string image)
    {
        Title = string.IsNullOrWhiteSpace(title) ? "Untitled" : title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Image = image?.Trim() ?? string.Empty;
    }

    public string Title { get; }
    public string Description { get; }
    public string Image { get; }
}

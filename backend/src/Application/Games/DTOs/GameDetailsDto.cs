namespace Application.Games.DTOs;

public sealed record GameDetailsDto(
    string Name,
    string Description,
    string BackgroundImage,
    IReadOnlyCollection<AchievementDto> Achievements);

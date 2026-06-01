using Application.Games.DTOs;

namespace Application.Games.Contracts;

public interface IGameSearchService
{
    Task<IReadOnlyCollection<GameDetailsDto>> SearchByNameAsync(string name, int limit, CancellationToken cancellationToken);
}

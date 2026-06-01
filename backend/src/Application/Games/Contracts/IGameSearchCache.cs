using Application.Games.DTOs;

namespace Application.Games.Contracts;

public interface IGameSearchCache
{
    bool TryGet(string query, int limit, out IReadOnlyCollection<GameDetailsDto>? value);
    void Set(string query, int limit, IReadOnlyCollection<GameDetailsDto> value);
}

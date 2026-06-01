using Api.Contracts;
using Application.Games.Contracts;
using Application.Games.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/games")]
public sealed class GamesController(IGameSearchService service) : ControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<GameDetailsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<GameDetailsDto>>>> SearchAsync(
        [FromQuery] string name,
        CancellationToken cancellationToken,
        [FromQuery] int limit = 10)
    {
        var result = await service.SearchByNameAsync(name, limit, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<GameDetailsDto>>.Ok(result));
    }
}

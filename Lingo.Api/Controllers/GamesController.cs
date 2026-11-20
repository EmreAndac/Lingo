
using Lingo.Application.DTOs.Games;
using Lingo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lingo.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly IGameSessionService _gameSessionService;

    public GamesController(IGameSessionService gameSessionService)
    {
        _gameSessionService = gameSessionService;
    }

    [HttpPost("today")]
    public async Task<ActionResult<GameSessionDto>> GetOrCreateToday(Guid playerId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _gameSessionService.GetOrCreateTodayAsync(playerId, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Oyuncu bulunamadı." });
        }
    }
}

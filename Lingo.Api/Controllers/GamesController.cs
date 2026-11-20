
using Lingo.Application.DTOs.Games;
using Lingo.Application.Interfaces;
using Lingo.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lingo.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{

    private readonly IGameSessionService _gameSessionService;
    private readonly IGameRoundService _gameRoundService;

    public GamesController(IGameSessionService gameSessionService, IGameRoundService gameRoundService)
    {
        _gameSessionService = gameSessionService;
        _gameRoundService = gameRoundService;
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


    [HttpPost("today/rounds/start")]
    public async Task<IActionResult> StartRound(Guid playerId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _gameRoundService.StartRoundAsync(playerId, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

}


using Lingo.Application.DTOs.Games;

namespace Lingo.Application.Interfaces;

public interface IGameSessionService
{
    Task<GameSessionDto> GetOrCreateTodayAsync(Guid playerId, CancellationToken cancellationToken = default);
}

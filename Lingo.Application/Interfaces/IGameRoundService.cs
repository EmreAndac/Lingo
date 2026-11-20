
using Lingo.Application.DTOs.Rounds;

namespace Lingo.Application.Interfaces;

public interface IGameRoundService
{
    Task<StartRoundDto> StartRoundAsync(Guid playerId, CancellationToken cancellationToken = default);
}

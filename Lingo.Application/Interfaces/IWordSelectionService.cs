
using Lingo.Domain.Entities;

namespace Lingo.Application.Interfaces;

public interface IWordSelectionService
{
    Task<Word> SelectWordAsync(Guid playerId, int wordLength, CancellationToken cancellationToken = default);
}

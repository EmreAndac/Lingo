
using Lingo.Application.Interfaces;
using Lingo.Domain.Entities;
using Lingo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Infrastructure.Services;

public class WordSelectionService : IWordSelectionService
{
    private readonly LingoDbContext _context;

    public WordSelectionService(LingoDbContext context)
    {
        _context = context;
    }

    public async Task<Word> SelectWordAsync(Guid playerId, int wordLength, CancellationToken cancellationToken = default)
    {
        var usedWordIds = _context.GameRounds
            .Where(x => x.Session.PlayerId == playerId)
            .Select(x => x.WordId);

        var candidates = await _context.Words
            .AsNoTracking()
            .Where(x => x.Length == wordLength
                && x.IsActive
                && x.IsPlayable
                && !usedWordIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (candidates.Count == 0)
            throw new InvalidOperationException("Bu seviyede kullanılabilecek kelime bulunamadı.");

        var weightedCandidates = candidates
            .Select(x => new
            {
                Word = x,
                Weight = Math.Max(1, x.FrequencyScore)
            })
            .ToList();

        int totalWeight = weightedCandidates.Sum(x => x.Weight);
        int randomValue = Random.Shared.Next(totalWeight);

        foreach (var candidate in weightedCandidates)
        {
            randomValue -= candidate.Weight;

            if (randomValue < 0)
                return candidate.Word;
        }

        return weightedCandidates[^1].Word;
    }
}

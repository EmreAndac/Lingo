
using System.Data;
using Lingo.Application.DTOs.Rounds;
using Lingo.Application.Interfaces;
using Lingo.Domain.Constants;
using Lingo.Domain.Entities;
using Lingo.Domain.Enums;
using Lingo.Domain.Services;
using Lingo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Infrastructure.Services;

public class GameRoundService : IGameRoundService
{
    private readonly LingoDbContext _context;
    private readonly IWordSelectionService _wordSelectionService;

    public GameRoundService(LingoDbContext context, IWordSelectionService wordSelectionService)
    {
        _context = context;
        _wordSelectionService = wordSelectionService;
    }

    public async Task<StartRoundDto> StartRoundAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
        var turkeyNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);
        var today = DateOnly.FromDateTime(turkeyNow);

        var session = await _context.GameSessions
            .Include(x => x.Rounds)
            .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.GameDate == today, cancellationToken);

        if (session == null)
            throw new KeyNotFoundException("Bugüne ait oyun bulunamadı.");

        if (session.Status == GameSessionStatus.Completed)
            throw new InvalidOperationException("Günlük oyun tamamlanmış.");

        if (session.Rounds.Any(x => x.Status == RoundStatus.Active || x.Status == RoundStatus.Waiting))
            throw new InvalidOperationException("Zaten devam eden bir kelime turu var.");

        int completedRounds = session.Rounds.Count(x =>
            x.Status == RoundStatus.Solved || x.Status == RoundStatus.Failed);

        if (GameProgressCalculator.IsGameCompleted(completedRounds))
            throw new InvalidOperationException("Bugünkü bütün kelimeler tamamlandı.");

        int wordLength = GameProgressCalculator.GetCurrentWordLength(completedRounds);
        var word = await _wordSelectionService.SelectWordAsync(playerId, wordLength, cancellationToken);

        var now = DateTime.UtcNow;

        var round = new GameRound
        {
            SessionId = session.Id,
            WordId = word.Id,
            WordLength = wordLength,
            AttemptCount = 0,
            ScoreEarned = 0,
            Status = RoundStatus.Active,
            StartedAtUtc = now,
            DeadlineUtc = now.AddSeconds(GameRules.GuessTimeSeconds)
        };

        _context.GameRounds.Add(round);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new StartRoundDto
        {
            RoundId = round.Id,
            SessionId = session.Id,
            WordLength = round.WordLength,
            MaxAttempts = GameRules.MaxAttempts,
            RemainingAttempts = GameRules.MaxAttempts,
            StartedAtUtc = round.StartedAtUtc!.Value,
            DeadlineUtc = round.DeadlineUtc!.Value
        };
    }
}

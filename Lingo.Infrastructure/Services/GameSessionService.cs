
using Lingo.Application.DTOs.Games;
using Lingo.Application.Interfaces;
using Lingo.Domain.Constants;
using Lingo.Domain.Entities;
using Lingo.Domain.Enums;
using Lingo.Domain.Services;
using Lingo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Infrastructure.Services;

public class GameSessionService : IGameSessionService
{
    private readonly LingoDbContext _context;

    public GameSessionService(LingoDbContext context)
    {
        _context = context;
    }

    public async Task<GameSessionDto> GetOrCreateTodayAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var playerExists = await _context.Players.AnyAsync(x => x.Id == playerId, cancellationToken);

        if (!playerExists)
            throw new KeyNotFoundException("Oyuncu bulunamadı.");

        var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
        var turkeyNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);
        var today = DateOnly.FromDateTime(turkeyNow);

        var session = await _context.GameSessions
            .Include(x => x.Rounds)
            .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.GameDate == today, cancellationToken);

        if (session == null)
        {
            session = new GameSession
            {
                PlayerId = playerId,
                GameDate = today,
                TotalScore = 0,
                PassesUsed = 0,
                Status = GameSessionStatus.InProgress
            };

            _context.GameSessions.Add(session);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // Aynı anda gelen iki istekten biri oturumu oluşturmuş olabilir.
                _context.Entry(session).State = EntityState.Detached;

                session = await _context.GameSessions
                    .Include(x => x.Rounds)
                    .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.GameDate == today, cancellationToken);

                if (session == null)
                    throw;
            }
        }

        int completedRounds = session.Rounds.Count(x =>
            x.Status == RoundStatus.Solved || x.Status == RoundStatus.Failed);

        bool isCompleted = GameProgressCalculator.IsGameCompleted(completedRounds);

        return new GameSessionDto
        {
            Id = session.Id,
            GameDate = session.GameDate,
            TotalScore = session.TotalScore,
            PassesRemaining = GameRules.MaxPasses - session.PassesUsed,
            CurrentWordLength = GameProgressCalculator.GetCurrentWordLength(completedRounds),
            CompletedWordsInLevel = GameProgressCalculator.GetCompletedWordsInLevel(completedRounds),
            TotalCompletedWords = completedRounds,
            IsCompleted = isCompleted
        };
    }
}

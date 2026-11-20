
using Lingo.Domain.Enums;

namespace Lingo.Domain.Entities;

public class GameSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlayerId { get; set; }
    public DateOnly GameDate { get; set; }
    public int TotalScore { get; set; }
    public int PassesUsed { get; set; }
    public GameSessionStatus Status { get; set; } = GameSessionStatus.InProgress;
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAtUtc { get; set; }

    public Player Player { get; set; } = null!;
    public ICollection<GameRound> Rounds { get; set; } = new List<GameRound>();
}

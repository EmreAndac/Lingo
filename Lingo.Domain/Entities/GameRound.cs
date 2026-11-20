
using Lingo.Domain.Enums;

namespace Lingo.Domain.Entities;

public class GameRound
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }
    public int WordId { get; set; }
    public int WordLength { get; set; }
    public int AttemptCount { get; set; }
    public int ScoreEarned { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Waiting;
    public RoundEndReason? EndReason { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? DeadlineUtc { get; set; }
    public DateTime? EndedAtUtc { get; set; }

    public GameSession Session { get; set; } = null!;
    public Word Word { get; set; } = null!;
    public ICollection<RoundGuess> Guesses { get; set; } = new List<RoundGuess>();
}

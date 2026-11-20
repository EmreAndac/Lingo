
namespace Lingo.Domain.Entities;

public class RoundGuess
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RoundId { get; set; }
    public string GuessText { get; set; } = string.Empty;
    public int AttemptNumber { get; set; }
    public bool IsValidWord { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;

    public GameRound Round { get; set; } = null!;
}

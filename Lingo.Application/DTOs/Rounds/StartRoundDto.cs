
namespace Lingo.Application.DTOs.Rounds;

public class StartRoundDto
{
    public Guid RoundId { get; set; }
    public Guid SessionId { get; set; }
    public int WordLength { get; set; }
    public int MaxAttempts { get; set; }
    public int RemainingAttempts { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime DeadlineUtc { get; set; }
}


namespace Lingo.Application.DTOs.Games;

public class GameSessionDto
{
    public Guid Id { get; set; }
    public DateOnly GameDate { get; set; }
    public int TotalScore { get; set; }
    public int PassesRemaining { get; set; }
    public int CurrentWordLength { get; set; }
    public int CompletedWordsInLevel { get; set; }
    public int TotalCompletedWords { get; set; }
    public bool IsCompleted { get; set; }
}

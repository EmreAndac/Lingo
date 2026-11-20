
namespace Lingo.Domain.Entities;

public class Word
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Length { get; set; }
    public int Difficulty { get; set; }
    public int FrequencyScore { get; set; }
    public bool IsValidGuess { get; set; } = true;
    public bool IsPlayable { get; set; } = true;
    public bool IsActive { get; set; } = true;
}

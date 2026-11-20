
namespace Lingo.Domain.Constants;

public static class GameRules
{
    public const int MinWordLength = 3;
    public const int MaxWordLength = 7;
    public const int WordsPerLevel = 5;
    public const int MaxAttempts = 5;
    public const int GuessTimeSeconds = 20;
    public const int MaxPasses = 3;
    public const int ScorePerLetter = 10;
    public const int TotalWords = (MaxWordLength - MinWordLength + 1) * WordsPerLevel;
}

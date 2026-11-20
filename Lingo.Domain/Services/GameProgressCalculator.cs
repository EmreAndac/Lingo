
using Lingo.Domain.Constants;

namespace Lingo.Domain.Services;

public static class GameProgressCalculator
{
    public static int GetCurrentWordLength(int completedRounds)
    {
        if (completedRounds < 0 || completedRounds > GameRules.TotalWords)
            throw new ArgumentOutOfRangeException(nameof(completedRounds));

        if (completedRounds == GameRules.TotalWords)
            return GameRules.MaxWordLength;

        return GameRules.MinWordLength + completedRounds / GameRules.WordsPerLevel;
    }

    public static bool IsGameCompleted(int completedRounds)
    {
        return completedRounds >= GameRules.TotalWords;
    }

    public static int GetCompletedWordsInLevel(int completedRounds)
    {
        if (completedRounds < 0 || completedRounds > GameRules.TotalWords)
            throw new ArgumentOutOfRangeException(nameof(completedRounds));

        return completedRounds == GameRules.TotalWords
            ? GameRules.WordsPerLevel
            : completedRounds % GameRules.WordsPerLevel;
    }
}

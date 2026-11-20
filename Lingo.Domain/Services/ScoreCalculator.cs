
using Lingo.Domain.Constants;

namespace Lingo.Domain.Services;

public static class ScoreCalculator
{
    public static int Calculate(int wordLength, bool isCorrect)
    {
        if (wordLength < GameRules.MinWordLength || wordLength > GameRules.MaxWordLength)
            throw new ArgumentOutOfRangeException(nameof(wordLength));

        return isCorrect ? wordLength * GameRules.ScorePerLetter : 0;
    }
}

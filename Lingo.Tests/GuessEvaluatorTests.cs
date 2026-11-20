
using Lingo.Domain.Enums;
using Lingo.Domain.Services;

namespace Lingo.Tests;

public class GuessEvaluatorTests
{
    [Fact]
    public void Evaluate_ShouldHandleDuplicateLetters()
    {
        var result = GuessEvaluator.Evaluate("SEVGİ", "ERGEN");

        Assert.Equal(new[]
        {
            LetterStatus.Present,
            LetterStatus.Absent,
            LetterStatus.Present,
            LetterStatus.Absent,
            LetterStatus.Absent
        }, result);
    }

    [Fact]
    public void Evaluate_ShouldDetectCorrectPositions()
    {
        var result = GuessEvaluator.Evaluate("SEVGİ", "SAYGI");

        Assert.Equal(LetterStatus.Correct, result[0]);
        Assert.Equal(LetterStatus.Correct, result[3]);
    }

    [Fact]
    public void Evaluate_ShouldMarkAllLettersCorrect()
    {
        var result = GuessEvaluator.Evaluate("SEVGİ", "SEVGİ");

        Assert.All(result, status => Assert.Equal(LetterStatus.Correct, status));
    }


    [Fact]
    public void Evaluate_ShouldPrioritizeCorrectPositions()
    {
        var result = GuessEvaluator.Evaluate("KALEM", "ELALE");

        Assert.Equal(new[]
        {
        LetterStatus.Present,
        LetterStatus.Present,
        LetterStatus.Present,
        LetterStatus.Absent,
        LetterStatus.Absent
        }, result);
    }

    [Fact]
    public void Evaluate_ShouldReserveLettersForCorrectPositions()
    {
        var result = GuessEvaluator.Evaluate("KALEM", "KALAL");

        Assert.Equal(new[]
        {
        LetterStatus.Correct,
        LetterStatus.Correct,
        LetterStatus.Correct,
        LetterStatus.Absent,
        LetterStatus.Absent
    }, result);
    }

}

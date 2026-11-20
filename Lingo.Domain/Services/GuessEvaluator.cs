
using System.Globalization;
using Lingo.Domain.Enums;

namespace Lingo.Domain.Services;

public static class GuessEvaluator
{
    private static readonly CultureInfo TurkishCulture = new("tr-TR");

    public static LetterStatus[] Evaluate(string target, string guess)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(guess))
            throw new ArgumentException("Kelime boş olamaz.");

        target = target.ToUpper(TurkishCulture);
        guess = guess.ToUpper(TurkishCulture);

        if (target.Length != guess.Length)
            throw new ArgumentException("Kelime uzunlukları eşit olmalıdır.");

        var result = new LetterStatus[target.Length];
        var remaining = new Dictionary<char, int>();

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == guess[i])
            {
                result[i] = LetterStatus.Correct;
            }
            else
            {
                remaining[target[i]] = remaining.GetValueOrDefault(target[i]) + 1;
            }
        }

        for (int i = 0; i < guess.Length; i++)
        {
            if (result[i] == LetterStatus.Correct)
                continue;

            char letter = guess[i];

            if (remaining.GetValueOrDefault(letter) > 0)
            {
                result[i] = LetterStatus.Present;
                remaining[letter]--;
            }
            else
            {
                result[i] = LetterStatus.Absent;
            }
        }

        return result;
    }
}

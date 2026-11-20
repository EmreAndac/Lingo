
using System.Globalization;
using System.Text;
using Lingo.Infrastructure.Persistence;
using Lingo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Infrastructure.Services;

public class WordImportService
{
    private readonly LingoDbContext _context;
    private static readonly CultureInfo TurkishCulture = new("tr-TR");
    private const string TurkishLetters = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public WordImportService(LingoDbContext context)
    {
        _context = context;
    }

    public async Task<int> ImportAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Kelime dosyası bulunamadı.", filePath);

        var existingWords = await _context.Words
            .AsNoTracking()
            .Select(x => x.Text)
            .ToListAsync(cancellationToken);

        var existingSet = new HashSet<string>(existingWords, StringComparer.Ordinal);

        var wordsToImport = new List<Word>();

        foreach (var line in File.ReadLines(filePath, Encoding.UTF8))
        {
            var original = line.Trim();
            var text = original.ToUpper(TurkishCulture);

            if (text.Length < 3 || text.Length > 7)
                continue;

            if (text.Any(c => !TurkishLetters.Contains(c)))
                continue;

            if (!existingSet.Add(text))
                continue;

            wordsToImport.Add(new Word
            {
                Text = text,
                Length = text.Length,
                Difficulty = 1,
                FrequencyScore = 1,
                IsValidGuess = true,
                IsPlayable = false,
                IsActive = true
            });
        }

        const int batchSize = 500;

        for (int i = 0; i < wordsToImport.Count; i += batchSize)
        {
            var batch = wordsToImport.Skip(i).Take(batchSize).ToList();

            _context.Words.AddRange(batch);
            await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
        }

        return wordsToImport.Count;
    }
}

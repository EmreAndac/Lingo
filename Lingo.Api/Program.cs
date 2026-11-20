
using Lingo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Lingo.Application.Interfaces;
using Lingo.Infrastructure.Services;
using Lingo.Application.DTOs.Rounds;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<LingoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<IWordSelectionService, WordSelectionService>();
builder.Services.AddScoped<IGameRoundService, GameRoundService>();
builder.Services.AddScoped<WordImportService>();

var app = builder.Build();


if (app.Environment.IsDevelopment() &&
    args.Contains("--import-words"))
{
    using var scope = app.Services.CreateScope();

    var importer = scope.ServiceProvider.GetRequiredService<WordImportService>();


    var filePath = Path.GetFullPath(
        Path.Combine(app.Environment.ContentRootPath, "..", "TDK_Sözlük_Kelime_Listesi.txt"));


    int importedCount = await importer.ImportAsync(filePath);

    Console.WriteLine($"İçe aktarılan kelime sayısı: {importedCount}");

    return;
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

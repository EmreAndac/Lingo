
using Lingo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingo.Infrastructure.Persistence;

public class LingoDbContext : DbContext
{
    public LingoDbContext(DbContextOptions<LingoDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Word> Words => Set<Word>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();
    public DbSet<GameRound> GameRounds => Set<GameRound>();
    public DbSet<RoundGuess> RoundGuesses => Set<RoundGuess>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => x.UserName).IsUnique();
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Text).HasMaxLength(30).IsRequired();
            entity.HasIndex(x => x.Text).IsUnique();
        });

        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.PlayerId, x.GameDate }).IsUnique();

            entity.HasOne(x => x.Player)
                  .WithMany(x => x.GameSessions)
                  .HasForeignKey(x => x.PlayerId);
        });

        modelBuilder.Entity<GameRound>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Session)
                  .WithMany(x => x.Rounds)
                  .HasForeignKey(x => x.SessionId);

            entity.HasOne(x => x.Word)
                  .WithMany()
                  .HasForeignKey(x => x.WordId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RoundGuess>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.GuessText).HasMaxLength(30).IsRequired();

            entity.HasOne(x => x.Round)
                  .WithMany(x => x.Guesses)
                  .HasForeignKey(x => x.RoundId);
        });
    }
}

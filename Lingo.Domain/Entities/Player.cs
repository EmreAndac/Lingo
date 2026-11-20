
namespace Lingo.Domain.Entities;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
}

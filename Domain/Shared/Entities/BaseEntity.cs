namespace Domain.Shared.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime DateTimeCreatedUtc { get; protected set; } = DateTime.UtcNow;
}
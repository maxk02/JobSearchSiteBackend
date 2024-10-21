namespace Domain.Shared.Entities;

public abstract class BaseEntity
{
    protected BaseEntity() { }

    public int Id { get; protected set; }
    public DateTime DateTimeCreatedUtc { get; protected set; } = DateTime.Now;
}
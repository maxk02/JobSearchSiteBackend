namespace Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime DateTimeCreatedUtc { get; set; }
}
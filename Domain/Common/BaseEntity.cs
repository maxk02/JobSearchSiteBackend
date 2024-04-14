namespace Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTime DateTimeCreatedUtc { get; set; }
}
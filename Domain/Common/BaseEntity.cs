namespace Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTimeOffset DateTimeCreated { get; set; }
}
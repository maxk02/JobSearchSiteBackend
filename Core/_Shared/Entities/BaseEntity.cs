namespace Core._Shared.Entities;

public abstract class BaseEntity
{
    protected BaseEntity() {}

    protected BaseEntity(long id, Guid guidIdentifier)
    {
        Id = id;
        GuidIdentifier = guidIdentifier;
    }

    public long Id { get; protected set; }
    public Guid GuidIdentifier { get; protected set; } = Guid.NewGuid();
}
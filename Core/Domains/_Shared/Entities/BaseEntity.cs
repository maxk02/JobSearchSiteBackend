namespace Core.Domains._Shared.Entities;

public abstract class BaseEntity
{
    protected BaseEntity() {}

    protected BaseEntity(long id)
    {
        Id = id;
    }

    public long Id { get; protected set; }
}
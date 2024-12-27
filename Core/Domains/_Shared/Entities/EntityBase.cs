namespace Core.Domains._Shared.Entities;

public abstract class EntityBase
{
    protected EntityBase() {}

    protected EntityBase(long id)
    {
        Id = id;
    }

    public long Id { get; protected set; }
}
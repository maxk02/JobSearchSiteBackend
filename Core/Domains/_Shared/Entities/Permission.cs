namespace Core.Domains._Shared.Entities;

public class Permission : BaseEntity
{
    protected Permission(long id, Guid guidIdentifier, string name) : base(id, guidIdentifier)
    {
        Name = name;
    }
    
    public string Name { get; protected set; }
}
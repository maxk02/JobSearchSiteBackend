namespace Core.Domains._Shared.Entities;

public class Permission : BaseEntity
{
    protected Permission(long id, Guid guidIdentifier, string name, string description) : base(id, guidIdentifier)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; protected set; }
    public string Description { get; protected set; }
}
namespace Domain.Shared.Entities;

public interface IHideableEntity
{
    public bool IsHidden { get; protected set; }
}
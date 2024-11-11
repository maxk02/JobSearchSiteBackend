using Shared.Results;

namespace Domain.Shared.Entities;

public interface IHideableEntity
{
    public bool IsHidden { get; }
    public Result MakeHidden();
    public Result MakeVisible();
}
using Shared.Result;

namespace Domain._Shared.Entities.Interfaces;

public interface IPublicOrPrivateEntity
{
    public bool IsPublic { get; }
    public Result MakePublic();
    public Result MakePrivate();
}
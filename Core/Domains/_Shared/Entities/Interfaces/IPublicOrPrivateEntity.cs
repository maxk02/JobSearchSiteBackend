using Shared.Result;

namespace Core.Domains._Shared.Entities.Interfaces;

public interface IPublicOrPrivateEntity
{
    public bool IsPublic { get; }
    public Result MakePublic();
    public Result MakePrivate();
}
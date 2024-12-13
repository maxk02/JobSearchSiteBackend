using Shared.Result;

namespace API.Domains._Shared.EntityInterfaces;

public interface IPublicOrPrivateEntity
{
    public bool IsPublic { get; }
    public Result MakePublic();
    public Result MakePrivate();
}
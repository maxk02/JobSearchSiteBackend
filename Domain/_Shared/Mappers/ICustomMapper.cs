using Shared.Result;

namespace Domain._Shared.Mappers;

public interface ICustomMapper<TSource, TDestination>
{
    public Result<TDestination> Map(TSource source);
}
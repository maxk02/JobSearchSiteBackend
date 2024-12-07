namespace Shared.Result;

public interface IResult<T> : IResult
{
    T? Value { get; }
    Type ValueType { get; }
}
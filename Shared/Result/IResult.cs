namespace Shared.Result
{
    public interface IResult<T>
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        IEnumerable<ValidationError> ValidationErrors { get; }
        Type ValueType { get; }
        T? Value { get; }
        string Location { get; }
    }
}

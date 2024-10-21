using Domain.Shared.Errors;

namespace Domain.Shared.Results;

public class Result
{
    protected internal Result(bool isSuccess, ICollection<DomainLayerError> errors)
    {
        if (isSuccess && errors.Count > 0)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && errors.Count == 0)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ICollection<DomainLayerError> Errors { get; }

    public static Result Success() => new(true, []);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, []);
    

    public static Result Failure(List<DomainLayerError> errors) => new(false, errors);
    public static Result<TValue> Failure<TValue>(List<DomainLayerError> errors) => new(default, false, errors);
}

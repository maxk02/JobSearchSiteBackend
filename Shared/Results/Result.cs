using FluentValidation.Results;

namespace Shared.Results;

public class Result
{
    protected internal Result(bool isSuccess, ICollection<ValidationFailure> errors)
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

    public ICollection<ValidationFailure> Errors { get; }

    public static Result Success() => new(true, []);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, []);
    

    public static Result Failure(ICollection<ValidationFailure> errors) => new(false, errors);
    public static Result<TValue> Failure<TValue>(ICollection<ValidationFailure> errors) => new(default, false, errors);
}

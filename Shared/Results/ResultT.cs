using FluentValidation.Results;

namespace Shared.Results;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, ICollection<ValidationFailure> errors)
        : base(isSuccess, errors)
    {
        if (isSuccess && value == null)
        {
            throw new InvalidOperationException("The value of a success result can not be null.");
        }
        
        _value = value;
    }
        

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
}
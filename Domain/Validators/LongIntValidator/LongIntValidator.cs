using Domain.Errors.Validation;
using Domain.Shared.Errors;

namespace Domain.Validators.LongIntValidator;

public class LongIntValidator : ValidatorBase<long?>
{
    public LongIntValidator(long? value, string className, string propertyName) : base(value, className, propertyName)
    {
    }
    
    public LongIntValidator MustBeBetween(long min, long max)
    {
        if (Value is null)
            return this;

        if (Value < min || Value > max)
        {
            _errors.Add(new ValidationErrors.LongIntegers.LongIntOutOfRangeError(_className, _propertyName,
                Value.Value, min, max));
        }
        
        return this;
    }
    
    public LongIntValidator MustBeEqualOrMoreThan(long min)
    {
        if (Value is null)
            return this;

        if (Value < min)
        {
            _errors.Add(new ValidationErrors.LongIntegers.LongIntTooSmallNumberError(_className, _propertyName,
                Value.Value, min));
        }
        
        return this;
    }
    
    public LongIntValidator MustBeEqualOrLessThan(long max)
    {
        if (Value is null)
            return this;

        if (Value > max)
        {
            _errors.Add(new ValidationErrors.LongIntegers.LongIntTooBigNumberError(_className, _propertyName,
                Value.Value, max));
        }
        
        return this;
    }
    
    public LongIntValidator MustBeOneOfValuesProvided(IEnumerable<long> values)
    {
        if (Value is null)
            return this;

        var allowedValues = values.ToArray();
        if (allowedValues.Contains(Value.Value))
        {
            _errors.Add(new ValidationErrors.LongIntegers.LongIntOutOfAllowedListError(_className, _propertyName,
                Value.Value, allowedValues));
        }
        
        return this;
    }
    
    public override LongIntValidator ForbidNull()
    {
        if (Value is null)
        {
            _errors.Add(new ValidationErrors.General.NullValueError(_className, _propertyName));
        }
        
        return this;
    }

    public override IEnumerable<DomainLayerValidationError> ReturnErrors()
    {
        return _errors;
    }

    public override IDictionary<string, string> ReturnSpecification()
    {
        throw new NotImplementedException();
    }
}

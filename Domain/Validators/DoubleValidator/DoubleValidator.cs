using Domain.Errors.Validation;
using Domain.Shared.Errors;

namespace Domain.Validators.DoubleValidator;

public class DoubleValidator : ValidatorBase<double?>
{
    public DoubleValidator(double? value, string className, string propertyName) : base(value, className, propertyName)
    {
    }
    
    public DoubleValidator MustBeBetween(double min, double max)
    {
        if (Value is null)
            return this;

        if (Value < min || Value > max)
        {
            _errors.Add(new ValidationErrors.Doubles.DoubleOutOfRangeError(_className, _propertyName,
                Value.Value, min, max));
        }
        
        return this;
    }
    
    public DoubleValidator MustBeEqualOrMoreThan(double min)
    {
        if (Value is null)
            return this;

        if (Value < min)
        {
            _errors.Add(new ValidationErrors.Doubles.DoubleTooSmallNumberError(_className, _propertyName,
                Value.Value, min));
        }
        
        return this;
    }
    
    public DoubleValidator MustBeEqualOrLessThan(double max)
    {
        if (Value is null)
            return this;

        if (Value > max)
        {
            _errors.Add(new ValidationErrors.Doubles.DoubleTooBigNumberError(_className, _propertyName,
                Value.Value, max));
        }
        
        return this;
    }
    
    public override DoubleValidator ForbidNull()
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

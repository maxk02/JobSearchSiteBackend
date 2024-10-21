using Domain.Errors.Validation;
using Domain.Shared.Errors;

namespace Domain.Validators.DecimalValidator;

public class DecimalValidator : ValidatorBase<decimal?>
{
    public DecimalValidator(decimal? value, string className, string propertyName) : base(value, className, propertyName)
    { }
    
    public DecimalValidator ForcePrecisionBetween(int min, int max)
    {
        if (Value is null)
            return this;

        var convertedToString = Value.ToString();
        if (convertedToString is null) throw new FormatException(nameof(DecimalValidator) +
                                                                 "." + nameof(ForcePrecisionBetween) + 
                                                                 ": decimal-to-string conversion failed." );
        
        var dotPosition = convertedToString.IndexOf('.');
        var fractionPartInString = convertedToString[dotPosition..];
        var fractionPartLength = fractionPartInString.Length;

        if (fractionPartLength < min || fractionPartLength > max)
        {
            _errors.Add(new ValidationErrors.Decimals.DecimalPrecisionError(_className, _propertyName,
                fractionPartLength, min, max));
        }
        
        return this;
    }
    
    public DecimalValidator MustBeBetween(decimal min, decimal max)
    {
        if (Value is null)
            return this;

        if (Value < min || Value > max)
        {
            _errors.Add(new ValidationErrors.Decimals.DecimalValueError(_className, _propertyName,
                Value.Value, min, max));
        }
        
        return this;
    }
    
    public override DecimalValidator ForbidNull()
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

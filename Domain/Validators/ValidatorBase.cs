using Domain.Errors.Validation;
using Domain.Shared.Errors;

namespace Domain.Validators;

public abstract class ValidatorBase<T> : ISealedValidator
{
    protected readonly string _className;
    protected readonly string _propertyName;
    
    protected readonly IList<DomainLayerValidationError> _errors = new List<DomainLayerValidationError>();
    
    public ValidatorBase(T? value, string className, string propertyName)
    {
        Value = value;
        _className = className;
        _propertyName = propertyName;
    }
    
    public T? Value { get; protected set; }

    public abstract ValidatorBase<T> ForbidNull();

    
    public ISealedValidator ReturnSealed() => this;
    

    public abstract IEnumerable<DomainLayerValidationError> ReturnErrors();
    public abstract IDictionary<string, string> ReturnSpecification();
}
using Domain.Shared.Errors;

namespace Domain.Validators;

public interface ISealedValidator
{
    public IEnumerable<DomainLayerValidationError> ReturnErrors();
    public IDictionary<string, string> ReturnSpecification();
}
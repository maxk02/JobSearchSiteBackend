namespace Domain.Shared.Errors;

public abstract class DomainLayerValidationError(string className, string propertyName, string message) 
    : DomainLayerError(className, propertyName, message);
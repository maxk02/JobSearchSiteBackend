namespace Domain.Shared.Errors;

public abstract class BusinessRuleError(string className, string propertyName, string message) 
    : DomainLayerError(className, propertyName, message);
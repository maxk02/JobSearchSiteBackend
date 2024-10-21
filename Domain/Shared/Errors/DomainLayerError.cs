using Shared.Errors;

namespace Domain.Shared.Errors;

public abstract class DomainLayerError(string className, string propertyName, string message) 
    : Error(className, propertyName, message);
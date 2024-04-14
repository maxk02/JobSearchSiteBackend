namespace Application.Common.Exceptions;

public class NullToNonNullableMappingException()
    : Exception("Null value to non-nullable value mapping attempt. Check for missing validations.");
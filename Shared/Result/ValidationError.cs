namespace Shared.Result;

public class ValidationError
{
    public ValidationError(string identifier, string errorMessage, string errorCode, ValidationSeverity severity)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        Severity = severity;
    }

    public string Identifier { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public string ErrorCode { get; protected set; }
    public ValidationSeverity Severity { get; protected set; }
}
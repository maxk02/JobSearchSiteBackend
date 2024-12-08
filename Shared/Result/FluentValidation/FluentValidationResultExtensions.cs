using FluentValidation;
using FluentValidation.Results;

namespace Shared.Result.FluentValidation;

public static class FluentValidationResultExtensions
{
    public static List<ValidationError> AsErrors(this ValidationResult valResult)
    {
        var resultErrors = new List<ValidationError>();

        foreach (var valFailure in valResult.Errors)
        {
            resultErrors.Add(new ValidationError(
                valFailure.PropertyName,
                valFailure.ErrorMessage,
                valFailure.ErrorCode,
                FromSeverity(valFailure.Severity)));
        }

        return resultErrors;
    }

    public static ValidationSeverity FromSeverity(Severity severity)
    {
        switch (severity)
        {
            case Severity.Error: return ValidationSeverity.Error;
            case Severity.Warning: return ValidationSeverity.Warning;
            case Severity.Info: return ValidationSeverity.Info;
            default: throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity");
        }
    }
}
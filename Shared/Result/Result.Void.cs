using System.Text.Json.Serialization;

namespace Shared.Result;

public class Result : IResult
{
    public Result() { }

    protected Result(ResultStatus status) => Status = status;

    public static Result WithMetadataFrom<T>(Result<T> result) => new()
    {
        Status = result.Status,
        Errors = result.Errors,
        SuccessMessage = result.SuccessMessage,
        CorrelationId = result.CorrelationId,
        ValidationErrors = result.ValidationErrors,
    };
        
    [JsonInclude] public ResultStatus Status { get; protected set; } = ResultStatus.Ok;

    public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Created;

    [JsonInclude] public string SuccessMessage { get; protected set; } = string.Empty;
    [JsonInclude] public string CorrelationId { get; protected set; } = string.Empty;
    [JsonInclude] public string Location { get; protected set; } = string.Empty;
    [JsonInclude] public IEnumerable<string> Errors { get; protected set; } = [];
    [JsonInclude] public IEnumerable<ValidationError> ValidationErrors { get; protected set; } = [];

    /// <summary>
    /// Represents a successful operation without return type
    /// </summary>
    /// <returns>A Result</returns>
    public static Result Success() => new();

    /// <summary>
    /// Represents a successful operation without return type
    /// </summary>
    /// <param name="successMessage">Sets the SuccessMessage property</param>
    /// <returns>A Result</returns>
    public static Result SuccessWithMessage(string successMessage) => new() { SuccessMessage = successMessage };

    public static Result Error(IEnumerable<string> errorMessages) => new(ResultStatus.Error)
    {
        Errors = errorMessages,
    };

    /// <summary>
    /// Represents an error that occurred during the execution of the service.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="error">An optional instance of ErrorList with list of string error messages and CorrelationId.</param>
    /// <returns>A Result</returns>
    public static Result Error(ErrorList? error = null) => new(ResultStatus.Error)
    {
        CorrelationId = error?.CorrelationId ?? string.Empty,
        Errors = error?.ErrorMessages ?? []
    };

    /// <summary>
    /// Represents an error that occurred during the execution of the service.
    /// A single error message may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns>A Result</returns>
    public static Result Error(string errorMessage) =>
        new(ResultStatus.Error) { Errors = [errorMessage] };

    /// <summary>
    /// Represents the validation error that prevents the underlying service from completing.
    /// </summary>
    /// <param name="validationError">The validation error encountered</param>
    /// <returns>A Result</returns>
    public static Result Invalid(ValidationError validationError)
        => new(ResultStatus.Invalid) { ValidationErrors = [validationError] };

    /// <summary>
    /// Represents validation errors that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A Result</returns>
    public static Result Invalid(params ValidationError[] validationErrors)
        => new(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };

    /// <summary>
    /// Represents validation errors that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A Result</returns>
    public static Result Invalid(IEnumerable<ValidationError> validationErrors)
        => new(ResultStatus.Invalid) { ValidationErrors = validationErrors };

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// </summary>
    /// <returns>A Result</returns>
    public static Result NotFound() => new Result(ResultStatus.NotFound);

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A Result</returns>
    public static Result NotFound(params string[] errorMessages) =>
        new(ResultStatus.NotFound) { Errors = errorMessages };

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A Result</returns>
    public static Result Forbidden() => new(ResultStatus.Forbidden);

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param> 
    /// <returns>A Result</returns>
    public static Result Forbidden(params string[] errorMessages) =>
        new(ResultStatus.Forbidden) { Errors = errorMessages };

    /// <summary>
    /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
    /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A Result</returns>
    public static Result Unauthorized() => new(ResultStatus.Unauthorized);

    /// <summary>
    /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
    /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>  
    /// <returns>A Result</returns>
    public static Result Unauthorized(params string[] errorMessages) =>
        new(ResultStatus.Unauthorized) { Errors = errorMessages };

    /// <summary>
    /// Represents a situation where a service is in conflict due to the current state of a resource,
    /// such as an edit conflict between multiple concurrent updates.
    /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A Result</returns>
    public static Result Conflict() => new(ResultStatus.Conflict);

    /// <summary>
    /// Represents a situation where a service is in conflict due to the current state of a resource,
    /// such as an edit conflict between multiple concurrent updates.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A Result</returns>
    public static Result Conflict(params string[] errorMessages) =>
        new(ResultStatus.Conflict) { Errors = errorMessages };

    /// <summary>
    /// Represents a situation where a service is unavailable, such as when the underlying data store is unavailable.
    /// Errors may be transient, so the caller may wish to retry the operation.
    /// See also HTTP 503 Service Unavailable: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages</param>
    /// <returns></returns>
    public static Result Unavailable(params string[] errorMessages) =>
        new(ResultStatus.Unavailable) { Errors = errorMessages };

    /// <summary>
    /// Represents a critical error that occurred during the execution of the service.
    /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
    /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A Result</returns>
    public static Result CriticalError(params string[] errorMessages) =>
        new(ResultStatus.CriticalError) { Errors = errorMessages };

    /// <summary>
    /// Represents a situation where the server has successfully fulfilled the request, but there is no content to send back in the response body.
    /// </summary>
    /// <returns>A Result object</returns>
    public static Result NoContent() => new(ResultStatus.NoContent);
}
namespace Core.Services.Auth.Exceptions;

public class CurrentAccountDataNotAvailableException()
    : Exception("Current account data not available in handler that requires user to be logged in.");
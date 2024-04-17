namespace Application.Common.Exceptions;

public class AppSettingsNullException()
    : Exception("Null value found in required app configuration string.");
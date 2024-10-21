namespace Shared.Errors;

public abstract class Error : IEquatable<Error>
{
    public Error(string className, string propertyName, string message)
    {
        ClassName = className;
        PropertyName = propertyName;
        Message = message;
    }

    public string ClassName { get; }
    public string PropertyName { get; }
    public virtual string Code { get; } = "";

    public string Message { get; }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}

namespace Application.Providers;

public interface ICurrentUserProvider
{
    long? UserId { get; }
}
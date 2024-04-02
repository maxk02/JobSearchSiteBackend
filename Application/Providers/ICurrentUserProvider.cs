namespace Application.Providers;

public interface ICurrentUserProvider
{
    bool IsAuthenticated { get; }
    long UserId { get; }
}
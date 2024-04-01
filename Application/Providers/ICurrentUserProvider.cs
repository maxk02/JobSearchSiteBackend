namespace Application.Providers;

public interface ICurrentUserProvider
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    List<string> UserRoles { get; }
}
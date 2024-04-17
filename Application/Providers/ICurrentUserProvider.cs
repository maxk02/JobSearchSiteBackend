namespace Application.Providers;

public interface ICurrentUserProvider
{
    int? UserId { get; }
}
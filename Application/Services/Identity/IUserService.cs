using Domain.Enums;

namespace Application.Services.Identity;

public interface IUserService
{
    // jwt, error text
    Task<(string?, string?)> SignIn(string email, string password, bool rememberMe);
    Task SignOut();
    Task<string?> Register(string email, string password);
    Task<string?> Delete(long id);
    Task<bool> IsInRole(long id, RoleValue roleValue);
}
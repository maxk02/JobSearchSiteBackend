using Domain.Enums;

namespace Application.Services.Identity;

public interface IIdentityUserService
{
    Task<string?> SignInWithEmail(string email, string password);
    Task<string?> Register(string email, string password);
    Task<bool> Delete(int id);
    Task<bool?> IsInRole(int id, RoleValue roleValue);
}
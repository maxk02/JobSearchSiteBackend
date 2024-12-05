using Domain._Shared.Enums;
using Domain._Shared.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Infrastructure.Auth.AspNetCoreIdentity;

public sealed class IdentityAccountService(UserManager<MyIdentityUser> userManager) : IAccountService
{
    public async Task<Result<UserClaimsDto>> SignInWithEmailAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, password);

        if (!isPasswordCorrect)
            return Result.Unauthorized();
        
        var roles = await userManager.GetRolesAsync(user);
        
        return new UserClaimsDto(user.Id, roles);

    }

    public async Task<Result<UserClaimsDto>> RegisterAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(email);

        if (userFromDb is not null)
            return Result.Conflict("User already registered.");
        
        var user = new MyIdentityUser { Email = email };
        var aspNetIdentityResult = await userManager.CreateAsync(user, password);

        return aspNetIdentityResult.Succeeded ? new UserClaimsDto(user.Id, []) : Result.Error();
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();
        
        var aspNetIdentityResult = await userManager.DeleteAsync(user);
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
    
    public async Task<Result> AddToRoleAsync(string userId, RoleValues roleToAdd,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.AddToRoleAsync(user, roleToAdd.ToString());
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
    
    public async Task<Result> RemoveFromRoleAsync(string userId, RoleValues roleToRemove,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.NotFound();
        
        var aspNetIdentityResult = await userManager.RemoveFromRoleAsync(user, roleToRemove.ToString());
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
    
    public async Task<Result> RemoveFromAllRolesAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.NotFound();
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        var aspNetIdentityResult = await userManager.RemoveFromRolesAsync(user, userRoles);
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
    
    public async Task<Result> ChangePasswordAsync(string id, string oldPassword, string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }

    public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<Result> ConfirmEmailAsync(string id, string emailConfirmationToken,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        
        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }

    public async Task<Result<string>> GeneratePasswordResetTokenAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    
    public async Task<Result> ResetPasswordAsync(string id, string passwordResetToken, string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var aspNetIdentityResult = await userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);

        return aspNetIdentityResult.Succeeded ? Result.Success() : Result.Error();
    }
}
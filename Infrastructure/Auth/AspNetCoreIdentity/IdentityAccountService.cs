using Domain._Shared.Enums;
using Domain._Shared.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Infrastructure.Auth.AspNetCoreIdentity;

public class IdentityAccountService : IAccountService
{
    private readonly UserManager<MyIdentityUser> _userManager;

    public IdentityAccountService(UserManager<MyIdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<UserClaimsDto>> SignInWithEmail(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.NotFound();

        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
        
        if (isPasswordCorrect)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserClaimsDto(user.Id, roles);
        }
        
        return Result.Unauthorized();
    }

    public async Task<Result<UserClaimsDto>> Register(string email, string password)
    {
        var user = new MyIdentityUser { Email = email };
        var result = await _userManager.CreateAsync(user, password);
        
        return result.Succeeded ? new UserClaimsDto(user.Id, []) : Result.Error();
    }

    public async Task<Result> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();
        
        var result = await _userManager.DeleteAsync(user);
        
        return result.Succeeded ? Result.Success() : Result.Error();
    }

    public async Task<Result> ChangeRole(string id, RoleValues newRoleValue)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return Result.NotFound();
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
        if (!removeResult.Succeeded)
            return Result.Error();

        var addResult = await _userManager.AddToRoleAsync(user, newRoleValue.ToString());
        
        return addResult.Succeeded ? Result.Success() : Result.Error();
    }
    
    public async Task<Result> ChangePassword(string id, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        
        return result.Succeeded ? Result.Success() : Result.Error();
    }

    public async Task<Result<string>> GenerateEmailConfirmationToken(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<Result> ConfirmEmail(string id, string emailConfirmationToken)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        
        return result.Succeeded ? Result.Success() : Result.Error();
    }

    public async Task<Result<string>> GeneratePasswordResetToken(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.NotFound();
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    
    public async Task<Result> ResetPassword(string id, string passwordResetToken, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.NotFound();

        var result = await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);

        return result.Succeeded ? Result.Success() : Result.Error();
    }
}
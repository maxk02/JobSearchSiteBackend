using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Application.Common.Exceptions;
using Application.Services.Identity;
using Domain.Shared.Enums;

namespace Infrastructure.Identity;

public class IdentityAccountService : IAccountService
{
    private readonly UserManager<MyIdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityAccountService(UserManager<MyIdentityUser> userManager, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<string?> SignInWithEmail(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return GenerateJwtToken(user);
        }
        return null;
    }

    public async Task<string?> Register(string email, string password)
    {
        var user = new MyIdentityUser { Email = email };
        var result = await _userManager.CreateAsync(user, password);
        
        return result.Succeeded ? GenerateJwtToken(user) : null;
    }

    public async Task<bool> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;
        
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangeRole(string id, RoleValues newRoleValue)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
        if (!removeResult.Succeeded)
            return removeResult.Succeeded;

        var addResult = await _userManager.AddToRoleAsync(user, newRoleValue.ToString());
        
        return addResult.Succeeded;
    }
    
    public async Task<bool> ChangePassword(string id, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        
        return result.Succeeded;
    }

    public async Task<string?> GenerateEmailConfirmationToken(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return null;
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> ConfirmEmail(string id, string emailConfirmationToken)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        
        return result.Succeeded;
    }

    public async Task<string?> GeneratePasswordResetToken(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return null;
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    
    public async Task<bool> ResetPassword(string id, string passwordResetToken, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;

        var result = await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);

        return result.Succeeded;
    }

    private string? GenerateJwtToken(MyIdentityUser user)
    {
        var secretKeyString = _configuration["Jwt:Key"];
        if (secretKeyString is null)
            throw new AppSettingsNullException();

        var issuerString = _configuration["Jwt:Issuer"];
        if (issuerString is null)
            throw new AppSettingsNullException();

        var audienceString = _configuration["Jwt:Audience"];
        if (audienceString is null)
            throw new AppSettingsNullException();
        
        if (_configuration["Jwt:DurationDays"] is null)
            throw new AppSettingsNullException();
        
        var durationParsedSuccessfully = int.TryParse(_configuration["Jwt:DurationDays"], out int durationDays);
        if (!durationParsedSuccessfully)
            throw new AppSettingsBadValueException();
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var userKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
        var credentials = new SigningCredentials(userKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuerString,
            audienceString,
            claims,
            expires: DateTime.Now.AddDays(durationDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
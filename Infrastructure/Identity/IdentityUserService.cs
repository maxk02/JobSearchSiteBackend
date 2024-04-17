using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Exceptions;
using Application.Services.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<MyIdentityUser> _userManager;
    private readonly RoleManager<MyIdentityUser> _roleManager;
    private readonly IConfiguration _configuration;

    public IdentityUserService(UserManager<MyIdentityUser> userManager,
        RoleManager<MyIdentityUser> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
        if (result.Succeeded)
        {
            return GenerateJwtToken(user);
        }
        return null;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        return false;
    }

    public async Task<bool?> IsInRole(int id, RoleValue roleValue)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var result = await _userManager.IsInRoleAsync(user, roleValue.ToString());
            return result;
        }
        return null;
    }
    
    private string? GenerateJwtToken(IdentityUser user)
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
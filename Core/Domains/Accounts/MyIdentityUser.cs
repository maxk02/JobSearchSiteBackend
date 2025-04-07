using Core.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace Core.Domains.Accounts;

public class MyIdentityUser : IdentityUser<long>
{
    public UserProfile? Profile { get; set; }
}
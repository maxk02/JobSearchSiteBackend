using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace JobSearchSiteBackend.Main.Domains.Accounts;

public class MyIdentityUser : IdentityUser<long>
{
    public UserProfile? Profile { get; set; }
}
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace JobSearchSiteBackend.Core.Domains.Accounts;

public class MyIdentityUser : IdentityUser<long>
{
    public UserProfile? Profile { get; set; }
}
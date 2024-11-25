using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.AspNetCoreIdentity;

public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser>
{
    public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : base(options) { }
}
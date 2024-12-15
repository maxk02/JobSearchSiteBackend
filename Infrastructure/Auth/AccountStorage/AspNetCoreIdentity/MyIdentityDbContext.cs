using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.AccountStorage.AspNetCoreIdentity;

public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, long>
{
    public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : base(options) { }
}
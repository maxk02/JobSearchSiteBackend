using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser>
{
    public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
    {
    }
}
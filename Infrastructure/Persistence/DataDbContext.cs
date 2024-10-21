using Domain.Entities;
using Domain.Entities.Categories;
using Domain.Entities.Companies;
using Domain.Entities.ContractTypes;
using Domain.Entities.Fileinfos;
using Domain.Entities.Jobs;
using Domain.Entities.Locations;
using Domain.Entities.Tags;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DataDbContext : DbContext
{
#pragma warning disable CS8618
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ContractType> ContractTypes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Domain.Entities.Applications.Application> MyApplications { get; set; }
    public DbSet<Fileinfo> MyFiles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCompanyBookmark> UserCompanyBookmarks { get; set; }
    public DbSet<UserCompanyPermissionSet> UserCompanyPermissionSets { get; set; }
    public DbSet<UserJobBookmark> UserJobBookmarks { get; set; }
    public DbSet<UserTagPermissionSet> UserTagPermissionSets { get; set; }
    
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Job>().OwnsOne(job => job.SalaryRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<Job>().OwnsOne(job => job.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        modelBuilder.Entity<Job>().OwnsMany(job => job.Addresses,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<Job>().OwnsMany(job => job.Advantages,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<Job>().OwnsMany(job => job.Requirements,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<Job>().OwnsMany(job => job.Responsibilities,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        modelBuilder.Entity<User>().OwnsOne(user => user.SalaryRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<User>().OwnsOne(user => user.EmploymentTypeRecord,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        
        modelBuilder.Entity<User>().OwnsMany(user => user.EducationRecords,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<User>().OwnsMany(user => user.Skills,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson());
        modelBuilder.Entity<User>().OwnsMany(user => user.WorkRecords,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(workRecord => workRecord.Responsibilities,
                    nestedOwnedNavigationBuilder => nestedOwnedNavigationBuilder.ToJson());
            });

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Jobs)
            .WithOne(j => j.Category)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .Property(j => j.IsExpired)
            .HasComputedColumnSql("CASE WHEN DateTimeExpiringUtc < GETUTCDATE() THEN 1 ELSE 0 END");
    }
}
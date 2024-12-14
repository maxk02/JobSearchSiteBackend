using Core.Categories;
using Core.Companies;
using Core.CompanyPermissions;
using Core.ContractTypes;
using Core.Countries;
using Core.FolderPermissions;
using Core.Folders;
using Core.JobApplications;
using Core.Jobs;
using Core.Locations;
using Core.PersonalFiles;
using Core.UserProfiles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Context;

public class MyEfCoreDataContext : DbContext
{
#pragma warning disable CS8618
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ContractType> ContractTypes { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<PersonalFile> PersonalFiles { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<CompanyPermission> CompanyPermissions { get; set; }
    public DbSet<FolderPermission> FolderPermissions { get; set; }
    public DbSet<UserProfile> Users { get; set; }


    public MyEfCoreDataContext(DbContextOptions<MyEfCoreDataContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlServer("");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyEfCoreDataContext).Assembly);
    }
}
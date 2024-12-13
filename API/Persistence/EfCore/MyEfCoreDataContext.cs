using API.Domains.Categories;
using API.Domains.Companies;
using API.Domains.CompanyPermissions;
using API.Domains.ContractTypes;
using API.Domains.Countries;
using API.Domains.FolderPermissions;
using API.Domains.Folders;
using API.Domains.JobApplications;
using API.Domains.Jobs;
using API.Domains.Locations;
using API.Domains.PersonalFiles;
using API.Domains.UserProfiles;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence.EfCore.Context;

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
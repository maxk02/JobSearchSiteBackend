using Core.Domains.Accounts;
using Core.Domains.Categories;
using Core.Domains.Companies;
using Core.Domains.CompanyClaims;
using Core.Domains.Countries;
using Core.Domains.JobApplications;
using Core.Domains.JobContractTypes;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Domains.Jobs;
using Core.Domains.Locations;
using Core.Domains.PersonalFiles;
using Core.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence;

public class MainDataContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, long>
{
#pragma warning disable CS8618
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<JobContractType> ContractTypes { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<PersonalFile> PersonalFiles { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<JobFolder> JobFolders { get; set; }
    public DbSet<JobFolderRelation> JobFolderRelations { get; set; }
    public DbSet<CompanyClaim> CompanyClaims { get; set; }
    public DbSet<UserCompanyClaim> UserCompanyClaims { get; set; }
    public DbSet<JobFolderClaim> JobFolderClaims { get; set; }
    public DbSet<UserJobFolderClaim> UserJobFolderClaims { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }


    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDataContext).Assembly);
    }
}
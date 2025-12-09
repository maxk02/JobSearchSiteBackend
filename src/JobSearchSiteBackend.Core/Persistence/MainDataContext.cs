using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Categories;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Countries;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobContractTypes;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Persistence;

public class MainDataContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, long>
{
#pragma warning disable CS8618
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyAvatar> CompanyAvatars { get; set; }
    public DbSet<CompanyBalanceTransaction>  CompanyBalanceTransactions { get; set; }
    public DbSet<JobContractType> ContractTypes { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryCurrency> CountryCurrencies { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<PersonalFile> PersonalFiles { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JobPublicationInterval>  JobPublicationIntervals { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationRelation> LocationRelations { get; set; }
    public DbSet<JobFolder> JobFolders { get; set; }
    public DbSet<JobFolderRelation> JobFolderRelations { get; set; }
    public DbSet<CompanyClaim> CompanyClaims { get; set; }
    public DbSet<UserCompanyClaim> UserCompanyClaims { get; set; }
    public DbSet<JobFolderClaim> JobFolderClaims { get; set; }
    public DbSet<UserJobApplicationBookmark> UserJobApplicationBookmarks { get; set; }
    public DbSet<UserJobBookmark> UserJobBookmarks { get; set; }
    public DbSet<UserJobFolderClaim> UserJobFolderClaims { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserAvatar>  UserAvatars { get; set; }

    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDataContext).Assembly);
    }
}
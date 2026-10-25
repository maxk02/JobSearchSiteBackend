using JobSearchSiteBackend.Main.Domains.Categories;
using JobSearchSiteBackend.Main.Domains.Companies;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.Countries;
using JobSearchSiteBackend.Main.Domains.EmploymentOptions;
using JobSearchSiteBackend.Main.Domains.JobApplications;
using JobSearchSiteBackend.Main.Domains.JobContractTypes;
using JobSearchSiteBackend.Main.Domains.Jobs;
using JobSearchSiteBackend.Main.Domains.Locations;
using JobSearchSiteBackend.Main.Domains.PersonalFiles;
using JobSearchSiteBackend.Main.Domains.Accounts;
using JobSearchSiteBackend.Main.Domains.UserProfiles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Persistence;

public class MainDataContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, long>
{
#pragma warning disable CS8618
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyAvatar> CompanyAvatars { get; set; }
    public DbSet<CompanyBalanceTransaction> CompanyBalanceTransactions { get; set; }
    // public DbSet<CompanyClaim> CompanyClaims { get; set; }
    public DbSet<CompanyEmployeeInvitation> CompanyEmployeeInvitations { get; set; }
    // CompanyEmployees
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryCurrency> CountryCurrencies { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    //public DbSet<EmploymentOption> EmploymentOptions { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    // public DbSet<JobApplicationTag> JobApplicationTags { get; set; }
    public DbSet<JobContractType> JobContractTypes { get; set; }
    public DbSet<JobPublicationInterval> JobPublicationIntervals { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobSalaryInfo> JobSalaryInfos { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationRelation> LocationRelations { get; set; }
    public DbSet<PersonalFile> PersonalFiles { get; set; }
    public DbSet<UserCompanyClaim> UserCompanyClaims { get; set; }
    public DbSet<UserJobBookmark> UserJobBookmarks { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserAvatar> UserAvatars { get; set; }

    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDataContext).Assembly);
    }
}
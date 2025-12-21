using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore;

public class MainDataContextSeed
{
    private record JobSeedingItem(long Id, bool IsPublic, long JobFolderId,
    long CategoryId, string Title, string? Description,
    DateTime DateTimePublishedUtc, DateTime DateTimeExpiringUtc, List<long> EmploymentOptionIds,
    List<string> Responsibilities, List<string> Requirements, List<string> NiceToHaves);

    public static async Task SeedLocationsAsync(MainDataContext context)
    {       
        await context.Locations.ExecuteDeleteAsync();

        var locations = await SeedFileHelper.LoadJsonAsync<List<Location>>("Domains/Locations/Seed/locations.json");

        if (locations is null || locations.Count == 0)
            throw new InvalidDataException();
        
        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();
    }
    
    public static async Task SeedLocationRelationsAsync(MainDataContext context)
    {
        await context.LocationRelations.ExecuteDeleteAsync();

        var relations = await SeedFileHelper.LoadJsonAsync<List<LocationRelation>>("Domains/Locations/Seed/location_relations.json");
        if (relations is null || relations.Count == 0)
            throw new InvalidDataException();
        
        context.LocationRelations.AddRange(relations);
        await context.SaveChangesAsync();
    }

    public static async Task SeedCompaniesAsync(MainDataContext context)
    {
        await context.Companies.ExecuteDeleteAsync();

        var companies = await SeedFileHelper.LoadJsonAsync<List<Company>>("Domains/Companies/Seed/companies.json");

        if (companies is null || companies.Count == 0)
            throw new InvalidDataException();
        
        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();
    }

    public static async Task SeedCompanyAvatarsAsync(MainDataContext context)
    {
        await context.CompanyAvatars.ExecuteDeleteAsync();

        var companyAvatars = await SeedFileHelper.LoadJsonAsync<List<CompanyAvatar>>("Domains/Companies/Seed/company_avatars.json");

        if (companyAvatars is null || companyAvatars.Count == 0)
            throw new InvalidDataException();
        
        context.CompanyAvatars.AddRange(companyAvatars);
        await context.SaveChangesAsync();
    }

    public static async Task SeedJobsAsync(MainDataContext context)
    {
        await context.Jobs.ExecuteDeleteAsync();

        var jobSeedingItems = await SeedFileHelper.LoadJsonAsync<List<JobSeedingItem>>("Domains/Jobs/Seed/jobs.json");

        if (jobSeedingItems is null || jobSeedingItems.Count == 0)
            throw new InvalidDataException();

        var jobs = new List<Job>();

        foreach (var x in jobSeedingItems)
        {
            var job = new Job(x.CategoryId, x.JobFolderId, x.Title,
            x.Description, x.IsPublic, x.DateTimePublishedUtc, x.DateTimeExpiringUtc,
            x.Responsibilities, x.Requirements, x.NiceToHaves, null,
            x.EmploymentOptionIds.Select(eoid => EmploymentOption.AllValues.Where(eo => eo.Id == eoid).Single()).ToList());
        }
        
        context.Jobs.AddRange(jobs);
        await context.SaveChangesAsync();
    }

    public static async Task SeedJobSalaryInfosAsync(MainDataContext context)
    {
        await context.JobSalaryInfos.ExecuteDeleteAsync();

        var jobSalaryInfoSeedingItems = await SeedFileHelper.LoadJsonAsync<List<JobSalaryInfo>>("Domains/Jobs/Seed/job_salary_infos.json");

        if (jobSalaryInfoSeedingItems is null || jobSalaryInfoSeedingItems.Count == 0)
            throw new InvalidDataException();
        
        context.JobSalaryInfos.AddRange(jobSalaryInfoSeedingItems);
        await context.SaveChangesAsync();
    }
}
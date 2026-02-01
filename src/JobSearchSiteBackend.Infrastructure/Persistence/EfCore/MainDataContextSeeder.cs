using System.Globalization;
using JobSearchSiteBackend.Core.Domains.Accounts;
using JobSearchSiteBackend.Core.Domains.Companies;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.EmploymentOptions;
using JobSearchSiteBackend.Core.Domains.JobApplications;
using JobSearchSiteBackend.Core.Domains.JobApplications.Enums;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore;

public class MainDataContextSeeder(MainDataContext context,
    UserManager<MyIdentityUser> userManager, IConfiguration configuration)
{
    private record JobSeedingItem(long Id, bool IsPublic, long CompanyId, long CategoryId, string Title, string? Description,
        DateTime DateTimePublishedUtc, DateTime DateTimeExpiringUtc, List<long> EmploymentOptionIds, List<long> LocationIds,
        List<long> ContractTypeIds, List<string> Responsibilities, List<string> Requirements, List<string> NiceToHaves);

    private static string GetFileExtensionFromNumber(int number) => number switch
    {
        1 => "docx",
        2 => "pdf",
        3 => "odt",
        _ => throw new ApplicationException()
    };

    public async Task SeedAllAsync()
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await SeedCompaniesAsync();
        await SeedCompanyAvatarsAsync();
        await SeedLocationsAsync();
        await SeedLocationRelationsAsync();
        await SeedUsersThatHaveClaimsAsync();
        await SeedJobsAsync();
        await SeedJobSalaryInfosAsync();
        await SeedUsersThatApplyForJobsAsync();
    }

    private async Task SeedLocationsAsync()
    {       
        var locations = await SeedFileHelper.LoadJsonAsync<List<Location>>("Domains/Locations/Seed/locations.json");

        if (locations is null || locations.Count == 0)
            throw new InvalidDataException();
        
        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();
    }
    
    private async Task SeedLocationRelationsAsync()
    {
        var relations = await SeedFileHelper.LoadJsonAsync<List<LocationRelation>>("Domains/Locations/Seed/location_relations.json");
        if (relations is null || relations.Count == 0)
            throw new InvalidDataException();
        
        context.LocationRelations.AddRange(relations);
        await context.SaveChangesAsync();
    }

    private async Task SeedCompaniesAsync()
    {
        await using var transaction = context.Database.BeginTransaction();

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Companies ON");

        var companies = await SeedFileHelper.LoadJsonAsync<List<Company>>("Domains/Companies/Seed/companies.json");

        if (companies is null || companies.Count == 0)
            throw new InvalidDataException();

        var sqlScript = $"INSERT INTO dbo.Companies (Id, CountryId, IsPublic, Name, Description, CountrySpecificFieldsJson, IsDeleted, DateTimeUpdatedUtc) VALUES ";

        for (var i = 0; i < companies.Count; i++)
        {
            sqlScript += $"({companies[i].Id}, ";
            sqlScript += $"{companies[i].CountryId}, ";
            sqlScript += companies[i].IsPublic ? "1" : "0";
            sqlScript += ", ";
            sqlScript += $"\'{companies[i].Name.Replace("\'", "\'\'")}\', ";

            var description = companies[i].Description?.Replace("\'", "\'\'");
            if (description is null)
                sqlScript += $"NULL, ";
            else
                sqlScript += $"\'{description}\', ";

            sqlScript += $"N\'{{" + companies[i].CountrySpecificFieldsJson + "}\', ";

            sqlScript += "0, GETUTCDATE())";

            if (i < companies.Count - 1)
                sqlScript += ",\n\n";
            else
                sqlScript += ";";
        }

        await context.Database.ExecuteSqlRawAsync(sqlScript);

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Companies OFF");

        await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT (\'dbo.Companies\', RESEED, 1000);");

        await transaction.CommitAsync();
    }

    private async Task SeedCompanyAvatarsAsync()
    {
        await using var transaction = context.Database.BeginTransaction();

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.CompanyAvatars ON");

        var companyAvatars = await SeedFileHelper.LoadJsonAsync<List<CompanyAvatar>>("Domains/Companies/Seed/company_avatars.json");

        if (companyAvatars is null || companyAvatars.Count == 0)
            throw new InvalidDataException();

        var sqlScript = "INSERT INTO CompanyAvatars (Id, GuidIdentifier, DateTimeUpdatedUtc, IsDeleted, CompanyId, Extension, Size, IsUploadedSuccessfully) VALUES \n";

        for (var i = 0; i < companyAvatars.Count; i++)
        {
            sqlScript += $"({companyAvatars[i].Id}, \'{companyAvatars[i].GuidIdentifier}\', GETUTCDATE(), 0, {companyAvatars[i].CompanyId}, \'{companyAvatars[i].Extension}\', 10000000, 1)";
            if (i < companyAvatars.Count - 1)
                sqlScript += ",\n";
            else
                sqlScript += ";";
        }

        await context.Database.ExecuteSqlRawAsync(sqlScript);

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.CompanyAvatars OFF");

        await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT (\'dbo.CompanyAvatars\', RESEED, 1000);");

        await transaction.CommitAsync();
    }

    private async Task SeedJobsAsync()
    {
        await using var transaction = context.Database.BeginTransaction();

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Jobs ON");

        var jobSeedingItems = await SeedFileHelper.LoadJsonAsync<List<JobSeedingItem>>("Domains/Jobs/Seed/jobs.json");

        if (jobSeedingItems is null || jobSeedingItems.Count == 0)
            throw new InvalidDataException();

        var jobs = new List<Job>();

        var sqlScript = "INSERT INTO Jobs (Id, DateTimeUpdatedUtc, DateTimeSyncedWithSearchUtc, IsDeleted, CategoryId, CompanyId, Title, Description, DateTimePublishedUtc, DateTimeExpiringUtc, MaxDateTimeExpiringUtcEverSet, Responsibilities, Requirements, NiceToHaves, IsPublic) VALUES \n";

        var sqlLocationsScript = "INSERT INTO JobLocation (JobsId, LocationsId) VALUES \n";

        var sqlEmploymentOptionsScript = "INSERT INTO EmploymentOptionJob (JobsId, EmploymentOptionsId) VALUES \n";

        var sqlContractTypesScript = "INSERT INTO JobJobContractType (JobsId, JobContractTypesId) VALUES \n";

        List<CompanyBalanceTransaction> companyBalanceTransactionsToAdd = [];
        decimal balanceSumNeededToAdd = decimal.Zero;

        for (var i = 0; i < jobSeedingItems.Count; i++)
        {
            sqlScript += $"({jobSeedingItems[i].Id}, ";
            sqlScript += $"GETDATE(), ";
            sqlScript += $"NULL, ";
            sqlScript += $"0, ";
            sqlScript += $"{jobSeedingItems[i].CategoryId}, ";
            sqlScript += $"{jobSeedingItems[i].CompanyId}, ";
            sqlScript += $"\'{jobSeedingItems[i].Title}\', ";
            if (jobSeedingItems[i].Description is not null)
                sqlScript += $"\'{jobSeedingItems[i].Description}\', ";
            else sqlScript += $"NULL, ";
            sqlScript += $"\'{jobSeedingItems[i].DateTimePublishedUtc.ToString("yyyy-MM-ddTHH:mm:ss.fff")}\', ";
            sqlScript += $"\'{jobSeedingItems[i].DateTimeExpiringUtc.ToString("yyyy-MM-ddTHH:mm:ss.fff")}\', ";
            // maxDateTimeExpiringUtcEverSet (same as dateTimeExpiringUtc)
            sqlScript += $"\'{jobSeedingItems[i].DateTimeExpiringUtc.ToString("yyyy-MM-ddTHH:mm:ss.fff")}\', ";
            sqlScript += $"N\'" + JsonConvert.SerializeObject(jobSeedingItems[i].Responsibilities) + "\', ";
            sqlScript += $"N\'" + JsonConvert.SerializeObject(jobSeedingItems[i].Requirements) + "\', ";
            sqlScript += $"N\'" + JsonConvert.SerializeObject(jobSeedingItems[i].NiceToHaves) + "\', ";
            sqlScript += "1)";

            if (i < jobSeedingItems.Count - 1)
                sqlScript += ",\n";
            else
                sqlScript += ";";

            for (var l = 0; l < jobSeedingItems[i].LocationIds.Count; l++)
            {
                var lId = jobSeedingItems[i].LocationIds[l];

                var location = await context.Locations.FindAsync([lId]);

                if (location is null)
                    throw new ApplicationException($"Location with id=${lId} not found.");

                sqlLocationsScript += $"({jobSeedingItems[i].Id}, {lId})";

                if (i == jobSeedingItems.Count - 1 && l == jobSeedingItems[i].LocationIds.Count - 1)
                    sqlLocationsScript += ";";
                else
                    sqlLocationsScript += ", \n";
            }

            for (var eo = 0; eo < jobSeedingItems[i].EmploymentOptionIds.Count; eo++)
            {
                var eoId = jobSeedingItems[i].EmploymentOptionIds[eo];

                sqlEmploymentOptionsScript += $"({jobSeedingItems[i].Id}, {eoId})";

                if (i == jobSeedingItems.Count - 1 && eo == jobSeedingItems[i].EmploymentOptionIds.Count - 1)
                    sqlEmploymentOptionsScript += ";";
                else
                    sqlEmploymentOptionsScript += ", \n";
            }
            
            for (var ct = 0; ct < jobSeedingItems[i].ContractTypeIds.Count; ct++)
            {
                var ctId = jobSeedingItems[i].ContractTypeIds[ct];

                sqlContractTypesScript += $"({jobSeedingItems[i].Id}, {ctId})";

                if (i == jobSeedingItems.Count - 1 && ct == jobSeedingItems[i].ContractTypeIds.Count - 1)
                    sqlContractTypesScript += ";";
                else
                    sqlContractTypesScript += ", \n";
            }


            //

            var diff = jobSeedingItems[i].DateTimeExpiringUtc.Subtract(jobSeedingItems[i].DateTimePublishedUtc).TotalDays;
            var daysCeiled = (int)Math.Ceiling(diff);
            
            // suppose we have implemented it only for one currency for now
            var jobPublicationInterval = await context.JobPublicationIntervals
                .Include(jpi => jpi.CountryCurrency)
                .Where(jpi => jpi.CountryCurrency!.CountryId == 1)
                .Where(jpi => jpi.MaxDaysOfPublication >= daysCeiled)
                .OrderBy(v => v.MaxDaysOfPublication)
                .FirstOrDefaultAsync();
            
            if (jobPublicationInterval is null)
                throw new ApplicationException($"Job publication interval error on job id = {jobSeedingItems[i].Id}");
            
            var companyBalanceTransaction = new CompanyBalanceTransaction(4, -jobPublicationInterval.Price,
                $"Publikacja ogłoszenia \"{jobSeedingItems[i].Title}\" do {jobSeedingItems[i].DateTimeExpiringUtc.ToString(CultureInfo.InvariantCulture)}",
                jobPublicationInterval.CountryCurrency!.CurrencyId, 1);

            companyBalanceTransaction.DateTimeCommittedUtc = jobSeedingItems[i].DateTimePublishedUtc;
            
            companyBalanceTransactionsToAdd.Add(companyBalanceTransaction);
            balanceSumNeededToAdd += jobPublicationInterval.Price;

            //
        }

        await context.Database.ExecuteSqlRawAsync(sqlScript);
        await context.Database.ExecuteSqlRawAsync(sqlLocationsScript);
        await context.Database.ExecuteSqlRawAsync(sqlEmploymentOptionsScript);
        await context.Database.ExecuteSqlRawAsync(sqlContractTypesScript);

        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Jobs OFF");

        await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT (\'dbo.Jobs\', RESEED, 1000);");

        await transaction.CommitAsync();

        var topUpCompanyBalanceTransaction = new CompanyBalanceTransaction(4, balanceSumNeededToAdd + 150,
            $"Doładowanie konta", 1, 1);

        topUpCompanyBalanceTransaction.DateTimeCommittedUtc = DateTime.UtcNow.AddMonths(-4);
        
        context.CompanyBalanceTransactions.Add(topUpCompanyBalanceTransaction);

        await context.SaveChangesAsync();

        context.CompanyBalanceTransactions.AddRange(companyBalanceTransactionsToAdd);

        await context.SaveChangesAsync();
    }

    private async Task SeedJobSalaryInfosAsync()
    {
        var jobSalaryInfoSeedingItems = await SeedFileHelper.LoadJsonAsync<List<JobSalaryInfo>>("Domains/Jobs/Seed/job_salary_infos.json");

        if (jobSalaryInfoSeedingItems is null || jobSalaryInfoSeedingItems.Count == 0)
            throw new InvalidDataException();
        
        context.JobSalaryInfos.AddRange(jobSalaryInfoSeedingItems);
        await context.SaveChangesAsync();
    }

    private async Task SeedUsersThatHaveClaimsAsync()
    {
        var company = await context.Companies
            .Include(c => c.Employees)
            .Where(c => c.Id == 4)
            .SingleAsync();

        // owner (global admin)
        var user1FromDb = await userManager.FindByEmailAsync("admin@transworld.pl");

        if (user1FromDb is null)
        {
            var user1 = new MyIdentityUser
            {
                UserName = "admin@transworld.pl",
                Email = "admin@transworld.pl",
                EmailConfirmed = true
            };

            var aspNetIdentityResult1 = await userManager
                .CreateAsync(user1, configuration["admin_at_transworld.pl_USER_PASSWORD"] ?? throw new ApplicationException());

            if (!aspNetIdentityResult1.Succeeded)
                throw new ApplicationException(string.Join(",", aspNetIdentityResult1.Errors.Select(e => e.Description)));

            long user1Id = user1.Id;

            var newUser1Profile = new UserProfile(user1Id, "Administrator",
                "Firmy", null, true);

            context.UserProfiles.Add(newUser1Profile);
            company.Employees!.Add(newUser1Profile);

            await context.SaveChangesAsync();


            List<CompanyClaim> companyClaims = CompanyClaim.AllValues.ToList();

            var user1CompanyClaims = companyClaims
                .Select(x => new UserCompanyClaim(user1Id, 4, x.Id))
                .ToList();

            context.UserCompanyClaims.AddRange(user1CompanyClaims);


            await context.SaveChangesAsync();
        }

        // partial admin
        var user2FromDb = await userManager.FindByEmailAsync("partial_admin@transworld.pl");

        if (user2FromDb is null)
        {
            var user2 = new MyIdentityUser {
                UserName = "partial_admin@transworld.pl",
                Email = "partial_admin@transworld.pl",
                EmailConfirmed = true
            };
        
            var aspNetIdentityResult2 = await userManager
                .CreateAsync(user2, configuration["partial_admin_at_transworld.pl_USER_PASSWORD"] ?? throw new ApplicationException());

            if (!aspNetIdentityResult2.Succeeded)
                throw new ApplicationException();

            long user2Id = user2.Id;

            var newUser2Profile = new UserProfile(user2Id, "Administrator",
                "Częściowy", null, true);

            context.UserProfiles.Add(newUser2Profile);
            company.Employees!.Add(newUser2Profile);

            await context.SaveChangesAsync();

            List<CompanyClaim> companyClaims = [
                CompanyClaim.CanReadJobs,
                CompanyClaim.CanReadStats,
                CompanyClaim.CanManageApplications,
                CompanyClaim.CanEditJobs,
                CompanyClaim.IsAdmin
            ];

            var user2CompanyClaims = companyClaims
                .Select(x => new UserCompanyClaim(user2Id, 4, x.Id))
                .ToList();

            context.UserCompanyClaims.AddRange(user2CompanyClaims);

            await context.SaveChangesAsync();
        }

        // branch application reviewer
        var user3FromDb = await userManager.FindByEmailAsync("application_reviewer@transworld.pl");

        if (user3FromDb is null)
        {
            var user3 = new MyIdentityUser {
                UserName = "application_reviewer@transworld.pl",
                Email = "application_reviewer@transworld.pl",
                EmailConfirmed = true
            };
        
            var aspNetIdentityResult3 = await userManager
                .CreateAsync(user3, configuration["application_reviewer_at_transworld.pl_USER_PASSWORD"] ?? throw new ApplicationException());

            if (!aspNetIdentityResult3.Succeeded)
                throw new ApplicationException();

            long user3Id = user3.Id;

            var newUser3Profile = new UserProfile(user3Id, "Pracownik",
                "HR", null, true);

            context.UserProfiles.Add(newUser3Profile);
            company.Employees!.Add(newUser3Profile);

            List<CompanyClaim> companyClaims = [
                CompanyClaim.CanReadJobs,
                CompanyClaim.CanReadStats,
                CompanyClaim.CanManageApplications
            ];

            var user3CompanyClaims = companyClaims
                .Select(x => new UserCompanyClaim(user3Id, 4, x.Id))
                .ToList();

            context.UserCompanyClaims.AddRange(user3CompanyClaims);

            await context.SaveChangesAsync();
        }
    }

    private async Task SeedUsersThatApplyForJobsAsync()
    {
        // test user who will apply for jobs
        var user4FromDb = await userManager.FindByEmailAsync("test_user@transworld.pl");

        if (user4FromDb is null)
        {
            var user4 = new MyIdentityUser {
                UserName = "test_user@transworld.pl",
                Email = "test_user@transworld.pl",
                EmailConfirmed = true
            };
        
            var aspNetIdentityResult4 = await userManager
                .CreateAsync(user4, configuration["test_user_at_transworld.pl_USER_PASSWORD"] ?? throw new ApplicationException());

            if (!aspNetIdentityResult4.Succeeded)
                throw new ApplicationException();

            long user4Id = user4.Id;

            var newUser4Profile = new UserProfile(user4Id, "Użytkownik",
                "Testowy", null, true);

            context.UserProfiles.Add(newUser4Profile);

            var personalFile = new PersonalFile(user4.Id, $"CV",
                "pdf", 5453628, "wsparcie techniczne znajomość sql transporeon");

            personalFile.IsUploadedSuccessfully = true;

            var personalFile2 = new PersonalFile(user4.Id, $"Certyfikat z kursu SQL",
                "pdf", 3847389, "sql server bazy danych");

            personalFile2.IsUploadedSuccessfully = true;

            newUser4Profile.PersonalFiles = [personalFile, personalFile2];

            await context.SaveChangesAsync();

            var jobs = await context.Jobs
                .Include(j => j.Locations)
                .Where(j => j.Id >= 100 && j.Id <= 137)
                .ToListAsync();

            List<JobApplication> jobApplications = [];

            var rnd = new Random();

            foreach (var j in jobs)
            {
                int jobAppStatus = rnd.Next(1, 5);

                var jobApplication = new JobApplication(user4Id, j.Id, j.Locations!.First().Id, (JobApplicationStatus)jobAppStatus);

                jobApplication.PersonalFiles = [personalFile];

                jobApplications.Add(jobApplication);

                context.UserJobBookmarks.Add(new UserJobBookmark(user4Id, j.Id));
            }

            newUser4Profile.JobApplications = jobApplications;

            await context.SaveChangesAsync();
        }

        // users that send applications
        for (var i = 0; i < 50; i++)
        {
            var userFromDb = await userManager.FindByEmailAsync($"sample_user_{i}@znajdzprace.pl");

            if (userFromDb is not null)
                continue;

            var sampleUser = new MyIdentityUser { UserName = $"sample_user_{i}@znajdzprace.pl", Email = $"sample_user_{i}@znajdzprace.pl" };
        
            var aspNetIdentitySampleUserResult = await userManager
                .CreateAsync(sampleUser, configuration["SAMPLE_USER_PASSWORD"] ?? throw new ApplicationException());

            if (!aspNetIdentitySampleUserResult.Succeeded)
                throw new ApplicationException();

            var sampleUserProfile = new UserProfile(sampleUser.Id, "Użytkownik",
                $"Numer {i}", null, true);

            context.UserProfiles.Add(sampleUserProfile);
            await context.SaveChangesAsync();

            // add 1-3 files for each and then job applications with them
            var random = new Random();
            var randFileNumber = random.Next(1, 5);
            List<PersonalFile> personalFilesToAddApplicationWith = [];

            for (var f = 0; f < randFileNumber; f++)
            {
                var randFileExtensionNumber = random.Next(1, 4);
                var randFileSizeInBytes = random.Next(1048576, 10485761);

                var fileText = f switch
                {
                    12 => "wózek widłowy",
                    20 => "wózek widłowy UDT, prawo jazdy kat. B",
                    24 => "UDT",
                    _ => ""
                };

                var personalFile = new PersonalFile(sampleUser.Id, $"plik_{f}",
                    GetFileExtensionFromNumber(randFileExtensionNumber), randFileSizeInBytes, fileText);

                personalFile.IsUploadedSuccessfully = true;

                context.PersonalFiles.Add(personalFile);
                personalFilesToAddApplicationWith.Add(personalFile);
            }

            var randApplicationStatusNumber = random.Next(1, 5);

            var jobApplication = new JobApplication(sampleUser.Id, 117, 7, (JobApplicationStatus)randApplicationStatusNumber);
            jobApplication.PersonalFiles = personalFilesToAddApplicationWith;
            context.JobApplications.Add(jobApplication);

            await context.SaveChangesAsync();
        }
    }
}
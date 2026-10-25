using JobSearchSiteBackend.Main.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.SearchCompanySharedJobs;

public record SearchCompanySharedJobsResult(ICollection<CompanyJobListItemDto> Jobs);
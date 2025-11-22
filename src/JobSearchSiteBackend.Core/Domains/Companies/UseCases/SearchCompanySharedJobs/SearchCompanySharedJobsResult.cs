using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;

public record SearchCompanySharedJobsResult(ICollection<CompanyJobListItemDto> Jobs);
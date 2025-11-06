using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;

public record SearchCompanySharedJobsResponse(ICollection<CompanyJobListItemDto> Jobs);
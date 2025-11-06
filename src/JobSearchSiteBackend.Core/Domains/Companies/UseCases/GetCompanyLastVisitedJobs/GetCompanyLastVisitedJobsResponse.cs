using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public record GetCompanyLastVisitedJobsResponse(ICollection<CompanyJobListItemDto> Jobs);
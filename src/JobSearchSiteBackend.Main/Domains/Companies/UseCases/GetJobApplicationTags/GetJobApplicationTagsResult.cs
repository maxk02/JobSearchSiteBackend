using JobSearchSiteBackend.Core.Domains._Shared.Pagination;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetJobApplicationTags;

public record GetJobApplicationTagsResult(ICollection<string> Tags, PaginationResponse PaginationResponse);
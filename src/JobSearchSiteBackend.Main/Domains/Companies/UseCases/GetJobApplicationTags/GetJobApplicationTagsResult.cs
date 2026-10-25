using JobSearchSiteBackend.Main.Domains._Shared.Pagination;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetJobApplicationTags;

public record GetJobApplicationTagsResult(ICollection<string> Tags, PaginationResponse PaginationResponse);
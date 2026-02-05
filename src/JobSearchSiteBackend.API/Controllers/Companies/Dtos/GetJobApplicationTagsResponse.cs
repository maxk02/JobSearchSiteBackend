using JobSearchSiteBackend.Core.Domains._Shared.Pagination;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetJobApplicationTagsResponse(ICollection<string> Tags, PaginationResponse PaginationResponse);
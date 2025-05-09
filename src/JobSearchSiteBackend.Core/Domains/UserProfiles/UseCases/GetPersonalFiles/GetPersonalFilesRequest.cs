using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesRequest(PaginationSpec PaginationSpec) 
    : IRequest<Result<GetPersonalFilesResponse>>;
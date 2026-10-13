using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;

public record GetPersonalFilesQuery(int Page, int Size) 
    : IRequest<Result<GetPersonalFilesResult>>;
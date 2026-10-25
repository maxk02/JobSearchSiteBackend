using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public record GetBookmarkedJobsQuery(int Page, int Size) 
    : IRequest<Result<GetBookmarkedJobsResult>>;
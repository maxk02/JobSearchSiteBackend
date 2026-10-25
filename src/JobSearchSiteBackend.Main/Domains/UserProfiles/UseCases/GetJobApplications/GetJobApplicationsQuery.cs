using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsQuery(int? StatusId, int Page, int Size) 
    : IRequest<Result<GetJobApplicationsResult>>;
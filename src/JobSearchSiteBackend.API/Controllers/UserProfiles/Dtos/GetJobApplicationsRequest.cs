using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetJobApplicationsRequest(int Page, int Size) 
    : IRequest<Result<GetJobApplicationsResponse>>;
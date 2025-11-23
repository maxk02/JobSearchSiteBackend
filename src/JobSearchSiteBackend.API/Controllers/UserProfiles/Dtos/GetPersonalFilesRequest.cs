using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetPersonalFilesRequest(int Page, int Size) 
    : IRequest<Result<GetPersonalFilesResponse>>;
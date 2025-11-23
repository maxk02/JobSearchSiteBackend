using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public sealed record AddUserProfileRequest(string FirstName, string LastName, 
    string Email, string? Phone) : IRequest<Result<AddUserProfileResponse>>;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record UpdateUserProfileRequest(string? FirstName, string? LastName, string? Phone) : IRequest<Result>;
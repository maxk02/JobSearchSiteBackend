using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.Companies;

public record UpdateCompanyRequestDto(string? Name, string? Description, bool? IsPublic) : IRequest<Result>;
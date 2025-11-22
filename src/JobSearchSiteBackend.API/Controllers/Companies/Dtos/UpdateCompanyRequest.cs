using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record UpdateCompanyRequest(string? Name, string? Description, bool? IsPublic);
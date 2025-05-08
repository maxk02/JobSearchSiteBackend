using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.Companies;

public record UpdateCompanyRequestDto(string? Name, string? Description, bool? IsPublic) : IRequest<Result>;
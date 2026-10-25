using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.RemoveCompanyEmployee;

public record RemoveCompanyEmployeeCommand(long CompanyId, long UserId): IRequest<Result>;
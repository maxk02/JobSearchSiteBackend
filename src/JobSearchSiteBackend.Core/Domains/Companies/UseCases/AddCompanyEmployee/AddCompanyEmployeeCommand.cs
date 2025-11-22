using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;

public record AddCompanyEmployeeCommand(long CompanyId, long UserId): IRequest<Result>;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;

public record AddCompanyEmployeeRequest(long CompanyId, long UserId): IRequest<Result>;
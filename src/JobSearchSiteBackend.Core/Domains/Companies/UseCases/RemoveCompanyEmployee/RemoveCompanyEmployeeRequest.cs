using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyEmployee;

public record RemoveCompanyEmployeeRequest(long CompanyId, long UserId): IRequest<Result>;
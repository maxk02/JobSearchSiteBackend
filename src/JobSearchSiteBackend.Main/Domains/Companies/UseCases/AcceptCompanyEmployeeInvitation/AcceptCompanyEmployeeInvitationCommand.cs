using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AcceptCompanyEmployeeInvitation;

public record AcceptCompanyEmployeeInvitationCommand(long CompanyId, long UserId, string Token): IRequest<Result>;
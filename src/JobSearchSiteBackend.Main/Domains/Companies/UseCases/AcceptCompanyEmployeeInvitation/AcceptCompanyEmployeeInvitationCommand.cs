using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.AcceptCompanyEmployeeInvitation;

public record AcceptCompanyEmployeeInvitationCommand(long CompanyId, long UserId, string Token): IRequest<Result>;
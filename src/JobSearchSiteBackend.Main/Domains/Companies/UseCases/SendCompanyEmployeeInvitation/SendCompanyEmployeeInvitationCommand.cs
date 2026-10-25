using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.SendCompanyEmployeeInvitation;

public record SendCompanyEmployeeInvitationCommand(long CompanyId, string InvitedUserEmail): IRequest<Result<SendCompanyEmployeeInvitationResult>>;
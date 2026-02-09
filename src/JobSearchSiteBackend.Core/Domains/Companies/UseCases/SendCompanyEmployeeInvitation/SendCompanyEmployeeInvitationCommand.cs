using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SendCompanyEmployeeInvitation;

public record SendCompanyEmployeeInvitationCommand(long CompanyId, string InvitedUserEmail): IRequest<Result>;
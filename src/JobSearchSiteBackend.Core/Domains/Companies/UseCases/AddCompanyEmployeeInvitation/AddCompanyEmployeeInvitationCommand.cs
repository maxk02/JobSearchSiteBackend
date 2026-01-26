using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployeeInvitation;

public record AddCompanyEmployeeInvitationCommand(long CompanyId, string InvitedUserEmail): IRequest<Result>;
using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyEmployeeInvitation;

public record GetCompanyEmployeeInvitationQuery(long CompanyId, string InvitedUserEmail) : IRequest<Result<GetCompanyEmployeeInvitationResult>>;
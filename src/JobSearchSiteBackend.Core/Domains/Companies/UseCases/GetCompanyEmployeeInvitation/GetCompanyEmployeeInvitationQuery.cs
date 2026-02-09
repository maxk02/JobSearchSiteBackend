using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployeeInvitation;

public record GetCompanyEmployeeInvitationQuery(long CompanyId, string InvitedUserEmail) : IRequest<Result<GetCompanyEmployeeInvitationResult>>;
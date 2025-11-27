using AutoMapper;
using JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims;

public class CompanyClaimsControllerDtosMapper : Profile
{
    public CompanyClaimsControllerDtosMapper()
    {
        CreateMap<GetCompanyClaimIdsForUserResult, GetCompanyClaimIdsForUserResponse>();
        CreateMap<GetCompanyClaimsOverviewResult, GetCompanyClaimsOverviewResponse>();
    }
}
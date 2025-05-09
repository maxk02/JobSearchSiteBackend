using AutoMapper;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims;

public class CompanyClaimsControllerDtosMapper : Profile
{
    public CompanyClaimsControllerDtosMapper()
    {
        CreateMap<UpdateCompanyClaimIdsForUserRequestDto, UpdateCompanyClaimIdsForUserRequest>()
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("CompanyId", out var companyId))
                {
                    return (long)companyId;
                }

                return 0;
            }))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("UserId", out var userId))
                {
                    return (long)userId;
                }

                return 0;
            }));
    }
}
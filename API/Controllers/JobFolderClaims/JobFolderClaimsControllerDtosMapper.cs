using API.Controllers.CompanyClaims;
using AutoMapper;
using Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

namespace API.Controllers.JobFolderClaims;

public class JobFolderClaimsControllerDtosMapper : Profile
{
    public JobFolderClaimsControllerDtosMapper()
    {
        CreateMap<UpdateJobFolderClaimIdsForUserRequestDto, UpdateJobFolderClaimIdsForUserRequest>()
            .ForMember(dest => dest.FolderId, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("FolderId", out var folderId))
                {
                    return (long)folderId;
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
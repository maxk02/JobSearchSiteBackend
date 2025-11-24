using JobSearchSiteBackend.API.Controllers.CompanyClaims;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using UpdateJobFolderClaimIdsForUserRequest = JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos.UpdateJobFolderClaimIdsForUserRequest;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

public class JobFolderClaimsControllerDtosMapper : Profile
{
    public JobFolderClaimsControllerDtosMapper()
    {
        CreateMap<GetJobFolderClaimsOverviewRequest, GetJobFolderClaimsOverviewQuery>()
            .ForMember(dest => dest.JobFolderId, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("JobFolderId", out var jobFolderId))
                {
                    return (long)jobFolderId;
                }

                return 0;
            }));
        
        CreateMap<UpdateJobFolderClaimIdsForUserRequest, UpdateJobFolderClaimIdsForUserCommand>()
            .ForMember(dest => dest.JobFolderId, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("JobFolderId", out var jobFolderId))
                {
                    return (long)jobFolderId;
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
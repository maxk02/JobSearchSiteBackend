﻿using JobSearchSiteBackend.API.Controllers.CompanyClaims;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

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
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

public class JobFolderClaimsControllerDtosMapper : Profile
{
    public JobFolderClaimsControllerDtosMapper()
    {
        CreateMap<GetJobFolderClaimIdsForUserResult, GetJobFolderClaimIdsForUserResponse>();
        CreateMap<GetJobFolderClaimsOverviewResult, GetJobFolderClaimsOverviewResponse>();
    }
}
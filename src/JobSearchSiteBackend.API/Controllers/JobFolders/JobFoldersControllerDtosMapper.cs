using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetFolderJobs;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

public class JobFoldersControllerDtosMapper : Profile
{
    public JobFoldersControllerDtosMapper()
    {
        CreateMap<GetChildFoldersResult, GetChildFoldersResponse>();
        
        CreateMap<GetFolderJobsResult, GetFolderJobsResponse>();
        
        CreateMap<GetJobFolderResult, GetJobFolderResponse>();
    }
}
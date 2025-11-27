using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using UpdateJobFolderRequest = JobSearchSiteBackend.API.Controllers.JobFolders.Dtos.UpdateJobFolderRequest;

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
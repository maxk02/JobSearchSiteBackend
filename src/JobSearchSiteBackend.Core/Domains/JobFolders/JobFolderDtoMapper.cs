using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders;

public class JobFolderDtoMapper : Profile
{
    public JobFolderDtoMapper()
    {
        CreateMap<JobFolder, JobFolderMinimalDto>(); //todo detailed dto
    }
}
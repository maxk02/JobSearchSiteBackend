using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public class JobSalaryInfoDtoMapper : Profile
{
    public JobSalaryInfoDtoMapper()
    {
        CreateMap<JobSalaryInfo, JobSalaryInfoDto>().ReverseMap();
    }
}
using AutoMapper;

namespace Core.Domains.Jobs.Dtos;

public class JobSalaryInfoDtoMapper : Profile
{
    public JobSalaryInfoDtoMapper()
    {
        CreateMap<JobSalaryInfo, JobSalaryInfoDto>().ReverseMap();
    }
}
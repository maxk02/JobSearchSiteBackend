using AutoMapper;

namespace Core.Domains.JobFolders.Dtos;

public class JobFolderDtoMapper : Profile
{
    public JobFolderDtoMapper()
    {
        CreateMap<JobFolder, JobFolderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Company!.LogoLink));
    }
}
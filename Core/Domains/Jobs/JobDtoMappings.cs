using AutoMapper;

namespace Core.Domains.Jobs.Dtos;

public class JobDtoMappings : Profile
{
    public JobDtoMappings()
    {
        CreateMap<Job, JobCardDto>()
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.JobFolder!.Company!.Name))
            .ForMember(dest => dest.CompanyLogoLink,
                opt => opt.MapFrom(src => src.JobFolder!.Company!.LogoLink))
            .ForMember(dest => dest.EmploymentTypeIds,
                opt => opt.MapFrom(src => src.EmploymentTypes!.Select(x => x.Id).ToList()));
        
        
    }
}
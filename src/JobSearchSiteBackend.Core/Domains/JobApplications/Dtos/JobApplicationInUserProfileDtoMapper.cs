using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

public class JobApplicationInUserProfileDtoMapper : Profile
{
    public JobApplicationInUserProfileDtoMapper()
    {
        CreateMap<JobApplication, JobApplicationInUserProfileDto>()
            .ForMember(dest => dest.CompanyId,
                opt => opt.MapFrom(src => src.Job!.JobFolder!.CompanyId))
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Job!.JobFolder!.Company!.Name))
            .ForMember(dest => dest.JobTitle,
                opt => opt.MapFrom(src => src.Job!.Title))
            .ForMember(dest => dest.DateTimePublishedUtc,
                opt => opt.MapFrom(src => src.Job!.DateTimePublishedUtc))
            .ForMember(dest => dest.JobSalaryInfoDto,
                opt => opt.MapFrom(src => src.Job!.SalaryInfo))
            .ForMember(dest => dest.EmploymentTypeIds,
                opt => opt.MapFrom(src => src.Job!.EmploymentTypes!.Select(x => x.Id)))
            .ForMember(dest => dest.DateTimeAppliedUtc,
                opt => opt.MapFrom(src => src.DateTimeCreatedUtc));
    }
}
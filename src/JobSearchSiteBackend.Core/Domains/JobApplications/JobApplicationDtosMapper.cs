using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobApplications;

public class JobApplicationDtosMapper : Profile
{
    public JobApplicationDtosMapper()
    {
        CreateMap<JobApplication, JobApplicationForManagersDto>()
            .ForMember(dest => dest.UserFullName,
                opt => opt.MapFrom(src => $"{src.User!.FirstName} {src.User!.LastName}"))
            .ForMember(dest => dest.PersonalFiles,
                opt => opt.MapFrom(src => src.PersonalFiles!))
            .ForMember(dest => dest.DateTimeAppliedUtc,
                opt => opt.MapFrom(src => src.DateTimeCreatedUtc));
        
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
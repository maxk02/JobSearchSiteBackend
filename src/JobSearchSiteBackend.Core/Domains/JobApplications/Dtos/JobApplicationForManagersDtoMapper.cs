using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;

public class JobApplicationForManagersDtoMapper : Profile
{
    public JobApplicationForManagersDtoMapper()
    {
        CreateMap<JobApplication, JobApplicationForManagersDto>()
            .ForMember(dest => dest.UserFullName,
                opt => opt.MapFrom(src => $"{src.User!.FirstName} {src.User!.LastName}"))
            .ForMember(dest => dest.PersonalFiles,
                opt => opt.MapFrom(src => src.PersonalFiles!))
            .ForMember(dest => dest.DateTimeAppliedUtc,
                opt => opt.MapFrom(src => src.DateTimeCreatedUtc));
    }
}
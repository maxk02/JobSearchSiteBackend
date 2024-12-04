using AutoMapper;
using Domain.Jobs;

namespace Application.Features.JobFeatures.CreateJob;

public class CreateJobMapper : Profile
{
    public CreateJobMapper()
    {
        CreateMap<CreateJobRequest, Job>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => 0));
    }
}
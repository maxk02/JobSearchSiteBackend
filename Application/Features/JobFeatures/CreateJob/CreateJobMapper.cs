using Application.Common.Exceptions;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.JobFeatures.CreateJob;

public class CreateJobMapper : Profile
{
    public CreateJobMapper()
    {
        CreateMap<CreateJobRequest, Job>()
            .BeforeMap((src, dest) =>
            {
                if (src.CompanyId is null) throw new NullToNonNullableMappingException();
                if (src.CategoryId is null) throw new NullToNonNullableMappingException();
                if (src.Title is null) throw new NullToNonNullableMappingException();
                if (src.DateTimeExpiringUtc is null) throw new NullToNonNullableMappingException();
                if (src.Description is null) throw new NullToNonNullableMappingException();
                if (src.IsHidden is null) throw new NullToNonNullableMappingException();
            })
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => 0));
    }
}
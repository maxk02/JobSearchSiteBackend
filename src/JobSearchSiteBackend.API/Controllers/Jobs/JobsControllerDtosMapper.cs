using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;

namespace JobSearchSiteBackend.API.Controllers.Jobs;

public class JobsControllerDtosMapper : Profile
{
    public JobsControllerDtosMapper()
    {
        CreateMap<UpdateJobRequestDto, UpdateJobRequest>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("Id", out var id))
                {
                    return (long)id;
                }

                return 0;
            }));
    }
}
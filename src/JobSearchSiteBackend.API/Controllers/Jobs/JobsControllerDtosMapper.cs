using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;
using UpdateJobRequest = JobSearchSiteBackend.API.Controllers.Jobs.Dtos.UpdateJobRequest;

namespace JobSearchSiteBackend.API.Controllers.Jobs;

public class JobsControllerDtosMapper : Profile
{
    public JobsControllerDtosMapper()
    {
        CreateMap<UpdateJobRequest, UpdateJobCommand>()
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
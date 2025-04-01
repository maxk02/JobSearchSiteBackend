using AutoMapper;
using Core.Domains.JobApplications.UseCases.UpdateJobApplication;
using Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;

namespace API.Controllers.JobApplications;

public class JobApplicationsControllerDtosMapper : Profile
{
    public JobApplicationsControllerDtosMapper()
    {
        CreateMap<UpdateJobApplicationRequestDto, UpdateJobApplicationRequest>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("Id", out var id))
                {
                    return (long)id;
                }

                return 0;
            }));
        
        CreateMap<UpdateJobApplicationFilesRequestDto, UpdateJobApplicationFilesRequest>()
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
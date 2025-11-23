using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;
using UpdateJobApplicationFilesRequest = JobSearchSiteBackend.API.Controllers.JobApplications.Dtos.UpdateJobApplicationFilesRequest;
using UpdateJobApplicationStatusRequest = JobSearchSiteBackend.API.Controllers.JobApplications.Dtos.UpdateJobApplicationStatusRequest;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public class JobApplicationsControllerDtosMapper : Profile
{
    public JobApplicationsControllerDtosMapper()
    {
        CreateMap<UpdateJobApplicationStatusRequest, UpdateJobApplicationStatusCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("Id", out var id))
                {
                    return (long)id;
                }

                return 0;
            }));
        
        CreateMap<UpdateJobApplicationFilesRequest, UpdateJobApplicationFilesCommand>()
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
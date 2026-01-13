using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;
using UpdateJobRequest = JobSearchSiteBackend.API.Controllers.Jobs.Dtos.UpdateJobRequest;

namespace JobSearchSiteBackend.API.Controllers.Jobs;

public class JobsControllerDtosMapper : Profile
{
    public JobsControllerDtosMapper()
    {
        CreateMap<AddJobResult, AddJobResponse>();
        
        CreateMap<GetApplicationsForJobResult, GetApplicationsForJobResponse>();
        
        CreateMap<GetJobDataForCurrentAccountResult, GetJobDataForCurrentAccountResponse>();

        CreateMap<GetJobManagementDtoResult, GetJobManagementDtoResponse>();
        
        CreateMap<GetJobResult, GetJobResponse>();
        
        CreateMap<GetJobsResult, GetJobsResponse>();
    }
}
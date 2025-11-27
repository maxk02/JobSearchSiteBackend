using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;
using UpdateJobApplicationFilesRequest = JobSearchSiteBackend.API.Controllers.JobApplications.Dtos.UpdateJobApplicationFilesRequest;
using UpdateJobApplicationStatusRequest = JobSearchSiteBackend.API.Controllers.JobApplications.Dtos.UpdateJobApplicationStatusRequest;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public class JobApplicationsControllerDtosMapper : Profile
{
    public JobApplicationsControllerDtosMapper()
    {
        CreateMap<AddJobApplicationResult, AddJobApplicationResponse>();
    }
}
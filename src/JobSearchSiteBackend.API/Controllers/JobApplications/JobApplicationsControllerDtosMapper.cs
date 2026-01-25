using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

public class JobApplicationsControllerDtosMapper : Profile
{
    public JobApplicationsControllerDtosMapper()
    {
        CreateMap<AddJobApplicationResult, AddJobApplicationResponse>();
        CreateMap<GetFileDownloadLinkFromJobApplicationResult, GetFileDownloadLinkFromJobApplicationResponse>();
    }
}
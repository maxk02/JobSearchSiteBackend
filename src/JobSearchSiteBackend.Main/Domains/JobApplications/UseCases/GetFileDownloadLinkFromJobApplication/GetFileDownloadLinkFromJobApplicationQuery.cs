using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;

public record GetFileDownloadLinkFromJobApplicationQuery(long JobApplicationId, long PersonalFileId)
    : IRequest<Result<GetFileDownloadLinkFromJobApplicationResult>>;
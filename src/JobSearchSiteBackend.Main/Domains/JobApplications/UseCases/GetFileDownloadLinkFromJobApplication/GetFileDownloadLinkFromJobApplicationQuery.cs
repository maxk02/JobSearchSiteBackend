using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;

public record GetFileDownloadLinkFromJobApplicationQuery(long JobApplicationId, long PersonalFileId)
    : IRequest<Result<GetFileDownloadLinkFromJobApplicationResult>>;
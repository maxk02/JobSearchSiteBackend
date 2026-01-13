using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public record GetJobDataForCurrentAccountQuery(long JobId) : IRequest<Result<GetJobDataForCurrentAccountResult>>;